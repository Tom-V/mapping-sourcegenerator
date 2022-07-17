using System;

namespace MappingSourceGenerator.ConsoleApp;


class Program
{
    static void Main(string[] args)
    {
        var person = new Dtos.Person()
        {
            FirstName = "Tom",
            LastName = "Vervoort",
            BirthDate = new DateTime(1989, 1, 14),
            Age = 33
        };
        Console.WriteLine(Dtos.Mapper.ToPersonEntity(person));
        Console.WriteLine(Dtos.Mapper.ToPerson2Entity(person));


        var person2 = new Dtos.Person2()
        {
            FirstName = "Tom",
            LastName = "Vervoort",
            BirthDate = new DateTime(1989, 1, 14),
            Age = 33
        };
        Console.WriteLine(Dtos.Mapper.ToPersonEntity(person2));
        Console.WriteLine(Dtos.Mapper.ToPerson2Entity(person2));
    }
}