using GraphAlgorithms.Algorithms;
using GraphAlgorithms.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Factories
{
    public class RandomConnectedUndirectedGraphFactory : IGraphFactory
    {
        int n;
        double minEdgesFactor;
        Random rnd;

        public RandomConnectedUndirectedGraphFactory(int n, double minEdgesFactor = 0.2)
        {
            this.n = n;    
            this.minEdgesFactor = minEdgesFactor;
            this.rnd = new Random();
        }

        public Graph CreateGraph()
        {
            Graph g = new Graph(n);
            UnionFind uf = new UnionFind(n);

            // Create list of Nodes
            for (int i = 0; i < n; i++)
                g.AddNode(new Node(i, "v" + i.ToString()));

            // Generate edges until the Graph is connected and the minNumberOfEdges is reached, using Union-Find data structure
            int minNumberOfEdges = (int)((double)(n * (n - 1) / 2) * minEdgesFactor);
            int m = 0;
            
            while (uf.GetSetsCount() > 1 || m < minNumberOfEdges)
            {
                bool isEdgeCreated = GenerateNextRandomEdgeAndPerformUnion(g, uf);

                m += isEdgeCreated ? 1 : 0;
            }

            // set the final number of edges in graph
            g.M = m;

            return g;
        }

        private bool GenerateNextRandomEdgeAndPerformUnion(Graph g, UnionFind uf)
        {
            int p = rnd.Next(n);
            int q = rnd.Next(n);

            Node nodeP = g.GetNode(p);
            Node nodeQ = g.GetNode(q);

            if (p != q && !g.AreNodesAdjacent(nodeP, nodeQ))
            {
                g.ConnectNodes(nodeP, nodeQ);

                uf.Union(p, q);

                return true;
            }
            
            return false;
        }
    }
}
