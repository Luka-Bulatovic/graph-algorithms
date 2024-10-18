using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Core.Interfaces
{
    public interface IGraphClassifier
    {
        public bool BelongsToClass(Graph graph);
        public GraphClassEnum GetGraphClass();
    }
}
