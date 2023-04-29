using GmailServer.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface IAppleOrderRepository : IRepository<AppleOrder, long>
    {
        IQueryable<AppleOrder> FullTextSearch(IQueryable<AppleOrder> query, Expression<Func<AppleOrder, string>> keySelector, string value);

        Task ExecuteSqlRawAsync(string query);

        Task DeleteAppleOrderByTimeoutAsync(int timeout); // minutes
    }
}
