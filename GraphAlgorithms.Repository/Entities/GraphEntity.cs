using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Entities
{
    public class GraphEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public int Size { get; set; }
        public string DataXML { get; set; }
        public int WienerIndex { get; set; }

        public ICollection<GraphClassEntity> GraphClasses { get; set; }
        public ICollection<ActionEntity> Actions { get; set; }
    }
}
