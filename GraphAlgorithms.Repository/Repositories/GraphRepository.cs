using GraphAlgorithms.Repository.Data;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Shared;
using Microsoft.EntityFrameworkCore;
using static GraphAlgorithms.Shared.SearchParameter;

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

        public async Task<(List<GraphEntity>, int)> GetGraphsPaginatedAsync(int pageNumber, int pageSize, List<SearchParameter> searchParams = null, string sortBy = "")
        {
            var query = _context.Graphs.AsQueryable();

            if(searchParams != null && searchParams.Count > 0)
                query = ApplySearchCriteria(query, searchParams);

            var totalCount = await query.CountAsync();

            query = query
                            .Include(g => g.Action)
                                .ThenInclude(a => a.ActionType)
                            .Include(g => g.Action)
                                .ThenInclude(a => a.ForGraphClass)
                            .Include(g => g.GraphClasses)
                            .Include(g => g.GraphPropertyValues);

            query = ApplySortCriteria(query, sortBy);

            var graphs = await query
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            return (graphs, totalCount);
        }

        private IQueryable<GraphEntity> ApplySortCriteria(IQueryable<GraphEntity> query, string sortBy)
        {
            if (sortBy == "")
                return query;

            switch(sortBy)
            {
                case "wiener,asc":
                    query = query.OrderBy(g => g.WienerIndex);
                    break;
                case "wiener,desc":
                    query = query.OrderByDescending(g => g.WienerIndex);
                    break;
            }

            return query;
        }

        private IQueryable<GraphEntity> ApplySearchCriteria(IQueryable<GraphEntity> query, List<SearchParameter> searchParams)
        {
            foreach(var searchParam in searchParams)
            {
                if (!searchParam.IsValid)
                    continue;
                
                switch(searchParam.Key)
                {
                    case "id":
                        if (searchParam.ParamType != SearchParamType.Number)
                            continue;

                        var integerValuesList = searchParam.Values.Select(v => int.Parse(v)).ToList();
                        
                        query = query.Where(g => integerValuesList.Any(v => v == g.ID));
                        break;
                    case "order":
                        if (searchParam.ParamType != SearchParamType.NumberRange)
                            continue;

                        query = query.Where(g => g.Order >= int.Parse(searchParam.Values[0]) && g.Order <= int.Parse(searchParam.Values[1]));
                        break;
                    case "size":
                        if (searchParam.ParamType != SearchParamType.NumberRange)
                            continue;

                        query = query.Where(g => g.Size >= int.Parse(searchParam.Values[0]) && g.Size <= int.Parse(searchParam.Values[1]));
                        break;
                    case "class":
                        if (searchParam.ParamType != SearchParamType.MultiSelectList)
                            continue;

                        var graphClassIDs = searchParam.Values.Select(v => int.Parse(v)).ToList();
                        query = query.Where(g => g.GraphClasses.Any(gc => graphClassIDs.Contains(gc.ID)));
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
                                                .Include(g => g.GraphClasses)
                                                .Include(g => g.GraphPropertyValues)
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
                                        .Include(g => g.GraphClasses)
                                        .Include(g => g.GraphPropertyValues)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();
            return (graphs, totalCount);
        }
    }
}
