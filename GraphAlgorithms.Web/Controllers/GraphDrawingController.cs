using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GraphAlgorithms.Web.Controllers
{
    public class GraphDrawingController : Controller
    {
        private readonly IGraphDrawingService graphDrawingService;
        public GraphDrawingController(IGraphDrawingService mainService)
        {
            this.graphDrawingService = mainService;
        }

        public async Task<ViewResult> Index()
        {
            GraphDTO graph = new();

            GraphDrawingModel model = new GraphDrawingModel(graph);

            return View(model);
        }

        public async Task<ViewResult> Edit(int id)
        {
            GraphDTO graph = await graphDrawingService.GetGraphDTOByIDAsync(id);

            GraphDrawingModel model = new GraphDrawingModel(graph);

            return View("Index", model);
        }

        public async Task<ViewResult> View(int id)
        {
            GraphDTO graph = await graphDrawingService.GetGraphDTOByIDAsync(id);

            GraphDrawingModel model = new GraphDrawingModel(graph, isViewOnly: true);

            return View("Index", model);
        }

        public async Task<IActionResult> Store(GraphDrawingUpdateDTO graph)
        {
            var storedGraph = await graphDrawingService.StoreGraph(graph);

            string redirectUrl = Url.Action("View", "GraphDrawing", new { id = storedGraph.id });

            return Json(new { redirectUrl = redirectUrl });
        }

        public int CalculateWienerIndex(GraphDrawingUpdateDTO graph)
        {
            int result = graphDrawingService.CalculateWienerIndex(graph);

            return result;
        }
    }
}
