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

        public GrnService(IGrnRepository grnRepository)
        {
            _grnRepository = grnRepository;
        }

        public async Task<GetGrnDTO> CreateGrnAsync(CreateGrnDTO request)
        {
            // 1. PO must exist
            var po = await _grnRepository.GetPurchaseOrderByIdAsync(request.PurchaseOrderId);
            if (po == null)
                throw new KeyNotFoundException("PO_NOT_FOUND");

            // 2. PO status must be Approved or PartiallyReceived (fetched from DB)
            var receivableStatusIds = await _grnRepository.GetReceivablePoStatusIdsAsync();
            if (!receivableStatusIds.Contains(po.PurchaseOrderStatusId))
                throw new InvalidOperationException("PO_NOT_RECEIVABLE");

            // 3. ReceivedDate must not be future
            if (request.ReceivedDate > DateTime.UtcNow)
                throw new ArgumentException("INVALID_DATE");

            // 4. Fetch Open status from DB
            var openStatus = await _grnRepository.GetGrnStatusByCodeAsync("OPEN");
            if (openStatus == null)
                throw new InvalidOperationException("INTERNAL_ERROR");

            // 5. Create with Open status
            var grn = new GoodsReciept
            {
                PurchaseOrderId = request.PurchaseOrderId,
                ReceivedDate = request.ReceivedDate,
                Status = openStatus.GoodsReceiptStatusId
            };

            await _grnRepository.AddAsync(grn);

            return await _grnRepository.GetGrnDtoByIdAsync(grn.GoodsRecieptId)!;
        }

        public async Task<GetGrnDTO> UpdateGrnAsync(int grnId, UpdateGrnDTO request)
        {
            // 1. GRN must exist
            var grn = await _grnRepository.GetByIdAsync(grnId);
            if (grn == null)
                throw new KeyNotFoundException("GRN_NOT_FOUND");

            // Fetch status IDs from DB
            var openStatus     = await _grnRepository.GetGrnStatusByCodeAsync("OPEN");
            var postedStatus   = await _grnRepository.GetGrnStatusByCodeAsync("POSTED");
            var rejectedStatus = await _grnRepository.GetGrnStatusByCodeAsync("REJECTED");

            var openId     = openStatus!.GoodsReceiptStatusId;
            var postedId   = postedStatus!.GoodsReceiptStatusId;
            var rejectedId = rejectedStatus!.GoodsReceiptStatusId;

            //  Post GRN
            if (request.StatusId == postedId)
            {
                if (grn.Status != openId)
                    throw new InvalidOperationException("GRN_NOT_OPEN");

                var hasItems = await _grnRepository.HasGrnItemsAsync(grnId);
                if (!hasItems)
                    throw new InvalidOperationException("GRN_NO_ITEMS");

                grn.Status = postedId;
            }
            //  Reject GRN
            else if (request.StatusId == rejectedId)
            {
                if (grn.Status != postedId)
                    throw new InvalidOperationException("GRN_NOT_POSTED");

                grn.Status = rejectedId;
            }
            // Invalid statusId value
            else if (request.StatusId.HasValue)
            {
                throw new ArgumentException("INVALID_STATUS");
            }

            // Edit ReceivedDate
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
        {
            return await _grnRepository.GetGrnDtoByIdAsync(grnId);
        }

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
    }
}
