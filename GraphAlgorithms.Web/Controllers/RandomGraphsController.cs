using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Web.Controllers
{
    public class RandomGraphsController : Controller
    {
        public readonly IGraphClassService graphClassService;
        public readonly IRandomGraphsService randomGraphsService;

        public RandomGraphsController(IGraphClassService graphClassService, IRandomGraphsService randomGraphsService)
        {
            this.graphClassService = graphClassService;
            this.randomGraphsService = randomGraphsService;
        }


        public async Task<IActionResult> Index()
        {
            RandomGraphsModel model = new();

            List<GraphClassDTO> graphClasses = await graphClassService.GetGraphClassesForGeneratingRandomGraphs();
            model.GraphClassList = new SelectList(graphClasses, "ID", "Name");

            return View(model);
        }

        public async Task<IActionResult> Save(RandomGraphsModel model)
        {
            ActionDTO actionDTO = null;

            switch(model.GraphClassID)
            {
                case (int)GraphClassEnum.ConnectedGraph:
                    actionDTO = await randomGraphsService.GenerateRandomConnectedGraphs(model.RandomConnectedGraphModel.Nodes, (double)model.RandomConnectedGraphModel.MinEdgesFactor / 100, model.TotalNumberOfRandomGraphs, model.StoreTopNumberOfGraphs);
                    break;
                case (int)GraphClassEnum.UnicyclicBipartiteGraph:
                    actionDTO = await randomGraphsService.GenerateRandomUnicyclicBipartiteGraphs(model.RandomUnicyclicBipartiteGraphModel.FirstPartitionSize, model.RandomUnicyclicBipartiteGraphModel.SecondPartitionSize, model.RandomUnicyclicBipartiteGraphModel.CycleLength, model.TotalNumberOfRandomGraphs, model.StoreTopNumberOfGraphs);
                    break;
                default:
                    break;
            }

            return RedirectToAction("Action", "GraphLibrary", new { actionID = actionDTO.ID });
        }
    }
}
