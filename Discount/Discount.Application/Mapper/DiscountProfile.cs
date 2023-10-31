using AutoMapper;
using Discount.Core.Entities;
using DiscountAPI;

namespace Discount.Application.Mapper;

public class DiscountProfile : Profile
{
    public DiscountProfile()
    {
        CreateMap<Coupon, CouponModel>().ReverseMap();
    }
}
