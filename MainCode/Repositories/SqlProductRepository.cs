using Microsoft.Data.SqlClient;
using InventoryManagment.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Models;

namespace InventoryManagment.Repositories;

public class SqlProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public SqlProductRepository(string? overrideConnectionString = null)
    {
        _connectionString = overrideConnectionString
            ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
            ?? "Server=SAIFKHALIFA-PC;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True;";
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
            Console.WriteLine("SQL error: " + ex.Message);
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

    public async Task<Product?> GetProductByIdAsync(int id)
    {
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
                        return new Product
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
        return null;
    }

    public async Task UpdateProductAsync(Product product)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            string query = "UPDATE Products SET Name = @name, Quantity = @quantity, Price = @price WHERE Id = @id";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", product.Id);
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@price", product.Price);
                await cmd.ExecuteNonQueryAsync();
            }
        }
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
