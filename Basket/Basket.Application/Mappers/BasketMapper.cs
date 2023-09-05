using AutoMapper;

namespace Basket.Application.Mappers;

public static class BasketMapper
{
    private static Lazy<IMapper> _lazy = new Lazy<IMapper>(() =>
    {
        var configuration = new MapperConfiguration(conf =>
        {
            conf.ShouldMapProperty = a => a.GetMethod.IsPublic || a.GetMethod.IsAssembly;
            conf.AddProfile<BasketMappingProfile>();
        });
        var mapper = configuration.CreateMapper();
        return mapper;
    });

    public static IMapper Mapper = _lazy.Value;
}
