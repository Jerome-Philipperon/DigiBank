using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel
{
    public class Client : Person
    {
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }

        public Employee MyEmployees { get; set; }
        /*
        public ICollection<Deposit> MyDeposits { get; set; }
        public ICollection<Saving> MySavings { get; set; }
        */
    }
}
