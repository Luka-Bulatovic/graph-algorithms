using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Entities
{
    public class ActionEntity
    {
        public int ID { get; set; }
        public int ActionTypeID { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ActionTypeEntity ActionType { get; set; }
        public ICollection<GraphEntity> Graphs { get; set; }
    }
}
