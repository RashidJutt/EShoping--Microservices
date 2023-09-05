using MediatR;

namespace Basket.Application.Commands;

public class DeleteShoppingCartByUserNameCommand:IRequest
{
    public string UserName { get; set; }

    public DeleteShoppingCartByUserNameCommand(string userName)
    {
        UserName = userName;
    }
}
