using GraphAlgorithms.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Core.Factories
{
    public class RandomUnicyclicBipartiteGraphFactory : IGraphFactory
    {
        int n;
        int p;
        int q;
        int cycleLength;
        Random rnd;

        public RandomUnicyclicBipartiteGraphFactory(int p, int q, int cycleLength = 0)
        {
            this.p = p;
            this.q = q;
            this.n = p + q;
            this.cycleLength = cycleLength;
            this.rnd = new Random();
        }

        private int GetRandomCycleLength()
        {
            int maxCycleLength = Math.Min(p, q) * 2;
            int cycleLength = 0;

            while (cycleLength == 0)
            {
                int curr = rnd.Next(4, maxCycleLength + 1);

                if (curr % 2 == 0)
                    cycleLength = curr;
            }

            return cycleLength;
        }

        private void GenerateCycle(Graph g)
        {
            // Connect and mark nodes on a cycle as members of proper Bipartition Component, as well as assign proper label
            for (int i = 0; i < cycleLength; i++)
            {
                // Connect nodes
                int firstNodeIndex = i;
                int secondNodeIndex = (i == cycleLength - 1) ? 0 : i + 1;

                Node firstNode = g.GetNode(firstNodeIndex);
                Node secondNode = g.GetNode(secondNodeIndex);

                g.ConnectNodes(firstNode, secondNode);

                // Assign node to bipartition component
                firstNode.Label = string.Format("v{0},{1}", i % 2, i / 2);
                firstNode.NodeProperties.BipartitionComponent = i % 2;
                firstNode.NodeProperties.Color = firstNode.NodeProperties.BipartitionComponent == 0 ?
                                                        BipartiteGraphColors.FirstColor : BipartiteGraphColors.SecondColor;
            }
        }

        private int GetRandomComponentForNewNode(int currNodesCntInP, int currNodesCntInQ)
        {
            int newNodeComponent = 0;

            if (currNodesCntInP < p && currNodesCntInQ < q) // Both components still have unused nodes, pick random component for the next node
                newNodeComponent = rnd.Next(2);
            else if (currNodesCntInP < p) // Component P has unused nodes
                newNodeComponent = 0;
            else if (currNodesCntInQ < q) // Component Q has unused nodes
                newNodeComponent = 1;
            else
                throw new ArgumentException("Both Bipartition components are full.");

            return newNodeComponent;
        }

        private void ConnectNewNodeToGraph(Graph g, Node newNode, int currNodesConnected)
        {
            bool isNewNodeConnected = false;

            while (!isNewNodeConnected)
            {
                int randomNodeIndex = rnd.Next(currNodesConnected);
                Node randomNode = g.GetNode(randomNodeIndex);

                if (randomNode.NodeProperties.BipartitionComponent != newNode.NodeProperties.BipartitionComponent)
                {
                    g.ConnectNodes(randomNode, newNode);

                    isNewNodeConnected = true;
                }
            }
        }

        public Graph CreateGraph()
        {
            if (p < 2 || q < 2)
                throw new InvalidDataException("Invalid sizes of Bipartitions p and q");

            if (cycleLength % 2 != 0)
                throw new InvalidDataException("Cycle Length must be even.");

            // If passed value of cycleLength = 0, we generate random cycleLength value
            this.cycleLength = this.cycleLength == 0 ? GetRandomCycleLength() : this.cycleLength;

            // Create graph
            Graph g = new Graph(0, n);

            // Create list of Nodes
            for (int i = 0; i < n; i++)
                g.AddNode(new Node(i, "v" + i.ToString()));

            // Generate cycle
            GenerateCycle(g);

            /*
             *  The rest of the graph has to have the following shape (should be easy to prove):
             *  The nodes on a cycle form a single connected component. We add other nodes to that component, one by one, such that
             *  we pick any node not in a component already, choose its partition and attach it to a random node from the other partition
             *  that belongs to that connected component that we are building onto.
             */

            int currNodesCntInP = cycleLength / 2;
            int currNodesCntInQ = cycleLength / 2;
            int currNodesConnected = currNodesCntInP + currNodesCntInQ;

            while (currNodesConnected < n)
            {
                int newNodeIndex = currNodesConnected;
                Node newNode = g.GetNode(newNodeIndex);

                // Randomly choose component for new node
                int newNodeComponent = GetRandomComponentForNewNode(currNodesCntInP, currNodesCntInQ);
                currNodesCntInP += newNodeComponent == 0 ? 1 : 0;
                currNodesCntInQ += newNodeComponent == 1 ? 1 : 0;

                // Assign next node to its chosen component and assign the correct label to it
                newNode.NodeProperties.BipartitionComponent = newNodeComponent;
                newNode.Label = string.Format("v{0},{1}", newNodeComponent, newNodeComponent == 0 ? currNodesCntInP - 1 : currNodesCntInQ - 1);
                newNode.NodeProperties.Color = newNode.NodeProperties.BipartitionComponent == 0 ?
                                                        BipartiteGraphColors.FirstColor : BipartiteGraphColors.SecondColor;

                // Connect new node to the rest of the graph
                ConnectNewNodeToGraph(g, newNode, currNodesConnected);

                currNodesConnected++;
            }

            Console.WriteLine("Cycle Length = " + cycleLength);

            return g;
        }

        public GraphClassEnum GetGraphClass()
        {
            return GraphClassEnum.UnicyclicBipartiteGraph;
        }
    }
}
