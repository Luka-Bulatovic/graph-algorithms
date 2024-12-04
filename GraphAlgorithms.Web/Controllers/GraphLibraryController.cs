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

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 9, List<SearchParameter> searchParams = null, GraphLibraryViewType viewType = GraphLibraryViewType.Grid)
        {
            GraphLibraryModel model = new GraphLibraryModel(viewType);
            await model.InitializeSearchModel(graphClassService);

            (List<GraphDTO> graphs, int totalCount) = await graphLibraryService.GetGraphsPaginated(pageNumber, pageSize, searchParams);

            model.Graphs = graphs;
            model.PaginationInfo.SetData(pageNumber, pageSize, totalCount, searchParams);
            model.SearchModel.SetSelectedSearchParams(searchParams);

            return View("Index", model);
        }

        public async Task<IActionResult> IndexTable(int pageNumber = 1, int pageSize = 9, List<SearchParameter> searchParams = null)
        {
            return await Index(pageNumber, pageSize, searchParams, GraphLibraryViewType.Table);
        }

        public async Task<IActionResult> Action(int actionID, int pageNumber = 1, int pageSize = 9, GraphLibraryViewType viewType = GraphLibraryViewType.Grid)
        {
            GraphLibraryModel model = new GraphLibraryModel(viewType);

            (List<GraphDTO> graphs, int totalCount) = await graphLibraryService.GetGraphsForActionPaginated(actionID, pageNumber, pageSize);

            model.ForActionID = actionID;
            model.Graphs = graphs;
            model.PaginationInfo.SetData(pageNumber, pageSize, totalCount);
            model.AllowAddingToCustomGraphSets = true;

            return View("Index", model);
        }

        public async Task<IActionResult> SaveToCustomSet(SaveActionGraphsToCustomSetModel model)
        {
            return Ok();
        }
    }
}
