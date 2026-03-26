using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using PharmaStock.Core.DTO.Auth;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;

namespace PharmaStock.Core.Services
{
    public class DrugService : IDrugService
    {
        private readonly IDrugRepository _drugRepository;

        public DrugService(IDrugRepository drugRepository)
        {
            _drugRepository = drugRepository;
        }
        public async Task<DrugDeletedResponseDTO> DeleteDrug(int DrugId)
        {
            return await _drugRepository.DeleteDrug(DrugId);
        }
    }
}