using PharmaStock.Core.DTO.Vendor;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IVendorService
    {
        Task<VendorDTO> CreateAsync(VendorDTO dto);
        Task<VendorDTO?> GetByIdAsync(int vendorId);
        Task<IEnumerable<VendorDTO>> GetAllAsync(bool includeInactive, string? name);
        Task UpdateAsync(int vendorId, VendorDTO dto);
        Task DeleteAsync(int vendorId);
    }
}