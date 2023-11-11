using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Core.Algorithms
{
    public class BreadthFirstSearchAlgorithm : GraphAlgorithm
    {
        private NodeVisitedTracker _visited;
        private NodePropertyArray<Node> _prev;
        private NodePropertyArray<int> _distance;
        private Node _startNode;

        public BreadthFirstSearchAlgorithm(Graph g, Node startNode) : base(g)
        {
            _visited = new NodeVisitedTracker(g.N);
            _prev = new NodePropertyArray<Node>(g.N);
            _distance = new NodePropertyArray<int>(g.N);
            _startNode = startNode;

            InitializeValues();
        }

        public override void InitializeValues()
        {
            _prev.InitializeValues(null);
            _distance.InitializeValues(INF_DISTANCE);
        }

        private void BFS()
        {
            Queue<Node> q = new Queue<Node>();

            q.Enqueue(_startNode);
            _distance[_startNode] = 0;
            _visited[_startNode] = true;

            while (q.Count > 0)
            {
                Node currNode = q.Dequeue();
                int currNodeDistance = _distance[currNode];

                List<Edge> adjEdges = G.GetAdjacentEdges(currNode);

                for (int i = 0; i < adjEdges.Count; i++)
                {
                    Node toNode = adjEdges[i].DestNode;

                    if (!_visited[toNode])
                    {
                        q.Enqueue(toNode);

                        _distance[toNode] = currNodeDistance + 1;
                        _visited[toNode] = true;
                        _prev[toNode] = currNode;
                    }
                }
            }
        }

        public override void Run()
        {
            OutputDescription.AppendLine("------- Breadth-first Search -------");
            OutputDescription.AppendLine(string.Format("Running from node {0}...", _startNode.Label));

            BFS();

            OutputDescription.AppendLine(string.Format("Distances from node {0}:", _startNode.Label));

            foreach (Node node in G.Nodes)
            {
                if (node.Index == _startNode.Index)
                    continue;

                OutputDescription.AppendLine(string.Format("Node {0}: {1}", node.Label, _distance[node]));
            }

            OutputDescription.AppendLine("------- END Breadth-first Search -------");
        }

        public int GetDistanceToNode(Node node)
        {
            return _distance[node];
        }
    }
}
