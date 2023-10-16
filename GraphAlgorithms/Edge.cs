using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms
{
    public class Edge
    {
        public Node SrcNode { get; set; }
        public Node DestNode { get; set; }

        public Edge(Node srcNode, Node destNode)
        {
            SrcNode = srcNode;
            DestNode = destNode;
        }

        public int GetSrcNodeIndex()
        {
            return SrcNode.Index;
        }

        public int GetDestNodeIndex()
        {
            return DestNode.Index;
        }

        public override string ToString()
        {
            return SrcNode.ToString() + " -> " + DestNode.ToString();
        }
    }
}
