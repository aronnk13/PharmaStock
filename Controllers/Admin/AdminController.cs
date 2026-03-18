using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Register;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController(IUserService userService) : ControllerBase
    {
        [HttpPost("UserRegistration")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegistrationDTO userRegistrationDTO, [FromHeader] int admindId)
        {
            await userService.RegisterUser(userRegistrationDTO, admindId);
            return Ok();
        }
    }
}