using GmailServer.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface IFakeSettingRepository : IRepository<FakeSetting, long> 
    {
        IQueryable<FakeSetting> FullTextSearch(IQueryable<FakeSetting> query, Expression<Func<FakeSetting, string>> keySelector, string value);
    }
}
