namespace MauiApp1;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        Routing.RegisterRoute("constructor", typeof(ConstructorPage));
        Routing.RegisterRoute("cart", typeof(CartPage));
        Routing.RegisterRoute("checkout", typeof(CheckoutPage));

        MainPage = new AppShell();
    }
}