namespace Ambev.DeveloperEvaluation.Application.Auth.Login
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
    }
} 