using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Core
{
    public class Graph
    {
        public int ID { get; set; }
        public int N { get; set; } // Number of Nodes
        public int M // Number of Edges
        {
            get { return edges.Count; } 
        }

        private List<Node> nodes;
        public List<Node> Nodes => nodes;

        private List<Edge> edges;
        public List<Edge> Edges => edges;

        public GraphProperties GraphProperties { get; }

        private AdjacencyList AdjList;
        private AdjacencyMatrix AdjMatrix;

        private bool isUndirected;
        public bool IsUndirected => isUndirected;

        private List<GraphClassEnum> graphClasses;
        public List<GraphClassEnum> GraphClasses => graphClasses;

        public Graph(int id, int n, bool isUndirected = true)
        {
            ID = id;
            N = n;
            nodes = new List<Node>();
            edges = new List<Edge>();
            
            AdjMatrix = new AdjacencyMatrix(N);
            AdjList = new AdjacencyList(N);

            GraphProperties = new GraphProperties();
            graphClasses = new List<GraphClassEnum>();

            this.isUndirected = isUndirected;
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
            AdjMatrix.ConnectNodes(a, b, isUndirected);

            Edge e = AdjList.ConnectNodes(a, b, isUndirected);
            edges.Add(e);
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
