using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Directory.Core.Dtos;
using Directory.Services.Phonebook.Dtos.Contact;
using Directory.Services.Phonebook.Dtos.Phonebook;
using Directory.Services.Phonebook.Models;
using Directory.Services.Phonebook.Services.Abstract;
using Directory.Services.Phonebook.Settings.Abstract;
using MongoDB.Driver;

namespace Directory.Services.Phonebook.Services.Concrete
{
    public class PhonebookService : IPhonebookService
    {
        private readonly IMongoCollection<Models.Phonebook> _phonebookCollection;
        private readonly IMapper _mapper;

        public PhonebookService(IMapper mapper, IDatabaseSettings databaseSettings)
        {

            var client = new MongoClient(databaseSettings.ConnectionString);
            MongoDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _phonebookCollection = database.GetCollection<Models.Phonebook>(databaseSettings.PhonebookCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<PhonebookDto>> CreateAsync(PhonebookCreateDto phonebookCreateDto)
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

            return Response<PhonebookDto>.Success(_mapper.Map<PhonebookDto>(newPhonebook), 200);
        }

        public async Task<Response<NoContent>> DeleteAsync(Guid id)
        {
            var result = await _phonebookCollection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("Phonebook not found", 404);

        }

        public async Task<Response<PhonebookDetailDto>> AddNewContactAsync(ContactInsertDto contactInsetDto)
        {

            var phonebook = await _phonebookCollection.Find(x => x.Id == contactInsetDto.PhonebookId).FirstOrDefaultAsync();
            if (phonebook == null)
            {
                return Response<PhonebookDetailDto>.Fail("Phonebook not found", 404);
            }

            var checkContactExists = phonebook.Contacts.Any(
                x => x.ContactInformation == contactInsetDto.ContactInformation
                     && x.ContactType == contactInsetDto.ContactType
            );
            if (checkContactExists)
            {
                return Response<PhonebookDetailDto>.Fail("Contact already exists", 400);

            }

            var newContact = _mapper.Map<Contact>(contactInsetDto);
            newContact.Id = Guid.NewGuid();
            phonebook.Contacts.Add(newContact);

            var filter = Builders<Models.Phonebook>.Filter.And(
                Builders<Models.Phonebook>.Filter.Where(x => x.Id == contactInsetDto.PhonebookId));
            var update = Builders<Models.Phonebook>.Update.Push(x => x.Contacts, newContact);
            await _phonebookCollection.FindOneAndUpdateAsync(filter, update);

            return Response<PhonebookDetailDto>.Success(_mapper.Map<PhonebookDetailDto>(phonebook), 200);

        }

        public async Task<Response<PhonebookDetailDto>> DeleteContactAsync(ContactDeleteDto contactDeleteDto)
        {

            var record = await _phonebookCollection.Find(x =>
                x.Id == contactDeleteDto.PhonebookId && x.Contacts.Any(y => y.Id == contactDeleteDto.ContactId)).FirstOrDefaultAsync();

            if (record == null)
            {
                return Response<PhonebookDetailDto>.Fail("Phonebook not found", 404);
            }


            var filter = Builders<Models.Phonebook>.Filter.Where(x => x.Id == contactDeleteDto.PhonebookId);
            var update = Builders<Models.Phonebook>.Update.PullFilter(x => x.Contacts,
                contact => contact.Id == contactDeleteDto.ContactId);

            await _phonebookCollection.UpdateOneAsync(filter, update);

            return Response<PhonebookDetailDto>.Success(200);

        }

        public async Task<Response<List<PhonebookDto>>> GetAllAsync()
        {
            var phonebooks = await _phonebookCollection.Find(phonebook => true).ToListAsync();

            if (!phonebooks.Any())
            {
                phonebooks = new List<Models.Phonebook>();
            }

            return Response<List<PhonebookDto>>.Success(_mapper.Map<List<PhonebookDto>>(phonebooks), 200);

        }

        public async Task<Response<List<PhonebookDetailDto>>> GetAllWithDetailAsync()
        {
            var phonebooks = await _phonebookCollection.Find(phonebook => true).ToListAsync();

            if (!phonebooks.Any())
            {
                phonebooks = new List<Models.Phonebook>();
            }

            return Response<List<PhonebookDetailDto>>.Success(_mapper.Map<List<PhonebookDetailDto>>(phonebooks), 200);

        }

        public async Task<Response<PhonebookDto>> GetByIdAsync(Guid id)
        {
            var phonebook = await _phonebookCollection.Find<Models.Phonebook>(x => x.Id == id).FirstOrDefaultAsync();

            return phonebook == null
                ? Response<PhonebookDto>.Fail("Phonebook not found", 404)
                : Response<PhonebookDto>.Success(_mapper.Map<PhonebookDto>(phonebook), 200);
        }

        public async Task<Response<PhonebookDetailDto>> GetWithDetailByIdAsync(Guid id)
        {
            var phonebook = await _phonebookCollection.Find<Models.Phonebook>(x => x.Id == id).FirstOrDefaultAsync();

            return phonebook == null
                ? Response<PhonebookDetailDto>.Fail("Phonebook not found", 404)
                : Response<PhonebookDetailDto>.Success(_mapper.Map<PhonebookDetailDto>(phonebook), 200);
        }
    }
}
