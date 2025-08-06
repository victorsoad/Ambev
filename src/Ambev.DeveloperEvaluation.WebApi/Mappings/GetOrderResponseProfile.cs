using Ambev.DeveloperEvaluation.Application.Orders.GetOrder;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.GetOrder;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class GetOrderResponseProfile : Profile
{
    public GetOrderResponseProfile()
    {
        CreateMap<GetOrderResult, GetOrderResponse>();
        CreateMap<OrderItemResult, OrderItemResponse>();
    }
} 