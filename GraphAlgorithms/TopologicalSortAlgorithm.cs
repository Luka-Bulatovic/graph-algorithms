using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms
{
    public class TopologicalSortAlgorithm : GraphAlgorithm
    {
        private int[] Visited;
        List<Node> SortedList;

        public TopologicalSortAlgorithm(Graph g) : base(g)
        {
            Visited = new int[G.N];
            SortedList = new List<Node>();
        }

        public override void InitializeValues()
        {
            for(int i = 0; i < G.N; i++)
            {
                Visited[i] = 0;
            }
        }

        private void DFS(Node node)
        {
            Visited[node.Index] = 1;

            List<Edge> adjEdges = G.GetAdjacentEdges(node);
            foreach (Edge edge in adjEdges)
            {
                int childNodeIndex = edge.GetDestNodeIndex();
                if (Visited[childNodeIndex] == 0)
                    DFS(G.Nodes[childNodeIndex]);
            }

            SortedList.Insert(0, node);
        }

        public override void Run()
        {
            for (int nodeId = 0; nodeId < G.N; nodeId++)
            {
                Node node = G.Nodes[nodeId];

                if (Visited[node.Index] == 0)
                    DFS(node);
            }

            this.OutputDescription.AppendLine("------- Topological Sort --------");

            foreach (Node node in SortedList)
                this.OutputDescription.AppendLine(string.Format("{0}", node.Label));
            
            this.OutputDescription.AppendLine("------- END Topological Sort --------");
        }
    }
}
