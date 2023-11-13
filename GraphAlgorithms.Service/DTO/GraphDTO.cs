using GraphAlgorithms.Core;

namespace GraphAlgorithms.Service.DTO
{
    public class GraphDTO
    {
        public List<NodeDTO> nodes { get; set; }
        public List<EdgeDTO> edges { get; set; }
        public int score { get; set; }

        public GraphDTO()
        {
            nodes = new List<NodeDTO>();
            edges = new List<EdgeDTO>();
            score = 0;
        }

        public GraphDTO(Graph g, int score) : this()
        {
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
