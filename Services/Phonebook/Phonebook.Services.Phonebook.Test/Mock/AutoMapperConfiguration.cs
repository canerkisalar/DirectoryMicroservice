using AutoMapper;
using Phonebook.Services.Phonebook.Mapping;

namespace Phonebook.Services.Phonebook.Test.Mock
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
