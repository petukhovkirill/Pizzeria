namespace MauiApp1;

public class Pizza
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal BasePrice { get; set; }
    public string ImageUrl { get; set; } = "";
}

public class PizzaSize
{
    public string Size { get; set; } = "";
    public decimal Multiplier { get; set; }
}

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public bool IsSelected { get; set; }
}

public class CartItem
{
    public Pizza Pizza { get; set; } = new Pizza();
    public PizzaSize Size { get; set; } = new PizzaSize();
    public List<Ingredient> SelectedIngredients { get; set; } = new List<Ingredient>();
    public int Quantity { get; set; } = 1;

    public decimal TotalPrice =>
        (Pizza.BasePrice * Size.Multiplier + SelectedIngredients.Where(i => i.IsSelected).Sum(i => i.Price)) * Quantity;
}

public class Order
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8].ToUpper();
    public List<CartItem> Items { get; set; } = new List<CartItem>();
    public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string CustomerName { get; set; } = "";
    public string CustomerPhone { get; set; } = "";
    public string DeliveryAddress { get; set; } = "";
}