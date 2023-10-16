using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms
{
    public class UnionFind
    {
        private List<int> Parent;  // parent[i] = parent of i
        private List<int> Rank;   // rank[i] = rank of subtree rooted at i (never more than 31)
        private int Count;     // number of components

        public UnionFind(int n)
        {
            Count = n;
            Parent = new List<int>(Count);
            Rank = new List<int>(Count);

            for (int i = 0; i < Count; i++)
            {
                Parent.Add(i);
                Rank.Add(0);
            }
        }

        public int Find(int p)
        {
            if(p != Parent[p])
                Parent[p] = Find(Parent[p]); // Path compression

            return Parent[p];
        }

        public void Union(int p, int q)
        {
            int rootP = Find(p);
            int rootQ = Find(q);

            if (rootP == rootQ)
                return;

            // make root of smaller rank point to root of larger rank
            if (Rank[rootP] < Rank[rootQ])
                Parent[rootP] = rootQ;
            else if (Rank[rootQ] < Rank[rootP])
                Parent[rootQ] = rootP;
            else
            {
                Parent[rootQ] = rootP;
                Rank[rootP]++;
            }

            Count--;
        }

        public bool AreConnected(int p, int q)
        {
            return Find(p) == Find(q);
        }

        public int GetSetsCount()
        {
            return Count;
        }
    }
}
