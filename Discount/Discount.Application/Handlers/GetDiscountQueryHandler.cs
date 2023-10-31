using Discount.Application.Mapper;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using DiscountAPI;
using MediatR;

namespace Discount.Application.Handlers;

public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponModel>
{
    private readonly IDiscountRepository _discountRepository;

    public GetDiscountQueryHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }
    public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _discountRepository.GetDiscount(request.ProductName);
        var couponModel = MapperProvider.Mapper.Map<CouponModel>(coupon);
        return couponModel;
    }
}
