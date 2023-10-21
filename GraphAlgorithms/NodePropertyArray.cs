using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms
{
    public class NodePropertyArray<T>
    {
        private T[] _values;

        public NodePropertyArray(int n)
        {
            _values = new T[n];
        }

        public T this[Node node]
        {
            get
            {
                return _values[node.Index];
            }
            set
            {
                _values[node.Index] = value;
            }
        }

        public void InitializeValues(T? value)
        {
            for (int i = 0; i < _values.Length; i++)
                _values[i] = value;
        }
    }
}
