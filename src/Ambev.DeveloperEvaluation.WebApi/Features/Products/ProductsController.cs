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
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IMediator mediator, IMapper mapper, ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            _logger.LogInformation("Recebida requisição para criar produto - Nome: {ProductName}", request.Nome);
            
            try
            {
                var command = _mapper.Map<CreateProductCommand>(request);
                var result = await _mediator.Send(command);
                var response = _mapper.Map<CreateProductResponse>(result);

                _logger.LogInformation("Produto criado com sucesso - ID: {ProductId}, Nome: {ProductName}", result.Id, result.Nome);
                return Ok(new ApiResponseWithData<CreateProductResponse> { Success = true, Data = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar produto - Nome: {ProductName}", request.Nome);
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            _logger.LogInformation("Recebida requisição para buscar produto - ID: {ProductId}", id);
            
            try
            {
                var command = new GetProductCommand { Id = id };
                var result = await _mediator.Send(command);

                if (result == null)
                {
                    _logger.LogWarning("Produto não encontrado - ID: {ProductId}", id);
                    return NotFound(new ApiResponse { Success = false, Message = "Produto não encontrado" });
                }

                var response = _mapper.Map<GetProductResponse>(result);
                _logger.LogInformation("Produto encontrado com sucesso - ID: {ProductId}, Nome: {ProductName}", id, result.Nome);

                return Ok(new ApiResponseWithData<GetProductResponse> { Success = true, Data = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar produto - ID: {ProductId}", id);
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
        {
            _logger.LogInformation("Recebida requisição para atualizar produto - ID: {ProductId}, Nome: {ProductName}", id, request.Nome);
            
            try
            {
                var command = _mapper.Map<UpdateProductCommand>(request);
                command.Id = id;

                var result = await _mediator.Send(command);

                if (result == null)
                {
                    _logger.LogWarning("Produto não encontrado para atualização - ID: {ProductId}", id);
                    return NotFound(new ApiResponse { Success = false, Message = "Produto não encontrado" });
                }

                var response = _mapper.Map<UpdateProductResponse>(result);
                _logger.LogInformation("Produto atualizado com sucesso - ID: {ProductId}, Nome: {ProductName}", id, result.Nome);

                return Ok(new ApiResponseWithData<UpdateProductResponse> { Success = true, Data = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar produto - ID: {ProductId}", id);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Recebida requisição para deletar produto - ID: {ProductId}", id);
            
            try
            {
                var command = new DeleteProductCommand { Id = id };
                var result = await _mediator.Send(command);

                if (!result.Success)
                {
                    _logger.LogWarning("Produto não encontrado para exclusão - ID: {ProductId}", id);
                    return NotFound(new ApiResponse { Success = false, Message = "Produto não encontrado" });
                }

                _logger.LogInformation("Produto deletado com sucesso - ID: {ProductId}", id);
                return Ok(new ApiResponse { Success = true, Message = "Produto deletado com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar produto - ID: {ProductId}", id);
                throw;
            }
        }
    }
} 