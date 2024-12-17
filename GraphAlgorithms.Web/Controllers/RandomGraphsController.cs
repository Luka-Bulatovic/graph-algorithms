using GraphAlgorithms.Core;
using GraphAlgorithms.Core.Interfaces;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using GraphAlgorithms.Service.Services;
using GraphAlgorithms.Shared.DTO;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class RandomGraphsController : Controller
    {
        public readonly IGraphClassService graphClassService;
        public readonly IRandomGraphsService randomGraphsService;
        public readonly GraphEvaluator graphEvaluator;
        public readonly RandomGraphsGenerator randomGraphsGenerator;

        public RandomGraphsController(IGraphClassService graphClassService, IRandomGraphsService randomGraphsService, GraphEvaluator graphEvaluator, RandomGraphsGenerator randomGraphsGenerator)
        {
            this.graphClassService = graphClassService;
            this.randomGraphsService = randomGraphsService;
            this.graphEvaluator = graphEvaluator;
            this.randomGraphsGenerator = randomGraphsGenerator;
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

            // Re-initialize metadata for properties used for random generation
            var graphProperties = await randomGraphsService.GetGraphClassProperties((GraphClassEnum)model.GraphClassID);
            model.InitializeMetadataForProperties(graphProperties);

            // Prepare Property values to store for Action
            var graphPropertyValues = new List<GraphPropertyValueDTO>();
            foreach (var graphProperty in graphProperties)
            {
                graphPropertyValues.Add(new GraphPropertyValueDTO()
                {
                    GraphPropertyID = graphProperty.ID,
                    Value = model.PropertiesMetadata[(GraphPropertyEnum)graphProperty.ID].Getter().ToString()
                });
            }

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

                actionDTO = await randomGraphsService.StoreGeneratedGraphs((GraphClassEnum)model.GraphClassID, graphPropertyValues, graphs);
            }
            catch(Exception ex)
            {
                // Process generating graphs locally
                var randomGraphRequestDTO = new RandomGraphRequestDTO()
                {
                    GraphClassID = model.GraphClassID,
                    ReturnNumberOfGraphs = model.StoreTopNumberOfGraphs,
                    TotalNumberOfRandomGraphs = model.TotalNumberOfRandomGraphs,
                    Data = model.Data
                };

                IGraphFactory factory = randomGraphsGenerator.GetGraphFactoryForRandomGeneration(randomGraphRequestDTO);
                List<Graph> graphs = randomGraphsGenerator.GenerateRandomGraphsWithLargestWienerIndex(factory, randomGraphRequestDTO.TotalNumberOfRandomGraphs, randomGraphRequestDTO.ReturnNumberOfGraphs);
                actionDTO = await randomGraphsService.StoreGeneratedGraphs((GraphClassEnum)model.GraphClassID, graphPropertyValues, graphs);
            }

            return RedirectToAction("Index", "GraphLibrary", new { actionID = actionDTO.ID });
        }
    }
}
