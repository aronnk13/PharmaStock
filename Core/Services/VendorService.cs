using System.Text.Json;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Vendor;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VendorService(IVendorRepository vendorRepository, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _vendorRepository = vendorRepository;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId() =>
            int.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value, out var id) ? id : 0;

        public async Task<VendorDTO> CreateAsync(VendorDTO dto)
        {
            var existing = await _vendorRepository.GetByNameAsync(dto.Name);
            if (existing != null)
                throw new InvalidOperationException($"Vendor '{existing.Name}' already exists (ID {existing.VendorId})");

            var vendor = new Vendor
            {
                Name = dto.Name,
                ContactInfo = dto.ContactInfo,
                Rating = dto.Rating,
                Email = dto.Email,
                Phone = dto.Phone,
                StatusId = true
            };

            await _vendorRepository.AddAsync(vendor);

            dto.VendorId = vendor.VendorId;
            dto.StatusId = (bool)vendor.StatusId;

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "VENDOR_CREATED",
                Resource = $"Vendor:{vendor.VendorId}",
                Metadata = JsonSerializer.Serialize(dto)
            });

            return dto;
        }

        public async Task<VendorDTO?> GetByIdAsync(int vendorId)
        {
            var vendor = await _vendorRepository.GetByIdAsync(vendorId);
            if (vendor == null) return null;

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "VENDOR_VIEWED",
                Resource = $"Vendor:{vendorId}",
                Metadata = null
            });

            return Map(vendor);
        }

        public async Task<IEnumerable<VendorDTO>> GetAllAsync(bool includeInactive, string? name)
        {
            var vendors = await _vendorRepository.GetAllAsync();

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "VENDOR_LIST_VIEWED",
                Resource = "Vendor:list",
                Metadata = null
            });

            return vendors
                .Where(v => (includeInactive || v.StatusId == true) &&
                            (string.IsNullOrEmpty(name) || v.Name.Contains(name, StringComparison.OrdinalIgnoreCase)))
                .Select(Map);
        }

        public async System.Threading.Tasks.Task UpdateAsync(int vendorId, VendorDTO dto)
        {
            var vendor = await _vendorRepository.GetByIdAsync(vendorId);
            if (vendor == null)
                throw new KeyNotFoundException("Vendor not found");

            vendor.ContactInfo = dto.ContactInfo;
            vendor.Rating = dto.Rating;
            vendor.Email = dto.Email;
            vendor.Phone = dto.Phone;

            await _vendorRepository.UpdateAsync(vendor);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "VENDOR_UPDATED",
                Resource = $"Vendor:{vendorId}",
                Metadata = JsonSerializer.Serialize(dto)
            });
        }

        public async System.Threading.Tasks.Task DeleteAsync(int vendorId)
        {
            var vendor = await _vendorRepository.GetByIdAsync(vendorId);
            if (vendor == null)
                throw new KeyNotFoundException("Vendor not found");

            vendor.StatusId = false;
            await _vendorRepository.UpdateAsync(vendor);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "VENDOR_DELETED",
                Resource = $"Vendor:{vendorId}",
                Metadata = null
            });
        }

        private static VendorDTO Map(Vendor v) => new()
        {
            VendorId = v.VendorId,
            Name = v.Name,
            ContactInfo = v.ContactInfo!,
            Rating = v.Rating,
            StatusId = (bool)v.StatusId,
            Email = v.Email,
            Phone = v.Phone
        };
    }
}
