﻿using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Interfaces
{
    public interface IGraphLibraryService
    {
        public Task<List<GraphDTO>> GetGraphs();
        public Task<(List<GraphDTO>, int)> GetGraphsPaginated(int pageNumber, int pageSize, int actionID, int customGraphSetID, List<SearchParameter> searchParams, string sortBy);
        public Task<string> ExportGraph(GraphDrawingUpdateDTO graphDTO, string rootFolder);
    }
}
