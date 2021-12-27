using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Phonebook.Services.Report.Mapping;

namespace Phonebook.Services.Report.Tests.Mock
{
    class AutoMapperConfiguration
    {
        public static IMapper SetMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new GeneralMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            return mapper;

        }
    }
}
