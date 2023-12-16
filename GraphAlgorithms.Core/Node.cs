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
        public NodeProperties NodeProperties { get; set; }

        public Node(int index, string label)
        {
            Index = index;
            Label = label;
            NodeProperties = new();

            NodeProperties.BipartitionComponent = 0;
        }

        public override string ToString()
        {
            return Label;
        }
    }
}
