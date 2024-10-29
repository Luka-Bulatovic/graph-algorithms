using GraphAlgorithms.Core;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service.Interfaces
{
    public interface IGraphConverter
    {
        /// <summary>
        /// Used to convert GraphDrawingUpdateDTO object to Graph object. Has optional parameters
        /// that allow it to either calculate or not Properties and Classes for Graph object.
        /// Used when calculating graph properties during drawing, or
        /// as helper when persisting drawn graphs.
        /// </summary>
        /// <param name="graphDTO"></param>
        /// <param name="calculateProperties"></param>
        /// <param name="calculateClasses"></param>
        /// <returns>Graph</returns>
        public Graph GetGraphFromGraphDrawingUpdateDTO(GraphDrawingUpdateDTO graphDTO, bool calculateWienerIndexOnly = false);

        /// <summary>
        /// Used for converting persisted GraphEntity object to GraphDTO object for displaying purposes.
        /// Will include any graph Properties or Classes that were persisted.
        /// </summary>
        /// <param name="graphEntity"></param>
        /// <returns>GraphDTO</returns>
        public GraphDTO GetGraphDTOFromGraphEntity(GraphEntity graphEntity);

        /// <summary>
        /// Used to convert GraphDrawingUpdateDTO object to new GraphEntity object for persisting in database.
        /// </summary>
        /// <param name="graphDTO"></param>
        /// <returns>GraphEntity</returns>
        public Task<GraphEntity> GetGraphEntityFromGraphDrawingUpdateDTO(GraphDrawingUpdateDTO graphDTO);

        /// <summary>
        /// Returns existing GraphEntity updated with new data, based on GraphDrawingUpdateDTO object.
        /// </summary>
        /// <param name="graphDTO"></param>
        /// <returns>GraphEntity</returns>
        public Task<GraphEntity> GetUpdatedGraphEntityFromGraphDrawingUpdateDTO(GraphDrawingUpdateDTO graphDTO);

        /// <summary>
        /// This function is used when converting Graph object to GraphEntity object,
        /// in order to be able to persist it in database. The Graph object is expected to have
        /// its properties and classes already calculated.
        /// Used when persisting randomly generated Graph objects, or drawn graphs (that were previously
        /// converted to Graph objects).
        /// </summary>
        /// <param name="graph"></param>
        /// <returns>GraphEntity</returns>
        public Task<GraphEntity> GetGraphEntityFromGraph(Graph graph);
    }
}
