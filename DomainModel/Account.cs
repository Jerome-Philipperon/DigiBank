using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace DomainModel
{
    public class Account
    {
        [JsonPropertyName("accountId")]
        [Key]
        public int AccountId { get; set; }
        [JsonPropertyName("bankCode")]
        [StringLength(5)]
        [Required]
        public string BankCode { get; set; }
        [JsonPropertyName("branchCode")]
        [StringLength(5)]
        [Required]
        public string BranchCode { get; set; }
        [JsonPropertyName("accountNumber")]
        [StringLength(11)]
        [Required]
        public string AccountNumber { get; set; }
        [JsonPropertyName("key")]
        [StringLength(2)]
        [Required]
        public string Key { get; set; }
        [JsonPropertyName("bban")]
        [Required]
        public string BBAN { get; set; }
        [JsonPropertyName("iban")]
        [StringLength(34)]
        [Required]
        public string IBAN { get; set; }
        [JsonPropertyName("bic")]
        [StringLength(11)]
        [Required]
        public string BIC { get; set; }
        [JsonPropertyName("balance")]
        [Required]
        public decimal Balance { get; set; }
        [JsonPropertyName("accountOwner")]
        [Required]
        public Client AccountOwner { get; set; }

        public Account()
        {
            //this.BBAN = $"{this.BankCode}{this.BranchCode}{this.AccountNumber}{this.Key}";
        }
    }
}
