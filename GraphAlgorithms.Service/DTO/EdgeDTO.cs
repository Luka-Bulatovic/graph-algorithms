using GraphAlgorithms.Core;

namespace GraphAlgorithms.Service.DTO
{
    public class EdgeDTO
    {
        public int from { get; set; }
        public int to { get; set; }

        public EdgeDTO()
        {
            from = 0;
            to = 0;
        }

        public EdgeDTO(Edge e)
        {
            from = e.GetSrcNodeIndex();
            to = e.GetDestNodeIndex();
        }
    }
}
