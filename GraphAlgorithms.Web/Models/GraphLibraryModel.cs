using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static GraphAlgorithms.Shared.SearchParameter;

namespace GraphAlgorithms.Web.Models
{
    public enum GraphLibraryViewType { Grid = 1, Table = 2 }

    public class GraphLibraryModel
    {
        public int ForActionID { get; set; }
        public List<GraphDTO> Graphs { get; set; }

        public PaginationModel PaginationInfo { get; set; }
        public SearchModel SearchModel { get; set; }

        public bool AllowAddingToCustomGraphSets { get; set; }

        public SaveActionGraphsToCustomSetModel CustomSetModel { get; set; }
        public GraphLibraryViewType ViewType { get; set; } = GraphLibraryViewType.Grid;

        public GraphLibraryModel(GraphLibraryViewType viewType)
        {
            ViewType = viewType;
            ForActionID = 0;
            PaginationInfo = new();
            AllowAddingToCustomGraphSets = false;

            SearchModel = new SearchModel(
                viewType == GraphLibraryViewType.Grid ? "/GraphLibrary/Index" : "/GraphLibrary/IndexTable",
                new List<SearchParameter>()
                {
                    new SearchParameter("id", "ID", SearchParamType.Number, allowMultipleValues: true),
                    new SearchParameter("order", "Order", SearchParamType.NumberRange),
                    new SearchParameter("size", "Size", SearchParamType.NumberRange)
                }
            );

            CustomSetModel = new SaveActionGraphsToCustomSetModel();
        }
    }
}
