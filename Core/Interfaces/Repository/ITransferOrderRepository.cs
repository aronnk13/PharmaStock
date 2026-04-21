using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO.Transfer;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface ITransferOrderRepository : IGenericRepository<TransferOrder>
    {
        Task<bool> IsLocationValidAsync(int locationId);
        Task<bool> IsItemValidAsync(int itemId);
        Task<bool> IsInventoryLotValidAsync(int inventoryLotId);
        Task<bool> IsTransferOrderValidAsync(int transferOrderId);
        Task<IEnumerable<TransferItem>> GetItemsByTransferOrderIdAsync(int transferOrderId);
        Task<IEnumerable<TransferOrder>> GetByFilterAsync(TransferOrderFilterDTO filter);
        Task<IEnumerable<TransferOrder>> GetAllWithDetailsAsync();
    }
}