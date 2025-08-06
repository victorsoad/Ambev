using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Common.Security;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Auth.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<RegisterHandler> _logger;

        public RegisterHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ILogger<RegisterHandler> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Tentativa de registro para usuário: {Username}", request.Username);

            try
            {
                // Validar se as senhas coincidem
                if (request.Password != request.ConfirmPassword)
                {
                    _logger.LogWarning("Tentativa de registro com senhas diferentes para usuário: {Username}", request.Username);
                    return new RegisterResult
                    {
                        Success = false,
                        Message = "As senhas não coincidem"
                    };
                }

                // Verificar se o username já existe
                if (await _userRepository.UsernameExistsAsync(request.Username))
                {
                    _logger.LogWarning("Tentativa de registro com username já existente: {Username}", request.Username);
                    return new RegisterResult
                    {
                        Success = false,
                        Message = "Nome de usuário já está em uso"
                    };
                }

                // Verificar se o email já existe
                if (await _userRepository.EmailExistsAsync(request.Email))
                {
                    _logger.LogWarning("Tentativa de registro com email já existente: {Email}", request.Email);
                    return new RegisterResult
                    {
                        Success = false,
                        Message = "Email já está em uso"
                    };
                }

                // Criar hash da senha
                var passwordHash = _passwordHasher.HashPassword(request.Password);

                // Criar novo usuário
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    Role = "User", // Role padrão
                    IsActive = true,
                    LastLogin = DateTime.UtcNow
                };

                await _userRepository.AddAsync(user);

                _logger.LogInformation("Registro bem-sucedido para usuário: {Username}", request.Username);

                return new RegisterResult
                {
                    Success = true,
                    Username = user.Username,
                    Email = user.Email,
                    Message = "Usuário registrado com sucesso"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o registro para usuário: {Username}", request.Username);
                return new RegisterResult
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                };
            }
        }
    }
} 