using Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.CreateOrder;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class CreateOrderResponseProfile : Profile
{
    public CreateOrderResponseProfile()
    {
        CreateMap<CreateOrderResult, CreateOrderResponse>();
        CreateMap<OrderItemResult, OrderItemResponse>();
    }
} 