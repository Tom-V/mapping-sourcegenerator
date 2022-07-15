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
        var result = Dtos.Mapper.Map(person);
        Console.WriteLine(result);
    }
}