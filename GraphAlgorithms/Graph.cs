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


        private List<Node> nodes;
        public List<Node> Nodes => nodes;

        private AdjacencyList AdjList;
        private AdjacencyMatrix AdjMatrix;
        private bool IsUndirected;

        public Graph(int n, bool isUndirected = true)
        {
            N = n;
            M = 0;
            nodes = new List<Node>();
            
            AdjMatrix = new AdjacencyMatrix(N);
            AdjList = new AdjacencyList(N);

            IsUndirected = isUndirected;
        }

        public void AddNode(Node node)
        {
            this.nodes.Add(node);
        }

        public Node GetNode(int v)
        {
            return nodes[v];
        }

        public void ConnectNodes(Node a, Node b)
        {
            AdjMatrix.ConnectNodes(a, b, IsUndirected);
            AdjList.ConnectNodes(a, b, IsUndirected);
        }

        public int GetNodesAdjacency(Node a, Node b)
        {
            return AdjMatrix.GetNodesAdjacency(a, b);
        }

        public bool AreNodesAdjacent(Node a, Node b)
        {
            return AdjMatrix.GetNodesAdjacency(a, b) > 0;
        }

        public List<Edge> GetAdjacentEdges(Node v)
        {
            return AdjList.GetAdjacentEdges(v);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("------- Printing Graph -------");
            sb.AppendLine(string.Format("Nodes: {0} ; Edges: {1}", N, M));
            sb.AppendLine();
            sb.AppendLine("List of Edges:");

            foreach (Node srcNode in nodes)
            {
                List<Edge> adjEdges = GetAdjacentEdges(srcNode);

                foreach (Edge edge in adjEdges)
                {
                    Node destNode = edge.DestNode;

                    // If connectivity between i and q is simmetric, print this edge only once with <-> symbol, in case when i < q
                    if (AreNodesAdjacent(srcNode, destNode) && AreNodesAdjacent(destNode, srcNode))
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
