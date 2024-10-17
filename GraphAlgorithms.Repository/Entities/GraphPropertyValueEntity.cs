using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Entities
{
    public class GraphPropertyValueEntity
    {
        public int GraphID { get; set; }
        public GraphEntity Graph { get; set; }

        public int GraphPropertyID { get; set; }
        public GraphPropertyEntity GraphProperty { get; set; }

        // This will store the value of the property for that graph
        public string PropertyValue { get; set; }
    }
}
