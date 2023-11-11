using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Core
{
    public class AdjacencyMatrix
    {
        #region Variables
        private int[,] Matrix;
        private int N;
        #endregion

        #region Constructors
        public AdjacencyMatrix(int n)
        {
            this.N = n;
            Matrix = new int[N, N];
        }
        #endregion

        #region Methods
        public void ConnectNodes(Node a, Node b, bool isUndirectedEdge = true)
        {
            Matrix[a.Index, b.Index] = 1;

            if(isUndirectedEdge)
                Matrix[b.Index, a.Index] = 1;
        }

        public int GetNodesAdjacency(Node a, Node b)
        {
            return Matrix[a.Index, b.Index];
        }
        #endregion
    }
}
