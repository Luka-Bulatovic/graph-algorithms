using GraphAlgorithms.Shared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using static GraphAlgorithms.Web.Models.SearchParamsModelBinder;

namespace GraphAlgorithms.Web.Models
{
    public class PaginationModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalCount / PageSize);
        public string ActionName { get; set; }
        public string SearchQueryString { get; set; }
        public Dictionary<string, object> AdditionalQueryParams { get; set; }

        public PaginationModel(string actionName = "Index")
        {
            PageNumber = 1;
            PageSize = 10;
            ActionName = actionName;
        }

        public PaginationModel(int pageNumber, int pageSize, int totalCount, string actionName = "Index", string searchQueryString = "", Dictionary<string, object> additionalQueryParams = null)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            ActionName = actionName;
            SearchQueryString = searchQueryString;
            AdditionalQueryParams = additionalQueryParams;
        }

        public Dictionary<string, object> GetQueryStringForPage(int pageNumber)
        {
            var queryStringValues = new Dictionary<string, object>
            {
                { "pageNumber", pageNumber },
                { "pageSize", this.PageSize },
            };

            if (!string.IsNullOrEmpty(this.SearchQueryString))
                queryStringValues["searchquery"] = this.SearchQueryString;

            if(this.AdditionalQueryParams != null)
                foreach (var kvp in this.AdditionalQueryParams)
                    queryStringValues[kvp.Key] = kvp.Value;

            return queryStringValues;
        }
    }
}
