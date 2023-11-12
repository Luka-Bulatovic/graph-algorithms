using GraphAlgorithms.Service;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace GraphAlgorithms.Web.Controllers
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
            var bestGraphs = InitialMainClass.GetBestUnicyclicBipartiteGraphs(p, q, k);

            return Json(bestGraphs);
        }

        public IActionResult GetWienerIndexValueForGraph(List<NodeDTO> nodes, List<EdgeDTO> edges)
        {
            int ret = InitialMainClass.GetWienerIndexValueForGraphFromDTO(nodes, edges);
            return Json(new { Value = ret });
        }
    }
}
