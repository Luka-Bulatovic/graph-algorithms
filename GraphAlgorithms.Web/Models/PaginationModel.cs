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

        public SearchModel SearchModel { get; set; }

        public PaginationModel(string actionName = "Index")
        {
            PageNumber = 1;
            PageSize = 10;
            ActionName = actionName;
        }

        public void SetData(int pageNumber, int pageSize, int totalCount, SearchModel searchModel = null)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;

            SearchModel = searchModel ?? new(ActionName);
        }
    }
}
