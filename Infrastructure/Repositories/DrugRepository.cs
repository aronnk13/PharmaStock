using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.Auth;
using PharmaStock.Core.DTO.Drug;
using PharmaStock.Core.Interfaces;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class DrugRepository : GenericRepository<Drug>, IDrugRepository
    {
        private readonly PharmaStockContext _pStockContext;

        public DrugRepository(PharmaStockContext pharmaStockContext)
            : base(pharmaStockContext)
        {
            _pStockContext = pharmaStockContext;
        }

        public async Task<GetDrugDTO?> GetDrugDtoByIdAsync(int id)
        {
            return await _pStockContext.Drugs
                .Where(d => d.DrugId == id)
                .Select(d => new GetDrugDTO
                {
                    DrugId = d.DrugId,
                    GenericName = d.GenericName,
                    BrandName = d.BrandName,
                    Strength = d.Strength,
                    FormId = d.Form,
                    FormName = d.FormNavigation.Form,
                    Atccode = d.Atccode,
                    ControlClassId = d.ControlClass,
                    ControlClassName = d.ControlClassNavigation.Class,
                    StorageClassId = d.StorageClass,
                    StorageClassName = d.StorageClassNavigation.Class,
                    Status = d.Status
                })
                .FirstOrDefaultAsync();
        }

        public async Task<(List<GetDrugDTO>, int)> GetDrugsByFilterAsync(DrugFilterDTO filter)
        {
            var query = _pStockContext.Drugs.AsQueryable();

            if (filter.GenericName != null)
                query = query.Where(d => d.GenericName.Contains(filter.GenericName));

            if (filter.StorageClass != null)
                query = query.Where(d => d.StorageClass == filter.StorageClass);

            if (filter.ControlClass != null)
                query = query.Where(d => d.ControlClass == filter.ControlClass);

            if (filter.Status != null)
                query = query.Where(d => d.Status == filter.Status);

            var totalCount = await query.CountAsync();

            var pageSize = filter.PageSize > 100 ? 100 : filter.PageSize;

            var drugs = await query
                .Skip((filter.Page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new GetDrugDTO
                {
                    DrugId = d.DrugId,
                    GenericName = d.GenericName,
                    BrandName = d.BrandName,
                    Strength = d.Strength,
                    FormId = d.Form,
                    FormName = d.FormNavigation.Form,
                    Atccode = d.Atccode,
                    ControlClassId = d.ControlClass,
                    ControlClassName = d.ControlClassNavigation.Class,
                    StorageClassId = d.StorageClass,
                    StorageClassName = d.StorageClassNavigation.Class,
                    Status = d.Status
                })
                .ToListAsync();

            return (drugs, totalCount);
        }

        public async Task<bool> IsDrugExists(string name, string strength, int form, int? excludeId = null)
        {
            return await _pStockContext.Drugs.AnyAsync(d =>
                d.GenericName == name &&
                d.Strength == strength &&
                d.Form == form &&
                (!excludeId.HasValue || d.DrugId != excludeId.Value));
        }

        public async Task<DrugDeletedResponseDTO> DeleteDrug(int drugId)
        {
            try
            {
                var drug = await _pStockContext.Drugs.FindAsync(drugId);
                if (drug == null)
                {
                    return new DrugDeletedResponseDTO
                    {
                        IsDeleted = false,
                        Message = "Drug not found."
                    };
                }

                // 2. Remove from database
                _pStockContext.Drugs.Remove(drug);

                var rowsAffected = await _pStockContext.SaveChangesAsync();

                if (rowsAffected > 0)
                    return new DrugDeletedResponseDTO { IsDeleted = true, Message = "Drug deleted successfully." };

                return new DrugDeletedResponseDTO { IsDeleted = false, Message = "Delete failed: no changes saved." };
            }
            catch (Exception ex)
            {
                return new DrugDeletedResponseDTO { IsDeleted = false, Message = $"An error occurred: {ex.Message}" };
            }
        }
    }
}
