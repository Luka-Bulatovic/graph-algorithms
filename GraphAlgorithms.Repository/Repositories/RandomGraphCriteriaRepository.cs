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
    public class RandomGraphCriteriaRepository : IRandomGraphCriteriaRepository
    {
        private readonly ApplicationDbContext _context;
        public RandomGraphCriteriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<RandomGraphCriteriaEntity>> GetAllAsync()
        {
            return await _context.RandomGraphCriteria.ToListAsync();
        }
    }
}
