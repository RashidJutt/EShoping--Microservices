using Basket.Application.Commands;
using Basket.Application.GrpcServices;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Common.Logging.Correlation;
using EventBuss.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    public class BasketController : APIController
    {
        private readonly IMediator _mediator;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICorrelationIdGenerator _correlationIdGenerator;

        public BasketController(IMediator mediator,
            DiscountGrpcService discountGrpcService,
            IPublishEndpoint publishEndpoint,
            ILogger<BasketController> logger,
            ICorrelationIdGenerator correlationIdGenerator)
        {
            _mediator = mediator;
            _discountGrpcService = discountGrpcService;
            _publishEndpoint = publishEndpoint;
            _correlationIdGenerator = correlationIdGenerator;
            logger.LogInformation("CorrelationId {correlationId}:", correlationIdGenerator.Get());
        }

        [HttpGet]
        [Route("[action]/{userName}", Name = "GetBasketByUserName")]
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
        [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            var deleteCommand = new DeleteShoppingCartByUserNameCommand(userName);
            await _mediator.Send(deleteCommand);
            return NoContent();
        }

        [Route("Checkout")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Checkout(BasketCheckout checkout)
        {
            var basketQuery = new GetBasketByUserNameQuery(checkout.UserName);
            var basket = await _mediator.Send(basketQuery);

            if (basket == null)
            {
                return BadRequest();
            }

            var checkoutEvent = BasketMapper.Mapper.Map<BasketCheckoutEvent>(checkout);
            checkoutEvent.TotalPrice = basket.TotalPrice;
            checkoutEvent.CorrelationId = _correlationIdGenerator.Get();
            await _publishEndpoint.Publish(checkoutEvent);

            var deleteCommand = new DeleteShoppingCartByUserNameCommand(checkoutEvent.UserName);

            await _mediator.Send(deleteCommand);

            return Accepted();
        }
    }
}
