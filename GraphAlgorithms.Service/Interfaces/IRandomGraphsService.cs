﻿using GraphAlgorithms.Service.DTO;
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
        Task<ActionDTO> GenerateRandomUnicyclicBipartiteGraphs(int firstPartitionSize, int secondPartitionSize, int cycleLength, int totalNumberOfRandomGraphs, int storeTopNumberOfGraphs);
        Task<ActionDTO> GenerateRandomAcyclicGraphsWithFixedDiameter(int numberOfNodes, int diameter, int totalNumberOfRandomGraphs, int storeTopNumberOfGraphs);
    }
}
