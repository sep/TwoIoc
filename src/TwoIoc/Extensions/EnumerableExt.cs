using System;
using System.Collections.Generic;
using Net35;

namespace TwoIoc.Extensions
{
    public static class EnumerableExt
    {
        public static T MaxItem<T>(this IEnumerable<T> target, Comparison<T> comparison)
        {
            var listed = target.ToList();

            if (listed.Empty()) return default(T);

            var maxItem = listed[0];

            foreach(var item in listed.Skip(1))
            {
                if (comparison(item, maxItem) > 0)
                {
                    maxItem = item;
                }
            }
            return maxItem;
        }
    }
}