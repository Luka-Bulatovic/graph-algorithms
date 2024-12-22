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
using static GraphAlgorithms.Web.Models.SearchParamsModelBinder;

namespace GraphAlgorithms.Web.Models
{
    public class SearchModel
    {
        public string BaseUrl { get; set; }

        public int SearchByID { get; set; }
        [DisplayName("Search by")]
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
        [DisplayName("Sort by")]
        public SelectList SortBy { get; set; }

        // This is used to carry additional query params when searching using Actions that have some more params
        public Dictionary<string, object> AdditionalQueryParams { get; set; }
        public string AdditionalQueryParamsJSON => JsonSerializer.Serialize(AdditionalQueryParams);

        public SearchModel(string baseUrl)
        {
            LoadSearchParams(new List<SearchParameter>());
            LoadSortParams(new List<SortParameter>());
            SelectedSearchParams = new();
            BaseUrl = baseUrl;
            AdditionalQueryParams = new();
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

        public void SetAdditionalQueryParams(Dictionary<string, object> additionalQueryParams)
        {
            AdditionalQueryParams = additionalQueryParams ?? new();
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

        public string GetSearchParamsQueryString()
        {
            SearchQueryPayload searchQueryPayload = new SearchQueryPayload();
            searchQueryPayload.sortBy = SortByID;
            searchQueryPayload.searchParams = new();

            if(SelectedSearchParams != null && SelectedSearchParams.Count > 0)
            {
                for (int i = 0; i < SelectedSearchParams.Count; i++)
                {
                    SearchParameter param = SelectedSearchParams[i];

                    var querySearchParam = new SearchQueryParam()
                    {
                        Key = param.Key,
                        Values = new List<string>()
                    };

                    for (int j = 0; j < param.Values.Count; j++)
                        querySearchParam.Values.Add(param.Values[j]);

                    searchQueryPayload.searchParams.Add(querySearchParam);
                }
            }


            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(searchQueryPayload)));
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
                    var payload = JsonSerializer.Deserialize<SearchQueryPayload>(decodedJson);

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

            // If 'searchquery' not present or fails to parse, just provide defaults
            bindingContext.Result = ModelBindingResult.Success(new SearchParamsWrapper());
            return Task.CompletedTask;
        }

        public class SearchQueryPayload
        {
            public string sortBy { get; set; }
            public List<SearchQueryParam> searchParams { get; set; }
        }

        public class SearchQueryParam
        {
            public string Key { get; set; }
            public List<string> Values { get; set; }
        }
    }
}
