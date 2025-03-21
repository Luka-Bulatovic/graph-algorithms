﻿using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Interfaces
{
    public interface IGraphClassService
    {
        Task<List<GraphClassDTO>> GetGraphClasses();
        Task<List<GraphClassDTO>> GetClassifiableGraphClasses();
        Task<List<GraphClassDTO>> GetGraphClassesForGeneratingRandomGraphs();
    }
}
