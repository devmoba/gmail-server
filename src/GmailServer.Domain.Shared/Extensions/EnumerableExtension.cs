using System;
using System.Collections.Generic;

namespace GmailServer.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<List<T>> Split<T>(this List<T> source, int count)
        {
            int rangeSize = source.Count / count;
            int firstRangeSize = rangeSize + source.Count % count;
            int index = 0;

            yield return source.GetRange(index, firstRangeSize);
            index += firstRangeSize;

            while (index < source.Count)
            {
                yield return source.GetRange(index, rangeSize);
                index += rangeSize;
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
