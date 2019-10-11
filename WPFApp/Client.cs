using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WPFApp
{
    class Client
    {
        [JsonPropertyName("id")]
        public string ClientId { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("zipCode")]
        public string ZipCode { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        public static List<Client> Parse(string json) =>
        System.Text.Json.JsonSerializer.Deserialize<List<Client>>(json);

        public static Client ParseClient(string json) =>
        System.Text.Json.JsonSerializer.Deserialize<Client>(json);
    }
}
