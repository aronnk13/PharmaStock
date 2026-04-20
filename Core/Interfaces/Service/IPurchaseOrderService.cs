using PharmaStock.Core.DTO.PurchaseOrder;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IPurchaseOrderService
    {
        Task<IEnumerable<PurchaseOrderResponseDTO>> GetAllAsync();
        Task<PurchaseOrderResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<PurchaseOrderStatusDTO>> GetStatusesAsync();
        Task<PurchaseOrderResponseDTO> CreateAsync(CreatePurchaseOrderDTO dto);
        Task<PurchaseOrderResponseDTO> UpdateAsync(int id, UpdatePurchaseOrderDTO dto);
        System.Threading.Tasks.Task DeleteAsync(int id);
    }
}
