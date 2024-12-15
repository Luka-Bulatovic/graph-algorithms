using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using GraphAlgorithms.Shared;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphAlgorithms.Web.Controllers
{
    public class GraphLibraryController : Controller
    {
        public readonly IGraphLibraryService graphLibraryService;
        public readonly IGraphClassService graphClassService;

        public GraphLibraryController(IGraphLibraryService graphLibraryService, IGraphClassService graphClassService)
        {
            this.graphLibraryService = graphLibraryService;
            this.graphClassService = graphClassService;
        }

        public async Task<IActionResult> Index(
            [ModelBinder(typeof(SearchParamsModelBinder))] SearchParamsWrapper searchWrapper,
            int actionID = 0,
            int pageNumber = 1, 
            int pageSize = 9,
            GraphLibraryViewType viewType = GraphLibraryViewType.Grid)
        {
            Dictionary<string, object> additionalQueryParams = new Dictionary<string, object>();

            if (actionID > 0)
                additionalQueryParams.Add("actionID", actionID);

            GraphLibraryModel model = new GraphLibraryModel(viewType);
            
            string actionName = viewType == GraphLibraryViewType.Grid ? "Index" : "IndexTable";
            await model.InitializeSearchModel(actionName, graphClassService, searchWrapper.SearchParams, searchWrapper.SortBy, additionalQueryParams);

            (List<GraphDTO> graphs, int totalCount) = await graphLibraryService.GetGraphsPaginated(pageNumber, pageSize, actionID, model.SearchModel.SelectedSearchParams, model.SearchModel.SortByID);

            model.Graphs = graphs;
            if (actionID > 0)
            {
                model.ForActionID = actionID;
                model.AllowAddingToCustomGraphSets = true;
            }
            model.PaginationInfo = new PaginationModel(
                pageNumber, pageSize, totalCount, actionName,
                model.SearchModel.GetSearchParamsQueryString(),
                additionalQueryParams);

            return View("Index", model);
        }

        public async Task<IActionResult> IndexTable(
            [ModelBinder(typeof(SearchParamsModelBinder))] SearchParamsWrapper searchWrapper,
            int actionID = 0,
            int pageNumber = 1, 
            int pageSize = 9)
        {
            return await Index(searchWrapper, actionID, pageNumber, pageSize, GraphLibraryViewType.Table);
        }


        public async Task<IActionResult> SaveToCustomSet(SaveActionGraphsToCustomSetModel model)
        {
            return Ok();
        }
    }
}
