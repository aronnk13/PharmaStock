using PharmaStock.Core.DTO.Location;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<GetLocationDTO?> GetLocationbyId(int id)
        {
            var location = await _locationRepository.GetLocationById(id);
            if (location == null) return null;
            return MapToGetDTO(location);
        }

        public async Task<IEnumerable<GetLocationDTO>> GetLocations()
        {
            var locations = await _locationRepository.GetLocations();
            return locations.Select(MapToGetDTO);
        }

        public async Task<GetLocationDTO> CreateLocation(CreateLocationDTO dto)
        {
            // Duplicate check: same name + type combination
            var isDuplicate = await _locationRepository.IsLocationExists(dto.Name, dto.LocationTypeId);
            if (isDuplicate) throw new InvalidOperationException("LOCATION_DUPLICATE");

            // Hierarchy validation
            await ValidateHierarchy(dto.LocationTypeId, dto.ParentLocationId);

            var location = new Location
            {
                Name = dto.Name,
                LocationTypeId = dto.LocationTypeId,
                ParentLocationId = dto.ParentLocationId,
                StatusId = dto.StatusId
            };

            var created = await _locationRepository.CreateLocation(location);
            return MapToGetDTO(created);
        }

        public async Task<bool> UpdateLocation(UpdateLocationDTO dto)
        {
            // Existence check
            var existing = await _locationRepository.GetLocationById(dto.LocationId);
            if (existing == null) return false;

            // Duplicate check excluding the current record
            var isDuplicate = await _locationRepository.IsLocationExists(dto.Name, dto.LocationTypeId, dto.LocationId);
            if (isDuplicate) throw new InvalidOperationException("LOCATION_DUPLICATE");

            // Hierarchy validation
            await ValidateHierarchy(dto.LocationTypeId, dto.ParentLocationId);

            existing.Name = dto.Name;
            existing.LocationTypeId = dto.LocationTypeId;
            existing.ParentLocationId = dto.ParentLocationId;
            existing.StatusId = dto.StatusId;

            return await _locationRepository.UpdateLocation(existing);
        }

        public async Task<bool> DeleteLocation(int id)
        {
            // Existence check
            var existing = await _locationRepository.GetLocationById(id);
            if (existing == null) return false;

            // Type-aware child validation
            var typeName = await _locationRepository.GetLocationTypeName(existing.LocationTypeId);

            if (typeName == "MainStore")
            {
                var hasSubStores = await _locationRepository.HasChildLocations(id);
                if (hasSubStores)
                    throw new InvalidOperationException("MAINSTORE_HAS_SUBSTORES");
            }
            else if (typeName == "SubStore")
            {
                var hasChildLocations = await _locationRepository.HasChildLocations(id);
                if (hasChildLocations)
                    throw new InvalidOperationException("SUBSTORE_HAS_CHILD_LOCATIONS");
            }

            return await _locationRepository.DeleteLocation(id);
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

                case "OR":
                case "ICU":
                case "Ward":
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
