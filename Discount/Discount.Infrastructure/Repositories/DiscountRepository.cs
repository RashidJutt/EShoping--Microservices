using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Infrastructure.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;
    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetSection("DatabaseSettings:ConnectionString").Value);
        var effectedRows = await connection
            .ExecuteAsync("Insert Into Coupon (ProductName,Description,Amount) Values (@ProductName,@Description,@Amount)"
            , new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });
        if (effectedRows == 0)
            return false;
        return true;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetSection("DatabaseSettings:ConnectionString").Value);
        var effectedRows = await connection.ExecuteAsync("Delete from Coupon where ProductName=@ProductName", new { ProductName = productName });
        if (effectedRows == 0)
            return false;
        return true;
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetSection("DatabaseSettings:ConnectionString").Value);
        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("select * from Coupon where ProductName=@ProductName", new { ProductName = productName });

        if (coupon == null)
            return new Coupon { ProductName = productName, Amount = 0, Description = "No coupon found" };

        return coupon;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetSection("DatabaseSettings:ConnectionString").Value);
        var effectedRows = await connection.ExecuteAsync("Update Coupon set ProductName=@ProductName,Amount=@Amount,Description=@Description",
            new { ProductName = coupon.ProductName, Amount = coupon.Amount, Description = coupon.Description });
        if (effectedRows == 0)
            return false;
        return true;
    }
}
