using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DomainModel
{
    public class Saving:Account
    {
        [Required]
        public int MinimumAmount { get; set; }
        public int MaximumAmount { get; set; }
        [Required]
        public double InterestRate { get; set; }
        public DateTime MaximumDate { get; set; }
    }
}
