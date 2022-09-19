using GmailServer.Entities;
using GmailServer.Enums;
using GmailServer.Permissions;
using GmailServer.RecoveryEmails;
using GmailServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

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
        private new readonly IRecoveryEmailRepository Repository;

        private readonly Random random = new Random();
        public RecoveryEmailAppService(IRecoveryEmailRepository repository) : base(repository)
        {
            Repository = repository;

            GetPolicyName = GmailServerPermissions.RecoveryEmails.Default;
            GetListPolicyName = GmailServerPermissions.RecoveryEmails.Default;
            UpdatePolicyName = GmailServerPermissions.RecoveryEmails.Update;
            DeletePolicyName = GmailServerPermissions.RecoveryEmails.Delete;
        }

        public override async Task<PagedResultDto<RecoveryEmailDto>> GetListAsync(RecoveryEmailFilterDto input)
        {
            var query = Repository.AsQueryable();
            query = query.WhereIf(input.Status.HasValue, x => x.Status == input.Status.Value);
            query = query.WhereIf(!string.IsNullOrEmpty(input.Username), x => x.Username == x.Username);

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

        public async Task<RecoveryEmailDto> GetRecoveryEmailRandomAsync()
        {
            var query = Repository.Where(x => x.Status == RecoveryEmailStatus.Ready);
            var recoveryEmails = await AsyncExecuter.ToArrayAsync(query);
            if (recoveryEmails.Length > 0)
            {
                var index = random.Next(recoveryEmails.Count());
                var recoveryEmail = recoveryEmails[index];
                var res = ObjectMapper.Map<RecoveryEmail, RecoveryEmailDto>(recoveryEmail);
                recoveryEmail.Status = RecoveryEmailStatus.Completed;
                await Repository.UpdateAsync(recoveryEmail, autoSave: true);
                return res;
            }
            return new RecoveryEmailDto();
        }

        private bool ValidateRecoveryEmailInput(string str)
        {
            return Regex.IsMatch(str, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\|(.+)");

        }

        public override async Task<RecoveryEmailDto> CreateAsync(CreateUpdateRecoveryEmailDto input)
        {
            var recoveryEmail = ObjectMapper.Map<CreateUpdateRecoveryEmailDto, RecoveryEmail>(input);
            recoveryEmail.Created = DateTime.Now;
            recoveryEmail.Status = RecoveryEmailStatus.Ready;
            var res = await Repository.InsertAsync(recoveryEmail, autoSave: true);

            return await MapToGetOutputDtoAsync(res);
        }

        [Authorize(GmailServerPermissions.RecoveryEmails.Create)]
        public async Task CreateManyAsync(CreateManyRecoveryEmailInputDto input)
        {
            var recoveryEmails = input.Emails.Split("\r\n").ToList();
            var entities = new List<RecoveryEmail>();
            recoveryEmails.ForEach((Action<string>)(re =>
            {
                if (ValidateRecoveryEmailInput(re))
                {
                    var reSplit = re.Split('|').ToArray();
                    var entity = new RecoveryEmail()
                    {
                        Username = input.Username,
                        Email = reSplit[0],
                        Password = reSplit[1],
                        Status = RecoveryEmailStatus.Ready,
                        Created = DateTime.Now,
                    };
                    entities.Add(entity);
                }
            }));
            if (entities.Count > 0)
            {
                await Repository.BulkInsertAsync(entities);
            }
        }

        [Authorize(GmailServerPermissions.RecoveryEmails.Delete)]
        public async Task DeleteAllAsync()
        {
            await Repository.DeleteAllAsync();
        }

        public async Task<RecoveryEmailDto> GetFirstRecoveryEmailAsync()
        {
            var query = Repository.Where(x => x.Status == RecoveryEmailStatus.Ready);
            query = query.OrderByDescending(x => x.Created);
            var recoveryEmail = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (recoveryEmail != null)
            {
                var res = ObjectMapper.Map<RecoveryEmail, RecoveryEmailDto>(recoveryEmail);
                recoveryEmail.Status = RecoveryEmailStatus.Completed;
                await Repository.UpdateAsync(recoveryEmail, autoSave: true);
                return res;
            }
            return new RecoveryEmailDto();
        }
    }
}
