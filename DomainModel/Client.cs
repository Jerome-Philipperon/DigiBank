using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DomainModel
{
    public class Client : Person
    {
        [NotNull]
        public string Street { get; set; }
        [NotNull]
        public string ZipCode { get; set; }
        [NotNull]
        public string City { get; set; }

        public Employee MyEmployees { get; set; }
        
        public ICollection<Deposit> MyDeposits { get; set; }
        public ICollection<Saving> MySavings { get; set; }
        
    }
}
