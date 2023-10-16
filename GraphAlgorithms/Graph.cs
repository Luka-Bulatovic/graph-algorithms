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
        private AdjacencyMatrix AdjMatrix;
        private bool IsUndirected;

        public Graph(int n, bool isUndirected = true)
        {
            N = n;
            M = 0;
            Nodes = new List<Node>();
            Adj = new Dictionary<int, List<Edge>>();
            AdjMatrix = new AdjacencyMatrix(N);

            for (int i = 0; i < N; i++)
                Adj.Add(i, new List<Edge>());

            IsUndirected = isUndirected;
        }

        public Node GetNode(int v)
        {
            return Nodes[v];
        }

        public void SetNodesAdjacency(Node a, Node b)
        {
            AdjMatrix.SetNodesAdjacency(a, b, IsUndirected);
        }

        public int GetNodesAdjacency(Node a, Node b)
        {
            return AdjMatrix.GetNodesAdjacency(a, b);
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
                Node srcNode = Nodes[i];
                List<Edge> edges = Adj[i];

                for(int j = 0; j < edges.Count; j++)
                {
                    Node destNode = edges[j].DestNode;

                    // If connectivity between i and q is simmetric, print this edge only once with <-> symbol, in case when i < q
                    if (AdjMatrix.GetNodesAdjacency(srcNode, destNode) > 0 && AdjMatrix.GetNodesAdjacency(destNode, srcNode) > 0)
                    {
                        if(srcNode.Index < destNode.Index)
                            sb.AppendLine(string.Format("{0} <-> {1}", srcNode.Label, destNode.Label));
                    }
                    else
                        sb.AppendLine(string.Format("{0} -> {1}", srcNode.Label, destNode.Label));
                }
            }

            sb.AppendLine("------- END Printing Graph -------");

            return sb.ToString();
        }
    }
}
