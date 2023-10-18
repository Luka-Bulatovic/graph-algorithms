using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms
{
    public class NodeVisitedTracker
    {
        private bool[] _visitedArray;

        public NodeVisitedTracker(int n)
        {
            _visitedArray = new bool[n];
        }

        public bool this[Node node]
        {
            get
            {
                return _visitedArray[node.Index];
            }
            set
            {
                _visitedArray[node.Index] = value;
            }
        }

        public void Reset()
        {
            for (int i = 0; i < _visitedArray.Length; i++)
                _visitedArray[i] = false;
        }
    }
}
