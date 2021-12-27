using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Phonebook.Core.Domain.Dtos;

namespace Phonebook.Core.ControllerBases
{
    [ExcludeFromCodeCoverage]
    public class CustomBaseController : ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
