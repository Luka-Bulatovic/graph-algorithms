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
        private NodePropertyArray<BipartiteColors> _bipartiteColoring;
        private Node _startNode;
        private int _numberOfComponents = 0;
        private bool _isBipartiteGraph = true;

        public BreadthFirstSearchAlgorithm(Graph g, Node startNode) : base(g)
        {
            _visited = new NodeVisitedTracker(g.N);
            _prev = new NodePropertyArray<Node>(g.N);
            _distance = new NodePropertyArray<int>(g.N);
            _bipartiteColoring = new NodePropertyArray<BipartiteColors>(g.N);
            _startNode = startNode;

            InitializeValues();
        }

        public override void InitializeValues()
        {
            _isBipartiteGraph = true;
            _numberOfComponents = 0;
            _prev.InitializeValues(null);
            _distance.InitializeValues(INF_DISTANCE);
            _bipartiteColoring.InitializeValues(BipartiteColors.Undefined);
        }

        private void BFS(Node startNode)
        {
            Queue<Node> q = new Queue<Node>();

            q.Enqueue(startNode);
            _distance[startNode] = 0;
            _visited[startNode] = true;
            _bipartiteColoring[startNode] = BipartiteColors.First;

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
                        _bipartiteColoring[toNode] = 
                            _bipartiteColoring[currNode] == BipartiteColors.First ? 
                            BipartiteColors.Second 
                            : BipartiteColors.First;
                    }
                    else if (_visited[toNode] && _bipartiteColoring[toNode] == _bipartiteColoring[currNode])
                    {
                        _isBipartiteGraph = false;
                    }
                }
            }
        }

        public override void Run()
        {
            _isExecuted = false;
            InitializeValues();

            OutputDescription.AppendLine("------- Breadth-first Search -------");
            OutputDescription.AppendLine(string.Format("Running from node {0}...", _startNode.Label));

            _numberOfComponents++;
            BFS(_startNode);

            foreach(Node node in G.Nodes)
            {
                if (!_visited[node])
                {
                    _numberOfComponents++;
                    BFS(node);
                }
            }

            OutputDescription.AppendLine(string.Format("Distances from node {0}:", _startNode.Label));

            foreach (Node node in G.Nodes)
            {
                if (node.Index == _startNode.Index)
                    continue;

                OutputDescription.AppendLine(string.Format("Node {0}: {1}", node.Label, _distance[node]));
            }

            OutputDescription.AppendLine("------- END Breadth-first Search -------");

            _isExecuted = true;
        }

        public int GetDistanceToNode(Node node)
        {
            return _distance[node];
        }

        public bool IsBipartiteGraph()
        {
            return _isBipartiteGraph;
        }
    }
}
