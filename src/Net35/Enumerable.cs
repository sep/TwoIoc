using System.Collections.Generic;

namespace Net35
{
    public static class Enumerable
    {
        public static IEnumerable<int> Range(int numElements)
        {
            for (var i = 0; i < numElements; i++)
                yield return i;
        }

        public static IEnumerable<int> Infinite()
        {
            var cnt = 0;
            while(true)
                yield return cnt++;
        }
    }
}