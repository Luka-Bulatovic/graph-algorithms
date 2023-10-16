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

            for (int i = 0; i < g.Nodes.Count; i++)
                nodes.Add(new NodeDTO(g.Nodes[i]));

            for (int i = 0; i < g.N; i++)
            {
                Node currNode = g.GetNode(i);
                List<Edge> adjEdges = g.GetAdjacentEdges(currNode);

                foreach (Edge e in adjEdges)
                {
                    if (i < e.GetDestNodeIndex())
                        edges.Add(new EdgeDTO(e));
                }
            }

            this.score = score;
        }
    }
}
