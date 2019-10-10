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
using DAL;
using DomainModel;
using System.IO;
using System.Runtime.Serialization.Json;

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
            Connection();
        }

        /// <summary>
        /// Récuparation des donnés de la wabAPI et initialisation des donnés dans l'application
        /// </summary>
        private async void Connection()
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

        private void AffichageInfoClient(object sender, RoutedEventArgs e)
        {
            Info_Client.Visibility = Visibility.Visible;
            Statistiques.Visibility = Visibility.Collapsed;
        }

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
            var deserializedUser = new List<List<string>>();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var ser = new DataContractJsonSerializer(deserializedUser.GetType());
            deserializedUser = ser.ReadObject(ms) as List<List<string>>;
            ms.Close();
            return deserializedUser;
        }
    }
}
