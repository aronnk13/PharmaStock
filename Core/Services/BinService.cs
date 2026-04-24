using PharmaStock.Core.DTO.Bin;
using PharmaStock.Core.DTO.Common;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class BinService : IBinService
    {
        private readonly IBinRepository _binRepository;

        public BinService(IBinRepository binRepository)
        {
            _binRepository = binRepository;
        }

        public async Task<GetBinDTO> CreateBinAsync(CreateBinDTO request)
        {
            // 1. Validate Location exists and is active
            var location = await _binRepository.GetLocationByIdAsync(request.LocationId);
            if (location == null)
                throw new KeyNotFoundException("LOCATION_NOT_FOUND");
            if (!location.StatusId)
                throw new InvalidOperationException("LOCATION_INACTIVE");

            // 2. Validate StorageClass exists
            var storageClass = await _binRepository.GetBinStorageClassByIdAsync(request.BinStorageClassId);
            if (storageClass == null)
                throw new KeyNotFoundException("STORAGE_CLASS_NOT_FOUND");

            // 3. Bin code must be unique within the location
            var isDuplicate = await _binRepository.IsBinCodeExistsInLocation(request.LocationId, request.Code);
            if (isDuplicate)
                throw new InvalidOperationException("BIN_CODE_DUPLICATE");

            // 4. Map DTO → Entity
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

            return new GetBinDTO
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
        }

        public async Task<GetBinDTO> UpdateBinAsync(int binId, UpdateBinDTO request)
        {
            // 1. Bin must exist
            var bin = await _binRepository.GetByIdAsync(binId);
            if (bin == null)
                throw new KeyNotFoundException("BIN_NOT_FOUND");

            // 2. Code change — must be unique within the same location
            if (request.Code != null && request.Code != bin.Code)
            {
                var isDuplicate = await _binRepository.IsBinCodeExistsInLocation(bin.LocationId, request.Code, binId);
                if (isDuplicate)
                    throw new InvalidOperationException("BIN_CODE_DUPLICATE");

                bin.Code = request.Code;
            }

            // 3. Storage class change — block if bin has inventory
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

            // 4. Removing quarantine flag — block if bin still has stock
            if (request.IsQuarantine.HasValue && bin.IsQuarantine && !request.IsQuarantine.Value)
            {
                var hasInventory = await _binRepository.HasInventoryAsync(binId);
                if (hasInventory)
                    throw new InvalidOperationException("QUARANTINE_HAS_STOCK");
            }

            if (request.IsQuarantine.HasValue)
                bin.IsQuarantine = request.IsQuarantine.Value;

            // 5. Deactivating bin — block if inventory or open put-away tasks exist
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

            return await _binRepository.GetBinDtoByIdAsync(binId)!;
        }

        public async Task<GetBinDTO> DeleteBinAsync(int binId)
        {
            // 1. Bin must exist
            var bin = await _binRepository.GetByIdAsync(binId);
            if (bin == null)
                throw new KeyNotFoundException("BIN_NOT_FOUND");

            // 2. Block if inventory exists
            var hasInventory = await _binRepository.HasInventoryAsync(binId);
            if (hasInventory)
                throw new InvalidOperationException("BIN_HAS_INVENTORY");

            // 3. Block if open put-away tasks exist
            var hasOpenTasks = await _binRepository.HasOpenPutAwayTasksAsync(binId);
            if (hasOpenTasks)
                throw new InvalidOperationException("BIN_OPEN_TASKS");

            // Snapshot for audit before deleting
            var snapshot = await _binRepository.GetBinDtoByIdAsync(binId);

            await _binRepository.DeleteAsync(binId);

            return snapshot!;
        }

        public async Task<GetBinDTO?> GetBinByIdAsync(int binId)
        {
            return await _binRepository.GetBinDtoByIdAsync(binId);
        }

        public async Task<PaginatedResult<GetBinDTO>> GetAllBinsAsync(BinFilterDTO filter)
        {
            var (bins, totalCount) = await _binRepository.GetBinsByFilterAsync(filter);

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
