using System.Text.Json;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Location;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocationService(ILocationRepository locationRepository, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _locationRepository = locationRepository;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId() =>
            int.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value, out var id) ? id : 0;

        public async Task<GetLocationDTO?> GetLocationbyId(int id)
        {
            var location = await _locationRepository.GetLocationById(id);
            if (location == null) return null;

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "LOCATION_VIEWED",
                Resource = $"Location:{id}",
                Metadata = null
            });

            return MapToGetDTO(location);
        }

        public async Task<IEnumerable<GetLocationDTO>> GetLocations()
        {
            var locations = await _locationRepository.GetLocations();

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "LOCATION_LIST_VIEWED",
                Resource = "Location:list",
                Metadata = null
            });

            return locations.Select(MapToGetDTO);
        }

        public async Task<GetLocationDTO> CreateLocation(CreateLocationDTO dto)
        {
            var isDuplicate = await _locationRepository.IsLocationExists(dto.Name, dto.LocationTypeId);
            if (isDuplicate) throw new InvalidOperationException("LOCATION_DUPLICATE");

            await ValidateHierarchy(dto.LocationTypeId, dto.ParentLocationId);

            var location = new Location
            {
                Name = dto.Name,
                LocationTypeId = dto.LocationTypeId,
                ParentLocationId = dto.ParentLocationId,
                StatusId = dto.StatusId
            };

            var created = await _locationRepository.CreateLocation(location);
            var result = MapToGetDTO(created);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "LOCATION_CREATED",
                Resource = $"Location:{created.LocationId}",
                Metadata = JsonSerializer.Serialize(result)
            });

            return result;
        }

        public async Task<bool> UpdateLocation(UpdateLocationDTO dto)
        {
            var existing = await _locationRepository.GetLocationById(dto.LocationId);
            if (existing == null) return false;

            var isDuplicate = await _locationRepository.IsLocationExists(dto.Name, dto.LocationTypeId, dto.LocationId);
            if (isDuplicate) throw new InvalidOperationException("LOCATION_DUPLICATE");

            if (existing.LocationTypeId != dto.LocationTypeId)
            {
                var hasChildren = await _locationRepository.HasChildLocations(dto.LocationId);
                if (hasChildren) throw new InvalidOperationException("LOCATION_TYPE_CHANGE_HAS_CHILDREN");
            }

            await ValidateHierarchy(dto.LocationTypeId, dto.ParentLocationId);

            existing.Name = dto.Name;
            existing.LocationTypeId = dto.LocationTypeId;
            existing.ParentLocationId = dto.ParentLocationId;
            existing.StatusId = dto.StatusId;

            var success = await _locationRepository.UpdateLocation(existing);

            if (success)
            {
                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "LOCATION_UPDATED",
                    Resource = $"Location:{dto.LocationId}",
                    Metadata = JsonSerializer.Serialize(dto)
                });
            }

            return success;
        }

        public async Task<bool> DeleteLocation(int id)
        {
            var existing = await _locationRepository.GetLocationById(id);
            if (existing == null) return false;

            var typeName = await _locationRepository.GetLocationTypeName(existing.LocationTypeId);

            if (typeName == "MainStore")
            {
                var hasSubStores = await _locationRepository.HasChildLocations(id);
                if (hasSubStores) throw new InvalidOperationException("MAINSTORE_HAS_SUBSTORES");
            }
            else if (typeName == "SubStore")
            {
                var hasChildLocations = await _locationRepository.HasChildLocations(id);
                if (hasChildLocations) throw new InvalidOperationException("SUBSTORE_HAS_CHILD_LOCATIONS");
            }

            var success = await _locationRepository.DeleteLocation(id);

            if (success)
            {
                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "LOCATION_DELETED",
                    Resource = $"Location:{id}",
                    Metadata = null
                });
            }

            return success;
        }

        public async Task<IEnumerable<GetLocationTypeDTO>> GetAllLocationTypes()
        {
            var types = await _locationRepository.GetAllLocationTypesAsync();

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "LOCATION_TYPE_LIST_VIEWED",
                Resource = "LocationType:list",
                Metadata = null
            });

            return types.Select(t => new GetLocationTypeDTO { LocationTypeId = t.LocationTypeId, Type = t.Type });
        }

        public async Task<GetLocationTypeDTO?> GetLocationTypeById(int id)
        {
            var type = await _locationRepository.GetLocationTypeByIdAsync(id);
            if (type == null) return null;

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "LOCATION_TYPE_VIEWED",
                Resource = $"LocationType:{id}",
                Metadata = null
            });

            return new GetLocationTypeDTO { LocationTypeId = type.LocationTypeId, Type = type.Type };
        }

        private async System.Threading.Tasks.Task ValidateHierarchy(int locationTypeId, int? parentLocationId)
        {
            var typeName = await _locationRepository.GetLocationTypeName(locationTypeId);
            if (typeName == null) throw new InvalidOperationException("INVALID_LOCATION_TYPE");

            switch (typeName)
            {
                case "MainStore":
                    if (parentLocationId != null)
                        throw new InvalidOperationException("MAINSTORE_NO_PARENT");
                    break;

                case "SubStore":
                    if (parentLocationId == null)
                        throw new InvalidOperationException("SUBSTORE_NEEDS_PARENT");
                    var subStoreParent = await _locationRepository.GetLocationById(parentLocationId.Value);
                    if (subStoreParent == null)
                        throw new InvalidOperationException("PARENT_NOT_FOUND");
                    var subStoreParentType = await _locationRepository.GetLocationTypeName(subStoreParent.LocationTypeId);
                    if (subStoreParentType != "MainStore")
                        throw new InvalidOperationException("SUBSTORE_PARENT_MUST_BE_MAINSTORE");
                    break;

                default:
                    // All types other than MainStore and SubStore (Ward, ICU, OR, Pharmacy, etc.)
                    // must be placed directly under a SubStore
                    if (parentLocationId == null)
                        throw new InvalidOperationException("LOCATION_NEEDS_PARENT");
                    var parent = await _locationRepository.GetLocationById(parentLocationId.Value);
                    if (parent == null)
                        throw new InvalidOperationException("PARENT_NOT_FOUND");
                    var parentTypeName = await _locationRepository.GetLocationTypeName(parent.LocationTypeId);
                    if (parentTypeName != "SubStore")
                        throw new InvalidOperationException("LOCATION_PARENT_MUST_BE_SUBSTORE");
                    break;
            }
        }

        private static GetLocationDTO MapToGetDTO(Location location) => new GetLocationDTO
        {
            LocationId = location.LocationId,
            Name = location.Name,
            LocationTypeId = location.LocationTypeId,
            ParentLocationId = location.ParentLocationId,
            StatusId = location.StatusId
        };
    }
}
