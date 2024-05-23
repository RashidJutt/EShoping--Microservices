using AutoMapper;
using EventBuss.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commnads;

namespace Ordering.API.Consumers;

public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<BasketOrderingConsumer> _logger;

    public BasketOrderingConsumer(IMediator mediator, IMapper mapper, ILogger<BasketOrderingConsumer> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
        var result = await _mediator.Send(command);

        _logger.LogInformation("Basket checkout event completed.");
    }
}
