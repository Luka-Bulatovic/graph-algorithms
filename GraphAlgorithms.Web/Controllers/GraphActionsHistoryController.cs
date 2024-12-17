using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphAlgorithms.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace GraphAlgorithms.Web.Controllers
{
    [Authorize]
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
            model.PaginationInfo = new PaginationModel(pageNumber, pageSize, totalCount);

            return View(model);
        }
    }
}
