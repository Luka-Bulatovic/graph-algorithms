using GraphAlgorithms.Core;

namespace GraphAlgorithms.Service.DTO
{
    public class GraphDTO
    {
        public int id { get; set; }
        public List<NodeDTO> nodes { get; set; }
        public List<EdgeDTO> edges { get; set; }
        public int score { get; set; }
        public string actionTypeName { get; set; }
        public string actionForGraphClassName { get; set; }
        public string classNames { get; set; }
        public DateTime createdDate { get; set; }
        public GraphProperties properties { get; set; }

        public GraphDTO()
        {
            id = 0;
            nodes = new List<NodeDTO>();
            edges = new List<EdgeDTO>();
            score = 0;
            actionTypeName = "";
            actionForGraphClassName = "";
            classNames = "";
            createdDate = DateTime.Now;
            properties = new GraphProperties();
        }
    }
}
