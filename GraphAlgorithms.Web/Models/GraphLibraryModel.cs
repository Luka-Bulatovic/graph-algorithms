using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;

namespace GraphAlgorithms.Web.Models
{
    public class GraphLibraryModel
    {
        public int ForActionID { get; set; }
        public List<GraphDTO> Graphs { get; set; }

        public PaginationModel PaginationInfo { get; set; }

        public GraphLibraryModel()
        {
            ForActionID = 0;
            PaginationInfo = new();
        }
    }
}
