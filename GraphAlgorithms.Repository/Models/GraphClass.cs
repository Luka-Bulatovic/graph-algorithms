using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Models
{
    public class GraphClass
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<Graph> Graphs { get; set; }
    }

    public enum GraphClassEnum { Tree = 1, UnicyclicGraph = 2 };
}
