using Discount.Application.Commands;
using Discount.Application.Mapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using DiscountAPI;
using MediatR;

namespace Discount.Application.Handlers;

public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, CouponModel>
{
    private readonly IDiscountRepository _discountRepository;

    public CreateDiscountCommandHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }
    public async Task<CouponModel> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        var coupon = MapperProvider.Mapper.Map<Coupon>(request);
        await _discountRepository.CreateDiscount(coupon);
        var couponModel = MapperProvider.Mapper.Map<CouponModel>(request);
        return couponModel;
    }
}
