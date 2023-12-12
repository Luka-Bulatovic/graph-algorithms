using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Repository.Repositories;
using GraphAlgorithms.Service.DTO;
using GraphAlgorithms.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Services
{
    public class GraphClassService : IGraphClassService
    {
        public readonly IGraphClassRepository graphClassRepository;
        public readonly IGraphClassConverter graphClassConverter;

        public GraphClassService(IGraphClassRepository graphClassRepository, IGraphClassConverter graphClassConverter)
        {
            this.graphClassRepository = graphClassRepository;
            this.graphClassConverter = graphClassConverter;
        }

        public async Task<List<GraphClassDTO>> GetGraphClasses()
        {
            List<GraphClassEntity> graphClassEntities = await graphClassRepository.GetGraphClassesAsync();

            return graphClassEntities.Select(graphClassEntity => graphClassConverter.GetGraphClassDTOFromGraphClassEntity(graphClassEntity))
                                     .ToList();
        }
    }
}
