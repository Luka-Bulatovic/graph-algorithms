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

        public GraphLibraryController(IGraphLibraryService graphLibraryService)
        {
            this.graphLibraryService = graphLibraryService;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 9, List<SearchParameter> searchParams = null)
        {
            GraphLibraryModel model = new GraphLibraryModel();

            (List<GraphDTO> graphs, int totalCount) = await graphLibraryService.GetGraphsPaginated(pageNumber, pageSize, searchParams);

            model.Graphs = graphs;
            model.PaginationInfo.SetData(pageNumber, pageSize, totalCount);

            return View(model);
        }

        public async Task<IActionResult> Action(int actionID, int pageNumber = 1, int pageSize = 9)
        {
            GraphLibraryModel model = new GraphLibraryModel();

            (List<GraphDTO> graphs, int totalCount) = await graphLibraryService.GetGraphsForActionPaginated(actionID, pageNumber, pageSize);

            model.ForActionID = actionID;
            model.Graphs = graphs;
            model.PaginationInfo.SetData(pageNumber, pageSize, totalCount);

            return View("Index", model);
        }
    }
}
