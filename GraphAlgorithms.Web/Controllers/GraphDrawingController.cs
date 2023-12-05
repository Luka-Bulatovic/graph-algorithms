using GraphAlgorithms.Service;
using GraphAlgorithms.Service.DTO;
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

            GraphDrawingModel model = new GraphDrawingModel(graph, showSaveButton: true);

            return View(model);
        }

        public async Task<ViewResult> Edit(int id)
        {
            bool showSaveButton = true; // we should handle this depending on the logic whether graph is editable

            GraphDTO graph = await graphDrawingService.GetGraphDTOByIDAsync(id);

            GraphDrawingModel model = new GraphDrawingModel(graph, showSaveAsNewButton: true, showSaveButton: showSaveButton);

            return View("Index", model);
        }

        public async Task Store(GraphDTO graph)
        {
            await graphDrawingService.StoreGraph(graph, 1);
        }
    }
}
