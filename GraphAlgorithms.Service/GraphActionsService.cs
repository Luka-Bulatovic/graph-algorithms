using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Repository.Repositories;
using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service
{
    public class GraphActionsService : IGraphActionsService
    {
        public readonly IActionRepository actionRepository;
        public readonly IActionConverter actionConverter;

        public GraphActionsService(IActionRepository actionRepository, IActionConverter actionConverter)
        {
            this.actionRepository = actionRepository;
            this.actionConverter = actionConverter;
        }

        public async Task<(List<ActionDTO>, int)> GetActionsPaginated(int pageNumber, int pageSize)
        {
            (List<ActionEntity> actionEntities, int totalCount) = await actionRepository.GetActionsPaginatedAsync(pageNumber, pageSize);

            List<ActionDTO> actionDTOs = actionEntities
                                        .Select(actionEntity => actionConverter.GetActionDTOFromActionEntity(actionEntity))
                                        .ToList();

            return (actionDTOs, totalCount);
        }
    }
}
