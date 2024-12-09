using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Core
{
    public class NodeProperties
    {
        public string Color { get; set; }
        public int BipartitionComponent { get; set; }
        public int PlanePosX { get; set; }
        public int PlanePosY { get; set; }

        public NodeProperties()
        {
            Color = "#66ccff";
            BipartitionComponent = 0;
            PlanePosX = 0;
            PlanePosY = 0;
        }
    }
}
