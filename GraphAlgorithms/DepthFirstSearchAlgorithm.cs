using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms
{
    public class DepthFirstSearchAlgorithm : GraphAlgorithm
    {
        private NodeVisitedTracker Visited;
        private int[] Prev;
        private int[] Component;
        private int StartNodeIndex;

        public DepthFirstSearchAlgorithm(Graph g, int startNodeIndex = 0) : base(g)
        {
            Visited = new NodeVisitedTracker(g.N);
            Prev = new int[g.N];
            Component = new int[g.N];
            StartNodeIndex = startNodeIndex;

            InitializeValues();
        }

        public override void InitializeValues()
        {
            for (int i = 0; i < G.N; i++)
                Prev[i] = -1;
        }

        private void DFS(int v, int p)
        {
            this.OutputDescription.Append(v.ToString() + " ");
            Node vNode = G.GetNode(v);

            Visited[vNode] = true;
            Prev[v] = p;
            Component[v] = p == -1 ? v : Component[p];

            List<Edge> adjEdges = G.GetAdjacentEdges(vNode);

            for(int i = 0; i < adjEdges.Count; i++)
            {
                int destIndex = adjEdges[i].GetDestNodeIndex();
                Node destNode = G.GetNode(destIndex);

                if (!Visited[destNode])
                    DFS(destIndex, v);
            }
        }

        public override void Run()
        {
            this.OutputDescription.AppendLine("------- Depth-first Search -------");
            this.OutputDescription.Append("DFS order: ");

            // First, we DFS from StartNodeIndex node
            DFS(StartNodeIndex, -1);

            // We check if any other node is unvisited, so we run DFS from them too (case for disconnected graph)
            foreach(Node node in G.Nodes)
                if (!Visited[node])
                    DFS(node.Index, -1);

            this.OutputDescription.AppendLine();

            foreach(int component in Component.Distinct())
            {
                this.OutputDescription.Append(string.Format("Component {0}: ", component));

                for (int i = 0; i < G.N; i++)
                    if (Component[i] == component)
                        this.OutputDescription.Append(i.ToString() + " ");

                this.OutputDescription.AppendLine();
            }

            this.OutputDescription.AppendLine("------- END Depth-first Search -------");
        }
    }
}
