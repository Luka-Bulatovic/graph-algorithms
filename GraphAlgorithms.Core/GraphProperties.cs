using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GraphAlgorithms.Shared.Shared;

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

        private Dictionary<GraphPropertyEnum, PropertyMetadata> propertyMappings;
        public Dictionary<GraphPropertyEnum, PropertyMetadata> PropertyMappings
        {
            get
            {
                if (propertyMappings == null)
                    InitializePropertyMappings();

                return propertyMappings;
            }
        }

        // Insert all mappings here
        private void InitializePropertyMappings()
        {
            propertyMappings = new Dictionary<GraphPropertyEnum, PropertyMetadata>()
            {
                { GraphPropertyEnum.Order, new PropertyMetadata(() => this.Order, typeof(int)) },
                { GraphPropertyEnum.Diameter, new PropertyMetadata(() => this.Diameter, typeof(int)) },
                { GraphPropertyEnum.FirstBipartitionSize, new PropertyMetadata(() => this.FirstPartitionSize, typeof(int)) },
                { GraphPropertyEnum.SecondBipartitionSize, new PropertyMetadata(() => this.SecondPartitionSize, typeof(int)) },
                { GraphPropertyEnum.CycleLength, new PropertyMetadata(() => this.CycleLength, typeof(int)) },
                { GraphPropertyEnum.MinSizeCoef, new PropertyMetadata(() => this.MinEdgesFactor, typeof(int)) },
            };
        }
    }
}
