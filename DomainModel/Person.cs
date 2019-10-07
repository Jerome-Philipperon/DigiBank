using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DomainModel
{
    public abstract class Person
    {
        [Key]
        public int PersonId { get; set; }
        [NotNull]
        public string FisrtName { get; set; }
        [NotNull]
        public string LastName { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; set; }
    }
}
