using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Core
{
    public class AdjacencyList
    {
        private Dictionary<int, List<Edge>> AdjList;
        private int N;

        public AdjacencyList(int n)
        {
            this.N = n;
            AdjList = new Dictionary<int, List<Edge>>();

            for(int i = 0; i < N; i++)
                AdjList.Add(i, new List<Edge>());
        }

        public Edge ConnectNodes(Node a, Node b, bool isUndirectedEdge = true)
        {
            Edge e = new Edge(a, b);

            AdjList[a.Index].Add(e);

            if (isUndirectedEdge)
                AdjList[b.Index].Add(new Edge(b, a));

            return e;
        }

        public List<Edge> GetAdjacentEdges(Node v)
        {
            return AdjList[v.Index];
        }
    }
}
