﻿using GraphAlgorithms.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Service
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceProjectServices(this IServiceCollection services)
        {
            services.AddScoped<IMainService, MainService>();

            return services;
        }
    }
}
