using PharmaStock.Core.DTO.Auth;
using PharmaStock.Core.DTO.Drug;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces
{
    public interface IDrugRepository : IGenericRepository<Drug>
    {
        Task<GetDrugDTO?> GetDrugDtoByIdAsync(int id);
        Task<(List<GetDrugDTO>, int)> GetDrugsByFilterAsync(DrugFilterDTO filter);
        Task<bool> IsDrugExists(string genericName, string strength, int form, int? excludeId = null);
        Task<DrugDeletedResponseDTO> DeleteDrug(int drugId);
    }
}
