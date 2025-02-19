using Models;

namespace InventoryManagment.Models;
public class Inventory
{
    private List<Product> _products = new List<Product>();
    
    //Error handling
    private const string 
        ErrorEmpty = "\aThe inventory is empty.",
        ErrorNullName = "\aThe product name cannot be null or empty.",
        ErrorInvalidPrice = "\aThe product price must be greater than zero",
        ErrorInvalidQuantity = "\aThe product quantity cannot be negative.";
    private enum ErrorCode
    {
        NullName = 1,
        InvalidPrice = 2,
        InvalidQuantity = 3,
        EmptyInventory = 4
    }

    private string _productName = "";
    private float _price = 0;
    private int _quantity = 0;

    private void PrintError(ErrorCode errorCode)
    {
        switch (errorCode)
        {
            case ErrorCode.NullName:
                Console.WriteLine(ErrorNullName);
                break;
            case ErrorCode.InvalidPrice:
                Console.WriteLine(ErrorInvalidPrice);
                break;
            case ErrorCode.InvalidQuantity:
                Console.WriteLine(ErrorInvalidQuantity);
                break;
            case ErrorCode.EmptyInventory:
                Console.WriteLine(ErrorEmpty);
                break;
            default:
                Console.WriteLine("Unknown error");
                break;
        }
    }
    
    public void AddProduct()
    {
        Console.Write("Enter product name: ");
        _productName = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(_productName)) // Error handle
        {
            PrintError(ErrorCode.NullName);
            return;
        }

        Console.Write("Enter product price: ");
        if (!float.TryParse(Console.ReadLine(), out _price) || _price <= 0) // Error handle
        {
            PrintError(ErrorCode.InvalidPrice);
            return;
        }

        Console.Write("Enter product quantity: ");
        if (!int.TryParse(Console.ReadLine(), out _quantity) || _quantity < 0) // Error handle
        {
            PrintError(ErrorCode.InvalidQuantity);
            return;
        }

        _products.Add(new Product(_productName, _quantity, _price));
        Console.WriteLine($"Product '{_productName}' successfully added to te inventory!");
    }

    public void PrintAllProducts()
    {
        if (_products.Count > 0)
        {
            foreach (var prod in _products)
            {
                Console.WriteLine($"Product name : '{prod.Name}'\n" +
                                  $"Quantity : '{prod.Quantity}'\n" +
                                  $"Price : '{prod.Price}'");
                Console.WriteLine("----------");
            }
        }
        else
            PrintError(ErrorCode.EmptyInventory);
    }

    public bool EditProduct()
    {
        int index = SearchProduct();
        
        if (index == -1) 
        {
            Console.WriteLine("Product not found. Cannot edit.");
            return false;
        }
        
        Product product = _products[index];

        Console.WriteLine($"Current product name: {_products[index].Name}\n," +
                          $"Current quantity: {_products[index].Quantity}\n," +
                          $"Current price: {_products[index].Price}\n");
        
        Console.Write("Enter new product name (LEAVE IT BLANK TO KEEP IT AS IT'S): ");
        _productName = Console.ReadLine()?.Trim();
        if (!string.IsNullOrWhiteSpace(_productName))
        {
            product.Name = _productName;
        }
        
        Console.Write("Enter new product price (LEAVE IT BLANK TO KEEP IT AS IT'S): ");
        
        string priceInput = Console.ReadLine()?.Trim();
        if (!string.IsNullOrWhiteSpace(priceInput) && float.TryParse(priceInput, out float newPrice) && newPrice > 0)
        {
            product.Price = newPrice;
        }

        Console.Write("Enter new product quantity (LEAVE IT BLANK TO KEEP IT AS IT'S): ");
        string quantityInput = Console.ReadLine()?.Trim();
        if (!string.IsNullOrWhiteSpace(quantityInput) && int.TryParse(quantityInput, out int newQuantity) && newQuantity >= 0)
        {
            product.Quantity = newQuantity;
        }
        return true;
    }
    
    public bool DeleteProduct()
    {
        int index = SearchProduct();
        if (index >= 0) _products.RemoveAt(index);
        else return false;
        
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

        for (int i = 0; i < _products.Count; i++)
        {
            if (_products[i].Name.Equals(productName, StringComparison.OrdinalIgnoreCase))
            {
                if (displayDetails)
                {
                    Console.WriteLine("\nProduct found! Here are the details:");
                    Console.WriteLine($"Product Name: {_products[i].Name}");
                    Console.WriteLine($"Quantity: {_products[i].Quantity}");
                    Console.WriteLine($"Price: {_products[i].Price}");
                }
                return i; // Return the index
            }
        }
        Console.WriteLine($"\aThere's no product with name '{productName}'!");
        return -1;
    }
    
}