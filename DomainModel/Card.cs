using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModel
{
    public class Card
    {
        [Key]
        public int CardId { get; set; }
        public string NetworkIssuer { get; set; }

        [StringLength(16)]
        public string CardNumber{ get; set; }
        [StringLength(4)]
        public string SecurityCode { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ExpirationDate { get; set; }

        public Deposit CardDeposit { get; set; }

    }
}
