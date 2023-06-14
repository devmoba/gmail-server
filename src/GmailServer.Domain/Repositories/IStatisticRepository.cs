using GmailServer.Entities;
using System.Linq.Expressions;
using System.Linq;
using System;
using Volo.Abp.Domain.Repositories;
using System.Threading.Tasks;

namespace GmailServer.Repositories
{
    public interface IStatisticRepository : IRepository<Statistic, long>
    {
        IQueryable<Statistic> FullTextSearch(IQueryable<Statistic> query, Expression<Func<Statistic, string>> keySelector, string value);

        Task AddOrUpdateForEntityAsync(string entityName, int recoveryDays = 1);

        Task ExecuteSqlRawAsync(string query);
    }
}
