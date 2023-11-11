using GraphAlgorithms.Core.DTO;
using GraphAlgorithms.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GraphAlgorithms.Core.Factories
{
    public class GraphFromDTONodesAndEdgesFactory : IGraphFactory
    {
        List<NodeDTO> nodes;
        List<EdgeDTO> edges;

        public GraphFromDTONodesAndEdgesFactory(List<NodeDTO> nodes, List<EdgeDTO> edges)
        {
            this.nodes = nodes;
            this.edges = edges;
        }

        public Graph CreateGraph()
        {
            Graph g = new Graph(nodes.Count);
            g.M = edges.Count;

            for (int i = 0; i < nodes.Count; i++)
                g.AddNode(new Node(nodes[i].id, nodes[i].label));

            for (int i = 0; i < edges.Count; i++)
            {
                int fromIndex = edges[i].from;
                int toIndex = edges[i].to;

                Node fromNode = g.GetNode(fromIndex);
                Node toNode = g.GetNode(toIndex);

                g.ConnectNodes(fromNode, toNode);
            }

            return g;
        }
    }
}
