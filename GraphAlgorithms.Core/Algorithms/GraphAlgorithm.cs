using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Core.Algorithms
{
    public abstract class GraphAlgorithm
    {
        public Graph G { get; set; }
        public StringBuilder OutputDescription;

        public static int INF_DISTANCE = 1 << 30;

        public enum BipartiteColors
        {
            Undefined = 0,
            First = 1,
            Second = 2
        }

        protected bool _isExecuted = false;

        public GraphAlgorithm(Graph g)
        {
            G = g;
            OutputDescription = new StringBuilder();
        }

        public abstract void InitializeValues();

        public abstract void Run();

        public bool IsExecuted()
        {
            return _isExecuted;
        }

        public override string ToString()
        {
            return '\n' + OutputDescription.ToString();
        }
    }
}
