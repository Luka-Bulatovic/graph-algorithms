using GraphAlgorithms.Repository.Data;
using GraphAlgorithms.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Repositories
{
    public class GraphRepository : IGraphRepository
    {
        private readonly ApplicationDbContext _context;

        public GraphRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Graph>> GetAllAsync()
        {
            List<Graph> graphs = await _context.Graphs.ToListAsync();
            return graphs;
        }

        public async Task<Graph> GetByIdAsync(int id)
        {
            Graph graph = await _context.Graphs.FindAsync(id);
            return graph;
        }

        public async Task AddAsync(Graph graph)
        {
            await _context.Graphs.AddAsync(graph);
            await _context.SaveChangesAsync();
        }
    }
}
