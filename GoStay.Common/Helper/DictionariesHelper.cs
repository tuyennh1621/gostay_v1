using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Common.Helper
{
    public static class MergeDictionaries
    {
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> first, Dictionary<TKey, TValue> second)
        {
            if (second == null || first == null) return null;
            foreach (var item in second)
                if (!first.ContainsKey(item.Key))
                    first.Add(item.Key, item.Value);
            return first;
        }
    }
}
