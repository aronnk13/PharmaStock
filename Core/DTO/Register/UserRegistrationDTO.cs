using System;
using System.ComponentModel.DataAnnotations;

namespace PharmaStock.Core.DTO.Register
{
    public class UserRegistrationDTO
    {
        public string Username { get; set; } = null!;
        public int RoleId { get; set; }

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;
    }
}