namespace Models;
class Product
{
    string name { get; set; }
    int quantity { get; set; }
    float price { get; set; }

    public Product(string name, int quantity, float price)
    {
        this.name = name;
        this.quantity = quantity;
        this.price = price;
    }
}