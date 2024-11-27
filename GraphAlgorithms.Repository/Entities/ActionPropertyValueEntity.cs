using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Entities
{
    public class ActionPropertyValueEntity
    {
        public int ActionID { get; set; }
        public ActionEntity Action { get; set; }

        public int GraphPropertyID { get; set; }
        public GraphPropertyEntity GraphProperty { get; set; }

        public string PropertyValue { get; set; }
    }
}
