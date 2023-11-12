using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Models
{
    public class Action
    {
        public int ID { get; set; }
        public int ActionTypeID { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ActionType ActionType { get; set; }
        public ICollection<Graph> Graphs { get; set; }
    }
}
