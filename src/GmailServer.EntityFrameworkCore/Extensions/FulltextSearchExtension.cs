using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace GmailServer.Extensions
{
    public static class FulltextSearchExtension
    {
        public static IQueryable<T> FullTextContains<T>(this IQueryable<T> query, Expression<Func<T, string>> keySelector, string value)
        {
            return query.Where(keySelector.Apply(key => EF.Functions.Contains(key, value)));
        }

        public static IQueryable<T> FullTextFreeText<T>(this IQueryable<T> query, Expression<Func<T, string>> keySelector, string value)
        {
            return query.Where(keySelector.Apply(key => EF.Functions.FreeText(key, value)));
        }
    }
}
