using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Shared
{
    public class SortParameter
    {
        public string Key { get; set; }
        public string Name { get; set; }

        public SortParameter(string key, string name)
        {
            Key = key;
            Name = name;
        }
    }
}
