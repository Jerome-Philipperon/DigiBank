using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        public List<Card> DepositCards { get; set; }
    }
}
