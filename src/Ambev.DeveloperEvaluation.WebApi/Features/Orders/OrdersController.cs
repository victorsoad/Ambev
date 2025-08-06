using Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;
using Ambev.DeveloperEvaluation.Application.Orders.DeleteOrder;
using Ambev.DeveloperEvaluation.Application.Orders.GetOrder;
using Ambev.DeveloperEvaluation.Application.Orders.UpdateOrder;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.CreateOrder;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.GetOrder;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.UpdateOrder;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requer autenticação
    public class OrdersController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IMediator mediator, IMapper mapper, ILogger<OrdersController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            _logger.LogInformation("Recebida requisição para criar pedido com {ItemCount} itens", request.Itens?.Count ?? 0);
            
            try
            {
                var command = _mapper.Map<CreateOrderCommand>(request);
                var result = await _mediator.Send(command);
                var response = _mapper.Map<CreateOrderResponse>(result);

                _logger.LogInformation("Pedido criado com sucesso - ID: {OrderId}", result.Id);
                return Ok(new ApiResponseWithData<CreateOrderResponse> { Success = true, Data = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar pedido");
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            _logger.LogInformation("Recebida requisição para buscar pedido - ID: {OrderId}", id);
            
            try
            {
                var command = new GetOrderCommand { Id = id };
                var result = await _mediator.Send(command);

                if (result == null)
                {
                    _logger.LogWarning("Pedido não encontrado - ID: {OrderId}", id);
                    return NotFound(new ApiResponse { Success = false, Message = "Pedido não encontrado" });
                }

                var response = _mapper.Map<GetOrderResponse>(result);
                _logger.LogInformation("Pedido encontrado com sucesso - ID: {OrderId}", id);

                return Ok(new ApiResponseWithData<GetOrderResponse> { Success = true, Data = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar pedido - ID: {OrderId}", id);
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderRequest request)
        {
            _logger.LogInformation("Recebida requisição para atualizar pedido - ID: {OrderId}", id);
            
            try
            {
                var command = _mapper.Map<UpdateOrderCommand>(request);
                command.Id = id;

                var result = await _mediator.Send(command);

                if (result == null)
                {
                    _logger.LogWarning("Pedido não encontrado para atualização - ID: {OrderId}", id);
                    return NotFound(new ApiResponse { Success = false, Message = "Pedido não encontrado" });
                }

                var response = _mapper.Map<UpdateOrderResponse>(result);
                _logger.LogInformation("Pedido atualizado com sucesso - ID: {OrderId}", id);

                return Ok(new ApiResponseWithData<UpdateOrderResponse> { Success = true, Data = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar pedido - ID: {OrderId}", id);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Recebida requisição para deletar pedido - ID: {OrderId}", id);
            
            try
            {
                var command = new DeleteOrderCommand { Id = id };
                var result = await _mediator.Send(command);

                if (!result.Success)
                {
                    _logger.LogWarning("Pedido não encontrado para exclusão - ID: {OrderId}", id);
                    return NotFound(new ApiResponse { Success = false, Message = "Pedido não encontrado" });
                }

                _logger.LogInformation("Pedido deletado com sucesso - ID: {OrderId}", id);
                return Ok(new ApiResponse { Success = true, Message = "Pedido deletado com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar pedido - ID: {OrderId}", id);
                throw;
            }
        }
    }
} 