namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.Login
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
        public string? Message { get; set; }
    }
} 