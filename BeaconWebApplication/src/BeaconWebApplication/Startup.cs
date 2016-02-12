using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using BeaconWebApplication.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using AutoMapper;
using BeaconWebApplication.ViewModels;
using Sakura.AspNet.Mvc.PagedList;

namespace BeaconWebApplication
{
    public class Startup
    {
        public static IConfigurationRoot Configuration; 
        public Startup(IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
                
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<BeaconsContext>();

            services.AddTransient<BeaconsContextSeedData>();
            services.AddScoped<IBeaconsRepository, BeaconsRepository>();

            services.UseBootstrapPagerGenerator();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, BeaconsContextSeedData seeder)
        {

            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();


            Mapper.Initialize(config =>
            {
                config.CreateMap<Beacon, BeaconViewModel>().ReverseMap();
                config.CreateMap<Log, LogViewModel>().ReverseMap();
            });

            app.UseIISPlatformHandler();

            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", action ="Index"}
                    );
            });

            seeder.EnsureSeedData();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
