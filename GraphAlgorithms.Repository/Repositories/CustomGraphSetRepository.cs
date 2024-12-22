using GraphAlgorithms.Repository.Data;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Repository.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Repositories
{
    public class CustomGraphSetRepository : ICustomGraphSetRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomGraphSetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomGraphSetEntity> AddGraphsToSetAsync(CustomGraphSetEntity customGraphSet, List<GraphEntity> graphs)
        {
            if (customGraphSet == null || graphs == null)
                return null;

            foreach (var newGraph in graphs)
            {
                if (customGraphSet.Graphs.Where(g => g.ID == newGraph.ID).Count() == 0)
                    customGraphSet.Graphs.Add(newGraph);
            }

            await _context.SaveChangesAsync();

            return customGraphSet;
        }

        public async Task<CustomGraphSetEntity> Create(string customGraphSetName, string userID, List<GraphEntity> graphs)
        {
            CustomGraphSetEntity customGraphSet = new CustomGraphSetEntity()
            {
                Name = customGraphSetName,
                CreatedByID = userID,
                CreatedDate = DateTime.Now,
                Graphs = graphs ?? new List<GraphEntity>()
            };

            _context.CustomGraphSets.Add(customGraphSet);
            await _context.SaveChangesAsync();
            return customGraphSet;
        }

        public async Task<CustomGraphSetEntity> GetByIdAsync(int id)
        {
            var customGraphSet = await _context.CustomGraphSets
                                        .Include(s => s.CreatedBy)
                                        .Include(s => s.Graphs)
                                        .FirstOrDefaultAsync(s => s.ID == id);
            return customGraphSet;
        }

        public async Task<(List<CustomGraphSetEntity> customGraphSets, int totalCount)> GetCustomGraphSetsPaginatedAsync(string userID, int pageNumber, int pageSize)
        {
            var query = _context.CustomGraphSets
                            .Where(cgs => cgs.CreatedByID == userID)
                            .AsQueryable();
            var totalCount = await query.CountAsync();
            var customGraphSets = await query
                                        .Include(g => g.CreatedBy)
                                        .OrderByDescending(a => a.ID)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            return (customGraphSets, totalCount);
        }
    }
}
