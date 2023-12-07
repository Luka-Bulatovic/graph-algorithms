using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.DTO
{
    public class ActionDTO
    {
        public int ID { get; set; }
        public string ActionTypeName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserName { get; set; }
    }
}
