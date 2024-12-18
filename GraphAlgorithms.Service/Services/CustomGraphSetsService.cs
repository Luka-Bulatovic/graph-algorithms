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

        public CustomGraphSetsService(ICustomGraphSetRepository customGraphSetRepository, ICustomGraphSetConverter customGraphSetConverter)
        {
            this.customGraphSetRepository = customGraphSetRepository;
            this.customGraphSetConverter = customGraphSetConverter;
        }

        public async Task<(List<CustomGraphSetDTO>, int)> GetCustomGraphSetsPaginatedAsync(int pageNumber, int pageSize)
        {
            (List<CustomGraphSetEntity> customGraphSetEntities, int totalCount) = await customGraphSetRepository.GetCustomGraphSetsPaginatedAsync(pageNumber, pageSize);
            List<CustomGraphSetDTO> customGraphSetDTOs = customGraphSetEntities
                                        .Select(customGraphSetEntity => customGraphSetConverter.GetCustomGraphSetDTOFromEntity(customGraphSetEntity))
                                        .ToList();

            return (customGraphSetDTOs, totalCount);
        }
    }
}
