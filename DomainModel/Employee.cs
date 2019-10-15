using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace DomainModel
{
    public class Employee : Person
    {
        [JsonPropertyName("officeName")]
        [Required]
        public string OfficeName { get; set; }
        [JsonPropertyName("isJunior")]
        [Required]
        public bool IsJunior { get; set; }
        [JsonPropertyName("myManager")]
        public Manager MyManager{ get; set; }
        [JsonIgnore]
        public List<Client> MyClients { get; set; }
    }
}
