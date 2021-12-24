using Directory.Core.Repositories.EntityFramework;
using Directory.Services.DataCaptureService.Domain;
using Directory.Services.DataCaptureService.Infrastructure.Abstact;

namespace Directory.Services.DataCaptureService.Infrastructure.Concrete.EntityFramework
{
    public class EfPhonebookDal : EfEntityRepositoryBase<Phonebook,Context>,IPhonebookDal
    {
    }
}
