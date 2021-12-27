
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Phonebook.Core.Domain.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ErrorDto
    {
        public List<string> Errors { get; set; }
    }
}