﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModel
{
    public class Client : Person
    {
        [Required]
        public string Street { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }

        public Employee MyEmployees { get; set; }
        
        public ICollection<Deposit> MyDeposits { get; set; }
        public ICollection<Saving> MySavings { get; set; }
        
    }
}
