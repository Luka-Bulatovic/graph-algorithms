using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Entities
{
    public class GraphPropertyEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsGeneralDisplayProperty { get; set; }
        // DoAutoCalculate ?
        public ICollection<GraphClassEntity> RandomGenerationGraphClasses { get; set; }
    }
}
