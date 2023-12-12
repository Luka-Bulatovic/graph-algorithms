using GraphAlgorithms.Repository.Repositories;
using GraphAlgorithms.Service.Converters;
using GraphAlgorithms.Service.Interfaces;
using GraphAlgorithms.Service.Services;
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
            services.AddScoped<IGraphDrawingService, GraphDrawingService>();
            services.AddScoped<IGraphLibraryService, GraphLibraryService>();
            services.AddScoped<IGraphActionsService, GraphActionsService>();
            services.AddScoped<IGraphClassService, GraphClassService>();
            
            services.AddScoped<IGraphConverter, GraphConverter>();
            services.AddScoped<IActionConverter, ActionConverter>();
            services.AddScoped<IGraphClassConverter, GraphClassConverter>();

            return services;
        }
    }
}
