using GraphAlgorithms.Core;

namespace GraphAlgorithms.Service.DTO
{
    public class GraphDTO
    {
        public int id { get; set; }
        public List<NodeDTO> nodes { get; set; }
        public List<EdgeDTO> edges { get; set; }
        public int score { get; set; }

        public GraphDTO()
        {
            id = 0;
            nodes = new List<NodeDTO>();
            edges = new List<EdgeDTO>();
            score = 0;
        }
    }
}
