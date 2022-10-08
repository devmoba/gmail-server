using GmailServer.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface IGmailTypeRepository : IRepository<GmailType, long>
    {
        IQueryable<GmailType> FullTextSearch(IQueryable<GmailType> query, Expression<Func<GmailType, string>> keySelector, string value);
    }
}
