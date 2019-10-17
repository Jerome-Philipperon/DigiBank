using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace DomainModel
{
    public class Deposit : Account
    {
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public decimal AutorizedOverdraft { get; set; }
        public decimal FreeOverdraft { get; set; }
        [Required]
        public decimal OverdraftChargeRate { get; set; }
        [JsonIgnore]
        public ICollection<Card> DepositCards { get; set; }
    }
}
