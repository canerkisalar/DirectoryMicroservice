using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using FluentValidation;
using Phonebook.Services.Phonebook.Dtos.Contact;
using Phonebook.Services.Phonebook.Enums;

namespace Phonebook.Services.Phonebook.Application.Validations
{
    [ExcludeFromCodeCoverage]
    public class ContractCreateValidator : AbstractValidator<ContactCreateDto>
    {

        public ContractCreateValidator()
        {

            When(x => !string.IsNullOrEmpty(x.ContactType), () =>
            {
                RuleFor(x => x.ContactType)
                    .Must(x => x.Equals(ContactTypes.Email.ToString()) 
                               || x.Equals(ContactTypes.Location.ToString())
                               || x.Equals(ContactTypes.Phone.ToString())
                               )
                    .WithMessage("Contact Type must be  Location ,  Phone or  Email ");
            });

          


            When(x => x.ContactType == ContactTypes.Email.ToString(), () =>
            {
                RuleFor(x => x.ContactInformation).EmailAddress();
                RuleFor(x => x.ContactInformation).NotEmpty().NotNull();
            });

            When(x => x.ContactType == ContactTypes.Phone.ToString(), () =>
            {
                RuleFor(x => x.ContactInformation).Must(PrivateFunctions.IsPhoneNumber).WithMessage("Wrong Phone Number ");
            });
        }
    }
    [ExcludeFromCodeCoverage]
    public class ContactInsertValidator : AbstractValidator<ContactInsertDto>
    {
        public ContactInsertValidator()
        {

            When(x => !string.IsNullOrEmpty(x.ContactType), () =>
            {
                RuleFor(x => x.ContactType)
                    .Must(x => x.Equals(ContactTypes.Email.ToString())
                               || x.Equals(ContactTypes.Location.ToString())
                               || x.Equals(ContactTypes.Phone.ToString())
                    )
                    .WithMessage("Contact Type must be  Location ,  Phone or  Email ");
            });


            When(x => x.ContactType == ContactTypes.Email.ToString(), () =>
            {
                RuleFor(x => x.ContactInformation).EmailAddress();
                RuleFor(x => x.ContactInformation).NotEmpty().NotNull();
            });

            When(x => x.ContactType == ContactTypes.Phone.ToString(), () =>
            {
                RuleFor(x => x.ContactInformation).Must(PrivateFunctions.IsPhoneNumber).WithMessage("Wrong Phone Number ");
            });
        }
    }
    [ExcludeFromCodeCoverage]
    public static class PrivateFunctions
    {
        public static bool IsPhoneNumber(string arg)
        {
            Regex regex = new Regex(@"^[0-9]{10}$");
            return regex.IsMatch(arg);
        }
    }


}
