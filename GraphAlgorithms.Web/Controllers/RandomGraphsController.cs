using GraphAlgorithms.Core;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using GraphAlgorithms.Service.Services;
using GraphAlgorithms.Shared.DTO;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            /*
             * JSON structure:
            {
                GraphClassID: model.GraphClassID,
                TotalNumberOfRandomGraphs: ?,
                ReturnNumberOfGraphs: model.StoreTopNumberOfGraphs,
                Data: {...} // specific data for class
            }
            */


            try
            {
                var tasks = new List<Task<string>>();
                using var mqService = new RabbitMQService("localhost");

                int numberOfGraphsPerWorker = 20000;
                int numberOfMessages = (int)Math.Ceiling((decimal)model.TotalNumberOfRandomGraphs / numberOfGraphsPerWorker);
                for (int i = 0; i < numberOfMessages; i++)
                {
                    string message = JsonSerializer.Serialize(new RandomGraphRequestDTO()
                    {
                        GraphClassID = model.GraphClassID,
                        ReturnNumberOfGraphs = model.StoreTopNumberOfGraphs,
                        TotalNumberOfRandomGraphs = 
                            (i == numberOfMessages - 1 && model.TotalNumberOfRandomGraphs % numberOfGraphsPerWorker > 0) ?
                                model.TotalNumberOfRandomGraphs % numberOfGraphsPerWorker :
                                numberOfGraphsPerWorker,
                        Data = model.GetParamsModel().GetDataDTO()
                    });
                    
                    tasks.Add(mqService.CallAsync(message));
                }

                // Wait for all responses and process them
                string[] responses = await Task.WhenAll(tasks);
                List<Graph> graphs = new();

                foreach(string response in responses)
                {
                    List<string> currGraphsML = JsonSerializer.Deserialize<List<string>>(response);
                    foreach(var graphML in currGraphsML)
                    {
                        Graph graph = GraphEvaluator.GetGraphFromGraphML(0, graphML);
                        GraphEvaluator.CalculateGraphProperties(graph);
                        graphs.Add(graph);
                    }
                }

                graphs = graphs.OrderByDescending(graph => graph.GraphProperties.WienerIndex).ToList();
                graphs = graphs.GetRange(0, model.StoreTopNumberOfGraphs);

                actionDTO = await randomGraphsService.StoreGeneratedGraphs(graphs, model.GetParamsModel().GetGraphClass());
            }
            catch(Exception ex)
            {
                // Process generating graphs locally
                switch (model.GraphClassID)
                {
                    case (int)GraphClassEnum.ConnectedGraph:
                        actionDTO = await randomGraphsService.GenerateRandomConnectedGraphs(model.RandomConnectedGraphModel.Nodes, (double)model.RandomConnectedGraphModel.MinEdgesFactor / 100, model.TotalNumberOfRandomGraphs, model.StoreTopNumberOfGraphs);
                        break;
                    case (int)GraphClassEnum.UnicyclicBipartiteGraph:
                        actionDTO = await randomGraphsService.GenerateRandomUnicyclicBipartiteGraphs(model.RandomUnicyclicBipartiteGraphModel.FirstPartitionSize, model.RandomUnicyclicBipartiteGraphModel.SecondPartitionSize, model.RandomUnicyclicBipartiteGraphModel.CycleLength, model.TotalNumberOfRandomGraphs, model.StoreTopNumberOfGraphs);
                        break;
                    case (int)GraphClassEnum.AcyclicGraphWithFixedDiameter:
                        actionDTO = await randomGraphsService.GenerateRandomAcyclicGraphsWithFixedDiameter(model.RandomAcyclicGraphWithFixedDiameterModel.Nodes, model.RandomAcyclicGraphWithFixedDiameterModel.Diameter, model.TotalNumberOfRandomGraphs, model.StoreTopNumberOfGraphs);
                        break;
                    default:
                        break;
                }
            }

            return RedirectToAction("Action", "GraphLibrary", new { actionID = actionDTO.ID });
        }
    }
}
