using System.Text.Json;

namespace MauiApp1;

[QueryProperty(nameof(PizzaId), "pizzaId")]
public partial class ConstructorPage : ContentPage
{
    private Pizza _pizza;
    private List<PizzaSize> _sizes;
    private List<Ingredient> _ingredients;
    private int _quantity = 1;
    private PizzaSize _selectedSize;

    public int PizzaId { get; set; }

    public ConstructorPage()
    {
        InitializeComponent();
        InitializeSizes();
    }

    private void InitializeSizes()
    {
        _sizes = new List<PizzaSize>
        {
            new PizzaSize { Size = "Маленькая", Multiplier = 1.0m },
            new PizzaSize { Size = "Средняя", Multiplier = 1.3m },
            new PizzaSize { Size = "Большая", Multiplier = 1.6m }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("data.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var rootData = JsonSerializer.Deserialize<MainPage.RootData>(json, options);

            if (rootData != null)
            {
           
                _pizza = rootData.Pizzas.FirstOrDefault(p => p.Id == PizzaId);

                if (_pizza == null)
                {
                    await DisplayAlert("Ошибка", "Пицца не найдена", "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }


                _ingredients = rootData.Ingredients;

                DisplayPizzaInfo();
                DisplayIngredients();
                CalculatePrice();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка данных", ex.Message, "OK");
        }
    }

    private void DisplayPizzaInfo()
    {
        PizzaNameLabel.Text = _pizza.Name;
        PizzaImage.Source = _pizza.ImageUrl;

        SizePicker.ItemsSource = _sizes.Select(s => s.Size).ToList();
        SizePicker.SelectedIndex = 0;
        _selectedSize = _sizes[0];
    }

    private void DisplayIngredients()
    {
        IngredientsLayout.Children.Clear();
        foreach (var ingredient in _ingredients)
        {
            ingredient.IsSelected = false;

            var checkBox = new CheckBox { VerticalOptions = LayoutOptions.Center };
            checkBox.CheckedChanged += (s, e) => CalculatePrice();

            var label = new Label
            {
                Text = $"{ingredient.Name} (+{ingredient.Price} Рублей)",
                VerticalOptions = LayoutOptions.Center,
                FontSize = 16
            };

            var stack = new HorizontalStackLayout { Children = { checkBox, label }, Spacing = 10 };


            stack.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    checkBox.IsChecked = !checkBox.IsChecked;
                    ingredient.IsSelected = checkBox.IsChecked;
                    CalculatePrice();
                })
            });

            IngredientsLayout.Children.Add(stack);
        }
    }

    private void SizePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (SizePicker.SelectedIndex >= 0)
        {
            _selectedSize = _sizes[SizePicker.SelectedIndex];
            CalculatePrice();
        }
    }

    private void CalculatePrice()
    {
        if (_pizza == null) return;

        var selectedIngredients = _ingredients.Where(i => i.IsSelected).ToList();
        var ingredientsPrice = selectedIngredients.Sum(i => i.Price);
        var pizzaPrice = (_pizza.BasePrice * _selectedSize.Multiplier) + ingredientsPrice;
        var totalPrice = pizzaPrice * _quantity;

        PriceLabel.Text = $"{totalPrice} Рублей";
    }

    private void IncreaseQuantity_Clicked(object sender, EventArgs e)
    {
        _quantity++;
        QuantityLabel.Text = _quantity.ToString();
        CalculatePrice();
    }

    private void DecreaseQuantity_Clicked(object sender, EventArgs e)
    {
        if (_quantity > 1)
        {
            _quantity--;
            QuantityLabel.Text = _quantity.ToString();
            CalculatePrice();
        }
    }

    private async void AddToCart_Clicked(object sender, EventArgs e)
    {
        var selectedIngredients = _ingredients.Where(i => i.IsSelected).ToList();

        var cartItem = new CartItem
        {
            Pizza = _pizza,
            Size = _selectedSize,
            SelectedIngredients = selectedIngredients,
            Quantity = _quantity
        };

        CartService.Add(cartItem);
        await DisplayAlert("Успешно", "Пицца добавлена в корзину!", "OK");
        await Shell.Current.GoToAsync("..");
    }
}