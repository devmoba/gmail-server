using Volo.Abp.Application.Services;

namespace GmailServer.DownloadedApps
{
    public interface IDownloadedAppAppService :
        IReadOnlyAppService<DownloadedAppGetOutputDto, DownloadedAppGetListOutputDto, long, DownloadAppFilterDto>, 
        ICreateAppService<DownloadedAppGetOutputDto, CreateDownloadedAppDto>,
        IDeleteAppService<long>
    {
    }
}
