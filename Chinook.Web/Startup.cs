using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chinook.Model;
using Chinook.Model.Commands.Actors;
using Chinook.Model.Commands.Artists;
using Chinook.Model.Data;
using Chinook.Model.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chinook.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddTransient<ArtistQuery>();
            services.AddTransient<FilmQuery>();
            services.AddTransient<ActorQuery>();
            services.AddTransient<ActorCommand>();
            services.AddTransient<ArtistCommand>();
            services.AddTransient<AlbumQuery>();
            services.AddTransient<PlayListQuery>();
            services.AddTransient<IQuery<Customer>,Query<Customer>>();
            services.AddTransient<IRunner>(s => new Runner("server=localhost;username=Hussein;password=123456;database=chinook;"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}