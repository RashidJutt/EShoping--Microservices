using Catalog.Application.Responses;
using Catalog.Core.Entities;
using MediatR;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Application.Commands;

public class CreateProductCommand:IRequest<ProductReponse>
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public string ImageFile { get; set; }
    public ProductBrand Brands { get; set; }
    public ProductType Types { get; set; }
    public decimal Price { get; set; }
}
