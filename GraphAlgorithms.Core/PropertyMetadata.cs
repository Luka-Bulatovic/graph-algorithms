using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Core
{
    public class PropertyMetadata
    {
        public Func<object> Getter { get; set; }
        public Type Type { get; set; }

        public PropertyMetadata(Func<object> getter, Type type)
        {
            Getter = getter;
            Type = type;
        }
    }
}
