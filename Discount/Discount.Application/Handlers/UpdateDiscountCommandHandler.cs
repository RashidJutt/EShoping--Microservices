using Discount.Application.Commands;
using Discount.Application.Mapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using DiscountAPI;
using MediatR;

namespace Discount.Application.Handlers;

public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, CouponModel>
{
    private readonly IDiscountRepository _discountRepository;

    public UpdateDiscountCommandHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }
    public async Task<CouponModel> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        var coupon = MapperProvider.Mapper.Map<Coupon>(request);
        await _discountRepository.UpdateDiscount(coupon);
        var couponModel = MapperProvider.Mapper.Map<CouponModel>(coupon);
        return couponModel;
    }
}
