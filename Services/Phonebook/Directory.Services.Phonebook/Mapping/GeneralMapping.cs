using AutoMapper;
using Phonebook.Core.Messages.DataCapture;
using Phonebook.Services.Phonebook.Dtos.Contact;
using Phonebook.Services.Phonebook.Dtos.Phonebook;
using Contact = Phonebook.Services.Phonebook.Models.Contact;

namespace Phonebook.Services.Phonebook.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            #region Phonebook Mappings

            CreateMap<Models.Phonebook, PhonebookDto>().ReverseMap();
            CreateMap<Models.Phonebook, PhonebookCreateDto>().ReverseMap();
            CreateMap<Models.Phonebook, PhonebookUpdateDto>().ReverseMap();
            CreateMap<Models.Phonebook, PhonebookDetailDto>().ReverseMap();

            #endregion

            #region Contact Mappings

            CreateMap<Contact, ContactCreateDto>().ReverseMap();
            CreateMap<Contact, ContactDeleteDto>().ReverseMap();
            CreateMap<Contact, ContactInsertDto>().ReverseMap();

            #endregion

            #region Message Command Mappins

            CreateMap<Models.Phonebook, SnapPhonebookMessageCommand>()
                .ForMember(x => x.Modification, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Contact, Core.Messages.DataCapture.Contact>().ReverseMap();

            #endregion

        }
    }
}
