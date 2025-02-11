﻿namespace Models;
public class Inventory
{
    List<Product> products = new List<Product>();
    
    //Error handling
    private const string 
        ERROR_EMPTY = "\aThe inventory is empty.",
        ERROR_NULL_NAME = "\aThe product name cannot be null or empty.",
        ERROR_INVALID_PRICE = "\aThe product price must be greater than zero",
        ERROR_INVALID_QUANTITY = "\aThe product quantity cannot be negative.";
    private enum ErrorCode
    {
        NullName = 1,
        InvalidPrice = 2,
        InvalidQuantity = 3,
        EmptyInventory = 4
    }
    
    string productName = "";
    float price = 0;
    int quantity = 0;

    private void PrintError(ErrorCode errorCode)
    {
        switch (errorCode)
        {
            case ErrorCode.NullName:
                Console.WriteLine(ERROR_NULL_NAME);
                break;
            case ErrorCode.InvalidPrice:
                Console.WriteLine(ERROR_INVALID_PRICE);
                break;
            case ErrorCode.InvalidQuantity:
                Console.WriteLine(ERROR_INVALID_QUANTITY);
                break;
            case ErrorCode.EmptyInventory:
                Console.WriteLine(ERROR_EMPTY);
                break;
        }
    }
    
    public void AddProduct()
    {
        Console.Write("Enter product name: ");
        productName = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(productName)) // Error handle
        {
            PrintError(ErrorCode.NullName);
            return;
        }

        Console.Write("Enter product price: ");
        if (!float.TryParse(Console.ReadLine(), out price) || price <= 0) // Error handle
        {
            PrintError(ErrorCode.InvalidPrice);
            return;
        }

        Console.Write("Enter product quantity: ");
        if (!int.TryParse(Console.ReadLine(), out quantity) || quantity < 0) // Error handle
        {
            PrintError(ErrorCode.InvalidQuantity);
            return;
        }

        products.Add(new Product(productName, quantity, price));
        Console.WriteLine($"Product '{productName}' successfully added to te inventory!");
    }

    public void PrintAllProducts()
    {
        if (products.Count > 0)
        {
            foreach (var prod in products)
            {
                Console.WriteLine($"Product name : '{prod.Name}'\n" +
                                  $"Quantity : '{prod.Quantity}'\n" +
                                  $"Price : '{prod.Price}'");
                Console.WriteLine("----------");
            }
        }
        else
            PrintError(ErrorCode.NullName);
    }

    public int EditProduct()
    {
    }

    public bool DeleteProduct()
    {
        Console.Write("Enter product name to search for: ");
        productName = Console.ReadLine()?.Trim();
        
        if (string.IsNullOrWhiteSpace(productName)) // Error handle
        {
            PrintError(ErrorCode.NullName);
            return false;
        }
        
        int index = SearchProduct(false, productName);
        
        products.RemoveAt(index);
        return true;
    }

    public int SearchProduct(bool displayDetails = false, string productName = "")
    {
        Console.Write("Enter product name to search for: ");
        productName = Console.ReadLine()?.Trim();
        
        if (string.IsNullOrWhiteSpace(productName)) // Error handle
        {
            PrintError(ErrorCode.NullName);
            return -1;
        }

        for (int i = 0; i < products.Count; i++)
        {
            if (products[i].Name.Equals(productName, StringComparison.OrdinalIgnoreCase))
            {
                if (displayDetails)
                {
                    Console.WriteLine("\nProduct found! Here are the details:");
                    Console.WriteLine($"Product Name: {products[i].Name}");
                    Console.WriteLine($"Quantity: {products[i].Quantity}");
                    Console.WriteLine($"Price: {products[i].Price}");
                }
                return i; // Return the index
            }
        }
        Console.WriteLine($"\aThere's no product with name '{productName}'!");
        return -1;
    }
    
}