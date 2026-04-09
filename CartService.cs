namespace MauiApp1;

public static class CartService
{
    public static List<CartItem> Items { get; } = new List<CartItem>();

    public static int Count => Items.Count;

    public static decimal Total => Items.Sum(i => i.TotalPrice);

    public static void Add(CartItem item)
    {
        Items.Add(item);
    }

    public static void Remove(CartItem item)
    {
        Items.Remove(item);
    }

    public static void Clear()
    {
        Items.Clear();
    }
}