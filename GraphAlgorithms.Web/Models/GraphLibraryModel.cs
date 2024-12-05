using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using GraphAlgorithms.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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
            PaginationInfo = new(actionName: (viewType == GraphLibraryViewType.Grid ? "Index" : "IndexTable"));
            AllowAddingToCustomGraphSets = false;

            CustomSetModel = new SaveActionGraphsToCustomSetModel();
        }

        public async Task InitializeSearchModel(IGraphClassService graphClassService, List<SearchParameter> selectedSearchParams, string sortBy)
        {
            var graphClassDTOs = await graphClassService.GetClassifiableGraphClasses();
            var graphClassMultiSelectItemsList = graphClassDTOs
                    .Select(gc => new MultiSelectListItem() { Key = gc.ID.ToString(), Value = gc.Name })
                    .ToList();

            SearchModel = new SearchModel(
                ViewType == GraphLibraryViewType.Grid ? "/GraphLibrary/Index" : "/GraphLibrary/IndexTable",
                new List<SearchParameter>()
                {
                    new SearchParameter("id", "ID", SearchParamType.Number, allowMultipleValues: true),
                    new SearchParameter("order", "Order", SearchParamType.NumberRange),
                    new SearchParameter("size", "Size", SearchParamType.NumberRange),
                    new SearchParameter("class", "Class", SearchParamType.MultiSelectList,
                        multiSelectListID: "SearchByGraphClass",
                        multiSelectItems: graphClassMultiSelectItemsList
                    )
                },
                new List<SortParameter>()
                {
                    new SortParameter("id,desc", "ID, DESC"),
                    new SortParameter("id,asc", "ID, ASC"),
                    new SortParameter("wiener,desc", "Wiener Index, DESC"),
                    new SortParameter("wiener,asc", "Wiener Index, ASC"),
                }
            );

            // Set selected parameters
            SearchModel.SetSelectedSearchParams(selectedSearchParams, sortBy);
        }
    }
}