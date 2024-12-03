using GraphAlgorithms.Shared;
using System.Collections;
using System.Collections.Generic;

namespace GraphAlgorithms.Web.Models
{
    public class MultiSelectListModel
    {
        public string MultiSelectID { get; set; }
        public List<MultiSelectListItem> Items { get; }
        public string SelectedValues { get; set; }

        public MultiSelectListModel(string multiSelectID, List<MultiSelectListItem> items, string selectedValues = "")
        {
            this.MultiSelectID = multiSelectID;
            this.Items = items;
            this.SelectedValues = selectedValues;
        }
    }
}
