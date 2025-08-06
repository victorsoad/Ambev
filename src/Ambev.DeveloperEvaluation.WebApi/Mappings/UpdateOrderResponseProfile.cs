using Ambev.DeveloperEvaluation.Application.Orders.UpdateOrder;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.UpdateOrder;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class UpdateOrderResponseProfile : Profile
{
    public UpdateOrderResponseProfile()
    {
        CreateMap<UpdateOrderResult, UpdateOrderResponse>();
    }
} 