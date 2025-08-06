using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var command = _mapper.Map<CreateProductCommand>(request);
            var result = await _mediator.Send(command);
            var response = _mapper.Map<CreateProductResponse>(result);

            return Ok(new ApiResponseWithData<CreateProductResponse> { Success = true, Data = response });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var command = new GetProductCommand { Id = id };
            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound(new ApiResponse { Success = false, Message = "Produto não encontrado" });

            var response = _mapper.Map<GetProductResponse>(result);

            return Ok(new ApiResponseWithData<GetProductResponse> { Success = true, Data = response });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
        {
            var command = _mapper.Map<UpdateProductCommand>(request);
            command.Id = id;

            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound(new ApiResponse { Success = false, Message = "Produto não encontrado" });

            var response = _mapper.Map<UpdateProductResponse>(result);

            return Ok(new ApiResponseWithData<UpdateProductResponse> { Success = true, Data = response });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteProductCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result.Success)
                return NotFound(new ApiResponse { Success = false, Message = "Produto não encontrado" });

            return Ok(new ApiResponse { Success = true, Message = "Produto deletado com sucesso" });
        }
    }
} 