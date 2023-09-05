using Basket.Application.Commands;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;

public class DeleteShoppingCartByUserNameCommandHandler : IRequestHandler<DeleteShoppingCartByUserNameCommand>
{
    private readonly IBasketRepository _basketRepository;

    public DeleteShoppingCartByUserNameCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }
    public async Task<Unit> Handle(DeleteShoppingCartByUserNameCommand request, CancellationToken cancellationToken)
    {
        await _basketRepository.DeleteBasket(request.UserName);
        return Unit.Value;
    }
}
