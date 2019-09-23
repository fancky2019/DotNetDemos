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

        private void  Fun()
        {
            KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>("k1","v1");
  
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("key1", "value1");
            IEnumerable<KeyValuePair<string, string>> keyValuePairs = dictionary;

            var dict2 = keyValuePairs.ToDictionary(p => p.Key, p => p.Value);
        }
    }
}
