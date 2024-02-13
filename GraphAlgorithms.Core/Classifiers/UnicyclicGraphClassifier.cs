using GraphAlgorithms.Core.Algorithms;
using GraphAlgorithms.Core.Interfaces;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Core.Classifiers
{
    public class UnicyclicGraphClassifier : IGraphClassifier
    {
        private DepthFirstSearchAlgorithm _dfsAlgorithm;

        public UnicyclicGraphClassifier(DepthFirstSearchAlgorithm dfsAlgorithm)
        {
            _dfsAlgorithm = dfsAlgorithm;
        }
        
        public bool BelongsToClass()
        {
            if(!_dfsAlgorithm.IsExecuted())
                _dfsAlgorithm.Run();

            return
                _dfsAlgorithm.GetNumberOfComponents() == 1
                && _dfsAlgorithm.G.N == _dfsAlgorithm.G.M
                && _dfsAlgorithm.GetNumberOfCycles() == 1;
        }

        public GraphClassEnum GetGraphClass()
        {
            return GraphClassEnum.UnicyclicGraph;
        }
    }
}
