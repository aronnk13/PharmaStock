using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO.Common;
using PharmaStock.Core.DTO.Drug;
using PharmaStock.Models;
using PharmaStock.Core.DTO.Auth;

namespace PharmaStock.Core.Interfaces
{
    public interface IDrugRepository : IGenericRepository<Drug>
    {
        public Task<(List<Drug>, int)> GetDrugsByFilterAsync(DrugFilterDTO filter);
        Task<bool> IsDrugExists(string genericName, string strength, int form, int? excludeId = null);
        Task<DrugDeletedResponseDTO> DeleteDrug(int DrugId);
    }
}