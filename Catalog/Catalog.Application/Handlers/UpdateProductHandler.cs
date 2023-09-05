using Catalog.Application.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var updateResponse = await _productRepository.UpdateProduct(new Product
        {
            Id = request.Id,
            Name = request.Name,
            Brands = request.Brands,
            Description = request.Description,
            ImageFile = request.ImageFile,
            Price = request.Price,
            Summary = request.Summary,
            Types = request.Types,
        });

        return updateResponse;
    }
}
