using System;

namespace MappingSourceGenerator.ConsoleApp.Dtos
{
    [MapTo(typeof(Entities.Person), "ToPersonEntity")]
    [MapTo(typeof(Entities.Person2), "ToPerson2Entity")]
    [MapFrom(typeof(Entities.Person), "ToPerson")]
    [MapFrom(typeof(Entities.Person2), "ToPerson")]
    public class Person
    {
        [IgnoreProperty(ToType = typeof(Entities.Person2))]
        [IgnoreProperty(FromType = typeof(Entities.Person2))]

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
    [MapFrom(typeof(Entities.Person), "ToPerson2")]
    [MapFrom(typeof(Entities.Person2), "ToPerson2")]
    public class Person2
    {
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

