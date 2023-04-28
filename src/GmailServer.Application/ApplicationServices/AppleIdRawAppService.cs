using GmailServer.AppleIdRaws;
using GmailServer.Entities;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.ApplicationServices
{
    [RemoteService(IsEnabled = false)]
    public class AppleIdRawAppService : ApplicationService, IAppleIdRawAppService
    {
        private readonly IRepository<AppleIdRaw, long> _repository;

        public AppleIdRawAppService(IRepository<AppleIdRaw, long> repository)
        {
            _repository = repository;
        }

        public async Task<AppleIdRawDto> CreateAsync(CreateAppleIdRawInputDto input)
        {
            var appleIdRaw = ObjectMapper.Map<CreateAppleIdRawInputDto, AppleIdRaw>(input);
            appleIdRaw.Created = DateTime.Now;
            var entity = await _repository.InsertAsync(appleIdRaw, true);
            return ObjectMapper.Map<AppleIdRaw, AppleIdRawDto>(entity);
        }

        [Authorize(GmailServerPermissions.AppleIdRaws.Statistic)]
        public Task<PagedResultDto<AppleIdRawStatisticDailyDto>> GetAppleIdRawStatisticDailyAsync(AppleIdRawStatisticFilterDto input)
        {
            throw new NotImplementedException();
        }
    }
}
