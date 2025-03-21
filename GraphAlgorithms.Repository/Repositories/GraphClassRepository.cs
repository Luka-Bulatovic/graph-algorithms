﻿using GraphAlgorithms.Repository.Data;
using GraphAlgorithms.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Repositories
{
    public class GraphClassRepository : IGraphClassRepository
    {
        public readonly ApplicationDbContext _context;

        public GraphClassRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<GraphClassEntity> GetGraphClassByIdAsync(int id)
        {
            return await _context
                .GraphClasses
                .Where(gc => gc.ID == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<GraphClassEntity>> GetGraphClassesAsync()
        {
            List<GraphClassEntity> graphClasses = await _context.GraphClasses.ToListAsync();
            
            return graphClasses;
        }

        public async Task<List<GraphClassEntity>> GetClassifiableGraphClassesAsync()
        {
            List<GraphClassEntity> graphClasses = 
                await _context.GraphClasses
                        .Where(gc => gc.HasClassifier == true)
                        .ToListAsync();

            return graphClasses;
        }

        public async Task<List<GraphClassEntity>> GetGraphClassesByIDsAsync(List<int> ids)
        {
            return await _context
                    .GraphClasses
                    .Where(gc => ids.Contains(gc.ID))
                    .ToListAsync();
        }

        public async Task<List<GraphClassEntity>> GetGraphClassesForGeneratingRandomGraphsAsync()
        {
            List<GraphClassEntity> graphClasses = await _context.GraphClasses
                                                                .Where(gc => gc.CanGenerateRandomGraphs)
                                                                .ToListAsync();

            return graphClasses;
        }

        public async Task<List<GraphPropertyEntity>> GetRandomGenerationPropertiesForGraphClassAsync(int id)
        {
            GraphClassEntity? graphClass = await _context
                .GraphClasses
                .Where(gc => gc.ID == id)
                .Include(gc => gc.RandomGenerationGraphProperties)
                .FirstOrDefaultAsync();

            if (graphClass == null)
                return new List<GraphPropertyEntity>();

            return graphClass.RandomGenerationGraphProperties.ToList();
        }
    }
}
