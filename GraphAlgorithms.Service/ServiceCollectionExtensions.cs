using GraphAlgorithms.Core;
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
            services.AddScoped<IRandomGraphsService, RandomGraphsService>();
            services.AddScoped<IGraphImportService, GraphImportService>();
            services.AddScoped<ICustomGraphSetsService, CustomGraphSetsService>();
            services.AddScoped<IRandomGraphCriteriaService, RandomGraphCriteriaService>();
            
            services.AddScoped<IGraphConverter, GraphConverter>();
            services.AddScoped<IActionConverter, ActionConverter>();
            services.AddScoped<IGraphClassConverter, GraphClassConverter>();
            services.AddScoped<IGraphPropertyConverter, GraphPropertyConverter>();
            services.AddScoped<ICustomGraphSetConverter, CustomGraphSetConverter>();

            services.AddScoped<GraphEvaluator>();
            services.AddScoped<RandomGraphsGenerator>();
            services.AddScoped<GraphAlgorithmManager>();

            return services;
        }
    }
}
