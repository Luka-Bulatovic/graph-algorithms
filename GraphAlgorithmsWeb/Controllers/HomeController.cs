﻿using GraphAlgorithms;
using GraphAlgorithms.DTO;
using GraphAlgorithmsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GraphAlgorithmsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetBestUnicyclicBipartiteGraphs(int p, int q, int k)
        {
            List<GraphDTO> bestGraphs = new List<GraphDTO>();
            int numberOfRandomGraphs = 30000;
            int numberOfBestGraphs = 10;

            ConcurrentBag<WienerIndexAlgorithm> graphs = new ConcurrentBag<WienerIndexAlgorithm>();
            Parallel.For(0, numberOfRandomGraphs, i =>
            {
                Graph g = GraphFactory.GetRandomUnicyclicBipartiteGraph(p, q, k);
                WienerIndexAlgorithm wie = new WienerIndexAlgorithm(g);
                wie.Run();
                graphs.Add(wie);
                // graphs[i].Run();
            });

            var graphsList = graphs.ToList();
            graphsList.Sort((x, y) => { return y.WienerIndexValue - x.WienerIndexValue; });

            for (int i = 0; i < numberOfBestGraphs; i++)
                bestGraphs.Add(new GraphDTO(graphsList[i].G, graphsList[i].WienerIndexValue));

            return Json(bestGraphs);
        }

        public IActionResult GetWienerIndexValueForGraph(List<NodeDTO> nodes, List<EdgeDTO> edges)
        {
            Graph g = GraphFactory.GetGraphFromDTONodesAndEdges(nodes, edges);
            WienerIndexAlgorithm wie = new WienerIndexAlgorithm(g);
            wie.Run();

            return Json(new { Value = wie.WienerIndexValue });
        }
    }
}
