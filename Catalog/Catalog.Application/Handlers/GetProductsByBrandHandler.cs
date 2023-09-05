using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetProductsByBrandHandler : IRequestHandler<GetProductByBrandQuery, IList<ProductReponse>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsByBrandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<IList<ProductReponse>> Handle(GetProductByBrandQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProductByBrand(request.Brand);
        var productsResponse = ProductMapper.Mapper.Map<IList<ProductReponse>>(products);
        return productsResponse;
    }
}
