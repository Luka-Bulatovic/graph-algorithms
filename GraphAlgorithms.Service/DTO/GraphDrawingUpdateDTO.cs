using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.DTO
{
    public class GraphDrawingUpdateDTO
    {
        public int id { get; set; }
        public List<NodeDTO> nodes { get; set; }
        public List<EdgeDTO> edges { get; set; }

        public GraphDrawingUpdateDTO()
        {
            id = 0;
            nodes = new List<NodeDTO>();
            edges = new List<EdgeDTO>();
        }
    }
}
