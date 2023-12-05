using GraphAlgorithms.Service;
using GraphAlgorithms.Service.DTO;
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

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 9)
        {
            GraphLibraryModel model = new GraphLibraryModel();

            (List<GraphDTO> graphs, int totalCount) = await graphLibraryService.GetGraphsPaginated(pageNumber, pageSize);

            model.Graphs = graphs;
            model.PaginationInfo.SetData(pageNumber, pageSize, totalCount);

            return View(model);
        }
    }
}
