using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using MongoDB.Driver;
using Phonebook.Core.Domain.Dtos;
using Phonebook.Core.Domain.Enums;
using Phonebook.Core.Messages.DataCapture;
using Phonebook.Core.Messages.Report;
using Phonebook.Services.Phonebook.Dtos.Contact;
using Phonebook.Services.Phonebook.Dtos.Phonebook;
using Phonebook.Services.Phonebook.Services.Abstract;
using Phonebook.Services.Phonebook.Settings.Abstract;
using Contact = Phonebook.Services.Phonebook.Models.Contact;

namespace Phonebook.Services.Phonebook.Services.Concrete
{
    public class PhonebookService : IPhonebookService
    {
        private readonly IMongoCollection<Models.Phonebook> _phonebookCollection;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IMapper _mapper;

        public PhonebookService(IMapper mapper, IDatabaseSettings databaseSettings, ISendEndpointProvider sendEndpointProvider)
        {

            var client = new MongoClient(databaseSettings.ConnectionString);
            //MongoDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _phonebookCollection = database.GetCollection<Models.Phonebook>(databaseSettings.PhonebookCollectionName);
            _mapper = mapper;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task<Core.Domain.Dtos.Response<PhonebookDto>> CreateAsync(PhonebookCreateDto phonebookCreateDto)
        {
            var newPhonebook = _mapper.Map<Models.Phonebook>(phonebookCreateDto);

            // using UUID instead of ObjectId at MongoDB

            newPhonebook.Id = Guid.NewGuid();

            if (newPhonebook.Contacts != null)
            {
                foreach (var contact in newPhonebook.Contacts)
                {
                    contact.Id = Guid.NewGuid();
                }
            }

            await _phonebookCollection.InsertOneAsync(newPhonebook);
            await SendToQueueForCopy(newPhonebook, ModiTypes.Create);

            return Core.Domain.Dtos.Response<PhonebookDto>.Success(_mapper.Map<PhonebookDto>(newPhonebook), 200);
        }

        public async Task<Core.Domain.Dtos.Response<NoContent>> DeleteAsync(Guid id)
        {
            var result = await _phonebookCollection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount > 0)
            {

                await SendToQueueForCopy(new Models.Phonebook() { Id = id }, ModiTypes.Delete);
                return Core.Domain.Dtos.Response<NoContent>.Success(200);
            }
            return Core.Domain.Dtos.Response<NoContent>.Fail("Phonebook not found", 404);

        }

        public async Task<Core.Domain.Dtos.Response<PhonebookDetailDto>> AddNewContactAsync(ContactInsertDto contactInsetDto)
        {

            var phonebook = await _phonebookCollection.Find(x => x.Id == contactInsetDto.PhonebookId).FirstOrDefaultAsync();
            if (phonebook == null)
            {
                return Core.Domain.Dtos.Response<PhonebookDetailDto>.Fail("Phonebook not found", 404);
            }

            var checkContactExists = phonebook.Contacts.Any(
                x => x.ContactInformation == contactInsetDto.ContactInformation
                     && x.ContactType == contactInsetDto.ContactType
            );
            if (checkContactExists)
            {
                return Core.Domain.Dtos.Response<PhonebookDetailDto>.Fail("Contact already exists", 400);

            }

            var newContact = _mapper.Map<Contact>(contactInsetDto);
            newContact.Id = Guid.NewGuid();
            phonebook.Contacts.Add(newContact);

            var filter = Builders<Models.Phonebook>.Filter.And(
                Builders<Models.Phonebook>.Filter.Where(x => x.Id == contactInsetDto.PhonebookId));
            var update = Builders<Models.Phonebook>.Update.Push(x => x.Contacts, newContact);
            await _phonebookCollection.FindOneAndUpdateAsync(filter, update);

            await SendToQueueForCopy(phonebook, ModiTypes.Update);
            return Core.Domain.Dtos.Response<PhonebookDetailDto>.Success(_mapper.Map<PhonebookDetailDto>(phonebook), 200);

        }

        public async Task<Core.Domain.Dtos.Response<PhonebookDetailDto>> DeleteContactAsync(ContactDeleteDto contactDeleteDto)
        {

            var phonebook = await _phonebookCollection.Find(x =>
                x.Id == contactDeleteDto.PhonebookId && x.Contacts.Any(y => y.Id == contactDeleteDto.ContactId)).FirstOrDefaultAsync();

            if (phonebook == null)
            {
                return Core.Domain.Dtos.Response<PhonebookDetailDto>.Fail("Phonebook not found", 404);
            }

            var filter = Builders<Models.Phonebook>.Filter.Where(x => x.Id == contactDeleteDto.PhonebookId);
            var update = Builders<Models.Phonebook>.Update.PullFilter(x => x.Contacts,
                contact => contact.Id == contactDeleteDto.ContactId);

            await _phonebookCollection.UpdateOneAsync(filter, update);

            phonebook.Contacts = phonebook.Contacts.Where(x => x.Id != contactDeleteDto.ContactId).ToList();

            await SendToQueueForCopy(phonebook, ModiTypes.Update);

            return Core.Domain.Dtos.Response<PhonebookDetailDto>.Success(200);

        }

        public async Task<Core.Domain.Dtos.Response<List<PhonebookDto>>> GetAllAsync()
        {

            var phonebooks = await _phonebookCollection.Find(phonebook => true).ToListAsync();

            if (!phonebooks.Any())
            {
                phonebooks = new List<Models.Phonebook>();
            }

            return Core.Domain.Dtos.Response<List<PhonebookDto>>.Success(_mapper.Map<List<PhonebookDto>>(phonebooks), 200);

        }

        public async Task<Core.Domain.Dtos.Response<List<PhonebookDetailDto>>> GetAllWithDetailAsync()
        {
            var phonebooks = await _phonebookCollection.Find(phonebook => true).ToListAsync();

            if (!phonebooks.Any())
            {
                phonebooks = new List<Models.Phonebook>();
            }

            return Core.Domain.Dtos.Response<List<PhonebookDetailDto>>.Success(_mapper.Map<List<PhonebookDetailDto>>(phonebooks), 200);

        }

        public async Task<Core.Domain.Dtos.Response<PhonebookDto>> GetByIdAsync(Guid id)
        {
            var phonebook = await _phonebookCollection.Find<Models.Phonebook>(x => x.Id == id).FirstOrDefaultAsync();

            return phonebook == null
                ? Core.Domain.Dtos.Response<PhonebookDto>.Fail("Phonebook not found", 404)
                : Core.Domain.Dtos.Response<PhonebookDto>.Success(_mapper.Map<PhonebookDto>(phonebook), 200);
        }

        public async Task<Core.Domain.Dtos.Response<PhonebookDetailDto>> GetWithDetailByIdAsync(Guid id)
        {
            var phonebook = await _phonebookCollection.Find<Models.Phonebook>(x => x.Id == id).FirstOrDefaultAsync();

            return phonebook == null
                ? Core.Domain.Dtos.Response<PhonebookDetailDto>.Fail("Phonebook not found", 404)
                : Core.Domain.Dtos.Response<PhonebookDetailDto>.Success(_mapper.Map<PhonebookDetailDto>(phonebook), 200);
        }

        #region Private Functions
        public async Task SendToQueueForCopy(Models.Phonebook phonebook, ModiTypes modiType)
        {
            var sendEndPoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:snap-phonebook"));

            var snapPhonebookMessageCommand = new SnapPhonebookMessageCommand();

            snapPhonebookMessageCommand = _mapper.Map<Models.Phonebook, SnapPhonebookMessageCommand>(phonebook);

            snapPhonebookMessageCommand.Modification = (int)modiType;

            await sendEndPoint.Send<SnapPhonebookMessageCommand>(snapPhonebookMessageCommand);
        }

        public async Task SendDataToReportService(string data,Guid reportId)
        {
            var sendEndPoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:phonebook-alldata"));

            var MessageCommand = new AllPhonebooksMessageCommand();

            MessageCommand.PhonebookAllData = data;
            MessageCommand.ReportId = reportId;

            await sendEndPoint.Send<AllPhonebooksMessageCommand>(MessageCommand);
        }

        #endregion
    }
}
