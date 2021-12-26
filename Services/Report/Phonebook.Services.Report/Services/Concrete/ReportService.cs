using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Phonebook.Core.Domain.Enums;
using Phonebook.Core.Messages;
using Phonebook.Core.Messages.Report;
using Phonebook.Services.Report.Dtos;
using Phonebook.Services.Report.Dtos.Reports;
using Phonebook.Services.Report.Enums;
using Phonebook.Services.Report.Infrastructure.Abstract;
using Phonebook.Services.Report.Infrastructure.Abstract.Report;
using Phonebook.Services.Report.Models.Report;
using Phonebook.Services.Report.Services.Abstract;

namespace Phonebook.Services.Report.Services.Concrete
{
    public class ReportService : IReportService
    {
        private readonly IPhonebookDal _phonebookDal;
        private readonly IReportHeadDal _reportHeadDal;
        private readonly IReportItemDal _reportItemDal;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IMapper _mapper;

        public ReportService(IPhonebookDal phonebookDal, IReportHeadDal reportHeadDal, IReportItemDal reportItemDal, ISendEndpointProvider sendEndpointProvider, IMapper mapper)
        {
            _phonebookDal = phonebookDal;
            _reportHeadDal = reportHeadDal;
            _reportItemDal = reportItemDal;
            _sendEndpointProvider = sendEndpointProvider;
            _mapper = mapper;
        }


        public async Task<Core.Domain.Dtos.Response<ReportHead>> GetInstantDataAndReportAsync()
        {
            var newReportRequest = new ReportHead()
            {
                ReportHeadId = Guid.NewGuid(),
                PreparationDate = DateTime.MinValue,
                RequestDate = DateTime.Now,
                Status = ReportStatusTypes.Preparing.ToString()
            };

            var added = await _reportHeadDal.Add(newReportRequest);

            if (added != null)
            {
                var getDataRequest = await GetBatchData(newReportRequest.ReportHeadId);

                if (getDataRequest.IsSuccessful)
                {
                    return Core.Domain.Dtos.Response<ReportHead>.Success(newReportRequest,200);
                }
            }
            return Core.Domain.Dtos.Response<ReportHead>.Fail("Something goes wrong", 500);
        }

        public async Task<Core.Domain.Dtos.Response<List<LocationReportDto>>> GetLocationReportByIdAsync(Guid reportId)
        {
            var locationReportDto = new List<LocationReportDto>();
            var reportItem = await _reportItemDal.Get(x => x.ReportHeadId == reportId);

            if (reportItem != null)
            {
                locationReportDto = JsonConvert.DeserializeObject<List<LocationReportDto>>(reportItem.Code);
                return Core.Domain.Dtos.Response<List<LocationReportDto>>.Success(locationReportDto, 200);
            }

            return Core.Domain.Dtos.Response<List<LocationReportDto>>.Fail("Something goes wrong", 500);
        }

        public async Task<Core.Domain.Dtos.Response<List<ReportHeadDto>>> GetLocationReportList()
        {
            var reportHeadDto = await _reportHeadDal.GetAll();


            return reportHeadDto != null
                ? Core.Domain.Dtos.Response<List<ReportHeadDto>>.Success(_mapper.Map<List<ReportHeadDto>>(reportHeadDto), 200)
                : Core.Domain.Dtos.Response<List<ReportHeadDto>>.Fail("Something goes wrong", 500);
        }

        public async Task<Core.Domain.Dtos.Response<GetAllPhonebooksMessageCommand>> GetBatchData(Guid reportId)
        {
            var sendEndPoint = await _sendEndpointProvider
                .GetSendEndpoint(
                    new Uri("queue:give-all-phonebooks"));

            var messageCommand = new GetAllPhonebooksMessageCommand();
            messageCommand.ReportId = reportId;
            await sendEndPoint.Send<GetAllPhonebooksMessageCommand>(messageCommand);

            return Core.Domain.Dtos.Response<GetAllPhonebooksMessageCommand>.Success(200);
        }

        public async Task PrepLocationReport(SourceTypes sourceType, AllPhonebooksMessageCommand context = null, List<Models.Phonebook> dataFromDb = null)
        {

            var phonebooks = new List<Models.Phonebook>();

            switch (sourceType)
            {
                case SourceTypes.FromMessageQueue:
                    phonebooks = JsonConvert.DeserializeObject<List<Models.Phonebook>>(context.PhonebookAllData);
                    foreach (var phonebook in phonebooks)
                    {
                        phonebook.Contacts.ToList().ForEach(c => c.PhonebookId = phonebook.PhonebookId);
                    }
                    break;
                case SourceTypes.FromOwnDb:
                    phonebooks = dataFromDb;
                    break;


            }

            if (phonebooks is { Count: 0 })
            {
                return;
            }


            var report = new List<LocationReportDto>();


            var distinctLocations = phonebooks.SelectMany(a => a.Contacts
                .Where(c => c.ContactType == ContactTypes.Location.ToString())
                .Select(c => c.ContactInformation)).Distinct().ToList();

            distinctLocations.Add("Undefined");

            var contacts = phonebooks.SelectMany(a => a.Contacts).ToList();

            distinctLocations.ForEach(location =>
            {
                var phonebookIds = new List<Guid>();
                if (location == "Undefined")
                {
                    //phonebookIds.AddRange(allData
                    //    .Where(p => p.Contacts is not { Count: > 0 })
                    //    .Select(p => p.PhonebookId)
                    //    .Distinct()
                    //    .ToList());

                    phonebookIds.AddRange(phonebooks
                        .Where(p => !p.Contacts.Any(c => c.ContactType == ContactTypes.Location.ToString()))
                        .Select(p => p.PhonebookId)
                        .Distinct()
                        .ToList()
                    );
                }
                else
                {
                    phonebookIds = contacts.Where(z => z.ContactInformation == location).Select(c => c.PhonebookId)
                        .Distinct()
                        .ToList();
                }

                report.Add(new LocationReportDto()
                {
                    Location = location,
                    CountPhoneNumbers = contacts.Count(c =>
                        c.ContactType == ContactTypes.Phone.ToString() && phonebookIds.Contains(c.PhonebookId)),
                    CountPerson = phonebookIds.Count
                });
            });

            await _reportHeadDal.UpdateReportStatusById(context.ReportId, ReportStatusTypes.Done);
            await _reportItemDal.Add(new ReportItem()
            {
                ReportHeadId = context.ReportId,
                ReportItemId = Guid.NewGuid(),
                Code = JsonConvert.SerializeObject(report)
            });


        }
    }
}
