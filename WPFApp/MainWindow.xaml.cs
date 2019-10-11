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
        static readonly HttpClient client = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
            GetAndInitializeListClients();
            GetAndInitializeOngletStatistique();

        }


        private async void GetAndInitializeListClients()
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
                ListClient.Children.Add(comboBox);
            }
            catch (HttpRequestException e)
            {
                Body.Text = "\nException Caught!";
                Body.Text += "Message :{0} " + e.Message;
            }
        }

        /// <summary>
        /// Récuparation des donnés de la wabAPI et initialisation des donnés dans l'application
        /// </summary>
        private async void GetAndInitializeOngletStatistique()
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
                for (int it =0; it < responceTab.Count(); it++)
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
                Body.Text ="\nException Caught!";
                Body.Text += "Message :{0} " + e.Message;
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
            HttpResponseMessage responseHTTP;
            string responseBody;
            Client currentClient;
            try
            {
                DetailClient.Children.Clear();
                var comboBox = (ComboBox)sender;
                var item = (ComboBoxItem)comboBox.SelectedItem;

                responseHTTP = await client.GetAsync("https://localhost:44326/api/clients/"+ item.DataContext.ToString());
                responseHTTP.EnsureSuccessStatusCode();
                responseBody = await responseHTTP.Content.ReadAsStringAsync();
                //MessageBox.Show(responseBody);
                currentClient = Client.ParseClient(responseBody);
                TextBlock textBlock = new TextBlock();
                textBlock.Text = $"Nom : {currentClient.LastName} {Environment.NewLine}";
                textBlock.Text += $"Prénom : {currentClient.FirstName} {Environment.NewLine}";
                textBlock.Text += $"Date de Naissance : {currentClient.DateOfBirth} {Environment.NewLine}";
                textBlock.Text += $"Address : {currentClient.Street} {Environment.NewLine}";
                textBlock.Text += $"Code Postal : {currentClient.ZipCode} {Environment.NewLine}";
                textBlock.Text += $"Ville : {currentClient.City} {Environment.NewLine}";
                DetailClient.Children.Add(textBlock);
            }
            catch (HttpRequestException exception)
            {
                Body.Text = "\nException Caught!";
                Body.Text += "Message :{0} " + exception.Message;
            }
            
        }
    }
}
