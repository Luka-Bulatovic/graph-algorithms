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

        public void ConnectNodes(Node a, Node b, bool isUndirectedEdge = true)
        {
            AdjList[a.Index].Add(new Edge(a, b));

            if (isUndirectedEdge)
                AdjList[b.Index].Add(new Edge(b, a));
        }

        public List<Edge> GetAdjacentEdges(Node v)
        {
            return AdjList[v.Index];
        }
    }
}
