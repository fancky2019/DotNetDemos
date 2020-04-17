using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019
{
    public class KeyValuePairDemo
    {
        public void Test()
        {
            Fun();
        }

        private void Fun()
        {
            KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>("k1", "v1");

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("key1", "value1");

            IEnumerable<KeyValuePair<string, string>> keyValuePairs = dictionary;

            var dict2 = keyValuePairs.ToDictionary(p => p.Key, p => p.Value);

            ////key添加重复抛异常:已添加了具有相同键的项。
            //dictionary.Add("key1", "value1");

            foreach (var keyValuePair1 in dictionary)
            {
                var key = keyValuePair1.Key;
                var value = keyValuePair1.Value;
            }

            foreach (var key in dictionary.Keys)
            {
                var valuer = dictionary[key];
            }

            foreach (var value in dictionary.Values)
            {

            }
        }
    }
}
