using PharmaStock.Core.DTO.Auth;
using PharmaStock.Core.DTO.Common;
using PharmaStock.Core.DTO.Drug;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IDrugService
    {
        Task<GetDrugDTO?> GetDrugbyid(int id);
        Task<PaginatedResult<GetDrugDTO>> GetPaginatedResult(DrugFilterDTO filter);
        Task<GetDrugDTO> CreateDrug(CreateDrugDTO dto);
        Task<bool> UpdateDrug(int drugId, UpdateDrugDTO dto);
        Task<DrugDeletedResponseDTO> DeleteDrug(int drugId);
    }
}
