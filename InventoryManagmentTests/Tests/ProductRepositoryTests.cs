using Xunit;
using Moq;
using InventoryManagment.Repositories;
using InventoryManagment.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Models;

namespace InventoryManagment.Tests;

public class ProductRepositoryTests
{
    private const string _DummyConnectionString = "Server=localhost;Database=InventoryDB;Integrated Security=True;";

    [Fact]
    public void CheckDBConnection_Should_ReturnFalse_WhenConnectionFails()
    {
        var _invalidConnStr = "Server=invalid;Database=doesnotexist;Integrated Security=True;";
        var repo = new ProductRepository(_invalidConnStr);

        var result = repo.CheckDBConnection();

        Assert.False(result);
    }

    [Fact]
    public async Task AddProductAsync_Should_Insert_Product_Without_Exceptions()
    {
        var repo = new ProductRepository(_DummyConnectionString);
        var product = new Product { Name = $"Test_{Guid.NewGuid()}", Quantity = 5, Price = 9.99f };

        var ex = await Record.ExceptionAsync(() => repo.AddProductAsync(product));

        Assert.Null(ex); // No exceptions thrown = success
    }

    [Fact]
    public async Task GetAllProductsAsync_Should_Return_ListOfProducts()
    {
        var repo = new ProductRepository(_DummyConnectionString);
        var products = await repo.GetAllProductsAsync();

        Assert.NotNull(products);
        Assert.IsType<List<Product>>(products);
    }

    [Fact]
    public async Task UpdateProductAsync_Should_Modify_Existing_Product()
    {
        var repo = new ProductRepository(_DummyConnectionString);
        var product = new Product { Name = $"ToUpdate_{Guid.NewGuid()}", Quantity = 1, Price = 1.23f };

        await repo.AddProductAsync(product);

        var all = await repo.GetAllProductsAsync();
        var latest = all[all.Count - 1];

        latest.Name = "Updated Name";
        latest.Quantity = 99;
        latest.Price = 123.45f;

        await repo.UpdateProductAsync(latest);
        var updated = await repo.GetProductByIdAsync(latest.Id);

        Assert.Equal("Updated Name", updated.Name);
        Assert.Equal(99, updated.Quantity);
        Assert.Equal(123.45f, updated.Price);
    }

    [Fact]
    public async Task DeleteProductAsync_Should_Remove_The_Product()
    {
        var repo = new ProductRepository(_DummyConnectionString);
        var product = new Product { Name = $"ToDelete_{Guid.NewGuid()}", Quantity = 10, Price = 50f };

        await repo.AddProductAsync(product);

        var all = await repo.GetAllProductsAsync();

        var latest = all[all.Count - 1];

        await repo.DeleteProductAsync(latest.Id);

        var deleted = await repo.GetProductByIdAsync(latest.Id);
        Assert.Null(deleted);
    }
}
