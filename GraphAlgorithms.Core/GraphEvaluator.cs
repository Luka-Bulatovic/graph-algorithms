using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Core.Classifiers;
using GraphAlgorithms.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Core
{
    public class GraphEvaluator
    {
        #region Utils
        public static void CalculateWienerIndex(Graph graph)
        {
            if (graph == null)
                return;

            WienerIndexAlgorithm wienerAlg = new WienerIndexAlgorithm(graph);
            wienerAlg.Run();

            graph.GraphProperties.WienerIndex = wienerAlg.WienerIndexValue;
        }

        private static List<IGraphClassifier> GetGraphClassifiersList(Graph graph)
        {
            DepthFirstSearchAlgorithm dfsAlgorithm = new(graph, graph.Nodes[0]);
            BreadthFirstSearchAlgorithm bfsAlgorithm = new(graph, graph.Nodes[0]);

            List<IGraphClassifier> graphClassifiers = new List<IGraphClassifier>()
            {
                new ConnectedGraphClassifier(dfsAlgorithm),
                new TreeGraphClassifier(dfsAlgorithm),
                new BipartiteGraphClassifier(bfsAlgorithm),
                new UnicyclicGraphClassifier(dfsAlgorithm),
            };
            return graphClassifiers;
        }
        #endregion

        #region Main Methods
        public static void CalculateGraphProperties(Graph graph)
        {
            if (graph == null)
                return;

            CalculateWienerIndex(graph);
        }

        public static void CalculateGraphClasses(Graph graph)
        {
            if (graph == null || graph.Nodes == null || graph.Nodes.Count == 0)
                return;

            graph.GraphClasses.Clear();

            List<IGraphClassifier> graphClassifiers = GetGraphClassifiersList(graph);

            foreach (IGraphClassifier graphClassifier in graphClassifiers)
                if (graphClassifier.BelongsToClass())
                    graph.GraphClasses.Add(graphClassifier.GetGraphClass());
        }
        #endregion
    }
}
