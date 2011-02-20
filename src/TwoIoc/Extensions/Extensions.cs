using System.Collections.Generic;
using Net35;

namespace TwoIoc.Extensions
{
    public static class Extensions
    {
        public static IDictionary<string, object> PropertyValuesToDictionary(this object target)
        {
            return target
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(target, new object[0]));
        }
    }
}