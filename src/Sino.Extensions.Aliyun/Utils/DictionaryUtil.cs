using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Extensions.Aliyun.Utils
{
    public static class DictionaryUtil
    {
        public static void Add<T>(IDictionary<string, string> dic, string key, T value)
        {
            var stringValue = value == null ? null : value.ToString();
            Add<string, string>(dic, key, stringValue);
        }

        public static void Add<TKey, TValue>(IDictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (null == value)
            {
                return;
            }

            if (dic == null)
            {
                dic = new Dictionary<TKey, TValue>();
            }
            else if (dic.ContainsKey(key))
            {
                dic.Remove(key);
            }

            dic.Add(key, value);
        }
    }
}
