using GmailServer.Entities;
using GmailServer.Permissions;
using GmailServer.TaskChecks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.ApplicationServices
{
    [RemoteService(IsEnabled = false)]
    [Authorize]
    public class TaskCheckAppService : CrudAppService<
        TaskCheck, 
        TaskCheckDto, 
        long, 
        TaskCheckFilterDto,
        CreateUpdateTaskCheckDto,
        CreateUpdateTaskCheckDto>, ITaskCheckAppService
    {
        public TaskCheckAppService(IRepository<TaskCheck, long> repository): base(repository)
        {
            GetListPolicyName = GmailServerPermissions.TaskChecks.Default;
            GetPolicyName = GmailServerPermissions.TaskChecks.Default;
            CreatePolicyName = GmailServerPermissions.TaskChecks.Create;
            UpdatePolicyName = GmailServerPermissions.TaskChecks.Update;
            DeletePolicyName = GmailServerPermissions.TaskChecks.Delete;
        }
    }
}
