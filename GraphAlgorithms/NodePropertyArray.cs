using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms
{
    public class NodePropertyArray
    {
        private int[] _values;

        public NodePropertyArray(int n)
        {
            _values = new int[n];
        }

        public int this[Node node]
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

        public void Reset(int value = 0)
        {
            for (int i = 0; i < _values.Length; i++)
                _values[i] = value;
        }
    }
}
