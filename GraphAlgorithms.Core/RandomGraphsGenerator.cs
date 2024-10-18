using GraphAlgorithms.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                graphEvaluator.CalculateWienerIndex(graph);
                graphs.Add(graph);
            }

            // Take and return top returnNumberOfGraphs Graphs with largest Wiener index value
            graphs = graphs.OrderByDescending(graph => graph.GraphProperties.WienerIndex).ToList();
            graphs = graphs.GetRange(0, returnNumberOfGraphs);

            return graphs;
        }
    }
}
