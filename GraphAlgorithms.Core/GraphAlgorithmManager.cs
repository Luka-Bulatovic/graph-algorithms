using GraphAlgorithms.Core.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Core
{
    public class GraphAlgorithmManager
    {
        // Dictionary to store algorithms by graph and their factory functions
        private Dictionary<Guid, Dictionary<Type, GraphAlgorithm>> algorithmCache;

        public GraphAlgorithmManager()
        {
            algorithmCache = new Dictionary<Guid, Dictionary<Type, GraphAlgorithm>>();
        }

        // Method to run an algorithm with a factory delegate that creates the algorithm instance
        public T RunAlgorithm<T>(Graph graph, Func<Graph, T> algorithmFactory) where T : GraphAlgorithm
        {
            // Ensure a cache exists for the given graph
            if (!algorithmCache.ContainsKey(graph.GUID))
            {
                algorithmCache[graph.GUID] = new Dictionary<Type, GraphAlgorithm>();
            }

            // Get the type of the algorithm to be run
            Type algorithmType = typeof(T);

            // Check if the algorithm is already cached
            if (!algorithmCache[graph.GUID].ContainsKey(algorithmType))
            {
                // Use the factory to create the algorithm instance
                var algorithmInstance = algorithmFactory(graph);

                // Run the algorithm
                algorithmInstance.Run();

                // Cache the instance
                algorithmCache[graph.GUID][algorithmType] = algorithmInstance;
            }

            // Return the cached instance
            return algorithmCache[graph.GUID][algorithmType] as T;
        }

        // Method to run an algorithm that requires two parameters, Graph and startNode
        public T RunAlgorithm<T>(Graph graph, Node startNode, Func<Graph, Node, T> algorithmFactory) where T : GraphAlgorithm
        {
            // Ensure a cache exists for the given graph
            if (!algorithmCache.ContainsKey(graph.GUID))
            {
                algorithmCache[graph.GUID] = new Dictionary<Type, GraphAlgorithm>();
            }

            // Get the type of the algorithm to be run
            Type algorithmType = typeof(T);

            // Check if the algorithm is already cached
            if (!algorithmCache[graph.GUID].ContainsKey(algorithmType))
            {
                // Use the factory to create the algorithm instance
                var algorithmInstance = algorithmFactory(graph, startNode);

                // Run the algorithm
                algorithmInstance.Run();

                // Cache the instance
                algorithmCache[graph.GUID][algorithmType] = algorithmInstance;
            }

            // Return the cached instance
            return algorithmCache[graph.GUID][algorithmType] as T;
        }
    }
}
