using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO.Vendor;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorService(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        // AC‑VP‑01 & AC‑VP‑02
        public async Task<VendorDTO> CreateAsync(VendorDTO dto)
        {
            var existing = await _vendorRepository.GetByNameAsync(dto.Name);
            if (existing != null)
                throw new InvalidOperationException(
                    $"Vendor '{existing.Name}' already exists (ID {existing.VendorId})");

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
            return dto;
        }

        public async Task<VendorDTO?> GetByIdAsync(int vendorId)
        {
            var vendor = await _vendorRepository.GetByIdAsync(vendorId);
            return vendor == null ? null : Map(vendor);
        }

        public async Task<IEnumerable<VendorDTO>> GetAllAsync(bool includeInactive)
        {
            var vendors = await _vendorRepository.GetAllAsync();
            return vendors
                .Where(v => includeInactive || v.StatusId == true)
                .Select(Map);
        }

        // AC‑VP‑03
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
        }

        // AC‑VP‑04
        public async System.Threading.Tasks.Task SetStatusAsync(int vendorId, bool isActive)
        {
            var vendor = await _vendorRepository.GetByIdAsync(vendorId);
            if (vendor == null)
                throw new KeyNotFoundException("Vendor not found");

            vendor.StatusId = isActive;
            await _vendorRepository.UpdateAsync(vendor);
        }

        public async System.Threading.Tasks.Task DeleteAsync(int vendorId)
        {
            await SetStatusAsync(vendorId, false);
        }

        private static VendorDTO Map(Vendor v) => new()
        {
            VendorId = v.VendorId,
            Name = v.Name,
            ContactInfo = v.ContactInfo!,
            Rating = v.Rating,
            StatusId = (bool)v.StatusId,
            Email = v.Email,
            Phone = v.Phone,
            
        };
    }
}