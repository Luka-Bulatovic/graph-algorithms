using GraphAlgorithms.Service;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GraphAlgorithms.Web.Controllers
{
    public class GraphDrawingController : Controller
    {
        private readonly IMainService mainService;
        public GraphDrawingController(IMainService mainService)
        {
            this.mainService = mainService;
        }

        public async Task<ViewResult> Index()
        {
            GraphDrawingModel model = new GraphDrawingModel(0, showSaveButton: true);

            GraphDTO graph = new();
            model.SetCanvasGraph(graph);

            return View(model);
        }

        public async Task<ViewResult> Edit(int id)
        {
            bool showSaveButton = true; // we should handle this depending on the logic whether graph is editable
            GraphDrawingModel model = new GraphDrawingModel(id, showSaveAsNewButton: true, showSaveButton: showSaveButton);
            
            GraphDTO graph = await mainService.GetGraphDTOByIDAsync(id);
            model.SetCanvasGraph(graph);

            return View("Index", model);
        }

        public async Task Store(GraphDTO graph)
        {
            await mainService.StoreGraph(graph, 1);
        }
    }
}
