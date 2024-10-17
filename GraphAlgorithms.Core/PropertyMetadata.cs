using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Core
{
    public class PropertyMetadata
    {
        public string Name;
        public Func<object> Getter;
        public Action<object> Setter;
        public Type Type;

        public PropertyMetadata(string name, Func<object> getter, Action<object> setter, Type type)
        {
            Name = name;
            Getter = getter;
            Setter = setter;
            Type = type;
        }
    }
}
