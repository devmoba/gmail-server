using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Permissions;
using GmailServer.RecoveryEmails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.ApplicationServices
{
    [RemoteService(IsEnabled = false)]
    public class RecoveryEmailAppService : CrudAppService<
        RecoveryEmail, 
        RecoveryEmailDto, 
        long, 
        RecoveryEmailFilterDto, 
        CreateUpdateRecoveryEmailDto,
        CreateUpdateRecoveryEmailDto>, IRecoveryEmailAppService
    {
        private readonly Random random = new Random();
        public RecoveryEmailAppService(IRepository<RecoveryEmail, long> repository) : base(repository)
        {
            GetPolicyName = GmailServerPermissions.RecoveryEmails.Default;
            GetListPolicyName = GmailServerPermissions.RecoveryEmails.Default;
            CreatePolicyName = GmailServerPermissions.RecoveryEmails.Create;
            UpdatePolicyName = GmailServerPermissions.RecoveryEmails.Update;
            DeletePolicyName = GmailServerPermissions.RecoveryEmails.Delete;
        }

        public override async Task<PagedResultDto<RecoveryEmailDto>> GetListAsync(RecoveryEmailFilterDto input)
        {
            var query = Repository.AsQueryable();
            query.WhereIf(input.Status.HasValue, x => x.Status == input.Status.Value);
            query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == x.Username);

            var count = await AsyncExecuter.CountAsync(query);

            if (!string.IsNullOrEmpty(input.Sorting))
                query = ApplySorting(query, input);
            else
                query = ApplyDefaultSorting(query);

            if (input.MaxResultCount > 0 || input.SkipCount > 0)
                query = ApplyPaging(query, input);

            var entities = await AsyncExecuter.ToListAsync(query);

            var res = ObjectMapper.Map<List<RecoveryEmail>, List<RecoveryEmailDto>>(entities);

            return new PagedResultDto<RecoveryEmailDto>(count, res);
        }

        public override async Task<RecoveryEmailDto> CreateAsync(CreateUpdateRecoveryEmailDto input)
        {
            var recoveryEmail = ObjectMapper.Map<CreateUpdateRecoveryEmailDto, RecoveryEmail>(input);
            recoveryEmail.Status = RecoveryEmailStatus.Ready;
            recoveryEmail.Created = DateTime.Now;
            var res = await Repository.InsertAsync(recoveryEmail, true);

            return ObjectMapper.Map<RecoveryEmail, RecoveryEmailDto>(res);
        }

        public async Task<RecoveryEmailDto> GetRecoveryEmailRandomAsync()
        {
            var query = Repository.Where(x => x.Status == RecoveryEmailStatus.Ready);
            var recoveryEmails = await AsyncExecuter.ToArrayAsync(query);
            var index = random.Next(recoveryEmails.Count());
            var res = recoveryEmails[index];
            return ObjectMapper.Map<RecoveryEmail, RecoveryEmailDto>(res);
        }
    }
}
