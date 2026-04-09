namespace MauiApp1;

public partial class CartPage : ContentPage
{
    public CartPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadCart();
    }

    private void LoadCart()
    {
        CartList.ItemsSource = CartService.Items.ToList();
        UpdateTotal();
    }

    private void UpdateTotal()
    {
        TotalLabel.Text = $"Итого: {CartService.Total} Рублей";
        CheckoutButton.IsEnabled = CartService.Count > 0;
    }

    private void IncreaseQuantity_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = (CartItem)button.BindingContext;
        item.Quantity++;
        LoadCart();
    }

    private void DecreaseQuantity_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = (CartItem)button.BindingContext;
        if (item.Quantity > 1)
        {
            item.Quantity--;
            LoadCart();
        }
    }

    private void RemoveItem_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = (CartItem)button.BindingContext;
        CartService.Remove(item);
        LoadCart();
    }

    private async void Checkout_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("checkout");
    }
}