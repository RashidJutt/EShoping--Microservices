using Basket.Application.Commands;
using Basket.Application.GrpcServices;
using Basket.Application.Queries;
using Basket.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    public class BasketController : APIController
    {
        private readonly IMediator _mediator;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketController(IMediator mediator, DiscountGrpcService discountGrpcService)
        {
            _mediator = mediator;
            _discountGrpcService = discountGrpcService;
        }

        [HttpGet]
        [Route("[action]/userName", Name = "GetBasketByUserName")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }

        [HttpPost("CreateBasket")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> CreateBasket(CreateShoppingCartCommand command)
        {
            foreach (var item in command.Items)
            {
                var coupon = await _discountGrpcService.GetDiscountAsync(item.ProductName);
                if (coupon != null)
                {
                    item.Price -= coupon.Amount;
                }
            }
            var shopingCard = await _mediator.Send(command);
            return Ok(shopingCard);
        }

        [HttpDelete]
        [Route("[action]/userName", Name = "DeleteBasketByUserName")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            var deleteCommand = new DeleteShoppingCartByUserNameCommand(userName);
            await _mediator.Send(deleteCommand);
            return NoContent();
        }
    }
}
