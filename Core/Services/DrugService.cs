using System;
using System.Runtime.CompilerServices;
using Microsoft.IdentityModel.Tokens;
using PharmaStock.Core.DTO.Auth;
using PharmaStock.Core.DTO.Common;
using PharmaStock.Core.DTO.Drug;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Infrastructure.Repositories;
using PharmaStock.Core.Interfaces.Repository;


namespace PharmaStock.Core.Services
{
    public class DrugService: IDrugService
    {
        private readonly IDrugRepository _drugRepository;
        public DrugService(IDrugRepository drugRepository)
        {
            _drugRepository = drugRepository;
        }

        public async Task<GetDrugDTO> GetDrugbyid(int id)
        {
            var drugModel = await _drugRepository.GetByIdAsync(id);
            if (drugModel == null)
            {
                return null;
            }
            return new GetDrugDTO
            {
                //Id = drugModel.DrugId,
                Atccode = drugModel.Atccode,
                BrandName = drugModel.BrandName,
                ControlClass = drugModel.ControlClass,
                StorageClass = drugModel.StorageClass,
                Status = drugModel.Status
            };
        }

        public async Task<PaginatedResult<GetDrugDTO>> GetPaginatedResult(DrugFilterDTO filter)
        {
            var (drugs, totalCount) = await _drugRepository.GetDrugsByFilterAsync(filter);
            var dtoList = drugs.Select(a => new GetDrugDTO
            {
                Atccode = a.Atccode,
                BrandName = a.BrandName,
                ControlClass = a.ControlClass,
                StorageClass = a.StorageClass,
                Status = a.Status
            }).ToList();
            return new PaginatedResult<GetDrugDTO>
            {
                Items = dtoList,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }
        
        public async Task<DrugDeletedResponseDTO> DeleteDrug(int DrugId)
        {
            return await _drugRepository.DeleteDrug(DrugId);
        }
    }
}