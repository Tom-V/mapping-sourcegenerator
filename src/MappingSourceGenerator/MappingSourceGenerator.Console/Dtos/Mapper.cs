using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingSourceGenerator.ConsoleApp.Dtos
{
    public static partial class Mapper
    {
        static partial void _toEntity(Person input, Entities.Person output)
        {
            output.BirthDate = new DateTime(input.BirthDate.Year, input.BirthDate.Month, input.BirthDate.Day);
        }

    }
}
