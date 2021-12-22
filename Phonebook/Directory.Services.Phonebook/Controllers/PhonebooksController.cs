using System;
using System.Threading.Tasks;
using Directory.Core.ControllerBases;
using Directory.Services.Phonebook.Dtos.Contact;
using Directory.Services.Phonebook.Dtos.Phonebook;
using Directory.Services.Phonebook.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Directory.Services.Phonebook.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhonebooksController : CustomBaseController
    {
        private readonly IPhonebookService _phonebookService;

        public PhonebooksController(IPhonebookService phonebookService)
        {
            _phonebookService = phonebookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _phonebookService.GetAllAsync();

            return CreateActionResultInstance(response);
        }

        [HttpGet("api/[controller]/[action]")]
        public async Task<IActionResult> GetAllWithDetails()
        {
            var response = await _phonebookService.GetAllWithDetailAsync();

            return CreateActionResultInstance(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _phonebookService.GetByIdAsync(id);

            return CreateActionResultInstance(response);
        }

        [HttpGet]
        [Route("/api/[controller]/GetWithDetailByIdAsync/{id}")]
        public async Task<IActionResult> GetAllByUserId(Guid id)
        {
            var response = await _phonebookService.GetWithDetailByIdAsync(id);

            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PhonebookCreateDto phonebookCreateDto)
        {
            var response = await _phonebookService.CreateAsync(phonebookCreateDto);

            return CreateActionResultInstance(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _phonebookService.DeleteAsync(id);

            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewContact(ContactInsertDto contactInsertDto)
        {
            var response = await _phonebookService.AddNewContactAsync(contactInsertDto);

            return CreateActionResultInstance(response);
        }

        [HttpPut]
        public async Task<IActionResult> DeleteContact(ContactDeleteDto contactDeleteDto)
        {
            var response = await _phonebookService.DeleteContactAsync(contactDeleteDto);

            return CreateActionResultInstance(response);
        }
    }
}
