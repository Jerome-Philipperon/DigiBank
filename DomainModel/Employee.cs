using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DomainModel
{
    public class Employee : Person
    {
        [NotNull]
        public string OfficeName { get; set; }
        [NotNull]
        public bool IsJunior { get; set; }
        public Manager MyManager{ get; set; }
    }
}
