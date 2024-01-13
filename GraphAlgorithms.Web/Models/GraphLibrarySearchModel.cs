using GraphAlgorithms.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace GraphAlgorithms.Web.Models
{
    public class GraphLibrarySearchModel
    {
        public int SearchByID { get; set; }
        public SelectList SearchBy { get; set; }
        public List<SearchField> SearchFields { get; set; }
        public string SearchFieldsJSON => JsonSerializer.Serialize(SearchFields);

        public string TextValue { get; set; }
        public int NumberValue { get; set; }
        public int NumberRangeFromValue { get; set; }
        public int NumberRangeToValue { get; set; }
        public DateTime DateRangeFromValue { get; set; }
        public DateTime DateRangeToValue { get; set; }

        public List<string> SearchItems { get; set; }

        public GraphLibrarySearchModel()
        {
            SetSearchFields(new List<SearchField>());
        }

        public GraphLibrarySearchModel(List<SearchField> searchFields)
        {
            SetSearchFields(searchFields);
        }

        public void SetSearchFields(List<SearchField> searchFields)
        {
            SearchFields = searchFields;
            SearchBy = new SelectList(searchFields, "KeyValue", "DisplayValue");
        }
    }
}
