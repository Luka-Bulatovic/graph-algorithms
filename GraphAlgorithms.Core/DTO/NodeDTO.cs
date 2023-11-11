using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Core.DTO
{
    public class NodeDTO
    {
        public int id { get; set; }
        public string label { get; set; }
        public Dictionary<string, string> color { get; set; }

        public NodeDTO()
        {
            id = 0;
            label = "";
            color = new Dictionary<string, string>();
        }

        public NodeDTO(Node v)
        {
            id = v.Index;
            label = v.Label;

            color = new Dictionary<string, string>();
            color["background"] = v.BipartitionComponent == 0 ? "#66ccff" : "#ff6666";
        }
    }
}
