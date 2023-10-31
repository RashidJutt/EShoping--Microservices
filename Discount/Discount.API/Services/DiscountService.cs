using Discount.Application.Commands;
using Discount.Application.Queries;
using DiscountAPI;
using Grpc.Core;
using MediatR;

namespace Discount.API.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DiscountService> _logger;

    public DiscountService(IMediator mediator, ILogger<DiscountService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var query = new GetDiscountQuery(request.ProductName);
        var result = await _mediator.Send(query);

        this._logger.LogInformation($"Discount is retrived for product:{query.ProductName} amount:{result.Amount}");
        return result;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var createDiscountCommand = new CreateDiscountCommand()
        {
            ProductName = request.Coupon.ProductName,
            Amount = request.Coupon.Amount,
            Description = request.Coupon.Description,
        };

        var result = await _mediator.Send(createDiscountCommand);
        return result;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var updateDiscountCommand = new UpdateDiscountCommand()
        {
            ProductName = request.Coupon.ProductName,
            Amount = request.Coupon.Amount,
            Description = request.Coupon.Description,
            Id = request.Coupon.Id
        };

        var result = await _mediator.Send(updateDiscountCommand);

        _logger.LogInformation($"Discount is updated for product: {request.Coupon.ProductName}");
        return result;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var deleteDiscountCommand = new DeleteDiscountCommand()
        {
            ProductName = request.ProductName
        };

        var result = await _mediator.Send(deleteDiscountCommand);

        var deleteResponse = new DeleteDiscountResponse()
        {
            Success = result
        };

        return deleteResponse;
    }
}
