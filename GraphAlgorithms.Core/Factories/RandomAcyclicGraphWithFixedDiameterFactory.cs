using GraphAlgorithms.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Core.Factories
{
    public class RandomAcyclicGraphWithFixedDiameterFactory : IGraphFactory
    {
        int n;
        int d;
        Random rnd;


        /// <summary>
        /// Class representing Subtree Node with distance from diameter path P_{d+1}
        /// </summary>
        class SubtreeNode
        {
            public Node Node { get; }
            public int DistanceFromDiameterPath { get; }

            public SubtreeNode(Node node, int distanceFromDiameterPath)
            {
                Node = node;
                DistanceFromDiameterPath = distanceFromDiameterPath;
            }
        }

        public RandomAcyclicGraphWithFixedDiameterFactory(int n, int d)
        {
            this.n = n;
            this.d = d;
            this.rnd = new Random();
        }

        public Graph CreateGraph()
        {
            if (d <= 2 || n < d+1)
                throw new InvalidDataException(string.Format("Invalid values n and d: n={0}, d={1}", n, d));

            // Graph
            Graph g = new Graph(0, n);

            // Add Nodes
            for (int i = 0; i < n; i++)
            {
                g.AddNode(new Node(i, "v" + i.ToString()));
                g.Nodes[i].NodeProperties.Color = i <= d ? BipartiteGraphColors.FirstColor : BipartiteGraphColors.SecondColor;
            }

            // Connect nodes from diameter path P_{d+1}
            for (int i = 0; i < d; i++)
                g.ConnectNodes(g.Nodes[i], g.Nodes[i + 1]);

            // Allocate buckets for each subtree
            List<SubtreeNode>[] subtrees = new List<SubtreeNode>[d];
            for(int i = 0; i < d; i++)
            {
                subtrees[i] = new List<SubtreeNode>
                {
                    new SubtreeNode(g.Nodes[i], 0)
                };
            }

            // Connect each of the remaining nodes to some bucket
            int subtreeIdx = 0;
            int subtreeSize = 0;
            int subtreeNodeIdx = 0;
            for (int k = d+1; k < n; k++)
            {
                bool isConnected = false;

                // Pick a random subtree to connect this node to
                subtreeIdx = rnd.Next(1, d);
                subtreeSize = subtrees[subtreeIdx].Count;

                // Pick a random node from subtree to connect this node to
                while(!isConnected)
                {
                    subtreeNodeIdx = rnd.Next(0, subtreeSize);
                    // Check if it can be connected, based on distance from diameter
                    if (subtrees[subtreeIdx][subtreeNodeIdx].DistanceFromDiameterPath + 1 <= Math.Min(subtreeIdx, d-subtreeIdx))
                    {
                        g.ConnectNodes(g.Nodes[k], subtrees[subtreeIdx][subtreeNodeIdx].Node);
                        subtrees[subtreeIdx].Add(new SubtreeNode(g.Nodes[k], subtrees[subtreeIdx][subtreeNodeIdx].DistanceFromDiameterPath + 1));
                        isConnected = true;
                    }
                }
            }

            StoreInitialGraphProperties(g);

            return g;
        }

        private void StoreInitialGraphProperties(Graph g)
        {
            g.GraphProperties.Order = this.n;
            g.GraphProperties.Diameter = this.d;
        }

        public Shared.Shared.GraphClassEnum GetGraphClass()
        {
            return Shared.Shared.GraphClassEnum.AcyclicWithFixedDiameter;
        }
    }
}
