using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Interfaces
{
    public interface IRandomGraphsService
    {
        Task<ActionDTO> GenerateRandomConnectedGraphs(int numberOfNodes, double minEdgeFactor, int totalNumberOfRandomGraphs, int storeTopNumberOfGraphs);
    }
}
