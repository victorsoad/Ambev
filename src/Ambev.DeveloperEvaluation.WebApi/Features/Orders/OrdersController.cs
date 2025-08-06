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
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrdersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var command = _mapper.Map<CreateOrderCommand>(request);
            var result = await _mediator.Send(command);
            var response = _mapper.Map<CreateOrderResponse>(result);

            return Ok(new ApiResponseWithData<CreateOrderResponse> { Success = true, Data = response });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var command = new GetOrderCommand { Id = id };
            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound(new ApiResponse { Success = false, Message = "Pedido não encontrado" });

            var response = _mapper.Map<GetOrderResponse>(result);

            return Ok(new ApiResponseWithData<GetOrderResponse> { Success = true, Data = response });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderRequest request)
        {
            var command = _mapper.Map<UpdateOrderCommand>(request);
            command.Id = id;

            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound(new ApiResponse { Success = false, Message = "Pedido não encontrado" });

            var response = _mapper.Map<UpdateOrderResponse>(result);

            return Ok(new ApiResponseWithData<UpdateOrderResponse> { Success = true, Data = response });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteOrderCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result.Success)
                return NotFound(new ApiResponse { Success = false, Message = "Pedido não encontrado" });

            return Ok(new ApiResponse { Success = true, Message = "Pedido deletado com sucesso" });
        }
    }
} 