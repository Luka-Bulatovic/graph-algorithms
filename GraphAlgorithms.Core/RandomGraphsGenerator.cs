using GraphAlgorithms.Core.Factories;
using GraphAlgorithms.Core.Interfaces;
using GraphAlgorithms.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Core
{
    public class RandomGraphsGenerator
    {
        private readonly GraphEvaluator graphEvaluator;
        public RandomGraphsGenerator(GraphEvaluator graphEvaluator)
        {
            this.graphEvaluator = graphEvaluator;
        }

        public List<Graph> GenerateRandomGraphsWithLargestWienerIndex(IGraphFactory factory, int totalNumberOfRandomGraphs, int returnNumberOfGraphs)
        {
            List<Graph> graphs = new();

            // Generate totalNumberOfGraphs Random Graphs
            for (int i = 0; i < totalNumberOfRandomGraphs; i++)
            {
                Graph graph = factory.CreateGraph();
                graphEvaluator.CalculateWienerIndex(graph, cacheResult: false);
                graphs.Add(graph);
            }

            // Take and return top returnNumberOfGraphs Graphs with largest Wiener index value
            graphs = graphs.OrderByDescending(graph => graph.GraphProperties.WienerIndex).ToList();
            graphs = graphs.GetRange(0, returnNumberOfGraphs);

            return graphs;
        }

        public IGraphFactory GetGraphFactoryForRandomGeneration(RandomGraphRequestDTO randomGraphRequestDTO)
        {
            IGraphFactory factory;

            switch ((GraphClassEnum)randomGraphRequestDTO.GraphClassID)
            {
                case GraphClassEnum.ConnectedGraph:
                    factory = new RandomConnectedUndirectedGraphFactory(randomGraphRequestDTO.Data.Nodes, randomGraphRequestDTO.Data.MinEdgesFactor);
                    break;
                case GraphClassEnum.UnicyclicBipartiteGraph:
                    factory = new RandomUnicyclicBipartiteGraphFactory(randomGraphRequestDTO.Data.FirstPartitionSize, randomGraphRequestDTO.Data.SecondPartitionSize, randomGraphRequestDTO.Data.CycleLength);
                    break;
                case GraphClassEnum.AcyclicGraphWithFixedDiameter:
                    factory = new RandomAcyclicGraphWithFixedDiameterFactory(randomGraphRequestDTO.Data.Nodes, randomGraphRequestDTO.Data.Diameter);
                    break;
                default:
                    throw new InvalidOperationException("Invalid Graph Class detected.");
            }

            return factory;
        }
    }
}
