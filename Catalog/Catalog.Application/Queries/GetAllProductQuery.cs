using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;

namespace Catalog.Application.Queries;

public class GetAllProductQuery:IRequest<Pagination<ProductReponse>>
{
    public CatalogSpecsParams CatalogSpecsParams { get; }
    public GetAllProductQuery(CatalogSpecsParams catalogSpecsParams)
    {
        CatalogSpecsParams = catalogSpecsParams;
    }
}
