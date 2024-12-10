using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Shared
{
    public class SearchParamsWrapper
    {
        public List<SearchParameter> SearchParams { get; set; } = new List<SearchParameter>();
        public string SortBy { get; set; } = string.Empty;
    }
}
