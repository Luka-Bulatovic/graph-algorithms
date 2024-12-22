using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GraphAlgorithms.Web.Controllers
{
    [Authorize]
    public class GraphDrawingController : Controller
    {
        private readonly IGraphDrawingService graphDrawingService;
        private readonly ICustomGraphSetsService customGraphSetsService;
        public GraphDrawingController(IGraphDrawingService mainService, ICustomGraphSetsService customGraphSetsService)
        {
            this.graphDrawingService = mainService;
            this.customGraphSetsService = customGraphSetsService;
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
            await model.CustomSetModel.Load(customGraphSetsService);

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
