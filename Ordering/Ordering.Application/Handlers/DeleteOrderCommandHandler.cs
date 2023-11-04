using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commnads;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }
    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id);
        if (order == null)
        {
            throw new OrderNotFoundException(nameof(order), request.Id);
        }

        await _orderRepository.DeleteAsync(order);

        _logger.LogInformation("Order ({OrderId}) is deleted", request.Id);
    }
}
