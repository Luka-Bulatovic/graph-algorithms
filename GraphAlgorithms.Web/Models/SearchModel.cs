using GraphAlgorithms.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace GraphAlgorithms.Web.Models
{
    public class SearchModel
    {
        public string BaseUrl { get; set; }

        public int SearchByID { get; set; }
        public SelectList SearchBy { get; set; }
        public List<SearchParameter> SearchParams { get; set; }
        public string SearchParamsJSON => JsonSerializer.Serialize(SearchParams);
        public List<SearchParameter> SelectedSearchParams { get; set; }
        public string SelectedSearchParamsJSON => JsonSerializer.Serialize(SelectedSearchParams);

        public string TextValue { get; set; }
        public int NumberValue { get; set; }
        public int NumberRangeFromValue { get; set; }
        public int NumberRangeToValue { get; set; }
        public DateTime DateRangeFromValue { get; set; }
        public DateTime DateRangeToValue { get; set; }

        public List<string> SearchItems { get; set; }

        public SearchModel(string baseUrl)
        {
            LoadSearchParams(new List<SearchParameter>());
            SelectedSearchParams = new();
            BaseUrl = baseUrl;
        }

        public SearchModel(string baseUrl, List<SearchParameter> searchParams) : this(baseUrl)
        {
            LoadSearchParams(searchParams);
        }

        public void LoadSearchParams(List<SearchParameter> searchParams)
        {
            SearchParams = searchParams;
            SearchBy = new SelectList(searchParams, "Key", "DisplayName");
        }

        public void SetSelectedSearchParams(List<SearchParameter> selectedSearchParams)
        {
            SelectedSearchParams = selectedSearchParams ?? new();
        }
    }
}
