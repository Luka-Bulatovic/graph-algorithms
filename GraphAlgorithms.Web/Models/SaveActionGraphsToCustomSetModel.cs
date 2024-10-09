using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace GraphAlgorithms.Web.Models
{
    public class SaveActionGraphsToCustomSetModel
    {
        public int CustomGraphClassSaveType { get; set; }
        [DisplayName("Custom Set Name:")]
        public string NewCustomSetName { get; set; }
        public int ExistingCustomSetID { get; set; }
        [DisplayName("Existing Set:")]
        public SelectList ExistingCustomSet { get; set; }

        public SaveActionGraphsToCustomSetModel()
        {
        }
    }
}
