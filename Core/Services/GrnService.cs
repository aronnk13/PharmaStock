using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.Common;
using PharmaStock.Core.DTO.GoodsReceipt;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class GrnService : IGrnService
    {
        private readonly IGrnRepository _grnRepository;
        private readonly PharmaStockContext _context;

        public GrnService(IGrnRepository grnRepository, PharmaStockContext context)
        {
            _grnRepository = grnRepository;
            _context = context;
        }

        public async Task<GetGrnDTO> CreateGrnAsync(CreateGrnDTO request, int userId)
        {
            var po = await _grnRepository.GetPurchaseOrderByIdAsync(request.PurchaseOrderId);
            if (po == null)
                throw new KeyNotFoundException("PO_NOT_FOUND");

            var receivableStatusIds = await _grnRepository.GetReceivablePoStatusIdsAsync();
            if (!receivableStatusIds.Contains(po.PurchaseOrderStatusId))
                throw new InvalidOperationException("PO_NOT_RECEIVABLE");

            if (request.ReceivedDate > DateTime.UtcNow)
                throw new ArgumentException("INVALID_DATE");

            var openStatus = await _grnRepository.GetGrnStatusByCodeAsync("OPEN");
            if (openStatus == null)
                throw new InvalidOperationException("INTERNAL_ERROR");

            var username = await _grnRepository.GetUsernameByIdAsync(userId);

            var grn = new GoodsReciept
            {
                PurchaseOrderId = request.PurchaseOrderId,
                ReceivedDate = request.ReceivedDate,
                Status = openStatus.GoodsReceiptStatusId,
                ReceivedBy = username
            };

            await _grnRepository.AddAsync(grn);

            return await _grnRepository.GetGrnDtoByIdAsync(grn.GoodsRecieptId)!;
        }

        public async Task<GetGrnDTO> UpdateGrnAsync(int grnId, UpdateGrnDTO request)
        {
            var grn = await _grnRepository.GetByIdAsync(grnId);
            if (grn == null)
                throw new KeyNotFoundException("GRN_NOT_FOUND");

            var openStatus     = await _grnRepository.GetGrnStatusByCodeAsync("OPEN");
            var postedStatus   = await _grnRepository.GetGrnStatusByCodeAsync("POSTED");
            var rejectedStatus = await _grnRepository.GetGrnStatusByCodeAsync("REJECTED");

            var openId     = openStatus!.GoodsReceiptStatusId;
            var postedId   = postedStatus!.GoodsReceiptStatusId;
            var rejectedId = rejectedStatus!.GoodsReceiptStatusId;

            if (request.StatusId == postedId)
            {
                if (grn.Status != openId)
                    throw new InvalidOperationException("GRN_NOT_OPEN");

                var hasItems = await _grnRepository.HasGrnItemsAsync(grnId);
                if (!hasItems)
                    throw new InvalidOperationException("GRN_NO_ITEMS");

                grn.Status = postedId;
            }
            else if (request.StatusId == rejectedId)
            {
                if (grn.Status != postedId)
                    throw new InvalidOperationException("GRN_NOT_POSTED");

                grn.Status = rejectedId;
            }
            else if (request.StatusId.HasValue)
            {
                throw new ArgumentException("INVALID_STATUS");
            }

            if (request.ReceivedDate.HasValue)
            {
                if (grn.Status != openId)
                    throw new InvalidOperationException("GRN_NOT_EDITABLE");

                if (request.ReceivedDate.Value > DateTime.UtcNow)
                    throw new ArgumentException("INVALID_DATE");

                grn.ReceivedDate = request.ReceivedDate.Value;
            }

            await _grnRepository.UpdateAsync(grn);

            return await _grnRepository.GetGrnDtoByIdAsync(grnId)!;
        }

        public async Task<GetGrnDTO?> GetGrnByIdAsync(int grnId)
            => await _grnRepository.GetGrnDtoByIdAsync(grnId);

        public async Task<GetGrnWithItemsDTO?> GetGrnWithItemsAsync(int grnId)
            => await _grnRepository.GetGrnWithItemsAsync(grnId);

        public async Task<PaginatedResult<GetGrnDTO>> GetAllGrnsAsync(GrnFilterDTO filter)
        {
            var (grns, totalCount) = await _grnRepository.GetGrnsByFilterAsync(filter);
            return new PaginatedResult<GetGrnDTO>
            {
                Items = grns,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize > 100 ? 100 : filter.PageSize
            };
        }

        public async Task<List<GetGrnDTO>> GetPendingQcAsync()
            => await _grnRepository.GetPendingQcGrnsAsync();

        // ── Complete QC — transactional ───────────────────────────────────────────

        public async Task<CompleteQcResultDTO> CompleteQcAsync(
            int grnId, CompleteQcDTO dto, int userId)
        {
            Console.WriteLine($"[QC-SVC] CompleteQcAsync called — GRN={grnId}, userId={userId}, dto.Items.Count={dto.Items.Count}");
            foreach (var it in dto.Items)
                Console.WriteLine($"[QC-SVC]   item: GrnItemId={it.GrnItemId}, AcceptedQty={it.AcceptedQty}, RejectedQty={it.RejectedQty}");

            // Guard: reject immediately if no items were sent
            if (!dto.Items.Any())
                throw new ArgumentException("QC_ITEMS_EMPTY");

            // Load GRN with PO and items
            var grn = await _grnRepository.GetGrnWithPoAndItemsAsync(grnId)
                ?? throw new KeyNotFoundException("GRN_NOT_FOUND");

            Console.WriteLine($"[QC-SVC] Loaded GRN {grnId} — DB item count: {grn.GoodsReceiptItems.Count}, status: {grn.Status}");

            // Validate GRN is not already completed
            var completedStatusIds = await _context.GoodsReceiptStatuses
                .Where(s => s.Status == "Completed" || s.Status == "PartiallyAccepted"
                         || s.Status == "FullyRejected")
                .Select(s => s.GoodsReceiptStatusId)
                .ToListAsync();

            if (completedStatusIds.Contains(grn.Status))
                throw new InvalidOperationException("GRN_ALREADY_COMPLETED");

            // Validate each item in the request
            foreach (var item in dto.Items)
            {
                var grnItem = grn.GoodsReceiptItems
                    .FirstOrDefault(i => i.GoodsReceiptItemId == item.GrnItemId)
                    ?? throw new KeyNotFoundException($"GRN_ITEM_NOT_FOUND:{item.GrnItemId}");

                if (item.AcceptedQty + item.RejectedQty != grnItem.ReceivedQty)
                    throw new ArgumentException(
                        $"QTY_MISMATCH:Item {item.GrnItemId} — acceptedQty+rejectedQty must equal receivedQty ({grnItem.ReceivedQty})");

                if (item.AcceptedQty < 0 || item.RejectedQty < 0)
                    throw new ArgumentException($"NEGATIVE_QTY:Item {item.GrnItemId}");

                if (item.RejectedQty > 0 && string.IsNullOrWhiteSpace(item.RejectionReason))
                    throw new ArgumentException($"REASON_REQUIRED:Item {item.GrnItemId}");
            }

            // Look up status IDs
            var availableLotStatusId = await _grnRepository.GetInventoryLotStatusIdByNameAsync("Available")
                ?? throw new InvalidOperationException("MISSING_LOT_STATUS_AVAILABLE");
            var quarantineLotStatusId = await _grnRepository.GetInventoryLotStatusIdByNameAsync("Quarantine")
                ?? throw new InvalidOperationException("MISSING_LOT_STATUS_QUARANTINED");
            var receiptTypeId = await _grnRepository.GetStockTransitionTypeIdByNameAsync("Receipt")
                ?? throw new InvalidOperationException("MISSING_STOCK_TRANSITION_TYPE_RECEIPT");
            var locationId = grn.PurchaseOrder.LocationId;
            var binId = await _grnRepository.GetFirstBinIdAtLocationAsync(locationId)
                ?? throw new InvalidOperationException("NO_BIN_AT_LOCATION");

            // ── Begin transaction ─────────────────────────────────────────────────
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int lotsCreated = 0;
                int totalAccepted = 0;
                int totalRejected = 0;
                int quarantineCreated = 0;

                foreach (var qcItem in dto.Items)
                {
                    Console.WriteLine($"[QC-SVC] Processing item GrnItemId={qcItem.GrnItemId} — accepted={qcItem.AcceptedQty}, rejected={qcItem.RejectedQty}");

                    var grnItem = grn.GoodsReceiptItems
                        .First(i => i.GoodsReceiptItemId == qcItem.GrnItemId);

                    // Update GRN item quantities and reason
                    grnItem.AcceptedQty = qcItem.AcceptedQty;
                    grnItem.RejectedQty = qcItem.RejectedQty;
                    grnItem.Reason = qcItem.RejectionReason;
                    _context.GoodsReceiptItems.Update(grnItem);

                    totalAccepted += qcItem.AcceptedQty;
                    totalRejected += qcItem.RejectedQty;

                    // ── Accepted stock ────────────────────────────────────────────
                    if (qcItem.AcceptedQty > 0)
                    {
                        // Duplicate lot protection: check batchNumber + itemId
                        var existingLot = await _context.InventoryLots
                            .FirstOrDefaultAsync(l => l.BatchNumber == grnItem.BatchNumber
                                                   && l.ItemId == grnItem.ItemId);

                        InventoryLot lot;
                        if (existingLot != null)
                        {
                            lot = existingLot;
                        }
                        else
                        {
                            lot = new InventoryLot
                            {
                                ItemId = grnItem.ItemId,
                                BatchNumber = grnItem.BatchNumber,
                                ExpiryDate = grnItem.ExpiryDate,
                                Status = availableLotStatusId
                            };
                            _context.InventoryLots.Add(lot);
                            await _context.SaveChangesAsync();
                            lotsCreated++;
                        }

                        // Upsert InventoryBalance
                        var balance = await _context.InventoryBalances
                            .FirstOrDefaultAsync(b => b.LocationId == locationId
                                                   && b.ItemId == grnItem.ItemId
                                                   && b.InventoryLotId == lot.InventoryLotId);
                        if (balance != null)
                        {
                            balance.QuantityOnHand += qcItem.AcceptedQty;
                            _context.InventoryBalances.Update(balance);
                        }
                        else
                        {
                            _context.InventoryBalances.Add(new InventoryBalance
                            {
                                LocationId = locationId,
                                BinId = binId,
                                ItemId = grnItem.ItemId,
                                InventoryLotId = lot.InventoryLotId,
                                QuantityOnHand = qcItem.AcceptedQty,
                                ReservedQty = 0
                            });
                        }

                        // StockTransition (Receipt)
                        _context.StockTransitions.Add(new StockTransition
                        {
                            LocationId = locationId,
                            BinId = binId,
                            ItemId = grnItem.ItemId,
                            InventoryLotId = lot.InventoryLotId,
                            StockTransitionType = receiptTypeId,
                            Quantity = qcItem.AcceptedQty,
                            StockTransitionTypeDate = DateTime.UtcNow,
                            ReferenceId = grnId.ToString(),
                            Notes = $"GRN #{grnId} QC accepted",
                            PerformedBy = userId
                        });
                    }

                    // ── Rejected stock → Quarantine ───────────────────────────────
                    if (qcItem.RejectedQty > 0)
                    {
                        // Create a quarantined lot for the rejected batch
                        var rejectedLot = await _context.InventoryLots
                            .FirstOrDefaultAsync(l => l.BatchNumber == grnItem.BatchNumber
                                                   && l.ItemId == grnItem.ItemId
                                                   && l.Status == quarantineLotStatusId);

                        if (rejectedLot == null)
                        {
                            rejectedLot = new InventoryLot
                            {
                                ItemId = grnItem.ItemId,
                                BatchNumber = grnItem.BatchNumber,
                                ExpiryDate = grnItem.ExpiryDate,
                                Status = quarantineLotStatusId
                            };
                            _context.InventoryLots.Add(rejectedLot);
                            await _context.SaveChangesAsync();
                        }

                        _context.QuarantaineActions.Add(new QuarantaineAction
                        {
                            InventoryLotId = rejectedLot.InventoryLotId,
                            QuarantineDate = DateTime.UtcNow,
                            Reason = qcItem.RejectionReason ?? "Rejected during GRN QC",
                            Status = 1  // Quarantined
                        });
                        quarantineCreated++;
                    }
                }

                await _context.SaveChangesAsync();

                // ── Determine GRN final status ────────────────────────────────────
                string grnStatusName;
                if (totalAccepted == 0)
                    grnStatusName = "FullyRejected";
                else if (totalRejected > 0)
                    grnStatusName = "PartiallyAccepted";
                else
                    grnStatusName = "Completed";

                var newGrnStatus = await _context.GoodsReceiptStatuses
                    .FirstOrDefaultAsync(s => s.Status == grnStatusName)
                    ?? throw new InvalidOperationException($"MISSING_GRN_STATUS:{grnStatusName}");

                grn.Status = newGrnStatus.GoodsReceiptStatusId;
                _context.GoodsReciepts.Update(grn);
                await _context.SaveChangesAsync();

                // ── Update PO status ──────────────────────────────────────────────
                var allGrns = await _context.GoodsReciepts
                    .Include(g => g.GoodsReceiptItems)
                    .Where(g => g.PurchaseOrderId == grn.PurchaseOrderId)
                    .ToListAsync();

                var totalOrderedQty = grn.PurchaseOrder.PurchaseItems.Sum(pi => pi.OrderedQty);
                var totalAcceptedAllGrns = allGrns.SelectMany(g => g.GoodsReceiptItems)
                    .Sum(i => i.AcceptedQty);

                string poStatusName = totalAcceptedAllGrns >= totalOrderedQty
                    ? "Closed"
                    : "PartiallyReceived";

                var newPoStatus = await _context.PurchaseOrderStatuses
                    .FirstOrDefaultAsync(s => s.Status == poStatusName);

                if (newPoStatus != null)
                {
                    var po = await _context.PurchaseOrders.FindAsync(grn.PurchaseOrderId);
                    if (po != null)
                    {
                        po.PurchaseOrderStatusId = newPoStatus.PurchaseOrderStatusId;
                        _context.PurchaseOrders.Update(po);
                        await _context.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();

                Console.WriteLine($"[QC-SVC] QC completed — GRN status={grnStatusName}, PO status={poStatusName}, lots={lotsCreated}, accepted={totalAccepted}, rejected={totalRejected}");

                return new CompleteQcResultDTO
                {
                    GrnId = grnId,
                    GrnStatus = grnStatusName,
                    PoStatus = poStatusName,
                    InventoryLotsCreated = lotsCreated,
                    TotalAccepted = totalAccepted,
                    TotalRejected = totalRejected,
                    QuarantineCreated = quarantineCreated
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
