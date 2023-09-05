using AutoMapper;

namespace Catalog.Application.Mappers;

public static class ProductMapper
{
    private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
    {
        var configuration = new MapperConfiguration(config =>
        {
            config.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
            config.AddProfile<ProductMappingProfile>();
        });

        var mapper = configuration.CreateMapper();
        return mapper;
    });

    public static IMapper Mapper => Lazy.Value;
}
