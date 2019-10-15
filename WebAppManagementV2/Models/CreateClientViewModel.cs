using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppManagementV2.Models
{
    public class CreateClientViewModel
    {
        public Client Client { get; set; }
        public List<Employee> Employees { get; set; }
        public string IdSelected { get; set; }
    }
}
