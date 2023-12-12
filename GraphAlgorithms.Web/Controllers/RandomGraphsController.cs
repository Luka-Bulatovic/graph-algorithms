using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphAlgorithms.Web.Controllers
{
    public class RandomGraphsController : Controller
    {
        public readonly IGraphClassService graphClassService;

        public RandomGraphsController(IGraphClassService graphClassService)
        {
            this.graphClassService = graphClassService;
        }


        public async Task<IActionResult> Index()
        {
            RandomGraphsModel model = new();

            List<GraphClassDTO> graphClasses = await graphClassService.GetGraphClasses();
            model.GraphClassList = new SelectList(graphClasses, "ID", "Name");

            return View(model);
        }

        public IActionResult Save(RandomGraphsModel model)
        {
            int ret = 0;

            return Json(new { ret = ret });
        }
    }
}
