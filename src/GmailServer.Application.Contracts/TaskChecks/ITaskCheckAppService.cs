using Volo.Abp.Application.Services;

namespace GmailServer.TaskChecks
{
    public interface ITaskCheckAppService : ICrudAppService<
        TaskCheckDto, 
        long, 
        TaskCheckFilterDto, 
        CreateUpdateTaskCheckDto, 
        CreateUpdateTaskCheckDto>
    {

    }
}
