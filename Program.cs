using InventoryManagment.Models;
using InventoryManagment.Repositories;
using Models;
using System.Data.SqlClient;

namespace InventoryManagment;

class Program
{
    static async Task Main(string[] args)
    {
        string _connectionString = "Server=SAIFKHALIFA-PC;Database=InventoryDB;Trusted_Connection=True;";
        var _repo = new ProductRepository(_connectionString);

        if (!_repo.CheckDBConnection())
        {
            Console.WriteLine("Failed to connect to the database.");
            return;
        }

        while (true)
        {
            MainMenu();
            Console.Write("Enter your choice: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await AddProduct(_repo);
                    break;
                case "2":
                    await ViewAllProducts(_repo);
                    break;
                case "3":
                    await EditProduct(_repo);
                    break;
                case "4":
                    await DeleteProduct(_repo);
                    break;
                case "5":
                    await SearchProduct(_repo);
                    break;
                case "6":
                    Console.WriteLine("Exiting... Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine("\nPress Enter to return to menu...");
            Console.ReadLine();
        }
    }

    static void MainMenu()
    {
        const string hr = "============================================";
        Console.Clear();
        Console.WriteLine(hr);
        Console.WriteLine("\tSimple Inventory Management System");
        Console.WriteLine(hr);
        Console.WriteLine("\t[1] Add Product");
        Console.WriteLine("\t[2] View All Products");
        Console.WriteLine("\t[3] Edit A Product");
        Console.WriteLine("\t[4] Delete A Product");
        Console.WriteLine("\t[5] Search For A Product");
        Console.WriteLine("\t[6] Exit");
        Console.WriteLine(hr);
    }

    static async Task AddProduct(ProductRepository repo)
    {
        Console.Write("Enter product name: ");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Product name cannot be empty.");
            return;
        }

        Console.Write("Enter quantity: ");
        var quantity = int.Parse(Console.ReadLine());
        if (quantity <= 0)
        {
            Console.WriteLine("Quantity must be greater than zero.");
            return;
        }

        Console.Write("Enter price: ");
        var price = float.Parse(Console.ReadLine());
        if (price <= 0)
        {
            Console.WriteLine("Price must be greater than zero.");
            return;
        }

        await repo.AddProductAsync(new Product { Name = name, Quantity = quantity, Price = price });
        Console.WriteLine("Product added successfully.");
    }

    static async Task ViewAllProducts(ProductRepository repo)
    {
        var products = await repo.GetAllProductsAsync();
        Console.WriteLine("\n--- Product List ---");
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Id}: {product.Name} - {product.Quantity} pcs - {product.Price:C}");
        }
    }

    static async Task EditProduct(ProductRepository repo)
    {
        Console.Write("Enter product ID to edit: ");
        int id = int.Parse(Console.ReadLine());
        if (id <= 0)
        {
            Console.WriteLine("Invalid product ID.");
            return;
        }

        // Check if the product exists
        var existing = await repo.GetProductByIdAsync(id);
        if (existing == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        Console.Write("Enter new name (leave blank to keep current): ");
        var name = Console.ReadLine();

        Console.Write("Enter new quantity (leave blank to keep current): ");
        var quantityStr = Console.ReadLine();

        Console.Write("Enter new price (leave blank to keep current): ");
        var priceStr = Console.ReadLine();

        existing.Name = string.IsNullOrWhiteSpace(name) ? existing.Name : name;
        existing.Quantity = string.IsNullOrWhiteSpace(quantityStr) ? existing.Quantity : int.Parse(quantityStr);
        existing.Price = string.IsNullOrWhiteSpace(priceStr) ? existing.Price : float.Parse(priceStr);

        await repo.UpdateProductAsync(existing);
        Console.WriteLine("Product updated successfully.");
    }

    static async Task DeleteProduct(ProductRepository repo)
    {
        Console.Write("Enter product ID to delete: ");
        int id = int.Parse(Console.ReadLine());
        if (id <= 0)
        {
            Console.WriteLine("Invalid product ID.");
            return;
        }

        await repo.DeleteProductAsync(id);
        Console.WriteLine("Product deleted successfully.");
    }

    static async Task SearchProduct(ProductRepository repo)
    {
        Console.Write("Enter product ID to search: ");
        int id = int.Parse(Console.ReadLine());
        if (id <= 0)
        {
            Console.WriteLine("Invalid product ID.");
            return;
        }

        var product = await repo.GetProductByIdAsync(id);
        if (product == null)
        {
            Console.WriteLine("Product not found.");
        }
        else
        {
            Console.WriteLine($"Found: {product.Id}: {product.Name} - {product.Quantity} pcs - {product.Price:C}");
        }
    }
}
