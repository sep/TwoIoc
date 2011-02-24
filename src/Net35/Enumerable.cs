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
    }
}