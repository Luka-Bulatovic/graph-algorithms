using GraphAlgorithms.Algorithms;
using GraphAlgorithms.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms
{
    public class GraphFactory
    {
        public static Graph GetRandomConnectedGraph(int N, double minEdgesFactor = 0.2)
        {
            Graph g = new Graph(N);

            // Create list of Nodes
            for (int i = 0; i < N; i++)
                g.Nodes.Add(new Node(i, "v" + i.ToString()));

            // Generate edges until the Graph is connected and the minNumberOfEdges is reached, using Union-Find data structure
            int minNumberOfEdges = (int)((double)(N * (N - 1) / 2) * minEdgesFactor);
            int M = 0;
            Random rnd = new Random();
            UnionFind uf = new UnionFind(N);

            while(uf.GetSetsCount() > 1 || M < minNumberOfEdges)
            {
                int p = rnd.Next(N);
                int q = rnd.Next(N);

                Node nodeP = g.GetNode(p);
                Node nodeQ = g.GetNode(q);

                if (p != q && g.GetNodesAdjacency(nodeP, nodeQ) == 0)
                {
                    g.ConnectNodes(nodeP, nodeQ);

                    uf.Union(p, q);

                    M++;
                }
            }

            // set the final number of edges in graph
            g.M = M;

            return g;
        }

        public static Graph GetRandomUnicyclicBipartiteGraph(int p, int q, int cycleLength = 0)
        {
            if (p < 2 || q < 2)
                throw new InvalidDataException("Invalid sizes of bipartitions p and q");

            Random rnd = new Random();
            int maxCycleLength = Math.Min(p, q) * 2;

            // If passed value of cycleLength = 0, we generate random cycleLength value
            while(cycleLength == 0)
            {
                int curr = rnd.Next(4, maxCycleLength+1);

                if (curr % 2 == 0)
                    cycleLength = curr;
            }

            // Create graph
            int N = p + q;
            Graph g = new Graph(N);

            // Create list of Nodes
            for (int i = 0; i < N; i++)
                g.Nodes.Add(new Node(i, "v" + i.ToString()));

            // Connect and mark nodes on a cycle as members of proper BipartiteComponent, as well as assign proper label
            for (int i = 0; i < cycleLength; i++)
            {
                int firstNodeIndex = i;
                int secondNodeIndex = (i == cycleLength - 1) ? 0 : i + 1;

                Node firstNode = g.GetNode(firstNodeIndex);
                Node secondNode = g.GetNode(secondNodeIndex);

                g.ConnectNodes(firstNode, secondNode);

                g.M++;
            }

            int currNodesCntInP = 0;
            int currNodesCntInQ = 0;

            for (int i = 0; i < cycleLength; i++)
            {
                if (i % 2 == 0)
                {
                    g.Nodes[i].Label = string.Format("v0,{0}", currNodesCntInP++);
                    g.Nodes[i].BipartitionComponent = 0;
                }
                else
                {
                    g.Nodes[i].Label = string.Format("v1,{0}", currNodesCntInQ++);
                    g.Nodes[i].BipartitionComponent = 1;
                }
            }

            /*
             *  The rest of the graph has to have the following shape (should be easy to prove):
             *  The nodes on a cycle form a single connected component. We add other nodes to that component, one by one, such that
             *  we pick any node not in a component already, choose its partition and attach it to a random node from the other partition
             *  that belongs to that connected component that we are building onto.
             */

            int currNodesConnected = currNodesCntInP + currNodesCntInQ;

            while(currNodesConnected < N)
            {
                int newNodeIndex = currNodesConnected;
                int newNodeComponent = 0;

                if (currNodesCntInP < p && currNodesCntInQ < q) // Both components still have unused nodes, pick random component for the next node
                {
                    newNodeComponent = rnd.Next(2);

                    if (newNodeComponent == 0)
                        currNodesCntInP++;
                    else
                        currNodesCntInQ++;
                }
                else if (currNodesCntInP < p) // Component P has unused nodes
                    currNodesCntInP++;
                else if (currNodesCntInQ < q) // Component Q has unused nodes
                {
                    currNodesCntInQ++;
                    newNodeComponent = 1;
                }

                // Assign next node to its chosen component and assign the correct label to it
                g.Nodes[newNodeIndex].BipartitionComponent = newNodeComponent;
                g.Nodes[newNodeIndex].Label = string.Format("v{0},{1}", newNodeComponent, newNodeComponent == 0 ? currNodesCntInP - 1 : currNodesCntInQ - 1);

                bool isNewNodeConnected = false;
                
                while(!isNewNodeConnected)
                {
                    int randomNodeIndex = rnd.Next(currNodesConnected);

                    if (g.Nodes[randomNodeIndex].BipartitionComponent != g.Nodes[newNodeIndex].BipartitionComponent)
                    {
                        Node firstNode = g.GetNode(randomNodeIndex);
                        Node secondNode = g.GetNode(newNodeIndex);

                        g.ConnectNodes(firstNode, secondNode);

                        isNewNodeConnected = true;
                        g.M++;
                    }
                }

                currNodesConnected++;
            }

            Console.WriteLine("Cycle Length = " + cycleLength);

            return g;
        }

        public static Graph GetGraphFromDTONodesAndEdges(List<NodeDTO> nodes, List<EdgeDTO> edges)
        {
            Graph g = new Graph(nodes.Count);
            g.M = edges.Count;

            for(int i = 0; i < nodes.Count; i++)
                g.Nodes.Add(new Node(nodes[i].id, nodes[i].label));

            for (int i = 0; i < edges.Count; i++)
            {
                int fromIndex = edges[i].from;
                int toIndex = edges[i].to;

                Node fromNode = g.GetNode(fromIndex);
                Node toNode = g.GetNode(toIndex);

                g.ConnectNodes(fromNode, toNode);
            }

            return g;
        }

        public static Graph GetGraphFromFileWithNodeNames(string filename)
        {
            string[] lines = System.IO.File.ReadAllLines(@"TestGraphInputs/" + filename);
            int N = Int32.Parse(lines[0]);
            int M = lines.Length - 1;

            Graph g = new Graph(N, isUndirected: false);
            g.M = M;

            int currNodeIndex = 0;

            for(int i = 0; i < lines.Length; i++)
            {
                if (i == 0)
                    continue;

                string line = lines[i];

                string startNodeLabel = line.Split('\t')[0];
                string endNodeLabel = line.Split('\t')[1];

                Node? startNode = g.Nodes.Where(n => n.Label == startNodeLabel).FirstOrDefault();
                Node? endNode = g.Nodes.Where(n => n.Label == endNodeLabel).FirstOrDefault();

                int startNodeIndex = startNode != null ? startNode.Index : currNodeIndex++;
                int endNodeIndex = endNode != null ? endNode.Index : currNodeIndex++;

                if (startNode == null)
                {
                    startNode = new Node(startNodeIndex, startNodeLabel);
                    g.Nodes.Add(startNode);
                }

                if (endNode == null)
                {
                    endNode = new Node(endNodeIndex, endNodeLabel);
                    g.Nodes.Add(endNode);
                }

                g.ConnectNodes(startNode, endNode);
            }

            return g;
        }
    }
}
