using GraphAlgorithms.Service;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GraphAlgorithms.Web.Controllers
{
    public class GraphDrawingController : Controller
    {
        private readonly IMainService mainService;
        public GraphDrawingController(IMainService mainService)
        {
            this.mainService = mainService;
        }

        public IActionResult Index()
        {
            var model = new GraphDrawingModel(0);

            // we cannot do it like this
            model.GraphCanvasModel.Graph = mainService.GetBestUnicyclicBipartiteGraphs(7, 7, 4)[0];

            return View(model);
        }
    }
}
