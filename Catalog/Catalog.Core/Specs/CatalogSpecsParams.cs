namespace Catalog.Core.Specs;

public class CatalogSpecsParams
{
    private const int Max_Page_Size = 70;
    private int _pageSize = 10;
    public int PageIndex { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize =  (value > Max_Page_Size) ? Max_Page_Size : value;
    }

    public string? BrandId { get; set; }
    public string? TypeId { get; set;}
    public string? Sort { get; set; }
    public string? Search { get; set; }
}
