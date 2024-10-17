using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Core
{
    public class GraphProperties
    {
        public int WienerIndex { get; set; }
        /* Other indices in future */

        public int Order { get; set; }
        public int Diameter { get; set; }
        public int FirstPartitionSize { get; set; }
        public int SecondPartitionSize { get; set; }
        public int CycleLength { get; set; }
        public int MinEdgesFactor { get; set; }

        // private Dictionary<GraphPropertyEnum, FieldMetadata> 
    }
}
