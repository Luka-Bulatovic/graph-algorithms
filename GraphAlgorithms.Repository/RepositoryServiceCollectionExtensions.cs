﻿using GraphAlgorithms.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<IGraphRepository, GraphRepository>();
            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IGraphClassRepository, GraphClassRepository>();
            services.AddScoped<ICustomGraphSetRepository, CustomGraphSetRepository>();
            services.AddScoped<IRandomGraphCriteriaRepository, RandomGraphCriteriaRepository>();

            return services;
        }
    }
}
