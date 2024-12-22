using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using GraphAlgorithms.Service.Services;
using GraphAlgorithms.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphAlgorithms.Web.Controllers
{
    [Authorize]
    public class CustomGraphSetsController : Controller
    {
        public readonly ICustomGraphSetsService customGraphSetsService;

        public CustomGraphSetsController(ICustomGraphSetsService customGraphSetsService)
        {
            this.customGraphSetsService = customGraphSetsService;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 12)
        {
            (List<CustomGraphSetDTO> customGraphSets, int totalCount) = await customGraphSetsService.GetCustomGraphSetsPaginatedAsync(pageNumber, pageSize);

            CustomGraphSetsModel model = new CustomGraphSetsModel(
                customGraphSets,
                new PaginationModel(pageNumber, pageSize, totalCount)
            );

            return View(model);
        }

        public async Task<IActionResult> SaveToCustomSet(AddToCustomSetModel model)
        {
            CustomGraphSetDTO customGraphSet = null;
            if (model.CustomGraphClassSaveType == 1)
            {
                // Add to existing
                customGraphSet =
                    await customGraphSetsService.AddGraphsToExistingCustomSet(model.ExistingCustomSetID, model.SelectedGraphIDs);
            }
            else if(model.CustomGraphClassSaveType == 2)
            {
                // Save as new
                customGraphSet = 
                    await customGraphSetsService.SaveGraphsAsNewCustomSet(model.NewCustomSetName, model.SelectedGraphIDs);
            }

            return Redirect(Url.Action("Index", "GraphLibrary", new { customGraphSetID = customGraphSet.ID }));
        }
    }
}
