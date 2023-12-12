using GraphAlgorithms.Repository.Data;
using GraphAlgorithms.Repository.Entities;
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
                                                    .ToListAsync();
            return graphs;
        }

        public async Task<(List<GraphEntity>, int)> GetGraphsPaginatedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Graphs.CountAsync();
            var graphs = await _context.Graphs
                                       .Include(g => g.Action)
                                       .ThenInclude(a => a.ActionType)
                                       .Skip((pageNumber - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();
            return (graphs, totalCount);
        }

        public async Task<GraphEntity> GetByIdAsync(int id)
        {
            GraphEntity graph = await _context.Graphs
                                              .Include(g => g.Action)
                                              .ThenInclude(a => a.ActionType)
                                              .FirstOrDefaultAsync(g => g.ID == id);
            return graph;
        }

        public async Task SaveAsync(GraphEntity graph)
        {
            if(graph.ID == 0)
                await _context.Graphs.AddAsync(graph);
            else if(graph.ID > 0)
                _context.Graphs.Update(graph);
            
            await _context.SaveChangesAsync();
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
                                       .Skip((pageNumber - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();
            return (graphs, totalCount);
        }
    }
}
