using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.DTO
{
    public class GraphDTO
    {
        public List<NodeDTO> nodes { get; set; }
        public List<EdgeDTO> edges { get; set; }
        public int score { get; set; }

        public GraphDTO(Graph g, int score)
        {
            nodes = new List<NodeDTO>();
            edges = new List<EdgeDTO>();

            foreach (Node node in g.Nodes)
                nodes.Add(new NodeDTO(node));

            foreach (Node node in g.Nodes)
            {
                List<Edge> adjEdges = g.GetAdjacentEdges(node);

                foreach (Edge e in adjEdges)
                {
                    if (node.Index < e.GetDestNodeIndex())
                        edges.Add(new EdgeDTO(e));
                }
            }

            this.score = score;
        }
    }
}
