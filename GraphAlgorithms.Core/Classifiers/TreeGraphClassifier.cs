using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Core.Interfaces;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Core.Classifiers
{
    public class TreeGraphClassifier : IGraphClassifier
    {
        private readonly GraphAlgorithmManager graphAlgorithmManager;

        public TreeGraphClassifier(GraphAlgorithmManager graphAlgorithmManager) 
        {
            this.graphAlgorithmManager = graphAlgorithmManager;
        }

        public bool BelongsToClass(Graph graph)
        {
            DepthFirstSearchAlgorithm dfsAlgorithm =
                graphAlgorithmManager.RunAlgorithm(graph, graph.Nodes[0], (g, n) => new DepthFirstSearchAlgorithm(g, n));

            return dfsAlgorithm.GetNumberOfComponents() == 1
                && dfsAlgorithm.G.M == dfsAlgorithm.G.N - 1;
        }

        public GraphClassEnum GetGraphClass()
        {
            return GraphClassEnum.Tree;
        }
    }
}
