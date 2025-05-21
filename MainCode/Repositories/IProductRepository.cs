using Models;

namespace InventoryManagment.Repositories;

public interface IProductRepository
{
    Task AddProductAsync(Product product);
    Task<List<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
    bool CheckDBConnection();
}
