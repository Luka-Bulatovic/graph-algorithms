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
        private int _numberOfCycles;

        public DepthFirstSearchAlgorithm(Graph g, Node startNode) : base(g)
        {
            _numberOfComponents = 0;
            _numberOfCycles = 0;
            _visited = new NodeVisitedTracker(g.N);
            _prev = new NodePropertyArray<Node>(g.N);
            _component = new NodePropertyArray<int>(g.N);
            _startNode = startNode;

            InitializeValues();
        }

        public override void InitializeValues()
        {
            _numberOfComponents = 0;
            _numberOfCycles = 0;
            _prev.InitializeValues(null);
        }

        private void DFS(Node currNode, Node parentNode)
        {
            OutputDescription.Append(currNode.Label + " ");

            _visited[currNode] = true;
            _prev[currNode] = parentNode;
            _component[currNode] = parentNode == null ? currNode.Index : _component[parentNode];

            List<Edge> adjEdges = G.GetAdjacentEdges(currNode);

            foreach (Edge edge in adjEdges)
            {
                Node destNode = edge.DestNode;

                if (!_visited[destNode])
                    DFS(destNode, currNode);
                else if (_visited[destNode] && _prev[currNode] != null && _prev[currNode].Index != destNode.Index)
                    _numberOfCycles++;
            }
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
        
        public int GetNumberOfCycles()
        {
            return _numberOfCycles;
        }
    }
}
