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

        public async Task Store(GraphDTO graph)
        {
            await graphDrawingService.StoreGraph(graph);
        }

        public int CalculateWienerIndex(GraphDTO graph)
        {
            int result = graphDrawingService.CalculateWienerIndex(graph);

            return result;
        }
    }
}
