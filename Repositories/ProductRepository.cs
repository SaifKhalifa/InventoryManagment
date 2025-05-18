using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using InventoryManagment.Models;
using Models;

namespace InventoryManagment.Repositories;

public class ProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public bool CheckDBConnection()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return true;
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Database connection error: " + ex.Message);
            return false;
        }
    }

    public async Task AddProductAsync(Product product)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            string query = "INSERT INTO Products (Name, Quantity, Price) VALUES (@name, @quantity, @price)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@price", product.Price);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        var products = new List<Product>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            string query = "SELECT * FROM Products";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    products.Add(new Product
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Quantity = (int)reader["Quantity"],
                        Price = Convert.ToSingle(reader["Price"])
                    });
                }
            }
        }

        return products;
    }
    public async Task<Product> GetProductByIdAsync(int id)
    {
        Product product = null;
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            string query = "SELECT * FROM Products WHERE Id = @id";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        product = new Product
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Quantity = (int)reader["Quantity"],
                            Price = Convert.ToSingle(reader["Price"])
                        };
                    }
                }
            }
        }
        return product;
    }
    public async Task DeleteProductAsync(int id)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            string query = "DELETE FROM Products WHERE Id = @id";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
