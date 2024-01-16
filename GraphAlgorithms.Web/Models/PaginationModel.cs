using GraphAlgorithms.Shared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

namespace GraphAlgorithms.Web.Models
{
    public class PaginationModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalCount / PageSize);
        public string ActionName { get; set; }
        public List<SearchParameter> SearchParams { get; set; }
        public string SearchParamsJSON => JsonSerializer.Serialize(SearchParams);

        public PaginationModel(string actionName = "Index")
        {
            PageNumber = 1;
            PageSize = 10;
            SearchParams = new List<SearchParameter>();
            ActionName = actionName;
        }

        public void SetData(int pageNumber, int pageSize, int totalCount, List<SearchParameter> searchParams = null)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;

            SearchParams = searchParams ?? SearchParams;
        }

        public string GetSearchParamsQueryString()
        {
            if (SearchParams == null)
                return String.Empty;

            if (SearchParams.Count == 0)
                return String.Empty;

            StringBuilder queryStringBuilder = new StringBuilder();

            for(int i = 0; i < SearchParams.Count; i++)
            {
                SearchParameter param = SearchParams[i];

                queryStringBuilder.Append($"&searchParams[{i}].DisplayName={WebUtility.UrlEncode(param.DisplayName)}");
                queryStringBuilder.Append($"&searchParams[{i}].Key={WebUtility.UrlEncode(param.Key)}");
                queryStringBuilder.Append($"&searchParams[{i}].ParamType={WebUtility.UrlEncode(((int)param.ParamType).ToString())}");
                queryStringBuilder.Append($"&searchParams[{i}].AllowMultipleValues={WebUtility.UrlEncode(param.AllowMultipleValues.ToString().ToLower())}");

                for(int j = 0; j < param.Values.Count; j++)
                    queryStringBuilder.Append($"&searchParams[{i}].Values[{j}]={WebUtility.UrlEncode(param.Values[j])}");
            }

            return queryStringBuilder.ToString();
        }
    }
}
