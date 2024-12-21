using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Repository.Repositories;
using GraphAlgorithms.Service.Converters;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Services
{
    public class CustomGraphSetsService : ICustomGraphSetsService
    {
        private readonly ICustomGraphSetRepository customGraphSetRepository;
        private readonly ICustomGraphSetConverter customGraphSetConverter;
        private readonly IGraphRepository graphRepository;
        private readonly IUserContext userContext;

        public CustomGraphSetsService(
            ICustomGraphSetRepository customGraphSetRepository,
            ICustomGraphSetConverter customGraphSetConverter,
            IGraphRepository graphRepository,
            IUserContext userContext
        )
        {
            this.customGraphSetRepository = customGraphSetRepository;
            this.customGraphSetConverter = customGraphSetConverter;
            this.graphRepository = graphRepository;
            this.userContext = userContext;
        }

        public async Task<(List<CustomGraphSetDTO>, int)> GetCustomGraphSetsPaginatedAsync(int pageNumber, int pageSize)
        {
            (List<CustomGraphSetEntity> customGraphSetEntities, int totalCount) = await customGraphSetRepository.GetCustomGraphSetsPaginatedAsync(pageNumber, pageSize);
            List<CustomGraphSetDTO> customGraphSetDTOs = customGraphSetEntities
                                        .Select(customGraphSetEntity => customGraphSetConverter.GetCustomGraphSetDTOFromEntity(customGraphSetEntity))
                                        .ToList();

            return (customGraphSetDTOs, totalCount);
        }

        public async Task<CustomGraphSetDTO> AddGraphsToExistingCustomSet(int CustomGraphSetID, string GraphIDs)
        {
            var customGraphSet = await customGraphSetRepository.GetByIdAsync(CustomGraphSetID);

            var newGraphs = await graphRepository.GetGraphsByIDsAsync(GraphIDs);

            customGraphSet =
                await customGraphSetRepository.AddGraphsToSetAsync(customGraphSet, newGraphs);

            return customGraphSetConverter.GetCustomGraphSetDTOFromEntity(customGraphSet);
        }

        public async Task<CustomGraphSetDTO> SaveGraphsAsNewCustomSet(string CustomGraphSetName, string GraphIDs)
        {
            var graphs = await graphRepository.GetGraphsByIDsAsync(GraphIDs);

            CustomGraphSetEntity customGraphSet = 
                await customGraphSetRepository.Create(CustomGraphSetName, userContext.GetUserID(), graphs);

            return customGraphSetConverter.GetCustomGraphSetDTOFromEntity(customGraphSet);
        }
    }
}
