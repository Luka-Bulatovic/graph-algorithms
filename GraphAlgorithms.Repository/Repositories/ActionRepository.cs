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
    public class ActionRepository : IActionRepository
    {
        private readonly ApplicationDbContext _context;

        public ActionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(List<ActionEntity> actions, int totalCount)> GetActionsPaginatedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Actions.CountAsync();
            var actions = await _context.Actions
                                        .Include(g => g.ActionType)
                                        .Include(g => g.ForGraphClass)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .OrderByDescending(a => a.ID)
                                        .ToListAsync();
            return (actions, totalCount);
        }

        public async Task<ActionEntity> GetByIdAsync(int id)
        {
            var action = await _context.Actions
                                        .Include(g => g.ActionType)
                                        .Include(g => g.ForGraphClass)
                                        .FirstOrDefaultAsync(g => g.ID == id);
            return action;
        }
    }
}
