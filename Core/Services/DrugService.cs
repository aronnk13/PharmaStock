using System;
using System.Runtime.CompilerServices;
using Microsoft.IdentityModel.Tokens;
using PharmaStock.Core.DTO.Auth;
using PharmaStock.Core.DTO.Common;
using PharmaStock.Core.DTO.Drug;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Infrastructure.Repositories;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;


namespace PharmaStock.Core.Services
{
    public class DrugService : IDrugService
    {
        private readonly IDrugRepository _drugRepository;
        public DrugService(IDrugRepository drugRepository)
        {
            _drugRepository = drugRepository;
        }

        public async Task<GetDrugDTO> GetDrugbyid(int id)
        {
            var drugModel = await _drugRepository.GetByIdAsync(id);
            if (drugModel == null)
            {
                return null;
            }
            return new GetDrugDTO
            {
                //Id = drugModel.DrugId,
                Atccode = drugModel.Atccode,
                BrandName = drugModel.BrandName,
                ControlClass = drugModel.ControlClass,
                StorageClass = drugModel.StorageClass,
                Status = drugModel.Status
            };
        }

        public async Task<PaginatedResult<GetDrugDTO>> GetPaginatedResult(DrugFilterDTO filter)
        {
            var (drugs, totalCount) = await _drugRepository.GetDrugsByFilterAsync(filter);
            var dtoList = drugs.Select(a => new GetDrugDTO
            {
                Atccode = a.Atccode,
                BrandName = a.BrandName,
                ControlClass = a.ControlClass,
                StorageClass = a.StorageClass,
                Status = a.Status
            }).ToList();
            return new PaginatedResult<GetDrugDTO>
            {
                Items = dtoList,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }
        public async Task<Drug> CreateDrug(CreateDrugDTO dto)
        {
            // 1. Duplicate Check
            var isDuplicate = await _drugRepository.Exists(dto.GenericName, dto.Strength, dto.Form);
            if (isDuplicate) throw new InvalidOperationException("DRUG_DUPLICATE");

            // 2. Mapping DTO to Entity
            var drug = new Drug
            {
                GenericName = dto.GenericName,
                BrandName = dto.BrandName,
                Strength = dto.Strength,
                Form = dto.Form,
                Atccode = dto.Atccode,
                StorageClass = dto.StorageClass,
                ControlClass = dto.ControlClass,
                Status = dto.Status
            };

            // 3. Use Generic Repository Add
            await _drugRepository.AddAsync(drug);
            return drug;
        }

        public async Task<bool> UpdateDrug(UpdateDrugDTO dto)
        {
            // 1. Fetch existing entity (AWAIT this)
            var existingDrug = await _drugRepository.GetByIdAsync(dto.DrugId);
            if (existingDrug == null) return false;

            // 2. Update properties
            existingDrug.GenericName = dto.GenericName;
            existingDrug.BrandName = dto.BrandName;
            existingDrug.Strength = dto.Strength;
            existingDrug.Form = dto.Form;
            existingDrug.Atccode = dto.Atccode;
            existingDrug.StorageClass = dto.StorageClass;
            existingDrug.ControlClass = dto.ControlClass;
            existingDrug.Status = dto.Status;

            // 3. Save via Repository
            return await _drugRepository.UpdateAsync(existingDrug);
        }

        public async Task<DrugDeletedResponseDTO> DeleteDrug(int DrugId)
        {
            return await _drugRepository.DeleteDrug(DrugId);
        }
    }
}