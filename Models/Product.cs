namespace Models;
class Product
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public float Price { get; set; }

    public Product() {}

    public Product(string name, int quantity, float price)
    {
        this.Name = name;
        this.Quantity = quantity;
        this.Price = price;
    }
}