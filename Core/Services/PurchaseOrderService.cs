using PharmaStock.Core.DTO.PurchaseOrder;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _repo;

        public PurchaseOrderService(IPurchaseOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PurchaseOrderResponseDTO>> GetAllAsync()
        {
            return await _repo.GetAllWithDetailsAsync();
        }

        public async Task<PurchaseOrderResponseDTO?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdWithDetailsAsync(id);
        }

        public async Task<IEnumerable<PurchaseOrderStatusDTO>> GetStatusesAsync()
        {
            return await _repo.GetAllStatusesAsync();
        }

        public async Task<PurchaseOrderResponseDTO> CreateAsync(CreatePurchaseOrderDTO dto)
        {
            if (dto.ExpectedDate < dto.OrderDate)
                throw new ArgumentException("Expected date cannot be before order date.");

            var defaultStatusId = await _repo.GetDefaultStatusIdAsync();

            var po = new PurchaseOrder
            {
                VendorId              = dto.VendorId,
                LocationId            = dto.LocationId,
                OrderDate             = dto.OrderDate,
                ExpectedDate          = dto.ExpectedDate,
                PurchaseOrderStatusId = defaultStatusId
            };

            await _repo.AddAsync(po);

            var result = await _repo.GetByIdWithDetailsAsync(po.PurchaseOrderId);
            return result!;
        }

        public async Task<PurchaseOrderResponseDTO> UpdateAsync(int id, UpdatePurchaseOrderDTO dto)
        {
            var po = await _repo.GetByIdAsync(id);
            if (po == null)
                throw new KeyNotFoundException("Purchase order not found.");

            if (dto.ExpectedDate.HasValue)
                po.ExpectedDate = dto.ExpectedDate.Value;

            if (dto.PurchaseOrderStatusId.HasValue)
                po.PurchaseOrderStatusId = dto.PurchaseOrderStatusId.Value;

            await _repo.UpdateAsync(po);

            var result = await _repo.GetByIdWithDetailsAsync(id);
            return result!;
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var po = await _repo.GetByIdAsync(id);
            if (po == null)
                throw new KeyNotFoundException("Purchase order not found.");

            await _repo.DeleteAsync(id);
        }
    }
}
