using GraphAlgorithms.Shared;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

        // Sorting
        public string SortByID { get; set; }
        [DisplayName("Sort By:")]
        public SelectList SortBy { get; set; }

        public SearchModel(string baseUrl)
        {
            LoadSearchParams(new List<SearchParameter>());
            LoadSortParams(new List<SortParameter>());
            SelectedSearchParams = new();
            BaseUrl = baseUrl;
        }

        public SearchModel(string baseUrl, List<SearchParameter> searchParams, List<SortParameter> sortParams) : this(baseUrl)
        {
            LoadSearchParams(searchParams);
            LoadSortParams(sortParams);
        }

        public void LoadSearchParams(List<SearchParameter> searchParams)
        {
            SearchParams = searchParams;
            SearchBy = new SelectList(searchParams, "Key", "DisplayName");
        }

        public void LoadSortParams(List<SortParameter> sortParams)
        {
            SortBy = new SelectList(sortParams, "Key", "Name");

            if (sortParams.Count > 0)
                SortByID = sortParams[0].Key;
        }

        public void SetSelectedSearchParams(List<SearchParameter> selectedSearchParams, string sortBy)
        {
            this.SelectedSearchParams = new();

            if(selectedSearchParams != null)
            {
                foreach (var selectedSearchParam in selectedSearchParams)
                {
                    /* These selected params will have only Key and Values.
                    We need to copy actual Param definitions from SearchParams,
                    and copy Values from selected param too. */
                    var searchParam = this.SearchParams
                        .Where(sp => sp.Key == selectedSearchParam.Key)
                        .FirstOrDefault();

                    if (searchParam != null)
                    {
                        searchParam.Values = selectedSearchParam.Values;
                        this.SelectedSearchParams.Add(searchParam);
                    }
                }
            }

            if(sortBy != "")
                SortByID = sortBy;
        }
    }

    public class SearchParamsModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Check if 'searchquery' parameter exists in the query
            var qValue = bindingContext.HttpContext.Request.Query["searchquery"].FirstOrDefault();
            if (!string.IsNullOrEmpty(qValue))
            {
                try
                {
                    // Decode from Base64
                    var decodedJson = Encoding.UTF8.GetString(Convert.FromBase64String(qValue));

                    // Deserialize the JSON into a known structure
                    var payload = JsonSerializer.Deserialize<QueryPayload>(decodedJson);

                    // Convert the payload into SearchParamsWrapper
                    var wrapper = new Shared.SearchParamsWrapper
                    {
                        SortBy = payload.sortBy ?? string.Empty,
                        SearchParams = payload.searchParams?.Select(sp => new SearchParameter
                        {
                            Key = sp.Key,
                            Values = sp.Values ?? new List<string>()
                        }).ToList() ?? new List<SearchParameter>()
                    };

                    bindingContext.Result = ModelBindingResult.Success(wrapper);
                    return Task.CompletedTask;
                }
                catch
                {
                    // If decoding fails, we can fallback to empty params
                }
            }

            // If 'q' not present or fails to parse, just provide defaults
            bindingContext.Result = ModelBindingResult.Success(new SearchParamsWrapper());
            return Task.CompletedTask;
        }

        private class QueryPayload
        {
            public string sortBy { get; set; }
            public List<QuerySearchParam> searchParams { get; set; }
        }

        private class QuerySearchParam
        {
            public string Key { get; set; }
            public List<string> Values { get; set; }
        }
    }
}
