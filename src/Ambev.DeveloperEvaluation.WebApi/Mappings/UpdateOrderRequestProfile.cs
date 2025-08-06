using Ambev.DeveloperEvaluation.Application.Orders.UpdateOrder;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.UpdateOrder;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class UpdateOrderRequestProfile : Profile
{
    public UpdateOrderRequestProfile()
    {
        CreateMap<UpdateOrderRequest, UpdateOrderCommand>();
    }
} 