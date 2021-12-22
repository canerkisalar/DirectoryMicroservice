

using AutoMapper;
using Directory.Services.Phonebook.Dtos.Contact;
using Directory.Services.Phonebook.Dtos.Phonebook;
using Directory.Services.Phonebook.Models;

namespace Directory.Services.Phonebook.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            #region Phonebook Mapping

            CreateMap<Models.Phonebook, PhonebookDto>().ReverseMap();
            CreateMap<Models.Phonebook, PhonebookCreateDto>().ReverseMap();
            CreateMap<Models.Phonebook, PhonebookUpdateDto>().ReverseMap();
            CreateMap<Models.Phonebook, PhonebookDetailDto>().ReverseMap();

            #endregion

            #region Contact Mapping

            CreateMap<Contact, ContactCreateDto>().ReverseMap();
            CreateMap<Contact, ContactDeleteDto>().ReverseMap();
            CreateMap<Contact, ContactInsertDto>().ReverseMap();

            #endregion

        }
    }
}
