using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel
{
    public class Saving:Account
    {
        public int MinimumAmount { get; set; }
        public int MaximumAmount { get; set; }
        public double InterestRate { get; set; }
        public DateTime MaximumDate { get; set; }
    }
}
