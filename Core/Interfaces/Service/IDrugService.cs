using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO.Common;
using PharmaStock.Core.DTO.Drug;
using PharmaStock.Core.DTO.Auth;
using PharmaStock.Models;


namespace PharmaStock.Core.Interfaces.Service
{
    public interface IDrugService
    {
        public Task<GetDrugDTO> GetDrugbyid(int id);
        public Task<PaginatedResult<GetDrugDTO>> GetPaginatedResult(DrugFilterDTO filter);
        public Task<Drug> CreateDrug(CreateDrugDTO dto);
        public Task<bool> UpdateDrug(int drugId,UpdateDrugDTO dto);
        Task<DrugDeletedResponseDTO> DeleteDrug(int DrugId);
    }
}