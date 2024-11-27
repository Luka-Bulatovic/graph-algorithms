﻿using GraphAlgorithms.Core;
using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Service.Interfaces
{
    public interface IRandomGraphsService
    {
        Task<ActionDTO> StoreGeneratedGraphs(List<Graph> graphs, GraphClassEnum graphClass);
        Task<List<GraphPropertyDTO>> GetGraphClassProperties(GraphClassEnum graphClass);
    }
}
