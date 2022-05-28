using GmailServer.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using Volo.Abp.Domain.Repositories;

namespace GmailServer.Repositories
{
    public interface IGmailRepository : IRepository<Gmail, long> 
    {
        IQueryable<Gmail> FullTextSearch(IQueryable<Gmail> query, Expression<Func<Gmail, string>> keySelector, string value);
    }
}
