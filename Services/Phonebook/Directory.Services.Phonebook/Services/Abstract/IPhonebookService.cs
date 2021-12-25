using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Phonebook.Core.Domain.Dtos;
using Phonebook.Core.Domain.Enums;
using Phonebook.Services.Phonebook.Dtos.Contact;
using Phonebook.Services.Phonebook.Dtos.Phonebook;

namespace Phonebook.Services.Phonebook.Services.Abstract
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

        #region Private Functions

        Task SendToQueueForCopy(Models.Phonebook phonebook, ModiTypes modiType);

        Task SendDataToReportService(string data,Guid reportId);

        #endregion



    }
}
