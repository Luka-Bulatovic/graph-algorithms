using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Algorithms
{
    public class TopologicalSortAlgorithm : GraphAlgorithm
    {
        NodeVisitedTracker Visited;
        List<Node> SortedList;

        public TopologicalSortAlgorithm(Graph g) : base(g)
        {
            Visited = new NodeVisitedTracker(g.N);
            SortedList = new List<Node>();
        }

        public override void InitializeValues()
        {
        }

        private void DFS(Node node)
        {
            Visited[node] = true;

            List<Edge> adjEdges = G.GetAdjacentEdges(node);
            foreach (Edge edge in adjEdges)
            {
                Node childNode = edge.DestNode;
                if (!Visited[childNode])
                    DFS(childNode);
            }

            SortedList.Insert(0, node);
        }

        public override void Run()
        {
            foreach (Node node in G.Nodes)
            {
                if (!Visited[node])
                    DFS(node);
            }

            OutputDescription.AppendLine("------- Topological Sort --------");

            foreach (Node node in SortedList)
                OutputDescription.AppendLine(string.Format("{0}", node.Label));

            OutputDescription.AppendLine("------- END Topological Sort --------");
        }
    }
}
