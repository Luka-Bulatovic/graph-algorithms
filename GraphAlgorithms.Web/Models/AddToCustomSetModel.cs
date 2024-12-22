using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GraphAlgorithms.Web.Models
{
    public class AddToCustomSetModel
    {
        public int CustomGraphClassSaveType { get; set; }
        [DisplayName("Custom Set Name:")]
        public string NewCustomSetName { get; set; }
        public int ExistingCustomSetID { get; set; }
        [DisplayName("Existing Set:")]
        public SelectList ExistingCustomSet { get; set; }
        public string SelectedGraphIDs { get; set; }

        public AddToCustomSetModel()
        {
        }

        public async Task Load(ICustomGraphSetsService customGraphSetsService)
        {
            List<CustomGraphSetDTO> customGraphSets = await customGraphSetsService.GetAllCustomSetsAsync();
            ExistingCustomSet = new SelectList(customGraphSets, "ID", "Name");
        }
    }
}
