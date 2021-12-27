using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Phonebook.Services.Phonebook.Dtos.Phonebook;

namespace Phonebook.Services.Phonebook.Application.Validations
{
    [ExcludeFromCodeCoverage]
    public class PhonebookValidator :AbstractValidator<PhonebookCreateDto>
    {
        public PhonebookValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Surname).NotNull().NotEmpty();

        }
    }
}
