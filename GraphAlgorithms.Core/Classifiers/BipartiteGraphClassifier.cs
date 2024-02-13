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
        private BreadthFirstSearchAlgorithm _bfsAlgorithm;

        public BipartiteGraphClassifier(BreadthFirstSearchAlgorithm bfsAlgorithm)
        {
            this._bfsAlgorithm = bfsAlgorithm;
        }

        public bool BelongsToClass()
        {
            if (!_bfsAlgorithm.IsExecuted())
                _bfsAlgorithm.Run();

            return _bfsAlgorithm.IsBipartiteGraph();
        }

        public GraphClassEnum GetGraphClass()
        {
            return GraphClassEnum.BipartiteGraph;
        }
    }
}
