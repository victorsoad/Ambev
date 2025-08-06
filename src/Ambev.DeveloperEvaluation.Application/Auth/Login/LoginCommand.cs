using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Auth.Login
{
    public class LoginCommand : IRequest<LoginResult>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
} 