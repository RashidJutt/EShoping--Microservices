using AutoMapper;
using EventBuss.Messages.Events;
using Ordering.Application.Commnads;
using Ordering.Application.Responses;
using Ordering.Core.Entities;

namespace Ordering.Application.Mappers;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderResponse>().ReverseMap();
        CreateMap<CheckoutOrderCommand, Order>().ReverseMap();
        CreateMap<UpdateOrderCommand, Order>().ReverseMap();
        CreateMap<BasketCheckoutEvent, CheckoutOrderCommand>().ReverseMap();
    }
}
