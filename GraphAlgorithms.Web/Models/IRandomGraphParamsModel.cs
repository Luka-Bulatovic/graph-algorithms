using GraphAlgorithms.Shared.DTO;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Web.Models
{
    public interface IRandomGraphParamsModel
    {
        public RandomGraphDataDTO GetDataDTO();
        public GraphClassEnum GetGraphClass();
    }
}
