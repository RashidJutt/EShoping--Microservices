using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetProductByNameHandler : IRequestHandler<GetProductByNameQuery, IList<ProductReponse>>
{
    private readonly IProductRepository _productRepository;

    public GetProductByNameHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<IList<ProductReponse>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProductByName(request.Name);
        var productsResponse = ProductMapper.Mapper.Map<List<ProductReponse>>(products);
        return productsResponse;
    }
}
