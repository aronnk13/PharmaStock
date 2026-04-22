using PharmaStock.Core.DTO.PurchaseOrder;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IPurchaseOrderRepository : IGenericRepository<PurchaseOrder>
    {
        Task<IEnumerable<PurchaseOrderResponseDTO>> GetAllWithDetailsAsync();
        Task<PurchaseOrderResponseDTO?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<PurchaseOrderStatusDTO>> GetAllStatusesAsync();
        Task<int> GetDefaultStatusIdAsync();
    }
}
