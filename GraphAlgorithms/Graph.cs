using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms
{
    public class Graph
    {
        public int N { get; set; } // Number of Nodes
        public int M { get; set; } // Number of Edges
        
        public List<Node> Nodes { get; set; }
        public Dictionary<int, List<Edge>> Adj { get; set; }
        public int[,] AdjMatrix;

        public Graph(int n)
        {
            N = n;
            M = 0;
            Nodes = new List<Node>();
            Adj = new Dictionary<int, List<Edge>>();
            AdjMatrix = new int[N, N];

            for (int i = 0; i < N; i++)
                Adj.Add(i, new List<Edge>());
        }

        public Node GetNode(int v)
        {
            return Nodes[v];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("------- Printing Graph -------");
            sb.AppendLine(string.Format("Nodes: {0} ; Edges: {1}", N, M));
            sb.AppendLine();
            sb.AppendLine("List of Edges:");

            for (int i = 0; i < N; i++)
            {
                List<Edge> edges = Adj[i];

                for(int j = 0; j < edges.Count; j++)
                {
                    int q = edges[j].GetDestNodeIndex();

                    // If connectivity between i and q is simmetric, print this edge only once with <-> symbol, in case when i < q
                    if (AdjMatrix[i, q] > 0 && AdjMatrix[q, i] > 0)
                    {
                        if(i < q)
                            sb.AppendLine(string.Format("{0} <-> {1}", Nodes[i].Label, Nodes[q].Label));
                    }
                    else
                        sb.AppendLine(string.Format("{0} -> {1}", Nodes[i].Label, Nodes[q].Label));

                }
            }

            sb.AppendLine("------- END Printing Graph -------");

            return sb.ToString();
        }
    }
}
