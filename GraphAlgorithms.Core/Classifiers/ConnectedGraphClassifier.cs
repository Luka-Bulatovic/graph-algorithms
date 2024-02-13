using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Core.Interfaces;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Core.Classifiers
{
    public class ConnectedGraphClassifier : IGraphClassifier
    {
        private DepthFirstSearchAlgorithm _dfsAlgorithm;

        public ConnectedGraphClassifier(DepthFirstSearchAlgorithm dfsAlgorithm)
        {
            this._dfsAlgorithm = dfsAlgorithm;
        }

        public bool BelongsToClass()
        {
            if (!_dfsAlgorithm.IsExecuted())
                _dfsAlgorithm.Run();

            return _dfsAlgorithm.GetNumberOfComponents() == 1;
        }

        public GraphClassEnum GetGraphClass()
        {
            return GraphClassEnum.ConnectedGraph;
        }
    }
}
