using System.Text.Json;

namespace MauiApp1;

public partial class CheckoutPage : ContentPage
{
    public CheckoutPage()
    {
        InitializeComponent();
        OrderTotalLabel.Text = $"Сумма заказа: {CartService.Total} Рублей";
    }

    private async void SubmitOrder_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(PhoneEntry.Text) || string.IsNullOrWhiteSpace(AddressEntry.Text))
        {
            await DisplayAlert("Ошибка", "Заполните все поля", "ОК");
            return;
        }


        var order = new Order
        {
            CustomerName = NameEntry.Text,
            CustomerPhone = PhoneEntry.Text,
            DeliveryAddress = AddressEntry.Text,
            Items = CartService.Items.ToList(),
            OrderDate = DateTime.Now
        };


        await OrderService.SaveOrder(order);


        await DisplayAlert(
            "Заказ принят!",
            $"Номер: {order.Id}\nСумма: {order.TotalAmount} Рублей\n",
            "OK");


        CartService.Clear();
        await Shell.Current.GoToAsync("..");
    }
}