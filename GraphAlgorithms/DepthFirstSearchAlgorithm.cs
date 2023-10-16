using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms
{
    public class DepthFirstSearchAlgorithm : GraphAlgorithm
    {
        private int[] Visited;
        private int[] Prev;
        private int[] Component;
        private int StartNodeIndex;

        public DepthFirstSearchAlgorithm(Graph g, int startNodeIndex = 0) : base(g)
        {
            Visited = new int[g.N];
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

            Visited[v] = 1;
            Prev[v] = p;
            Component[v] = p == -1 ? v : Component[p];

            for(int i = 0; i < G.Adj[v].Count; i++)
            {
                int dest = G.Adj[v][i].GetDestNodeIndex();

                if (Visited[dest] == 0)
                    DFS(dest, v);
            }
        }

        public override void Run()
        {
            this.OutputDescription.AppendLine("------- Depth-first Search -------");
            this.OutputDescription.Append("DFS order: ");

            // First, we DFS from StartNodeIndex node
            DFS(StartNodeIndex, -1);

            // We check if any other node is unvisited, so we run DFS from them too (case for disconnected graph)
            for(int i = 0; i < G.N; i++)
                if (Visited[i] == 0)
                    DFS(i, -1);

            this.OutputDescription.AppendLine();

            foreach(int component in Component.Distinct())
            {
                this.OutputDescription.Append(string.Format("Component {0}: ", component));

                for (int i = 0; i < Visited.Length; i++)
                    if (Component[i] == component)
                        this.OutputDescription.Append(i.ToString() + " ");

                this.OutputDescription.AppendLine();
            }

            this.OutputDescription.AppendLine("------- END Depth-first Search -------");
        }
    }
}
