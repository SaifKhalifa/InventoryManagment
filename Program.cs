using Models;

namespace InventoryManagment;

class Program
{
    static void Main(string[] args)
    {
        var inventory = new Inventory();
        var exit = false;

        while (!exit)
        {
            MainMenu();
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    inventory.AddProduct();
                    break;
                case "2":
                    inventory.PrintAllProducts();
                    break;
                case "3":
                    if (inventory.EditProduct())
                    {
                        Console.WriteLine("Product edited!");
                    }
                    else
                    {
                        Console.WriteLine("Product not edited!");
                    }
                    break;
                case "4":
                    if (inventory.DeleteProduct())
                    {
                        Console.WriteLine("Product deleted!");
                    }
                    else
                    {
                        Console.WriteLine("Product not deleted!");
                    }
                    break;
                case "5":
                    inventory.SearchProduct(true);
                    break;
                case "6":
                    exit = true;
                    Console.WriteLine("Bye bye...");
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            
            Console.Write("\nPress any key to go back to the main menu. . . . ");
            Console.ReadKey();
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
}