using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Entities
{
    public class GraphEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [ForeignKey("Action")]
        public int ActionID { get; set; }
        public int Order { get; set; }
        public int Size { get; set; }
        public string DataXML { get; set; }
        public int WienerIndex { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ICollection<GraphClassEntity> GraphClasses { get; set; }
        public ICollection<CustomGraphSetEntity> CustomGraphSets { get; set; }
        public ActionEntity Action { get; set; }

        // Navigation property for the many-to-many relationship with properties
        public ICollection<GraphPropertyValueEntity> GraphPropertyValues { get; set; }
    }
}
