namespace PharmaStock.Core.DTO.Auth
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = null!;
        public int UserId { get; set; }
        public string Role { get; set; } = null!;
    }
}
