using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel
{
    public abstract class Account
    {
        public string BankCode { get; set; }
        public string BranchCode { get; set; }
        public string AccountNumber { get; set; }
        public string Key { get; set; }
        public string BBAN { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }
        public decimal Balance { get; set; }
    }
}
