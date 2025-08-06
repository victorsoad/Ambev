using Ambev.DeveloperEvaluation.Application.Auth.Login;
using Ambev.DeveloperEvaluation.Application.Auth.Register;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.Login;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Realiza o login do usuário
        /// </summary>
        /// <param name="request">Dados de login</param>
        /// <returns>Token JWT se o login for bem-sucedido</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Recebida requisição de login para usuário: {Username}", request.Username);

            try
            {
                var command = new LoginCommand
                {
                    Username = request.Username,
                    Password = request.Password
                };

                var result = await _mediator.Send(command);

                if (!result.Success)
                {
                    _logger.LogWarning("Login falhou para usuário: {Username}", request.Username);
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = result.Message
                    });
                }

                _logger.LogInformation("Login bem-sucedido para usuário: {Username}", request.Username);

                return Ok(new ApiResponseWithData<LoginResponse>
                {
                    Success = true,
                    Data = new LoginResponse
                    {
                        Token = result.Token,
                        Username = result.Username,
                        Role = result.Role,
                        Message = result.Message
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o login para usuário: {Username}", request.Username);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                });
            }
        }

        /// <summary>
        /// Registra um novo usuário
        /// </summary>
        /// <param name="request">Dados de registro</param>
        /// <returns>Confirmação do registro</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("Recebida requisição de registro para usuário: {Username}", request.Username);

            try
            {
                var command = new RegisterCommand
                {
                    Username = request.Username,
                    Email = request.Email,
                    Password = request.Password,
                    ConfirmPassword = request.ConfirmPassword
                };

                var result = await _mediator.Send(command);

                if (!result.Success)
                {
                    _logger.LogWarning("Registro falhou para usuário: {Username}", request.Username);
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = result.Message
                    });
                }

                _logger.LogInformation("Registro bem-sucedido para usuário: {Username}", request.Username);

                return Ok(new ApiResponseWithData<RegisterResponse>
                {
                    Success = true,
                    Data = new RegisterResponse
                    {
                        Username = result.Username,
                        Email = result.Email,
                        Message = result.Message
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o registro para usuário: {Username}", request.Username);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                });
            }
        }
    }
} 