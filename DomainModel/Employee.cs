using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel
{
    public class Employee : Person
    {
        public string OfficeName { get; set; }
        public bool IsJunior { get; set; }
        public Manager MyManager{ get; set; }
    }
}
