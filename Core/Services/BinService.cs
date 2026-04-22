using System.Text.Json;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Bin;
using PharmaStock.Core.DTO.Common;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class BinService : IBinService
    {
        private readonly IBinRepository _binRepository;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BinService(IBinRepository binRepository, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _binRepository = binRepository;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId() =>
            int.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value, out var id) ? id : 0;

        public async Task<GetBinDTO> CreateBinAsync(CreateBinDTO request)
        {
            var location = await _binRepository.GetLocationByIdAsync(request.LocationId);
            if (location == null)
                throw new KeyNotFoundException("LOCATION_NOT_FOUND");
            if (!location.StatusId)
                throw new InvalidOperationException("LOCATION_INACTIVE");

            var storageClass = await _binRepository.GetBinStorageClassByIdAsync(request.BinStorageClassId);
            if (storageClass == null)
                throw new KeyNotFoundException("STORAGE_CLASS_NOT_FOUND");

            var isDuplicate = await _binRepository.IsBinCodeExistsInLocation(request.LocationId, request.Code);
            if (isDuplicate)
                throw new InvalidOperationException("BIN_CODE_DUPLICATE");

            var bin = new Bin
            {
                LocationId = request.LocationId,
                Code = request.Code,
                BinStorageClass = request.BinStorageClassId,
                IsQuarantine = request.IsQuarantine,
                MaxCapacity = request.MaxCapacity,
                StatusId = request.IsActive
            };

            await _binRepository.AddAsync(bin);

            var result = new GetBinDTO
            {
                BinId = bin.BinId,
                LocationId = bin.LocationId,
                LocationName = location.Name,
                Code = bin.Code,
                BinStorageClassId = bin.BinStorageClass,
                StorageClass = storageClass.StorageClass,
                IsQuarantine = bin.IsQuarantine,
                MaxCapacity = bin.MaxCapacity,
                IsActive = bin.StatusId
            };

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "BIN_CREATED",
                Resource = $"Bin:{bin.BinId}",
                Metadata = JsonSerializer.Serialize(result)
            });

            return result;
        }

        public async Task<GetBinDTO> UpdateBinAsync(int binId, UpdateBinDTO request)
        {
            var bin = await _binRepository.GetByIdAsync(binId);
            if (bin == null)
                throw new KeyNotFoundException("BIN_NOT_FOUND");

            // 2. Code change
            if (request.Code != null && request.Code != bin.Code)
            {
                var isDuplicate = await _binRepository.IsBinCodeExistsInLocation(bin.LocationId, request.Code, binId);
                if (isDuplicate)
                    throw new InvalidOperationException("BIN_CODE_DUPLICATE");
                bin.Code = request.Code;
            }

            // 3. Storage class change
            if (request.BinStorageClassId.HasValue && request.BinStorageClassId.Value != bin.BinStorageClass)
            {
                var hasInventory = await _binRepository.HasInventoryAsync(binId);
                if (hasInventory)
                    throw new InvalidOperationException("BIN_HAS_INVENTORY");

                var storageClass = await _binRepository.GetBinStorageClassByIdAsync(request.BinStorageClassId.Value);
                if (storageClass == null)
                    throw new KeyNotFoundException("STORAGE_CLASS_NOT_FOUND");

                bin.BinStorageClass = request.BinStorageClassId.Value;
            }

            // 4. Removing quarantine flag
            if (request.IsQuarantine.HasValue && bin.IsQuarantine && !request.IsQuarantine.Value)
            {
                var hasInventory = await _binRepository.HasInventoryAsync(binId);
                if (hasInventory)
                    throw new InvalidOperationException("QUARANTINE_HAS_STOCK");
            }

            if (request.IsQuarantine.HasValue)
                bin.IsQuarantine = request.IsQuarantine.Value;

            // 5. Deactivating bin
            if (request.IsActive.HasValue && !request.IsActive.Value)
            {
                var hasInventory = await _binRepository.HasInventoryAsync(binId);
                if (hasInventory)
                    throw new InvalidOperationException("BIN_HAS_INVENTORY");

                var hasOpenTasks = await _binRepository.HasOpenPutAwayTasksAsync(binId);
                if (hasOpenTasks)
                    throw new InvalidOperationException("BIN_OPEN_TASKS");
            }

            if (request.IsActive.HasValue)
                bin.StatusId = request.IsActive.Value;

            if (request.MaxCapacity.HasValue)
                bin.MaxCapacity = request.MaxCapacity.Value;

            await _binRepository.UpdateAsync(bin);

            var result = await _binRepository.GetBinDtoByIdAsync(binId)!;

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "BIN_UPDATED",
                Resource = $"Bin:{binId}",
                Metadata = JsonSerializer.Serialize(request)
            });

            return result;
        }

        public async Task<GetBinDTO> DeleteBinAsync(int binId)
        {
            var bin = await _binRepository.GetByIdAsync(binId);
            if (bin == null)
                throw new KeyNotFoundException("BIN_NOT_FOUND");

            if (!bin.StatusId)
                throw new InvalidOperationException("BIN_ALREADY_DELETED");

            var hasInventory = await _binRepository.HasInventoryAsync(binId);
            if (hasInventory)
                throw new InvalidOperationException("BIN_HAS_INVENTORY");

            var hasOpenTasks = await _binRepository.HasOpenPutAwayTasksAsync(binId);
            if (hasOpenTasks)
                throw new InvalidOperationException("BIN_OPEN_TASKS");

            bin.StatusId = false;
            await _binRepository.UpdateAsync(bin);

            var result = await _binRepository.GetBinDtoByIdAsync(binId)!;

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "BIN_DELETED",
                Resource = $"Bin:{binId}",
                Metadata = null
            });

            return result;
        }

        public async Task<GetBinDTO?> GetBinByIdAsync(int binId)
        {
            var result = await _binRepository.GetBinDtoByIdAsync(binId);
            if (result == null) return null;

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "BIN_VIEWED",
                Resource = $"Bin:{binId}",
                Metadata = null
            });

            return result;
        }

        public async Task<PaginatedResult<GetBinDTO>> GetAllBinsAsync(BinFilterDTO filter)
        {
            var (bins, totalCount) = await _binRepository.GetBinsByFilterAsync(filter);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "BIN_LIST_VIEWED",
                Resource = "Bin:list",
                Metadata = null
            });

            return new PaginatedResult<GetBinDTO>
            {
                Items = bins,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize > 100 ? 100 : filter.PageSize
            };
        }
    }
}
