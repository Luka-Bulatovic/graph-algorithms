using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Shared.DTO
{
    public class RandomGraphRequestDTO
    {
        public int GraphClassID { get; set; }
        public int TotalNumberOfRandomGraphs { get; set; }
        public int ReturnNumberOfGraphs { get; set; }
        public RandomGraphDataDTO Data { get; set; }
    }
}
