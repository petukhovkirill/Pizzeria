using System.IO;
using System.Text.Json;

namespace MauiApp1;

public static class OrderService
{

    private static string FilePath => Path.Combine(FileSystem.AppDataDirectory, "orders.json");


    public static async Task SaveOrder(Order order)
    {
        List<Order> orders = new List<Order>();


        if (File.Exists(FilePath))
        {
            var json = await File.ReadAllTextAsync(FilePath);

            orders = JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
        }


        orders.Add(order);


        var newJson = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(FilePath, newJson);
    }


    public static async Task<List<Order>> GetOrdersAsync()
    {
        if (!File.Exists(FilePath)) return new List<Order>();

        var json = await File.ReadAllTextAsync(FilePath);
        return JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
    }
}