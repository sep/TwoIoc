using System;
using System.Collections;
using System.Collections.Generic;

namespace Net35
{
    public static class EnumerableExt
    {
        public static T Max<T>(this IEnumerable<T> target, Func<T, DateTime> selector)
        {
            var max = target.FirstOrDefault();
            foreach(var item in target)
                if(selector(item) > selector(max))
                    max = item;
            return max;
        }

        public static T Min<T>(this IEnumerable<T> target, Func<T, DateTime> selector)
        {
            var min = target.FirstOrDefault();
            foreach(var item in target)
                if(selector(item) < selector(min))
                    min = item;
            return min;
        }

        public static IDictionary<TKey, List<TValue>> GroupBy<T, TKey, TValue>(this IEnumerable<T> target, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        {
            var groups = new Dictionary<TKey, List<TValue>>();
            foreach (var item in target)
            {
                var key = keySelector(item);
                var value = valueSelector(item);

                if (!groups.ContainsKey(key))
                    groups.Add(key, new List<TValue>());

                groups[key].Add(value);
            }

            return groups;
        }

        public static IDictionary<TKey, TValue> ToDictionary<T, TKey, TValue>(this IEnumerable<T> target, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            foreach(var item in target)
                dictionary.Add(keySelector(item), valueSelector(item));
            return dictionary;
        }

        public static T IndexOrDefault<T>(this List<T> target, int index)
        {
            if (index < target.Count)
                return target[index];
            return default(T);
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> target, IEnumerable<T> other)
        {
            foreach (var item in target)
                yield return item;
            foreach (var item in other)
                yield return item;
        }

        public static bool Empty<T>(this IEnumerable<T> target)
        {
            foreach (var _ in target)
                return false;
            return true;
        }

        public static bool HasOne<T>(this IEnumerable<T> target)
        {
            return ToList(target.Take(2)).Count == 1;
        }

        public static IEnumerable<T> Cast<T>(this IEnumerable target)
        {
            foreach(var item in target)
                yield return (T) item;
        }

        public static void Each<T>(this IEnumerable<T> target, Action<T> action)
        {
            foreach(var item in target)
                action(item);
        }

        public static void Each<T>(this IEnumerable<T> target, Action<T, int> action)
        {
            var cnt = 0;
            foreach(var item in target)
                action(item, cnt++);
        }

        public static IEnumerable<T> Where<T>(this IEnumerable<T> target, Func<T, bool> filter)
        {
            foreach (var item in target)
                if (filter(item))
                    yield return item;
        }

        public static IEnumerable<T> Skip<T>(this IEnumerable<T> target, int numToSkip)
        {
            var cnt = 0;
            foreach(var item in target)
                if(cnt++ < numToSkip)
                    continue;
                else
                    yield return item;
        }
        public static IEnumerable<T> Take<T>(this IEnumerable<T> target, int numToTake)
        {
            var cnt = 0;
            foreach (var item in target)
                if (cnt++ < numToTake)
                    yield return item;
                else
                    yield break;
        }

        public static IEnumerable<TResult> Select<T, TResult>(this IEnumerable<T> target, Func<T, TResult> selector)
        {
            foreach (var item in target)
                yield return selector(item);
        }

        public static IEnumerable<TResult> Select<T, TResult>(this IEnumerable<T> target, Func<T, int, TResult> selector)
        {
            int cnt = 0;
            foreach (var item in target)
                yield return selector(item, cnt++);
        }

        public static IEnumerable<TResult> SelectMany<T, TResult>(this IEnumerable<T> target, Func<T, IEnumerable<TResult>> manySelector)
        {
            foreach (var item in target)
                foreach (var single in manySelector(item))
                    yield return single;
        }

        public static IEnumerable<T> Eval<T>(this IEnumerable<T> target)
        {
            return ToList(target);
        }

        public static List<T> ToList<T>(this IEnumerable<T> target)
        {
            var list = new List<T>();
            foreach (var item in target)
                list.Add(item);
            return list;
        }

        public static T[] ToArray<T>(this IEnumerable<T> target)
        {
            return ToList(target).ToArray();
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> target)
        {
            foreach (var item in target)
                return item;
            return default(T);
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> target, Func<T, bool> filter)
        {
            return FirstOrDefault(target.Where(filter));
        }

        public static T First<T>(this IEnumerable<T> target)
        {
            foreach(var item in target)
                return item;
            throw new InvalidOperationException("The source sequence is empty.");
        }

        public static T First<T>(this IEnumerable<T> target, Func<T, bool> filter)
        {
            return target.Where(filter).First();
        }

        public static T LastOrDefault<T>(this IEnumerable<T> target)
        {
            T storedItem = default(T);
            foreach (var item in target)
                storedItem = item;
            return storedItem;
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> target, Func<T, int> selector)
        {
            var list = new List<T>(target);
            list.Sort((x, y) => selector(x) - selector(y));

            return list;
        }

        public static bool Any<T>(this IEnumerable<T> target, Func<T, bool> predicate)
        {
            return !target.Where(predicate).Empty();
        }
    }
}