using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductReponse>
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<ProductReponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productEntity = ProductMapper.Mapper.Map<Product>(request);
        if (productEntity == null)
            throw new ApplicationException("Problem in mapping product in create product handler");

        var createdProduct = await _productRepository.CreateProduct(productEntity);
        var productResonse = ProductMapper.Mapper.Map<ProductReponse>(createdProduct);
        return productResonse;
    }
}
