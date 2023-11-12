using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Models
{
    public class ActionType
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public enum ActionTypeEnum { Draw = 1, Import = 2, GenerateRandom = 3 }
}
