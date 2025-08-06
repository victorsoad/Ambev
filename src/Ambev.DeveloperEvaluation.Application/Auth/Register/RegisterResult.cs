namespace Ambev.DeveloperEvaluation.Application.Auth.Register
{
    public class RegisterResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
} 