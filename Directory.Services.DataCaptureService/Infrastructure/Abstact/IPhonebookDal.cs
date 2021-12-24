using Directory.Core.Repositories;
using Directory.Services.DataCaptureService.Domain;

namespace Directory.Services.DataCaptureService.Infrastructure.Abstact
{
    public interface IPhonebookDal : IEntityRepository<Phonebook>
    {
    }
}
