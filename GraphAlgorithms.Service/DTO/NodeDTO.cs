using GraphAlgorithms.Core;

namespace GraphAlgorithms.Service.DTO
{
    public class NodeDTO
    {
        public int id { get; set; }
        public string label { get; set; }
        public Dictionary<string, string> color { get; set; }
        public int? x { get; set; }
        public int? y { get; set; }

        public NodeDTO()
        {
            id = 0;
            label = "";
            color = new Dictionary<string, string>();
        }

        public NodeDTO(Node v) : this()
        {
            id = v.Index;
            label = v.Label;

            color = new Dictionary<string, string>();
            color["background"] = v.NodeProperties.Color;
            
            if(v.NodeProperties.PlanePosX > 0 || v.NodeProperties.PlanePosY > 0)
            {
                x = v.NodeProperties.PlanePosX;
                y = v.NodeProperties.PlanePosY;
            }
        }
    }
}
