using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using GraphAlgorithms.Shared;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphAlgorithms.Web.Controllers
{
    [Authorize]
    public class GraphLibraryController : Controller
    {
        public readonly IGraphLibraryService graphLibraryService;
        public readonly IGraphClassService graphClassService;
        public readonly IWebHostEnvironment webHostEnvironment;
        public readonly ICustomGraphSetsService customGraphSetsService;

        public GraphLibraryController(
            IGraphLibraryService graphLibraryService, 
            IGraphClassService graphClassService, 
            IWebHostEnvironment webHostEnvironment,
            ICustomGraphSetsService customGraphSetsService)
        {
            this.graphLibraryService = graphLibraryService;
            this.graphClassService = graphClassService;
            this.webHostEnvironment = webHostEnvironment;
            this.customGraphSetsService = customGraphSetsService;
        }

        public async Task<IActionResult> Index(
            [ModelBinder(typeof(SearchParamsModelBinder))] SearchParamsWrapper searchWrapper,
            int actionID = 0,
            int customGraphSetID = 0,
            int pageNumber = 1, 
            int pageSize = 9,
            GraphLibraryViewType viewType = GraphLibraryViewType.Grid)
        {
            Dictionary<string, object> additionalQueryParams = new Dictionary<string, object>();

            if (actionID > 0)
                additionalQueryParams.Add("actionID", actionID);
            else if(customGraphSetID > 0)
                additionalQueryParams.Add("customGraphSetID", customGraphSetID);

            GraphLibraryModel model = new GraphLibraryModel(viewType);
            
            string actionName = viewType == GraphLibraryViewType.Grid ? Url.Action("Index", "GraphLibrary") : Url.Action("IndexTable", "GraphLibrary");
            await model.InitializeSearchModel(actionName, graphClassService, searchWrapper.SearchParams, searchWrapper.SortBy, additionalQueryParams);

            (List<GraphDTO> graphs, int totalCount) = await graphLibraryService.GetGraphsPaginated(pageNumber, pageSize, actionID, customGraphSetID, model.SearchModel.SelectedSearchParams, model.SearchModel.SortByID);

            model.Graphs = graphs;
            model.AllowAddingToCustomGraphSets = true;
            model.PaginationInfo = new PaginationModel(
                pageNumber, pageSize, totalCount, actionName,
                model.SearchModel.GetSearchParamsQueryString(),
                additionalQueryParams);
            await model.LoadAdditionalData(customGraphSetsService);

            return View("Index", model);
        }

        public async Task<IActionResult> IndexTable(
            [ModelBinder(typeof(SearchParamsModelBinder))] SearchParamsWrapper searchWrapper,
            int actionID = 0,
            int customGraphSetID = 0,
            int pageNumber = 1, 
            int pageSize = 9)
        {
            return await Index(searchWrapper, actionID, customGraphSetID, pageNumber, pageSize, GraphLibraryViewType.Table);
        }

        public async Task<IActionResult> Export(GraphDrawingUpdateDTO graph)
        {
            string url = await graphLibraryService.ExportGraph(graph, webHostEnvironment.ContentRootPath);
            return Json(new { url = url });
        }
    }
}
