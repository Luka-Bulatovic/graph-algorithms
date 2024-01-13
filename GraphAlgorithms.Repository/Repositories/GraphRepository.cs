using GraphAlgorithms.Repository.Data;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Shared;
using Microsoft.EntityFrameworkCore;

namespace GraphAlgorithms.Repository.Repositories
{
    public class GraphRepository : IGraphRepository
    {
        private readonly ApplicationDbContext _context;

        public GraphRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GraphEntity>> GetAllAsync()
        {
            List<GraphEntity> graphs = await _context.Graphs
                                                    .Include(g => g.Action)
                                                        .ThenInclude(a => a.ActionType)
                                                    .Include(g => g.Action)
                                                        .ThenInclude(a => a.ForGraphClass)
                                                    .OrderByDescending(g => g.ID)
                                                    .ToListAsync();
            return graphs;
        }

        public async Task<(List<GraphEntity>, int)> GetGraphsPaginatedAsync(int pageNumber, int pageSize, List<SearchParameter> searchParams = null)
        {
            var query = _context.Graphs.AsQueryable();

            if(searchParams != null && searchParams.Count > 0)
                query = ApplySearchCriteria(query, searchParams);

            var totalCount = await query.CountAsync();
            var graphs = await query
                            .Include(g => g.Action)
                                .ThenInclude(a => a.ActionType)
                            .Include(g => g.Action)
                                .ThenInclude(a => a.ForGraphClass)
                            .OrderByDescending(g => g.ID)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            return (graphs, totalCount);
        }

        private IQueryable<GraphEntity> ApplySearchCriteria(IQueryable<GraphEntity> query, List<SearchParameter> searchParams)
        {
            foreach(var searchParam in searchParams)
            {
                if (!searchParam.IsValid)
                    continue;
                
                switch(searchParam.Key)
                {
                    case "order":
                        if (searchParam.FieldType != SearchFieldType.NumberRange)
                            continue;

                        query = query.Where(g => g.Order >= int.Parse(searchParam.Values[0]) && g.Order <= int.Parse(searchParam.Values[1]));
                        break;
                    default:
                        break;
                }
            }

            return query;
        }

        public async Task<GraphEntity> GetByIdAsync(int id)
        {
            GraphEntity graph = await _context.Graphs
                                                .Include(g => g.Action)
                                                    .ThenInclude(a => a.ActionType)
                                                .Include(g => g.Action)
                                                    .ThenInclude(a => a.ForGraphClass)
                                                .FirstOrDefaultAsync(g => g.ID == id);
            return graph;
        }

        public async Task<GraphEntity> SaveAsync(GraphEntity graph)
        {
            if(graph.ID == 0)
                await _context.Graphs.AddAsync(graph);
            else if(graph.ID > 0)
                _context.Graphs.Update(graph);
            
            await _context.SaveChangesAsync();

            return await GetByIdAsync(graph.ID);
        }

        public async Task<(List<GraphEntity>, int)> GetGraphsForActionPaginatedAsync(int actionID, int pageNumber, int pageSize)
        {
            var totalCount = await _context.Graphs
                                            .Where(g => g.ActionID == actionID)
                                            .CountAsync();

            var graphs = await _context.Graphs
                                        .Where(g => g.ActionID == actionID)
                                        .Include(g => g.Action)
                                            .ThenInclude(a => a.ActionType)
                                        .Include(g => g.Action)
                                            .ThenInclude(a => a.ForGraphClass)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();
            return (graphs, totalCount);
        }
    }
}
