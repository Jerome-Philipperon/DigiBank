using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace DomainModel
{
    public class Client : Person
    {
        [JsonPropertyName("Street")]
        [Required]
        public string Street { get; set; }
        [JsonPropertyName("ZipCode")]
        [Required]
        public string ZipCode { get; set; }
        [Required]
        [JsonPropertyName("City")]
        public string City { get; set; }
        [JsonPropertyName("MyEmployee")]
        public Employee MyEmployee { get; set; }
        [JsonIgnore]
        public ICollection<Account> MyAccounts { get; set; }
        
    }
}
