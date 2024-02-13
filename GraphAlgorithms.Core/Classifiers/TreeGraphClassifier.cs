using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Core.Interfaces;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Core.Classifiers
{
    public class TreeGraphClassifier : IGraphClassifier
    {
        DepthFirstSearchAlgorithm _dfsAlgorithm;

        public TreeGraphClassifier(DepthFirstSearchAlgorithm dfsAlgorithm) 
        {
            this._dfsAlgorithm = dfsAlgorithm;
        }

        public bool BelongsToClass()
        {
            if (!_dfsAlgorithm.IsExecuted())
                _dfsAlgorithm.Run();

            return _dfsAlgorithm.GetNumberOfComponents() == 1
                && _dfsAlgorithm.G.M == _dfsAlgorithm.G.N - 1;
        }

        public GraphClassEnum GetGraphClass()
        {
            return GraphClassEnum.Tree;
        }
    }
}
