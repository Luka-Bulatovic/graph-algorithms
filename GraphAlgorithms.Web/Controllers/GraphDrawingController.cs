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
            var model = new GraphDrawingModel(0);

            // Get best of Unicyclic graphs
            //model.GraphCanvasModel.Graph = mainService.GetBestUnicyclicBipartiteGraphs(7, 7, 4)[0];
            
            // This is how we load graph by ID
            // We have to handle storing colors, indices, bipartite components etc.
            model.GraphCanvasModel.Graph = await mainService.GetGraphDTOByIDAsync(1);

            return View(model);
        }

        public async Task Store(GraphDTO graphDTO)
        {
            await mainService.StoreGraph(graphDTO);
        }
    }
}
