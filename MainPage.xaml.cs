using System.Text.Json;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
    private List<Pizza> _pizzas;

    public MainPage()
    {
        InitializeComponent();

        LoadDataAsync();
    }

    private async void LoadDataAsync()
    {
        try
        {

            using var stream = await FileSystem.OpenAppPackageFileAsync("data.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();


            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var rootData = JsonSerializer.Deserialize<RootData>(json, options);

            if (rootData != null)
            {
                _pizzas = rootData.Pizzas;
                PizzaList.ItemsSource = _pizzas;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось загрузить меню: {ex.Message}", "OK");
        }
        finally
        {
            UpdateCartButton();
        }
    }


    public class RootData
    {
        public List<Pizza> Pizzas { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }

    private void UpdateCartButton()
    {
        CartButton.Text = $"🛒 Корзина ({CartService.Count})";
    }

    private async void SelectPizza_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var pizza = (Pizza)button.BindingContext;

        await Shell.Current.GoToAsync($"constructor?pizzaId={pizza.Id}");
    }

    private async void CartButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("cart");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateCartButton();
    }
}