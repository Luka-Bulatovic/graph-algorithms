using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Core
{
    public class Node
    {
        public int Index { get; set; }
        public string Label { get; set; }
        public int BipartitionComponent { get; set; }

        public Node(int index, string label, int bipartitionComponent = 0)
        {
            Index = index;
            Label = label;
            BipartitionComponent = bipartitionComponent;
        }

        public override string ToString()
        {
            return Label;
        }
    }
}
