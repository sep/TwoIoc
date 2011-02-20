using System.Collections.Generic;
using Net35;

namespace TwoIoc.Extensions
{
    public static class EnumerableExt
    {
        public static T MaxItem<T>(this IEnumerable<T> target, Func<T, int> numberSelector)
        {
            var maxItem = default(T);
            var maxNumber = -1;
            foreach(var item in target)
            {
                var newNumber = numberSelector(item);
                if (newNumber > maxNumber)
                {
                    maxNumber = newNumber;
                    maxItem = item;
                }
            }
            return maxItem;
        }
    }
}