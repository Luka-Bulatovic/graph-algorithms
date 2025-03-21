﻿using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Core.Factories
{
    public class RandomConnectedUndirectedGraphFactory : IGraphFactory
    {
        int n;
        int minEdgesFactor;
        double minEdgesFactorPerc;
        Random rnd;

        public RandomConnectedUndirectedGraphFactory(int n, int minEdgesFactor = 20)
        {
            this.n = n;    
            this.minEdgesFactor = minEdgesFactor;
            this.minEdgesFactorPerc = (double)minEdgesFactor / 100;
            this.rnd = new Random();
        }

        public Graph CreateGraph()
        {
            Graph g = new Graph(0, n);
            UnionFind uf = new UnionFind(n);

            // Create list of Nodes
            for (int i = 0; i < n; i++)
                g.AddNode(new Node(i, "v" + i.ToString()));

            // Generate edges until the Graph is connected and the minNumberOfEdges is reached, using Union-Find data structure
            int minNumberOfEdges = (int)((double)(n * (n - 1) / 2) * minEdgesFactorPerc);
            int m = 0;
            
            while (uf.GetSetsCount() > 1 || m < minNumberOfEdges)
            {
                bool isEdgeCreated = GenerateNextRandomEdgeAndPerformUnion(g, uf);

                m += isEdgeCreated ? 1 : 0;
            }

            StoreInitialGraphProperties(g);

            return g;
        }

        public GraphClassEnum GetGraphClass()
        {
            return GraphClassEnum.Connected;
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

        private void StoreInitialGraphProperties(Graph g)
        {
            g.GraphProperties.Order = this.n;
            g.GraphProperties.MinEdgesFactor = this.minEdgesFactor;
        }
    }
}
