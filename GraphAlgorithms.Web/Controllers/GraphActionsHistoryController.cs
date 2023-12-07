using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphAlgorithms.Web.Controllers
{
    public class GraphActionsHistoryController : Controller
    {
        public readonly IGraphActionsService graphActionsService;

        public GraphActionsHistoryController(IGraphActionsService graphActionsService)
        {
            this.graphActionsService = graphActionsService;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 12)
        {
            GraphActionsHistoryModel model = new GraphActionsHistoryModel();

            (List<ActionDTO> actions, int totalCount) = await graphActionsService.GetActionsPaginated(pageNumber, pageSize);
            model.Actions = actions;
            model.PaginationInfo.SetData(pageNumber, pageSize, totalCount);

            return View(model);
        }
    }
}
