using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text;
using System.Windows;

namespace Client;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    HttpClient Client = new();
    HttpRequestMessage message = new();
    private List<Car> cars;

    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public List<Car> Cars { get => cars; set { cars = value; NotifyPropertyChanged(); } }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        Cars = new();
    }

    private async void GETButton_Click(object sender, RoutedEventArgs e)
    {
        message = new();
        Client = new();
        message.RequestUri = new Uri(@"http://localhost:27001/");
        message.Method = HttpMethod.Get;


        var response = await Client.GetAsync(message.RequestUri);
        var json = await response.Content.ReadAsStringAsync();
        Cars = JsonSerializer.Deserialize<List<Car>>(json);

    }
    private async void DELETEButton_Click(object sender, RoutedEventArgs e)
    {
        message = new();
        Client = new();
        var SelectedItem = CarBox.SelectedItem as Car;
        var Id = SelectedItem.Id;

        message.Method = HttpMethod.Delete;
        message.RequestUri = new Uri(@"http://localhost:27001/");
        message.Content = new StringContent(Id.ToString());

        var response = await Client.SendAsync(message);
        var json = await response.Content.ReadAsStringAsync();

        MessageBox.Show(json);
    }
    private async void POSTButton_Click(object sender, RoutedEventArgs e)
    {
        var CarName = ModelBox.Text;
        message = new();
        Client = new();

        message.RequestUri = new Uri(@"http://localhost:27001/");
        message.Method = HttpMethod.Post;

        message.Content = new StringContent(CarName);

        var response = await Client.PostAsync(message.RequestUri, message.Content);
        var json = await response.Content.ReadAsStringAsync();
        MessageBox.Show(json);
    }
    private async void PUTButton_Click(object sender, RoutedEventArgs e)
    {
        message = new();
        Client = new();
        message.RequestUri = new Uri(@"http://localhost:27001/");
        message.Method = HttpMethod.Put;
        var SelectedItemId = (CarBox.SelectedItem as Car).Id;
        var EditName = ModelBox.Text;

        var content = new
        {
            Id = SelectedItemId,
            Model = EditName
        };

        message.Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
        var response = await Client.PutAsync(message.RequestUri, message.Content);
        var json = await response.Content.ReadAsStringAsync();
        MessageBox.Show(json);
    }
}