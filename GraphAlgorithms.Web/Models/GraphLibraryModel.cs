using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Shared;
using System;
using System.Collections.Generic;
using static GraphAlgorithms.Shared.SearchParameter;

namespace GraphAlgorithms.Web.Models
{
    public class GraphLibraryModel
    {
        public int ForActionID { get; set; }
        public List<GraphDTO> Graphs { get; set; }

        public PaginationModel PaginationInfo { get; set; }
        public SearchModel SearchModel { get; set; }

        public GraphLibraryModel()
        {
            ForActionID = 0;
            PaginationInfo = new();
            
            SearchModel = new SearchModel(new List<SearchParameter>()
            {
                new SearchParameter("id", "ID", SearchParamType.Number, allowMultipleValues: true),
                new SearchParameter("order", "Order", SearchParamType.NumberRange),
                new SearchParameter("size", "Size", SearchParamType.NumberRange)
            });
        }
    }
}
