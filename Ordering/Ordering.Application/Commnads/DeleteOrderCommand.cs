using MediatR;

namespace Ordering.Application.Commnads;

public class DeleteOrderCommand : IRequest
{
    public int Id { get; set; }
    public DeleteOrderCommand(int id)
    {
        Id = id;
    }
}
