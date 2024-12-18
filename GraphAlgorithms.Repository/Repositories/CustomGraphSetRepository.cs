using GraphAlgorithms.Repository.Data;
using GraphAlgorithms.Repository.Entities;
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

        public async Task<CustomGraphSetEntity> GetByIdAsync(int id)
        {
            var customGraphSet = await _context.CustomGraphSets
                                        .Include(s => s.CreatedBy)
                                        .FirstOrDefaultAsync(s => s.ID == id);
            return customGraphSet;
        }

        public async Task<(List<CustomGraphSetEntity> customGraphSets, int totalCount)> GetCustomGraphSetsPaginatedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.CustomGraphSets.CountAsync();
            var customGraphSets = await _context.CustomGraphSets
                                        .Include(g => g.CreatedBy)
                                        .OrderByDescending(a => a.ID)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            return (customGraphSets, totalCount);
        }
    }
}
