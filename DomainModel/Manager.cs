using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DomainModel
{
    public class Manager : Employee
    {
        [JsonIgnore]
        public ICollection<Employee> MyEmployees { get; set; }
    }
}
