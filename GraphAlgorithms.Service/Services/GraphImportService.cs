using GraphAlgorithms.Core;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Repository.Repositories;
using GraphAlgorithms.Service.Converters;
using GraphAlgorithms.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GraphAlgorithms.Service.Services
{
    public class GraphImportService : IGraphImportService
    {
        private readonly GraphEvaluator graphEvaluator;
        private readonly IGraphConverter graphConverter;
        private readonly IGraphRepository graphRepository;

        public GraphImportService(
            GraphEvaluator graphEvaluator,
            IGraphConverter graphConverter,
            IGraphRepository graphRepository)
        {
            this.graphEvaluator = graphEvaluator;
            this.graphConverter = graphConverter;
            this.graphRepository = graphRepository;
        }

        public async Task<GraphEntity> ImportFromFile(string filePath)
        {
            // Check if file has valid XML
            // Read it as text
            string graphML = await System.IO.File.ReadAllTextAsync(filePath);

            // Try parsing it as XML
            var xdoc = XDocument.Parse(graphML);

            // Create graph based on text representation
            Graph graph = graphEvaluator.GetGraphFromGraphML(0, graphML);
            graphEvaluator.CalculateGraphPropertiesAndClasses(graph);
            GraphEntity graphEntity = await graphConverter.GetGraphEntityFromGraph(graph);

            var actionEntity = new ActionEntity
            {
                ActionTypeID = (int)ActionTypeEnum.Import,
                CreatedByID = 0,
                CreatedDate = DateTime.UtcNow,
                ActionPropertyValues = new List<ActionPropertyValueEntity>()
            };

            graphEntity.DataXML = graphML;
            graphEntity.Action = actionEntity;

            await graphRepository.SaveAsync(graphEntity);

            return graphEntity;
        }
    }
}
