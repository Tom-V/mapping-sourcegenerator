using System;

namespace MappingSourceGenerator.ConsoleApp.Dtos
{
    [MapTo(ToType = typeof(Entities.Person))]
    public class Person
    {
        //[IgnoreProperty(ForType = typeof(Entity.Person)]
        //[IgnoreProperty(ForType = typeof(Entity.Person2)]
        //[IgnoreProperty()]

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
    }
}

