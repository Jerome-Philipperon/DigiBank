using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Globalization;
using System.Windows.Markup;


namespace WPFApp.Converters
{
    public partial class Converter : MarkupExtension, IValueConverter
    {
        private static Converter _instance;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result = Double.Parse(value.ToString()) - Double.Parse(parameter.ToString());// * System.Convert.ToDouble(parameter);
            //MessageBox(result);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new Converter());
        }
    }
}

namespace WPFApp
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //static readonly HttpClient client = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
            GetAndInitializeListClients();
            GetAndInitializeOngletStatistique();

        }


        private async void GetAndInitializeListClients()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseHTTP;
                string responseBody;
                List<Client> Clients;

                try
                {
                    responseHTTP = await client.GetAsync("https://localhost:44326/api/clients");
                    responseHTTP.EnsureSuccessStatusCode();
                    responseBody = await responseHTTP.Content.ReadAsStringAsync();
                    Clients = Client.Parse(responseBody);

                    ComboBox comboBox = new ComboBox();
                    for (int i = 0; i < Clients.Count(); i++)
                    {
                        ComboBoxItem Item = new ComboBoxItem();
                        Item.Content = $"Mme, Mr {Clients[i].FirstName} {Clients[i].LastName}";
                        Item.DataContext = Clients[i].ClientId;
                        comboBox.Items.Add(Item);

                    }
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = "-- Select Client--";
                    comboBoxItem.IsEnabled = false;
                    comboBox.Items.Add(comboBoxItem);
                    comboBox.SelectedItem = comboBoxItem;

                    comboBox.SelectionChanged += Details;
                    comboBox.Margin = new Thickness(10, 10, 10, 0);
                    ListClient.Children.Add(comboBox);
                }
                catch (HttpRequestException e)
                {
                    Body.Text = "\nException Caught! \n";
                    Body.Text += "Message : " + e.Message;
                }
            }
        }

        /// <summary>
        /// Récuparation des donnés de la wabAPI et initialisation des donnés dans l'application
        /// </summary>
        private async void GetAndInitializeOngletStatistique()
        {
            using (HttpClient client = new HttpClient())
            {
                //Variables
                HttpResponseMessage responseHTTP;
                string responseBody;
                List<List<string>> responceTab;

                try
                {
                    // Nombre de clients totals
                    responseHTTP = await client.GetAsync("https://localhost:44326/api/clients/NbClient");
                    responseHTTP.EnsureSuccessStatusCode();
                    responseBody = await responseHTTP.Content.ReadAsStringAsync();
                    Body.Text = $"La banque a {responseBody} clients {Environment.NewLine} {Environment.NewLine}";

                    // Nombre de client par manager
                    responseHTTP = await client.GetAsync("https://localhost:44326/api/clients/NbClientByManager");
                    responseHTTP.EnsureSuccessStatusCode();
                    responseBody = await responseHTTP.Content.ReadAsStringAsync();
                    responceTab = ReadToObject(responseBody);
                    Body.Text += $"Nombre de client par manager : {Environment.NewLine}";
                    for (int it = 0; it < responceTab.Count(); it++)
                    {
                        Body.Text += $"\t - {responceTab[it][0]} a {responceTab[it][1]} clients  {Environment.NewLine}";
                    }
                    Body.Text += Environment.NewLine;

                    //Get Somme Epargne Par Clients
                    responseHTTP = await client.GetAsync("https://localhost:44326/api/clients/SavingsAmountByClients");
                    responseHTTP.EnsureSuccessStatusCode();
                    responseBody = await responseHTTP.Content.ReadAsStringAsync();
                    responceTab = ReadToObject(responseBody);
                    Body.Text += $"Somme épargné par les clients de la banque : {Environment.NewLine}";
                    for (int it = 0; it < responceTab.Count(); it++)
                    {
                        Body.Text += $"\t - {responceTab[it][0]} a épargné {responceTab[it][1]} €  {Environment.NewLine}";
                    }
                    Body.Text += Environment.NewLine;

                    //Get Somme Epargne Par Clients Par Manager
                    responseHTTP = await client.GetAsync("https://localhost:44326/api/clients/SavingsAmountByClientsByManager");
                    responseHTTP.EnsureSuccessStatusCode();
                    responseBody = await responseHTTP.Content.ReadAsStringAsync();
                    responceTab = ReadToObject(responseBody);
                    Body.Text += $"Somme épargné par les clients par manager : {Environment.NewLine}";
                    for (int it = 0; it < responceTab.Count(); it++)
                    {
                        Body.Text += $"\t - Les Clients de {responceTab[it][0]} ont épargné {responceTab[it][1]} €  {Environment.NewLine}";
                    }
                    Body.Text += Environment.NewLine;

                    //solde total de l’ensemble des comptes de la banque 
                    responseHTTP = await client.GetAsync("https://localhost:44326/api/clients/TotalBalanceOfBank");
                    responseHTTP.EnsureSuccessStatusCode();
                    responseBody = await responseHTTP.Content.ReadAsStringAsync();
                    Body.Text += $"Solde total de l'ensemble des comptes de la banque : {responseBody} € {Environment.NewLine} {Environment.NewLine}";

                    //Le pourcentage de clients qui possèdent une carte bancaire
                    responseHTTP = await client.GetAsync("https://localhost:44326/api/clients/PercentageOfCustomersWhoHaveCreditCard");
                    responseHTTP.EnsureSuccessStatusCode();
                    responseBody = await responseHTTP.Content.ReadAsStringAsync();
                    Body.Text += $" {responseBody}% des clients possède une carte bancaire{Environment.NewLine} {Environment.NewLine}";

                    //Le pourcentage de clients qui possèdent une carte bancaire par manager 
                    responseHTTP = await client.GetAsync("https://localhost:44326/api/clients/PercentageOfCustomersWhoHaveCreditCardByManager");
                    responseHTTP.EnsureSuccessStatusCode();
                    responseBody = await responseHTTP.Content.ReadAsStringAsync();
                    responceTab = ReadToObject(responseBody);
                    Body.Text += $"Pourcentage de clients qui possède une carte bancaire par manager : {Environment.NewLine}";
                    for (int it = 0; it < responceTab.Count(); it++)
                    {
                        Body.Text += $"\t - {responceTab[it][1]}% des Clients de {responceTab[it][0]} ont une carte bancaire  {Environment.NewLine}";
                    }
                    Body.Text += Environment.NewLine;

                    //Le pourcentage de clients qui possèdent un compte d’épargne 
                    responseHTTP = await client.GetAsync("https://localhost:44326/api/clients/PercentageOfCustomersWhoHaveBankAccount");
                    responseHTTP.EnsureSuccessStatusCode();
                    responseBody = await responseHTTP.Content.ReadAsStringAsync();
                    Body.Text += $"{responseBody}% des clients possède un compte bancaire {Environment.NewLine} {Environment.NewLine}";

                    //Le pourcentage de clients qui possèdent un compte d’épargne par manager
                    responseHTTP = await client.GetAsync("https://localhost:44326/api/clients/PercentageOfCustomersWhoHaveBankAccountByManager");
                    responseHTTP.EnsureSuccessStatusCode();
                    responseBody = await responseHTTP.Content.ReadAsStringAsync();
                    responceTab = ReadToObject(responseBody);
                    Body.Text += $"Pourcentage de clients qui possède un compte épargne par manager : {Environment.NewLine}";
                    for (int it = 0; it < responceTab.Count(); it++)
                    {
                        Body.Text += $"\t - {responceTab[it][1]}% des Clients de {responceTab[it][0]} ont un compte épargne  {Environment.NewLine}";
                    }
                    Body.Text += Environment.NewLine;
                }
                catch (HttpRequestException e)
                {
                    Body.Text = "\nException Caught!";
                    Body.Text += "Message :{0} " + e.Message;
                }
            }
        }

        /// <summary>
        /// Affichage de l'onglet info clients 
        /// </summary>
        private void AffichageInfoClient(object sender, RoutedEventArgs e)
        {
            Info_Client.Visibility = Visibility.Visible;
            Statistiques.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Affichage de l'onglet statistique 
        /// </summary>
        private void AffichageStatistiques(object sender, RoutedEventArgs e)
        {
            Statistiques.Visibility = Visibility.Visible;
            Info_Client.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Convert Response HTTP (srting) to Json to List<List<string>>
        /// </summary>
        /// <param name="json"></param>
        /// <returns> List<List<string>> </returns>
        public static List<List<string>> ReadToObject(string json)
        {
            var deserialized = new List<List<string>>();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var ser = new DataContractJsonSerializer(deserialized.GetType());
            deserialized = ser.ReadObject(ms) as List<List<string>>;
            ms.Close();
            return deserialized;
        }

        /// <summary>
        /// Affichage du details du client 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Details(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseHTTP;
                string responseBody;
                Client currentClient;
                try
                {
                    DetailClient.Children.Clear();
                    var comboBox = (ComboBox)sender;
                    var item = (ComboBoxItem)comboBox.SelectedItem;

                    responseHTTP = await client.GetAsync("https://localhost:44326/api/clients/" + item.DataContext.ToString());
                    responseHTTP.EnsureSuccessStatusCode();
                    responseBody = await responseHTTP.Content.ReadAsStringAsync();

                    currentClient = Client.ParseClient(responseBody);
                    Grid grid = new Grid();
                    grid.Margin = new Thickness(20);
                    ColumnDefinition column = new ColumnDefinition();
                    column.Width = new GridLength(200);
                    grid.ColumnDefinitions.Add(column);
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    for (int i = 0; i < 6; i++)
                    {
                        grid.RowDefinitions.Add(new RowDefinition());
                    }
                    TextBlock Header1 = new TextBlock();
                    Header1.FontSize = 14;
                    Header1.TextDecorations = TextDecorations.Underline;
                    Header1.FontWeight = FontWeights.Bold;
                    Header1.Text = "Nom : ";
                    Grid.SetRow(Header1, 0);
                    Grid.SetColumn(Header1, 0);
                    grid.Children.Add(Header1);

                    TextBlock Header2 = new TextBlock();
                    Header2.FontSize = 14;
                    Header2.TextDecorations = TextDecorations.Underline;
                    Header2.FontWeight = FontWeights.Bold;
                    Header2.Text = "Prénom : ";
                    Grid.SetRow(Header2, 1);
                    Grid.SetColumn(Header2, 0);
                    grid.Children.Add(Header2);

                    TextBlock Header3 = new TextBlock();
                    Header3.FontSize = 14;
                    Header3.TextDecorations = TextDecorations.Underline;
                    Header3.FontWeight = FontWeights.Bold;
                    Header3.Text = "Date de Naissance : ";
                    Grid.SetRow(Header3, 2);
                    Grid.SetColumn(Header3, 0);
                    grid.Children.Add(Header3);

                    TextBlock Header4 = new TextBlock();
                    Header4.FontSize = 14;
                    Header4.TextDecorations = TextDecorations.Underline;
                    Header4.FontWeight = FontWeights.Bold;
                    Header4.Text = "Address : ";
                    Grid.SetRow(Header4, 3);
                    Grid.SetColumn(Header4, 0);
                    grid.Children.Add(Header4);

                    TextBlock Header5 = new TextBlock();
                    Header5.FontSize = 14;
                    Header5.TextDecorations = TextDecorations.Underline;
                    Header5.FontWeight = FontWeights.Bold;
                    Header5.Text = "Code Postal : ";
                    Grid.SetRow(Header5, 4);
                    Grid.SetColumn(Header5, 0);
                    grid.Children.Add(Header5);

                    TextBlock Header6 = new TextBlock();
                    Header6.FontSize = 14;
                    Header6.TextDecorations = TextDecorations.Underline;
                    Header6.FontWeight = FontWeights.Bold;
                    Header6.Text = "Ville : ";
                    Grid.SetRow(Header6, 5);
                    Grid.SetColumn(Header6, 0);
                    grid.Children.Add(Header6);

                    TextBlock Content1 = new TextBlock();
                    Content1.Text = $"{currentClient.LastName}";
                    Grid.SetRow(Content1, 0);
                    Grid.SetColumn(Content1, 1);
                    grid.Children.Add(Content1);

                    TextBlock Content2 = new TextBlock();
                    Content2.Text = $"{currentClient.FirstName}";
                    Grid.SetRow(Content2, 1);
                    Grid.SetColumn(Content2, 1);
                    grid.Children.Add(Content2);

                    TextBlock Content3 = new TextBlock();
                    Content3.Text = $"{currentClient.DateOfBirth}";
                    Grid.SetRow(Content3, 2);
                    Grid.SetColumn(Content3, 1);
                    grid.Children.Add(Content3);

                    TextBlock Content4 = new TextBlock();
                    Content4.Text = $"{currentClient.Street}";
                    Grid.SetRow(Content4, 3);
                    Grid.SetColumn(Content4, 1);
                    grid.Children.Add(Content4);

                    TextBlock Content5 = new TextBlock();
                    Content5.Text = $"{currentClient.ZipCode}";
                    Grid.SetRow(Content5, 4);
                    Grid.SetColumn(Content5, 1);
                    grid.Children.Add(Content5);

                    TextBlock Content6 = new TextBlock();
                    Content6.Text = $"{currentClient.City}";
                    Grid.SetRow(Content6, 5);
                    Grid.SetColumn(Content6, 1);
                    grid.Children.Add(Content6);

                    DetailClient.Children.Add(grid);

                    //TextBlock textBlock = new TextBlock();
                    //textBlock.Margin = new Thickness(10);
                    //textBlock.Text = $"Nom : {currentClient.LastName} {Environment.NewLine}";
                    //textBlock.Text += $"Prénom : {currentClient.FirstName} {Environment.NewLine}";
                    //textBlock.Text += $"Date de Naissance : {currentClient.DateOfBirth} {Environment.NewLine}";
                    //textBlock.Text += $"Address : {currentClient.Street} {Environment.NewLine}";
                    //textBlock.Text += $"Code Postal : {currentClient.ZipCode} {Environment.NewLine}";
                    //textBlock.Text += $"Ville : {currentClient.City} {Environment.NewLine}";
                    //DetailClient.Children.Add(textBlock);
                }
                catch (HttpRequestException exception)
                {
                    Body.Text = "\nException Caught!";
                    Body.Text += "Message :{0} " + exception.Message;
                }

            }
        }

        /// <summary>
        /// Exit Fonction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
