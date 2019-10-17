using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace DomainModel
{
    public class Client : Person
    {
        [Required]
        public string Street { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }
        public Employee MyEmployee { get; set; }
        [JsonIgnore]
        public ICollection<Account> MyAccounts { get; set; }
        
    }
}
