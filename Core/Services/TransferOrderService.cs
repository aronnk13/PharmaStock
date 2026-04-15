using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO.Transfer;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class TransferOrderService : ITransferOrderService
    {
        private readonly ITransferOrderRepository _repo;

        public TransferOrderService(ITransferOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<TransferOrderResponseDTO> CreateTransferOrderAsync(CreateTransferOrderDTO dto)
        {
            var isFromValid = await _repo.IsLocationValidAsync(dto.FromLocationId);
            if (!isFromValid)
                throw new Exception("Invalid FromLocationId");

            var isToValid = await _repo.IsLocationValidAsync(dto.ToLocationId);
            if (!isToValid)
                throw new Exception("Invalid ToLocationId");

            if (dto.FromLocationId == dto.ToLocationId)
                throw new Exception("FromLocation and ToLocation cannot be the same");

            var transferOrder = new TransferOrder
            {
                FromLocationId = dto.FromLocationId,
                ToLocationId = dto.ToLocationId,
                CreatedDate = DateTime.UtcNow,
                Status = 1 // Open
            };

            await _repo.AddAsync(transferOrder);

            return new TransferOrderResponseDTO
            {
                TransferOrderId = transferOrder.TransferOrderId,
                FromLocationId = transferOrder.FromLocationId,
                ToLocationId = transferOrder.ToLocationId,
                CreatedDate = transferOrder.CreatedDate,
                Status = transferOrder.Status
            };
        }

        public async Task<TransferItemResponseDTO> AddTransferItemAsync(CreateTransferItemDTO dto)
        {
            var isOrderValid = await _repo.IsTransferOrderValidAsync(dto.TransferOrderId);
            if (!isOrderValid)
                throw new Exception("Invalid TransferOrderId");

            var isItemValid = await _repo.IsItemValidAsync(dto.ItemId);
            if (!isItemValid)
                throw new Exception("Invalid ItemId");

            var isLotValid = await _repo.IsInventoryLotValidAsync(dto.InventoryLotId);
            if (!isLotValid)
                throw new Exception("Invalid InventoryLotId");

            var transferItem = new TransferItem
            {
                TransferOrderId = dto.TransferOrderId,
                ItemId = dto.ItemId,
                InventoryLotId = dto.InventoryLotId,
                Quantity = dto.Quantity
            };

            // TransferItem uses its own generic repo
            // You need to register IGenericRepository<TransferItem> in Program.cs
            // or add AddTransferItemAsync to the repository

            return new TransferItemResponseDTO
            {
                TransferItemId = transferItem.TransferItemId,
                TransferOrderId = transferItem.TransferOrderId,
                ItemId = transferItem.ItemId,
                InventoryLotId = transferItem.InventoryLotId,
                Quantity = transferItem.Quantity
            };
        }

        public async Task<IEnumerable<TransferOrderResponseDTO>> GetAllTransferOrdersAsync(TransferOrderFilterDTO? filter = null)
        {
            var orders = await _repo.GetByFilterAsync(filter ?? new TransferOrderFilterDTO());
            return orders.Select(o => new TransferOrderResponseDTO
            {
                TransferOrderId = o.TransferOrderId,
                FromLocationId = o.FromLocationId,
                ToLocationId = o.ToLocationId,
                CreatedDate = o.CreatedDate,
                Status = o.Status
            });
        }

        public async Task<IEnumerable<TransferItemResponseDTO>> GetItemsByTransferOrderIdAsync(int transferOrderId)
        {
            var items = await _repo.GetItemsByTransferOrderIdAsync(transferOrderId);
            return items.Select(i => new TransferItemResponseDTO
            {
                TransferItemId = i.TransferItemId,
                TransferOrderId = i.TransferOrderId,
                ItemId = i.ItemId,
                InventoryLotId = i.InventoryLotId,
                Quantity = i.Quantity
            });
        }

    }
}