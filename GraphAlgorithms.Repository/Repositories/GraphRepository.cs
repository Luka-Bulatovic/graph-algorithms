﻿using GraphAlgorithms.Repository.Data;
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

        public async Task<IEnumerable<GraphEntity>> GetAllAsync()
        {
            List<GraphEntity> graphs = await _context.Graphs.ToListAsync();
            return graphs;
        }

        public async Task<GraphEntity> GetByIdAsync(int id)
        {
            GraphEntity graph = await _context.Graphs.FindAsync(id);
            return graph;
        }

        public async Task AddAsync(GraphEntity graph)
        {
            await _context.Graphs.AddAsync(graph);
            await _context.SaveChangesAsync();
        }
    }
}
