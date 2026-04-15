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
                DrugId = drugModel.DrugId,
                GenericName = drugModel.GenericName,
                BrandName = drugModel.BrandName,
                Strength = drugModel.Strength,
                Form = drugModel.Form,
                Atccode = drugModel.Atccode,
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
                DrugId = a.DrugId,
                GenericName = a.GenericName,
                BrandName = a.BrandName,
                Strength = a.Strength,
                Form = a.Form,
                Atccode = a.Atccode,
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
        public async Task<Drug> CreateDrug(CreateDrugDTO createDrugDTO)
        {
            // 1. Duplicate Check
            var isDuplicate = await _drugRepository.IsDrugExists(createDrugDTO.GenericName, createDrugDTO.Strength, createDrugDTO.Form);
            if (isDuplicate) throw new InvalidOperationException("DRUG_DUPLICATE");

            // 2. Mapping DTO to Entity
            var drug = new Drug
            {
                GenericName = createDrugDTO.GenericName,
                BrandName = createDrugDTO.BrandName,
                Strength = createDrugDTO.Strength,
                Form = createDrugDTO.Form,
                Atccode = createDrugDTO.Atccode,
                StorageClass = createDrugDTO.StorageClass,
                ControlClass = createDrugDTO.ControlClass,
                Status = createDrugDTO.Status
            };

            // 3. Use Generic Repository Add
            await _drugRepository.AddAsync(drug);
            return drug;
        }

        public async Task<bool> UpdateDrug(int drugId, UpdateDrugDTO updateDrugDTO)
        {
            // 1. Verify the drug exists before trying to update
            var existingDrug = await _drugRepository.GetByIdAsync(drugId);
            if (existingDrug == null) return false;

            // 2. Duplicate Check
            // Pass 'updateDrugDTO.DrugId' as the 'excludeId' parameter
            var isDuplicate = await _drugRepository.IsDrugExists(
                updateDrugDTO.GenericName,
                updateDrugDTO.Strength,
                updateDrugDTO.Form,
                drugId
            );

            if (isDuplicate) throw new InvalidOperationException("DRUG_DUPLICATE");

            // 3. Map properties
            existingDrug.GenericName = updateDrugDTO.GenericName;
            existingDrug.BrandName = updateDrugDTO.BrandName;
            existingDrug.Strength = updateDrugDTO.Strength;
            existingDrug.Form = updateDrugDTO.Form;
            existingDrug.Atccode = updateDrugDTO.Atccode;
            existingDrug.ControlClass = updateDrugDTO.ControlClass;
            existingDrug.StorageClass = updateDrugDTO.StorageClass;
            existingDrug.Status = updateDrugDTO.Status;

            return await _drugRepository.UpdateAsync(existingDrug);
        }

        public async Task<DrugDeletedResponseDTO> DeleteDrug(int DrugId)
        {
            return await _drugRepository.DeleteDrug(DrugId);
        }
    }
}