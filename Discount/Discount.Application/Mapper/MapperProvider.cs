using AutoMapper;

namespace Discount.Application.Mapper;

public static class MapperProvider
{
    private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
    {
        var configuration = new MapperConfiguration(config =>
        {
            config.ShouldMapProperty = p => p.GetMethod.IsPublic | p.GetMethod.IsAssembly;
            config.AddProfile<DiscountProfile>();
        });

        return configuration.CreateMapper();
    });

    public static IMapper Mapper = Lazy.Value;
}
