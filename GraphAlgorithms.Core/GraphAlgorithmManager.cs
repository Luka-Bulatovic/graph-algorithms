using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Shared;
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
        public T RunAlgorithm<T>(Graph graph, Func<Graph, T> algorithmFactory, bool cacheResult = true) where T : GraphAlgorithm
        {
            Guid graphKey = graph.GUID;

            Type algorithmType = typeof(T);

            // Check if the algorithm is already cached
            if (algorithmCache.ContainsKey(graphKey) && algorithmCache[graphKey].ContainsKey(algorithmType))
            {
                return (T)algorithmCache[graphKey][algorithmType];
            }

            // Instantiate and run the algorithm
            var algorithmInstance = algorithmFactory(graph);
            algorithmInstance.Run();

            // Cache the result only if needed
            if (cacheResult)
            {
                if (!algorithmCache.ContainsKey(graphKey))
                {
                    algorithmCache[graphKey] = new Dictionary<Type, GraphAlgorithm>();
                }

                algorithmCache[graphKey][algorithmType] = algorithmInstance;
            }

            return algorithmInstance;
        }

        public T RunAlgorithm<T>(Graph graph, Func<Graph, GraphAlgorithmManager, T> algorithmFactory, bool cacheResult = true) where T : GraphAlgorithm
        {
            Guid graphKey = graph.GUID;

            Type algorithmType = typeof(T);

            // Check if the algorithm is already cached
            if (algorithmCache.ContainsKey(graphKey) && algorithmCache[graphKey].ContainsKey(algorithmType))
            {
                return (T)algorithmCache[graphKey][algorithmType];
            }

            // Instantiate and run the algorithm
            var algorithmInstance = algorithmFactory(graph, this);
            algorithmInstance.Run();

            // Cache the result only if needed
            if (cacheResult)
            {
                if (!algorithmCache.ContainsKey(graphKey))
                {
                    algorithmCache[graphKey] = new Dictionary<Type, GraphAlgorithm>();
                }

                algorithmCache[graphKey][algorithmType] = algorithmInstance;
            }

            return algorithmInstance;
        }

        // Method to run an algorithm that requires two parameters, Graph and startNode
        public T RunAlgorithm<T>(Graph graph, Node startNode, Func<Graph, Node, T> algorithmFactory, bool cacheResult = true) where T : GraphAlgorithm
        {
            Guid graphKey = GuidCombiner.GenerateCombinedGuid(graph.GUID, startNode.Index);

            Type algorithmType = typeof(T);

            // Check if the algorithm is already cached
            if (algorithmCache.ContainsKey(graphKey) && algorithmCache[graphKey].ContainsKey(algorithmType))
            {
                return (T)algorithmCache[graphKey][algorithmType];
            }

            // Instantiate and run the algorithm
            var algorithmInstance = algorithmFactory(graph, startNode);
            algorithmInstance.Run();

            // Cache the result only if needed
            if (cacheResult)
            {
                if (!algorithmCache.ContainsKey(graphKey))
                {
                    algorithmCache[graphKey] = new Dictionary<Type, GraphAlgorithm>();
                }

                algorithmCache[graphKey][algorithmType] = algorithmInstance;
            }

            return algorithmInstance;
        }
    }
}
