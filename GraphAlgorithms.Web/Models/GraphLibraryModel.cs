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
        public string Heading { get; set; }
        public List<GraphDTO> Graphs { get; set; }

        public PaginationModel PaginationInfo { get; set; }
        public SearchModel SearchModel { get; set; }

        public bool AllowAddingToCustomGraphSets { get; set; }

        public AddGraphsToCustomSetModel CustomSetModel { get; set; }
        public GraphLibraryViewType ViewType { get; set; } = GraphLibraryViewType.Grid;

        public GraphLibraryModel(GraphLibraryViewType viewType)
        {
            ViewType = viewType;
            PaginationInfo = new();
            AllowAddingToCustomGraphSets = false;
            Heading = "Graph Library";

            CustomSetModel = new AddGraphsToCustomSetModel();
        }

        public async Task InitializeSearchModel(string actionName, IGraphClassService graphClassService, List<SearchParameter> selectedSearchParams, string sortBy, Dictionary<string, object> additionalQueryParams = null)
        {
            var graphClassDTOs = await graphClassService.GetClassifiableGraphClasses();
            var graphClassMultiSelectItemsList = graphClassDTOs
                    .Select(gc => new MultiSelectListItem() { Key = gc.ID.ToString(), Value = gc.Name })
                    .ToList();

            SearchModel = new SearchModel(
                actionName,
                new List<SearchParameter>()
                {
                    new SearchParameter("id", "ID", SearchParamType.Number, allowMultipleValues: true),
                    new SearchParameter("order", "Order", SearchParamType.NumberRange),
                    new SearchParameter("size", "Size", SearchParamType.NumberRange),
                    new SearchParameter("class", "Class", SearchParamType.MultiSelectList,
                        multiSelectListID: "SearchByGraphClass",
                        multiSelectItems: graphClassMultiSelectItemsList
                    ),
                    new SearchParameter("mindegree", "Min. Degree", SearchParamType.NumberRange),
                    new SearchParameter("maxdegree", "Max. Degree", SearchParamType.NumberRange),
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

            // If URL has more query params that we need to supply along with search params
            SearchModel.SetAdditionalQueryParams(additionalQueryParams);
        }

        public async Task LoadAdditionalData(ICustomGraphSetsService customGraphSetService)
        {
            if (SearchModel.AdditionalQueryParams.ContainsKey("actionID"))
                this.Heading = "Action #" + SearchModel.AdditionalQueryParams["actionID"];
            else if (SearchModel.AdditionalQueryParams.ContainsKey("customGraphSetID"))
            {
                CustomGraphSetDTO customGraphSet = 
                    await customGraphSetService.GetCustomGraphSetByIdAsync((int)SearchModel.AdditionalQueryParams["customGraphSetID"]);
                this.Heading = string.Format("Custom Set #{0} ({1})", customGraphSet.ID, customGraphSet.Name);
            }
        }
    }
}