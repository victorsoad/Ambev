using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Common.Security;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Auth.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<LoginHandler> _logger;

        public LoginHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            ILogger<LoginHandler> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }

        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Tentativa de login para usuário: {Username}", request.Username);

            try
            {
                // Buscar usuário por username
                var user = await _userRepository.GetByUsernameAsync(request.Username);
                if (user == null)
                {
                    _logger.LogWarning("Tentativa de login com usuário inexistente: {Username}", request.Username);
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Usuário ou senha inválidos"
                    };
                }

                // Verificar se o usuário está ativo
                if (!user.IsActive)
                {
                    _logger.LogWarning("Tentativa de login com usuário inativo: {Username}", request.Username);
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Usuário inativo"
                    };
                }

                // Verificar senha
                if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Tentativa de login com senha incorreta para usuário: {Username}", request.Username);
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Usuário ou senha inválidos"
                    };
                }

                // Gerar token JWT
                var jwtUser = new JwtUser
                {
                    Id = user.Id.ToString(),
                    Username = user.Username,
                    Role = user.Role
                };

                var token = _jwtTokenGenerator.GenerateToken(jwtUser);

                // Atualizar último login
                user.LastLogin = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                _logger.LogInformation("Login bem-sucedido para usuário: {Username}", request.Username);

                return new LoginResult
                {
                    Success = true,
                    Token = token,
                    Username = user.Username,
                    Role = user.Role,
                    Message = "Login realizado com sucesso"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o login para usuário: {Username}", request.Username);
                return new LoginResult
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                };
            }
        }
    }

    public class JwtUser : IUser
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
} 