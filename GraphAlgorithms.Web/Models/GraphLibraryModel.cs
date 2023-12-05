using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;

namespace GraphAlgorithms.Web.Models
{
    public class GraphLibraryModel
    {
        public List<GraphDTO> Graphs { get; set; }

        public PaginationInfoModel PaginationInfo { get; set; }

        public GraphLibraryModel()
        {
            PaginationInfo = new();
        }
    }
}
