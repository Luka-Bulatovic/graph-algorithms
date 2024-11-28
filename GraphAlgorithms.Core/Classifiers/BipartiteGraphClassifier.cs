using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Core.Classifiers
{
    public class BipartiteGraphClassifier : IGraphClassifier
    {
        private readonly GraphAlgorithmManager graphAlgorithmManager;

        public BipartiteGraphClassifier(GraphAlgorithmManager graphAlgorithmManager)
        {
            this.graphAlgorithmManager = graphAlgorithmManager;
        }

        public bool BelongsToClass(Graph graph)
        {
            BreadthFirstSearchAlgorithm bfsAlgorithm =
                graphAlgorithmManager.RunAlgorithm(graph, graph.Nodes[0], (g, n) => new BreadthFirstSearchAlgorithm(g, n));

            return bfsAlgorithm.IsBipartiteGraph();
        }

        public (int, int) GetNodeCountInPartitions(Graph graph)
        {
            if(!BelongsToClass(graph))
                return (0, 0);
            
            BreadthFirstSearchAlgorithm bfsAlgorithm =
                graphAlgorithmManager.RunAlgorithm(graph, graph.Nodes[0], (g, n) => new BreadthFirstSearchAlgorithm(g, n));

            var bipartiteColoring = bfsAlgorithm.BipartiteColoring;
            if (bipartiteColoring == null)
                return (0, 0);

            List<GraphAlgorithm.BipartiteColors> bipartiteColorsList = bipartiteColoring.ToList();
            
            int firstPartition = bipartiteColorsList
                                    .Where(c => c == GraphAlgorithm.BipartiteColors.First)
                                    .Count();
            int secondPartition = bipartiteColorsList
                                    .Where(c => c == GraphAlgorithm.BipartiteColors.Second)
                                    .Count();

            return (firstPartition, secondPartition);
        }

        public GraphClassEnum GetGraphClass()
        {
            return GraphClassEnum.Bipartite;
        }
    }
}
