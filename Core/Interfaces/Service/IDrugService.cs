using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO.Common;
using PharmaStock.Core.DTO.Drug;
using PharmaStock.Core.DTO.Auth;


namespace PharmaStock.Core.Interfaces.Service
{
    public interface IDrugService
    {
        public Task<GetDrugDTO> GetDrugbyid(int id);
        public Task<PaginatedResult<GetDrugDTO>> GetPaginatedResult(DrugFilterDTO filter);
        Task<DrugDeletedResponseDTO> DeleteDrug(int DrugId);
    }
}