using Microsoft.AspNetCore.Mvc;

namespace GraphAlgorithms.Web.Controllers
{
    public class RandomGraphsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
