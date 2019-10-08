using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DomainModel
{
    public abstract class Account
    {
        [Key]
        public int AccountId { get; set; }
        [StringLength(5)]
        [Required]
        public string BankCode { get; set; }
        [StringLength(5)]
        [Required]
        public string BranchCode { get; set; }
        [StringLength(11)]
        [Required]
        public string AccountNumber { get; set; }
        [StringLength(2)]
        [Required]
        public string Key { get; set; }
        [Required]
        public string BBAN { get; set; }
        [StringLength(34)]
        [Required]
        public string IBAN { get; set; }
        [StringLength(11)]
        [Required]
        public string BIC { get; set; }
        [Required]
        public decimal Balance { get; set; }
        [Required]
        public Client AccountOwner { get; set; }

        public Account()
        {
            this.BBAN = $"{this.BankCode}{this.BranchCode}{this.AccountNumber}{this.Key}";
        }
    }
}
