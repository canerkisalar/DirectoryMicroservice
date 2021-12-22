
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Directory.Core.Dtos;
using Directory.Services.Phonebook.Dtos.Contact;
using Directory.Services.Phonebook.Dtos.Phonebook;

namespace Directory.Services.Phonebook.Services.Abstract
{
    public interface IPhonebookService
    {

        Task<Response<PhonebookDto>> CreateAsync(PhonebookCreateDto phonebookCreateDto);
        Task<Response<NoContent>> DeleteAsync(Guid id);
        Task<Response<PhonebookDetailDto>> AddNewContactAsync(ContactInsertDto contactInsetDto);
        Task<Response<PhonebookDetailDto>> DeleteContactAsync(ContactDeleteDto contactDeleteDto);
        Task<Response<List<PhonebookDto>>> GetAllAsync();
        Task<Response<PhonebookDto>> GetByIdAsync(Guid id);
        Task<Response<List<PhonebookDetailDto>>> GetAllWithDetailAsync();
        Task<Response<PhonebookDetailDto>> GetWithDetailByIdAsync(Guid id);

    }
}
