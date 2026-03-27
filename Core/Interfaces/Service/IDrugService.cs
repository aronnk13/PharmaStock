using System;
using PharmaStock.Core.DTO.Auth;


namespace PharmaStock.Core.Interfaces
{
    public interface IDrugService
    {
        Task<DrugDeletedResponseDTO> DeleteDrug(int DrugId);
    }
}