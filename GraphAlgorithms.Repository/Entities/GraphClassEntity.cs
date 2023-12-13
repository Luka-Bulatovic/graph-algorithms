using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Entities
{
    public class GraphClassEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool CanGenerateRandomGraphs { get; set; } = false;

        public ICollection<GraphEntity> Graphs { get; set; }
    }

    public enum GraphClassEnum { ConnectedGraph = 1, UnicyclicBipartiteGraph = 2, Tree = 3, UnicyclicGraph = 4 };
}
