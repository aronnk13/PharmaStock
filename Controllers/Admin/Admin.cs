using Microsoft.AspNetCore.Mvc;

namespace PharmaStock.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class Admin : ControllerBase
    {
        [HttpPost("UserRegistration")]
        public IActionResult RegisterUser()
        {
            return Ok();
        }
    }
}