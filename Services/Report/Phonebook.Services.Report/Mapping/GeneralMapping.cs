using AutoMapper;
using Phonebook.Services.Report.Dtos;
using Phonebook.Services.Report.Models.Report;

namespace Phonebook.Services.Report.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<ReportHead, ReportHeadDto>().ReverseMap();

        }
    }
}