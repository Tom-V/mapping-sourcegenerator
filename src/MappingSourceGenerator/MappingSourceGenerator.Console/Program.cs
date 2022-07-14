using System;

namespace MappingSourceGenerator.ConsoleApp;


class Program
{
    static void Main(string[] args)
    {
        var person = new Dtos.Person();
        var result = Dtos.Mapper.Map(person);
        Console.WriteLine(result);
    }
}