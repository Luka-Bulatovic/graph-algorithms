using GraphAlgorithms.Repository;
using GraphAlgorithms.Repository.Data;
using GraphAlgorithms.Repository.Entities;
using GraphAlgorithms.Service;
using GraphAlgorithms.Service.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace GraphAlgorithms.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                    .AddViewOptions(options =>
                    {
                        options.HtmlHelperOptions.ClientValidationEnabled = true;
                    });


            // Inject DbContext with connection to SQL Server DB, used by Repository
            string connectionString = Configuration.GetConnectionString("WebAppDB");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Identity
            services.AddIdentity<UserEntity, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Login/Logout";
            });

            // Inject Service and Repository services
            services.AddServiceProjectServices();
            services.AddRepositoryServices();

            // Provide the ability to inject IHttpContextAccessor where needed, in order to access things such as User
            services.AddHttpContextAccessor();

            // DI for IUserContext - Web will provide UserContext implementation for Service
            services.AddScoped<IUserContext, UserContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Setup Temp folder for static files download
            var tempFolderPath = Path.Combine(env.ContentRootPath, "Temp");

            // MIME type for .graphml
            FileExtensionContentTypeProvider graphMLContentTypeProvider = new FileExtensionContentTypeProvider();
            graphMLContentTypeProvider.Mappings[".graphml"] = "application/graphml+xml";

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(tempFolderPath),
                RequestPath = "/Temp",
                ContentTypeProvider = graphMLContentTypeProvider
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
