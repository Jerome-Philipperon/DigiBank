using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModel
{
    public class Employee : Person
    {
        [Required]
        public string OfficeName { get; set; }
        [Required]
        public bool IsJunior { get; set; }
        public Manager MyManager{ get; set; }
    }
}
