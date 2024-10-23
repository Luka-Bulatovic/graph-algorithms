using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Core.Algorithms
{
    public class DepthFirstSearchAlgorithm : GraphAlgorithm
    {
        private NodeVisitedTracker _visited;
        private NodePropertyArray<Node> _prev;
        private NodePropertyArray<int> _component;
        private Node _startNode;
        private int _numberOfComponents;

        private NodePropertyArray<int> _coloring; // (0,1,2) Used for detecting cycles
        private int _firstCycleLength = 0;
        public int FirstCycleLength
        {
            get { return _firstCycleLength; }
        }

        public DepthFirstSearchAlgorithm(Graph g, Node startNode) : base(g)
        {
            _numberOfComponents = 0;
            _visited = new NodeVisitedTracker(g.N);
            _prev = new NodePropertyArray<Node>(g.N);
            _component = new NodePropertyArray<int>(g.N);
            _coloring = new NodePropertyArray<int>(g.N);
            _startNode = startNode;

            InitializeValues();
        }

        public override void InitializeValues()
        {
            _numberOfComponents = 0;
            _visited.Reset();
            _prev.InitializeValues(null);
            _component.InitializeValues(0);
            _coloring.InitializeValues(0);
        }

        private void DFS(Node currNode, Node parentNode)
        {
            OutputDescription.Append(currNode.Label + " ");

            _visited[currNode] = true;
            _coloring[currNode] = 1;
            _prev[currNode] = parentNode;
            _component[currNode] = parentNode == null ? currNode.Index : _component[parentNode];

            List<Edge> adjEdges = G.GetAdjacentEdges(currNode);

            foreach (Edge edge in adjEdges)
            {
                Node destNode = edge.DestNode;

                if (!_visited[destNode])
                    DFS(destNode, currNode);
                else if (_coloring[destNode] == 1 && destNode.Index != _prev[currNode].Index) // First Cycle detected
                {
                    if (_firstCycleLength == 0)
                        _firstCycleLength = CalculateCycleLength(destNode, currNode);
                }
            }

            _coloring[currNode] = 2;
        }

        private int CalculateCycleLength(Node firstNode, Node lastNode)
        {
            Node currNode = lastNode;
            int length = 1;
            
            while(currNode.Index != firstNode.Index)
            {
                length++;
                currNode = _prev[currNode];
            }

            return length;
        }

        public override void Run()
        {
            _isExecuted = false;
            InitializeValues();

            OutputDescription.AppendLine("------- Depth-first Search -------");
            OutputDescription.Append("DFS order: ");

            // First, we DFS from StartNodeIndex node
            _numberOfComponents++;
            DFS(_startNode, null);

            // We check if any other node is unvisited, so we run DFS from them too (case for disconnected graph)
            foreach (Node node in G.Nodes)
            {
                if (!_visited[node])
                {
                    _numberOfComponents++;
                    DFS(node, null);
                }
            }

            //OutputDescription.AppendLine();

            //int currentComponent = 0;
            //while (currentComponent < G.N)
            //{
            //    bool isComponentEmpty = true;

            //    foreach (Node node in G.Nodes)
            //    {
            //        if (_component[node] == currentComponent)
            //        {
            //            isComponentEmpty = false;
            //            break;
            //        }
            //    }

            //    if (isComponentEmpty)
            //        continue;

            //    OutputDescription.Append(string.Format("Component {0}: ", currentComponent));

            //    foreach (Node node in G.Nodes)
            //    {
            //        if (_component[node] == currentComponent)
            //        {
            //            OutputDescription.Append(node.Index.ToString() + " ");
            //        }
            //    }

            //    OutputDescription.AppendLine();

            //    currentComponent++;
            //}

            OutputDescription.AppendLine("------- END Depth-first Search -------");

            _isExecuted = true;
        }

        public int GetNumberOfComponents()
        {
            return _numberOfComponents;
        }

        public bool ContainsCycle()
        {
            return _firstCycleLength > 0;
        }
    }
}
