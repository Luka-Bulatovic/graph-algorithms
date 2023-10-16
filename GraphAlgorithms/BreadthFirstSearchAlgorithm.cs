using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms
{
    public class BreadthFirstSearchAlgorithm : GraphAlgorithm
    {
        private int[] Visited;
        private int[] Prev;
        private int[] Distance;
        private int StartNodeIndex;

        public BreadthFirstSearchAlgorithm(Graph g, int startNodeIndex = 0) : base(g)
        {
            Visited = new int[g.N];
            Prev = new int[g.N];
            Distance = new int[g.N];
            StartNodeIndex = startNodeIndex;

            InitializeValues();
        }

        public override void InitializeValues()
        {
            for (int i = 0; i < G.N; i++)
            {
                Prev[i] = -1;
                Distance[i] = INF_DISTANCE;
            }
        }

        private void BFS(int v)
        {
            Queue<int> q = new Queue<int>();

            q.Enqueue(v);
            Distance[v] = 0;
            Visited[v] = 1;

            while(q.Count > 0)
            {
                int currNodeIndex = q.Dequeue();
                Node currNode = G.GetNode(currNodeIndex);
                int currNodeDistance = Distance[currNodeIndex];

                List<Edge> adjEdges = G.GetAdjacentEdges(currNode);

                for(int i = 0; i < adjEdges.Count; i++)
                {
                    int toIndex = adjEdges[i].GetDestNodeIndex();

                    if (Visited[toIndex] == 0)
                    {
                        q.Enqueue(toIndex);

                        Distance[toIndex] = currNodeDistance + 1;
                        Visited[toIndex] = 1;
                        Prev[toIndex] = currNodeIndex;
                    }
                }
            }
        }

        public override void Run()
        {
            this.OutputDescription.AppendLine("------- Breadth-first Search -------");
            this.OutputDescription.AppendLine(string.Format("Running from node {0}...", StartNodeIndex));

            BFS(StartNodeIndex);

            this.OutputDescription.AppendLine(string.Format("Distances from node {0}:", StartNodeIndex));
            for(int i = 0; i < G.N; i++)
            {
                if (i == StartNodeIndex)
                    continue;

                this.OutputDescription.AppendLine(string.Format("Node {0}: {1}", i, Distance[i]));
            }

            this.OutputDescription.AppendLine("------- END Breadth-first Search -------");
        }

        public int GetDistanceToNode(int nodeIndex)
        {
            return Distance[nodeIndex];
        }
    }
}
