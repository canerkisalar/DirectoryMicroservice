using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Phonebook.Services.Phonebook.Dtos.Phonebook;

namespace Phonebook.Services.Phonebook.Application.Validations
{
    public class PhonebookValidator :AbstractValidator<PhonebookCreateDto>
    {
        public PhonebookValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Surname).NotNull().NotEmpty();

        }
    }
}
