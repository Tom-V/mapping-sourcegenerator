

using System;

namespace MappingSourceGenerator.ConsoleApp.Entities
{
    public class Person
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
        public override string ToString()
        {
            return @$"
LastName: {LastName},
Firstname: {FirstName},
Age: {Age},
BirthDate: {BirthDate}";
        }

    }
}
