

using System.ComponentModel.DataAnnotations;

namespace PharmaStock.Core.DTO.Auth
{
    public class LoginDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}