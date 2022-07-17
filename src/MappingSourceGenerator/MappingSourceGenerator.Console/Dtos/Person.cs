using System;

namespace MappingSourceGenerator.ConsoleApp.Dtos
{
    [MapTo(typeof(Entities.Person), "ToPersonEntity")]
    [MapTo(typeof(Entities.Person2), "ToPerson2Entity")]
    public class Person
    {
        //[IgnoreProperty(ForType = typeof(Entity.Person)]
        //[IgnoreProperty(ForType = typeof(Entity.Person2)]
        //[IgnoreProperty()]

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        [IgnoreProperty]
        public DateTime BirthDate { get; set; }

        public override string ToString()
        {
            return @$"DTO Person:
LastName: {LastName},
Firstname: {FirstName},
Age: {Age},
BirthDate: {BirthDate}";
        }
    }

    [MapTo(typeof(Entities.Person), "ToPersonEntity")]
    [MapTo(typeof(Entities.Person2), "ToPerson2Entity")]
    public class Person2
    {
        //[IgnoreProperty(ForType = typeof(Entity.Person)]
        //[IgnoreProperty(ForType = typeof(Entity.Person2)]
        //[IgnoreProperty()]

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        [IgnoreProperty]
        public DateTime BirthDate { get; set; }

        public override string ToString()
        {
            return @$"DTO Person2:
LastName: {LastName},
Firstname: {FirstName},
Age: {Age},
BirthDate: {BirthDate}";
        }
    }

}

