using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Shared;
using System;
using System.Collections.Generic;

namespace GraphAlgorithms.Web.Models
{
    public class GraphLibraryModel
    {
        public int ForActionID { get; set; }
        public List<GraphDTO> Graphs { get; set; }

        public PaginationModel PaginationInfo { get; set; }
        public GraphLibrarySearchModel SearchModel { get; set; }

        public GraphLibraryModel()
        {
            ForActionID = 0;
            PaginationInfo = new();
            
            SearchModel = new GraphLibrarySearchModel(new List<SearchField>()
            {
                new SearchField("id", "ID", SearchFieldType.Number, allowMultipleValues: true),
                new SearchField("order", "Order", SearchFieldType.NumberRange),
                new SearchField("size", "Size", SearchFieldType.NumberRange)
            });
        }
    }
}
