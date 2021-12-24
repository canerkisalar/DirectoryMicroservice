using Directory.Core.Messages.DataCapture;
using Directory.Core.Repositories;

namespace Directory.Services.DataCaptureService.Infrastructure.Abstact
{
    public interface IContactDal: IEntityRepository<Contact>
    {
    }
}
