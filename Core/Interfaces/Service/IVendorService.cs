using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Controllers.Vendor;
using PharmaStock.Core.DTO.Vendor;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IVendorService
    {
        Task<VendorDTO> CreateAsync(VendorDTO dto);
        Task<VendorDTO?> GetByIdAsync(int vendorId);
        Task<IEnumerable<VendorDTO>> GetAllAsync(bool includeInactive);
        System.Threading.Tasks.Task UpdateAsync(int vendorId, VendorDTO dto);
        System.Threading.Tasks.Task SetStatusAsync(int vendorId, bool isActive);
        System.Threading.Tasks.Task DeleteAsync(int vendorId);
    }
}