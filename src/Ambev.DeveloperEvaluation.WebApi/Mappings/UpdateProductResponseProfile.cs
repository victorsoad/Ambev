using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class UpdateProductResponseProfile : Profile
{
    public UpdateProductResponseProfile()
    {
        CreateMap<UpdateProductResult, UpdateProductResponse>();
    }
} 