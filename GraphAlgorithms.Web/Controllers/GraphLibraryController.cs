using GraphAlgorithms.Service;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index()
        {
            GraphLibraryModel model = new GraphLibraryModel();

            model.Graphs = await graphLibraryService.GetGraphs();

            return View(model);
        }
    }
}
