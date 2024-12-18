using GraphAlgorithms.Service.DTO;

namespace GraphAlgorithms.Web.Models
{
    public class CustomGraphSetCardModel
    {
        private CustomGraphSetDTO customGraphSet;
        public CustomGraphSetDTO CustomGraphSet => customGraphSet;

        public CustomGraphSetCardModel(CustomGraphSetDTO customGraphSet)
        {
            this.customGraphSet = customGraphSet;
        }
    }
}
