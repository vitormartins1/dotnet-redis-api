﻿using Dapper;
using Discount.Api.Entities;
using Npgsql;

namespace Discount.Api.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        NpgsqlConnection connection = GetConnectionPostgreSql();

        Coupon? coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
            ("SELECT * FROM Coupon WHERE ProductName=@ProductName",
            new { ProductName = productName });

        if (coupon is null)
        {
            return new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Desc"
            };
        }

        return coupon;
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        NpgsqlConnection connection = GetConnectionPostgreSql();

        int affected = await connection.ExecuteAsync
            ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
            new
            {
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount
            });

        if (affected == 0)
            return false;

        return true;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        NpgsqlConnection connection = GetConnectionPostgreSql();

        int affected = await connection.ExecuteAsync
            ("UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id=@Id",
            new
            {
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount,
                Id = coupon.Id
            });

        if (affected == 0)
            return false;

        return true;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        NpgsqlConnection connection = GetConnectionPostgreSql();

        int affected = await connection.ExecuteAsync
            ("DELETE FROM Coupon WHERE ProductName=@ProductName",
            new
            {
                ProductName = productName
            });

        if (affected == 0)
            return false;

        return true;
    }

    public NpgsqlConnection GetConnectionPostgreSql()
    {
        return new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
    }
}