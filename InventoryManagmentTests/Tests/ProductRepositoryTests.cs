using Xunit;
using Moq;
using InventoryManagment.Repositories;
using InventoryManagment.Models;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using Models;

namespace InventoryManagment.Tests;

public class ProductRepositoryTests : IAsyncLifetime
{
    private static readonly string _DummyConnectionString =
        Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
        "Server=SAIFKHALIFA-PC;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True;";

    private ProductRepository _repo;
    private readonly List<int> _createdTestProductIds = new();

    public Task InitializeAsync()
    {
        _repo = new ProductRepository(_DummyConnectionString);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        // Clean up test data, to keep the database clean.
        var all = await _repo.GetAllProductsAsync();

        foreach (var product in all.Where(p => p.Name.StartsWith("Test_") || p.Name.StartsWith("ToUpdate_") || p.Name.StartsWith("ToDelete_")))
        {
            await _repo.DeleteProductAsync(product.Id);
        }
    }

    [Fact]
    public void CheckDBConnection_Should_ReturnFalse_WhenConnectionFails()
    {
        var invalidConnStr = "Server=invalid;Database=doesnotexist;Integrated Security=True;";
        var repo = new ProductRepository(invalidConnStr);

        var result = repo.CheckDBConnection();

        Assert.False(result);
    }

    [Fact]
    public async Task AddProductAsync_Should_Insert_Product_Without_Exceptions()
    {
        var product = new Product { Name = $"Test_{Guid.NewGuid()}", Quantity = 5, Price = 9.99f };

        var ex = await Record.ExceptionAsync(() => _repo.AddProductAsync(product));

        Assert.Null(ex);
    }

    [Fact]
    public async Task GetAllProductsAsync_Should_Return_ListOfProducts()
    {
        var products = await _repo.GetAllProductsAsync();

        Assert.NotNull(products);
        Assert.IsType<List<Product>>(products);
    }

    [Fact]
    public async Task UpdateProductAsync_Should_Modify_Existing_Product()
    {
        var product = new Product { Name = $"ToUpdate_{Guid.NewGuid()}", Quantity = 1, Price = 1.23f };
        await _repo.AddProductAsync(product);

        var all = await _repo.GetAllProductsAsync();
        var latest = all[all.Count - 1];

        latest.Name = "Updated Name";
        latest.Quantity = 99;
        latest.Price = 123.45f;

        await _repo.UpdateProductAsync(latest);
        var updated = await _repo.GetProductByIdAsync(latest.Id);

        Assert.Equal("Updated Name", updated.Name);
        Assert.Equal(99, updated.Quantity);
        Assert.Equal(123.45f, updated.Price);
    }

    [Fact]
    public async Task DeleteProductAsync_Should_Remove_The_Product()
    {
        var product = new Product { Name = $"ToDelete_{Guid.NewGuid()}", Quantity = 10, Price = 50f };
        await _repo.AddProductAsync(product);

        var all = await _repo.GetAllProductsAsync();
        var latest = all[all.Count - 1];

        await _repo.DeleteProductAsync(latest.Id);

        var deleted = await _repo.GetProductByIdAsync(latest.Id);
        Assert.Null(deleted);
    }
}
