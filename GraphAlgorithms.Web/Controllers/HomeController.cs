﻿using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace GraphAlgorithms.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGraphDrawingService mainService;

        public HomeController(ILogger<HomeController> logger, IGraphDrawingService mainService)
        {
            _logger = logger;
            this.mainService = mainService;
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

        //public IActionResult GetBestUnicyclicBipartiteGraphs(int p, int q, int k)
        //{
        //    var bestGraphs = mainService.GetBestUnicyclicBipartiteGraphs(p, q, k);

        //    return Json(bestGraphs);
        //}

        //public IActionResult GetWienerIndexValueForGraph(List<NodeDTO> nodes, List<EdgeDTO> edges)
        //{
        //    int ret = mainService.GetWienerIndexValueForGraphFromDTO(nodes, edges);
        //    return Json(new { Value = ret });
        //}
    }
}
