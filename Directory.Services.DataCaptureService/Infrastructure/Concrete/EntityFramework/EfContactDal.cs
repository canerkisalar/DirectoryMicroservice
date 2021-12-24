using Directory.Core.Messages.DataCapture;
using Directory.Core.Repositories.EntityFramework;
using Directory.Services.DataCaptureService.Infrastructure.Abstact;

namespace Directory.Services.DataCaptureService.Infrastructure.Concrete.EntityFramework
{
    public class EfContactDal : EfEntityRepositoryBase<Contact,Context>,IContactDal

    {
    }
}
