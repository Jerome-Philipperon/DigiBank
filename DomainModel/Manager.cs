using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel
{
    public class Manager : Employee
    {
        public ICollection<Employee> MyEmployees { get; set; }
    }
}
