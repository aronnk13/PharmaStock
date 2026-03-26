using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO.Auth;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IDrugRepository
    {
        Task<DrugDeletedResponseDTO> DeleteDrug(int DrugId);    
    }
}