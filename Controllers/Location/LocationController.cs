using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Location;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Location
{
    [ApiController]
    [Authorize]
    [Route("api/locations")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _locationService.GetLocations());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var location = await _locationService.GetLocationbyId(id);
            if (location == null)
                return NotFound(new { errorCode = "LOCATION_NOT_FOUND", message = $"Location with ID {id} not found." });
            return Ok(location);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateLocationDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _locationService.CreateLocation(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.LocationId }, created);
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message switch
                {
                    "LOCATION_DUPLICATE" => Conflict(new { errorCode = ex.Message, message = "A location with this name and type already exists." }),
                    "INVALID_LOCATION_TYPE" => BadRequest(new { errorCode = ex.Message, message = "Invalid location type." }),
                    "MAINSTORE_NO_PARENT" => BadRequest(new { errorCode = ex.Message, message = "MainStore cannot have a parent location." }),
                    "SUBSTORE_NEEDS_PARENT" => BadRequest(new { errorCode = ex.Message, message = "SubStore must have a parent MainStore." }),
                    "SUBSTORE_PARENT_MUST_BE_MAINSTORE" => BadRequest(new { errorCode = ex.Message, message = "SubStore's parent must be a MainStore." }),
                    "LOCATION_NEEDS_PARENT" => BadRequest(new { errorCode = ex.Message, message = "This location type must have a parent SubStore." }),
                    "LOCATION_PARENT_MUST_BE_SUBSTORE" => BadRequest(new { errorCode = ex.Message, message = "Parent location must be a SubStore." }),
                    "PARENT_NOT_FOUND" => BadRequest(new { errorCode = ex.Message, message = "Parent location not found." }),
                    _ => BadRequest(new { errorCode = ex.Message, message = ex.Message })
                };
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLocationDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.LocationId)
                return BadRequest(new { message = "Route ID does not match body LocationId." });

            try
            {
                var updated = await _locationService.UpdateLocation(dto);
                if (!updated)
                    return NotFound(new { errorCode = "LOCATION_NOT_FOUND", message = $"Location with ID {id} not found." });
                return Ok(new { message = "Location updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message switch
                {
                    "LOCATION_DUPLICATE" => Conflict(new { errorCode = ex.Message, message = "A location with this name and type already exists." }),
                    "LOCATION_TYPE_CHANGE_HAS_CHILDREN" => Conflict(new { errorCode = ex.Message, message = "Cannot change location type because this location has child locations." }),
                    "INVALID_LOCATION_TYPE" => BadRequest(new { errorCode = ex.Message, message = "Invalid location type." }),
                    "MAINSTORE_NO_PARENT" => BadRequest(new { errorCode = ex.Message, message = "MainStore cannot have a parent location." }),
                    "SUBSTORE_NEEDS_PARENT" => BadRequest(new { errorCode = ex.Message, message = "SubStore must have a parent MainStore." }),
                    "SUBSTORE_PARENT_MUST_BE_MAINSTORE" => BadRequest(new { errorCode = ex.Message, message = "SubStore's parent must be a MainStore." }),
                    "LOCATION_NEEDS_PARENT" => BadRequest(new { errorCode = ex.Message, message = "This location type must have a parent SubStore." }),
                    "LOCATION_PARENT_MUST_BE_SUBSTORE" => BadRequest(new { errorCode = ex.Message, message = "Parent location must be a SubStore." }),
                    "PARENT_NOT_FOUND" => BadRequest(new { errorCode = ex.Message, message = "Parent location not found." }),
                    _ => BadRequest(new { errorCode = ex.Message, message = ex.Message })
                };
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _locationService.DeleteLocation(id);
                if (!deleted)
                    return NotFound(new { errorCode = "LOCATION_NOT_FOUND", message = $"Location with ID {id} not found." });
                return Ok(new { message = "Location deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message switch
                {
                    "MAINSTORE_HAS_SUBSTORES" => Conflict(new { errorCode = ex.Message, message = "Cannot delete MainStore that has SubStores." }),
                    "SUBSTORE_HAS_CHILD_LOCATIONS" => Conflict(new { errorCode = ex.Message, message = "Cannot delete SubStore that has child locations." }),
                    _ => BadRequest(new { errorCode = ex.Message, message = ex.Message })
                };
            }
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetAllLocationTypes()
        {
            return Ok(await _locationService.GetAllLocationTypes());
        }

        [HttpGet("types/{id}")]
        public async Task<IActionResult> GetLocationTypeById(int id)
        {
            var type = await _locationService.GetLocationTypeById(id);
            if (type == null)
                return NotFound(new { errorCode = "LOCATION_TYPE_NOT_FOUND", message = $"Location type with ID {id} not found." });
            return Ok(type);
        }
    }
}
