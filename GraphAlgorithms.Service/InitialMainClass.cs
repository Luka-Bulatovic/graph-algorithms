using GraphAlgorithms.Core;
using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Core.Factories;
using GraphAlgorithms.Service.DTO;
using System.Collections.Concurrent;

namespace GraphAlgorithms.Service
{
    public static class InitialMainClass
    {
        public static List<GraphDTO> GetBestUnicyclicBipartiteGraphs(int p, int q, int k)
        {
            List<GraphDTO> bestGraphs = new List<GraphDTO>();
            int numberOfRandomGraphs = 30000;
            int numberOfBestGraphs = 10;

            ConcurrentBag<WienerIndexAlgorithm> graphs = new ConcurrentBag<WienerIndexAlgorithm>();
            Parallel.For(0, numberOfRandomGraphs, i =>
            {
                var factory = new RandomUnicyclicBipartiteGraphFactory(p, q, k);
                Graph g = factory.CreateGraph();
                WienerIndexAlgorithm wie = new WienerIndexAlgorithm(g);
                wie.Run();
                graphs.Add(wie);
                // graphs[i].Run();
            });

            var graphsList = graphs.ToList();
            graphsList.Sort((x, y) => { return y.WienerIndexValue - x.WienerIndexValue; });

            for (int i = 0; i < numberOfBestGraphs; i++)
                bestGraphs.Add(new GraphDTO(graphsList[i].G, graphsList[i].WienerIndexValue));

            return bestGraphs;
        }

        public static int GetWienerIndexValueForGraphFromDTO(List<NodeDTO> nodes, List<EdgeDTO> edges)
        {
            Graph g = GetGraphFromNodesAndEdgesDTO(nodes, edges);
            WienerIndexAlgorithm wie = new WienerIndexAlgorithm(g);
            wie.Run();

            return wie.WienerIndexValue;
        }

        private static Graph GetGraphFromNodesAndEdgesDTO(List<NodeDTO> nodes, List<EdgeDTO> edges)
        {
            Graph g = new Graph(nodes.Count);
            g.M = edges.Count;

            for (int i = 0; i < nodes.Count; i++)
                g.AddNode(new Node(nodes[i].id, nodes[i].label));

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
    }
}