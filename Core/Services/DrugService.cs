using System.Text.Json;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Common;
using PharmaStock.Core.DTO.Auth;
using PharmaStock.Core.DTO.Drug;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class DrugService : IDrugService
    {
        private readonly IDrugRepository _drugRepository;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DrugService(IDrugRepository drugRepository, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _drugRepository = drugRepository;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId() =>
            int.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value, out var id) ? id : 0;

        public async Task<GetDrugDTO?> GetDrugbyid(int id)
        {
            var result = await _drugRepository.GetDrugDtoByIdAsync(id);
            if (result == null) return null;

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "DRUG_VIEWED",
                Resource = $"Drug:{id}",
                Metadata = null
            });

            return result;
        }

        public async Task<PaginatedResult<GetDrugDTO>> GetPaginatedResult(DrugFilterDTO filter)
        {
            var (drugs, totalCount) = await _drugRepository.GetDrugsByFilterAsync(filter);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "DRUG_LIST_VIEWED",
                Resource = "Drug:list",
                Metadata = null
            });

            return new PaginatedResult<GetDrugDTO>
            {
                Items = drugs,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize > 100 ? 100 : filter.PageSize
            };
        }

        public async Task<GetDrugDTO> CreateDrug(CreateDrugDTO request)
        {
            var isDuplicate = await _drugRepository.IsDrugExists(request.GenericName, request.Strength, request.Form);
            if (isDuplicate)
                throw new InvalidOperationException("DRUG_DUPLICATE");

            var drug = new Drug
            {
                GenericName = request.GenericName,
                BrandName = request.BrandName,
                Strength = request.Strength,
                Form = request.Form,
                Atccode = request.Atccode,
                StorageClass = request.StorageClass,
                ControlClass = request.ControlClass,
                Status = request.Status
            };

            await _drugRepository.AddAsync(drug);

            var result = await _drugRepository.GetDrugDtoByIdAsync(drug.DrugId)!;

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "DRUG_CREATED",
                Resource = $"Drug:{drug.DrugId}",
                Metadata = JsonSerializer.Serialize(result)
            });

            return result;
        }

        public async Task<bool> UpdateDrug(int drugId, UpdateDrugDTO request)
        {
            var existing = await _drugRepository.GetByIdAsync(drugId);
            if (existing == null) return false;

            var isDuplicate = await _drugRepository.IsDrugExists(request.GenericName, request.Strength, request.Form, drugId);
            if (isDuplicate)
                throw new InvalidOperationException("DRUG_DUPLICATE");

            existing.GenericName = request.GenericName;
            existing.BrandName = request.BrandName;
            existing.Strength = request.Strength;
            existing.Form = request.Form;
            existing.Atccode = request.Atccode;
            existing.ControlClass = request.ControlClass;
            existing.StorageClass = request.StorageClass;
            existing.Status = request.Status;

            var success = await _drugRepository.UpdateAsync(existing);

            if (success)
            {
                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "DRUG_UPDATED",
                    Resource = $"Drug:{drugId}",
                    Metadata = JsonSerializer.Serialize(request)
                });
            }

            return success;
        }

        public async Task<DrugDeletedResponseDTO> DeleteDrug(int drugId)
        {
            var response = await _drugRepository.DeleteDrug(drugId);

            if (response.IsDeleted)
            {
                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "DRUG_DELETED",
                    Resource = $"Drug:{drugId}",
                    Metadata = null
                });
            }

            return response;
        }
    }
}
