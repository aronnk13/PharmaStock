using PharmaStock.Core.DTO.Auth;
using PharmaStock.Core.DTO.Common;
using PharmaStock.Core.DTO.Drug;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Service;
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

        public async Task<GetDrugDTO?> GetDrugbyid(int id)
        {
            // Use repository method that includes navigation properties (FormName, ControlClassName, etc.)
            return await _drugRepository.GetDrugDtoByIdAsync(id);
        }

        public async Task<PaginatedResult<GetDrugDTO>> GetPaginatedResult(DrugFilterDTO filter)
        {
            var (drugs, totalCount) = await _drugRepository.GetDrugsByFilterAsync(filter);
            return new PaginatedResult<GetDrugDTO>
            {
                Items = drugs,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize > 100 ? 100 : filter.PageSize
            };
        }

        public async Task<GetDrugDTO> CreateDrug(CreateDrugDTO request)
        {
            // 1. Duplicate check
            var isDuplicate = await _drugRepository.IsDrugExists(request.GenericName, request.Strength, request.Form);
            if (isDuplicate)
                throw new InvalidOperationException("DRUG_DUPLICATE");

            // 2. Map DTO → Entity
            var drug = new Drug
            {
                GenericName = request.GenericName,
                BrandName = request.BrandName,
                Strength = request.Strength,
                Form = request.Form,
                Atccode = request.Atccode,
                StorageClass = request.StorageClass,
                ControlClass = request.ControlClass,
                Status = request.Status
            };

            await _drugRepository.AddAsync(drug);

            return await _drugRepository.GetDrugDtoByIdAsync(drug.DrugId)!;
        }

        public async Task<bool> UpdateDrug(int drugId, UpdateDrugDTO request)
        {
            // 1. Verify drug exists
            var existing = await _drugRepository.GetByIdAsync(drugId);
            if (existing == null) return false;

            // 2. Duplicate check (exclude self)
            var isDuplicate = await _drugRepository.IsDrugExists(
                request.GenericName, request.Strength, request.Form, drugId);
            if (isDuplicate)
                throw new InvalidOperationException("DRUG_DUPLICATE");

            // 3. Map properties
            existing.GenericName = request.GenericName;
            existing.BrandName = request.BrandName;
            existing.Strength = request.Strength;
            existing.Form = request.Form;
            existing.Atccode = request.Atccode;
            existing.ControlClass = request.ControlClass;
            existing.StorageClass = request.StorageClass;
            existing.Status = request.Status;

            return await _drugRepository.UpdateAsync(existing);
        }

        public async Task<DrugDeletedResponseDTO> DeleteDrug(int drugId)
        {
            return await _drugRepository.DeleteDrug(drugId);
        }
    }
}
