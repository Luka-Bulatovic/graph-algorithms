using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Shared
{
    // Since this is shared project, this should probably be called Key/Value pair, or Search Parameter should not be in Shared
    public class MultiSelectListItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
