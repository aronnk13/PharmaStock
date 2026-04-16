using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Location;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Location
{
    [ApiController]
    [Route("api/v1/locations")]
    [Authorize(Roles = "Admin")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLocations()
        {
            var locations = await _locationService.GetLocations();
            return Ok(locations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var location = await _locationService.GetLocationbyId(id);
            if (location == null)
                return NotFound(new { errorCode = "LOCATION_NOT_FOUND", message = "Location not found." });
            return Ok(location);
        }

        [HttpPost]
        [Route("CreateLocation")]
        public async Task<IActionResult> CreateLocation([FromBody] CreateLocationDTO request)
        {
            if (request == null)
                return BadRequest(new { message = "Location data is required." });
            try
            {
                var result = await _locationService.CreateLocation(request);
                return CreatedAtAction(nameof(GetLocationById), new { id = result.LocationId }, result);
            }
            catch (InvalidOperationException ex) when (ex.Message == "LOCATION_DUPLICATE")
            {
                return Conflict(new { errorCode = "LOCATION_DUPLICATE", message = "A location with this name and type already exists." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "INVALID_LOCATION_TYPE")
            {
                return BadRequest(new { errorCode = "INVALID_LOCATION_TYPE", message = "The specified location type does not exist." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "MAINSTORE_NO_PARENT")
            {
                return BadRequest(new { errorCode = "MAINSTORE_NO_PARENT", message = "MainStore cannot have a parent location." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "SUBSTORE_NEEDS_PARENT")
            {
                return BadRequest(new { errorCode = "SUBSTORE_NEEDS_PARENT", message = "SubStore must have a parent MainStore." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "SUBSTORE_PARENT_MUST_BE_MAINSTORE")
            {
                return BadRequest(new { errorCode = "SUBSTORE_PARENT_MUST_BE_MAINSTORE", message = "SubStore's parent must be a MainStore." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "LOCATION_NEEDS_PARENT")
            {
                return BadRequest(new { errorCode = "LOCATION_NEEDS_PARENT", message = "OR, ICU, and Ward must have a parent SubStore." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "LOCATION_PARENT_MUST_BE_SUBSTORE")
            {
                return BadRequest(new { errorCode = "LOCATION_PARENT_MUST_BE_SUBSTORE", message = "OR, ICU, and Ward's parent must be a SubStore." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "PARENT_NOT_FOUND")
            {
                return NotFound(new { errorCode = "PARENT_NOT_FOUND", message = "The specified parent location does not exist." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("UpdateLocation/{locationId}")]
        public async Task<IActionResult> UpdateLocation([FromRoute] int locationId, [FromBody] UpdateLocationDTO request)
        {
            if (request == null)
                return BadRequest(new { message = "Location data is required." });
            if (locationId <= 0 || request.LocationId <= 0)
                return BadRequest(new { message = "LocationId must be greater than 0." });
            if (locationId != request.LocationId)
                return BadRequest(new { message = "ID mismatch." });
            try
            {
                var success = await _locationService.UpdateLocation(request);
                if (!success)
                    return NotFound(new { errorCode = "LOCATION_NOT_FOUND", message = "Location not found." });
                return Ok(new { message = "Location updated successfully." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "LOCATION_DUPLICATE")
            {
                return Conflict(new { errorCode = "LOCATION_DUPLICATE", message = "A location with this name and type already exists." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "INVALID_LOCATION_TYPE")
            {
                return BadRequest(new { errorCode = "INVALID_LOCATION_TYPE", message = "The specified location type does not exist." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "MAINSTORE_NO_PARENT")
            {
                return BadRequest(new { errorCode = "MAINSTORE_NO_PARENT", message = "MainStore cannot have a parent location." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "SUBSTORE_NEEDS_PARENT")
            {
                return BadRequest(new { errorCode = "SUBSTORE_NEEDS_PARENT", message = "SubStore must have a parent MainStore." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "SUBSTORE_PARENT_MUST_BE_MAINSTORE")
            {
                return BadRequest(new { errorCode = "SUBSTORE_PARENT_MUST_BE_MAINSTORE", message = "SubStore's parent must be a MainStore." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "LOCATION_NEEDS_PARENT")
            {
                return BadRequest(new { errorCode = "LOCATION_NEEDS_PARENT", message = "OR, ICU, and Ward must have a parent SubStore." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "LOCATION_PARENT_MUST_BE_SUBSTORE")
            {
                return BadRequest(new { errorCode = "LOCATION_PARENT_MUST_BE_SUBSTORE", message = "OR, ICU, and Ward's parent must be a SubStore." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "PARENT_NOT_FOUND")
            {
                return NotFound(new { errorCode = "PARENT_NOT_FOUND", message = "The specified parent location does not exist." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteLocation/{locationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLocation([FromRoute] int locationId)
        {
            if (locationId <= 0)
                return BadRequest(new { message = "LocationId must be greater than 0." });
            try
            {
                var success = await _locationService.DeleteLocation(locationId);
                if (!success)
                    return NotFound(new { errorCode = "LOCATION_NOT_FOUND", message = "Location not found." });
                return Ok(new { message = "Location deleted successfully." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "MAINSTORE_HAS_SUBSTORES")
            {
                return Conflict(new { errorCode = "MAINSTORE_HAS_SUBSTORES", message = "Cannot delete MainStore because it still has SubStores. Delete all SubStores first." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "SUBSTORE_HAS_CHILD_LOCATIONS")
            {
                return Conflict(new { errorCode = "SUBSTORE_HAS_CHILD_LOCATIONS", message = "Cannot delete SubStore because it still has OR/ICU/Ward locations. Delete them first." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ── Location Types ────────────────────────────────────────────────────

        [HttpGet("types")]
        public async Task<IActionResult> GetAllLocationTypes()
        {
            var types = await _locationService.GetAllLocationTypes();
            return Ok(types);
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
