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
        public readonly GraphEvaluator graphEvaluator;

        public RandomGraphsController(IGraphClassService graphClassService, IRandomGraphsService randomGraphsService, GraphEvaluator graphEvaluator)
        {
            this.graphClassService = graphClassService;
            this.randomGraphsService = randomGraphsService;
            this.graphEvaluator = graphEvaluator;
        }


        public async Task<IActionResult> Index(int? id)
        {
            List<GraphClassDTO> graphClasses = 
                await graphClassService.GetGraphClassesForGeneratingRandomGraphs();

            List<GraphPropertyDTO> graphProperties = new();
            if(id.HasValue)
                graphProperties = await randomGraphsService.GetGraphClassProperties((GraphClassEnum)id.Value);

            RandomGraphsModel model = new RandomGraphsModel()
            {
                GraphClassID = id.HasValue ? id.Value : 0,
                GraphClassList = new SelectList(graphClasses, "ID", "Name"),
            };

            model.InitializeMetadataForProperties(graphProperties);

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
                        Data = model.Data
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
                        Graph graph = graphEvaluator.GetGraphFromGraphML(0, graphML);
                        graphEvaluator.CalculateWienerIndex(graph);
                        graphs.Add(graph);
                    }
                }

                graphs = graphs.OrderByDescending(graph => graph.GraphProperties.WienerIndex).ToList();
                graphs = graphs.GetRange(0, model.StoreTopNumberOfGraphs);

                actionDTO = await randomGraphsService.StoreGeneratedGraphs(graphs, (GraphClassEnum)model.GraphClassID);
            }
            catch(Exception ex)
            {
                // Process generating graphs locally
                switch (model.GraphClassID)
                {
                    case (int)GraphClassEnum.ConnectedGraph:
                        actionDTO = await randomGraphsService.GenerateRandomConnectedGraphs(model.Data.Nodes, model.Data.MinEdgesFactor, model.TotalNumberOfRandomGraphs, model.StoreTopNumberOfGraphs);
                        break;
                    case (int)GraphClassEnum.UnicyclicBipartiteGraph:
                        actionDTO = await randomGraphsService.GenerateRandomUnicyclicBipartiteGraphs(model.Data.FirstPartitionSize, model.Data.SecondPartitionSize, model.Data.CycleLength, model.TotalNumberOfRandomGraphs, model.StoreTopNumberOfGraphs);
                        break;
                    case (int)GraphClassEnum.AcyclicGraphWithFixedDiameter:
                        actionDTO = await randomGraphsService.GenerateRandomAcyclicGraphsWithFixedDiameter(model.Data.Nodes, model.Data.Diameter, model.TotalNumberOfRandomGraphs, model.StoreTopNumberOfGraphs);
                        break;
                    default:
                        break;
                }
            }

            return RedirectToAction("Action", "GraphLibrary", new { actionID = actionDTO.ID });
        }
    }
}
