using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel
{
    public class Deposit : Account
    {
        public DateTime CreationDate { get; set; }
        public decimal AutorizedOverdraft { get; set; }
        public decimal FreeOverdraft { get; set; }
        public decimal OverdraftChargeRate { get; set; }
    }
}
