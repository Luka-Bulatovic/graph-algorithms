using System;

namespace GraphAlgorithms.Web.Models
{
    public class PaginationModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalCount / PageSize);

        public PaginationModel()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public void SetData(int pageNumber, int pageSize, int totalCount)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }
}
