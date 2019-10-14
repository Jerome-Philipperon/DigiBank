using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace WebAPIWPF
{
    public class WebTransferClients
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


        public WebTransferClients(string id, string firstName, string lastName, DateTime dateOfBirth, string street, string zipCode, string city)
        {
            ClientId = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Street = street;
            ZipCode = zipCode;
            City = city;
        }

    }
}
