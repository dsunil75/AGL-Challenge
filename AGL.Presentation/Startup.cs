using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGL.BusinessLogic;
using AGL.ExternalServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AGL.Presentation
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

            services.AddOptions();

            //dependency injection
            services.AddScoped<IPeopleBusinessLogic, PeopleBusinessLogic>();
            services.AddScoped<IPeopleExternalService, PeopleExternalService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/People/Error");
            }

            app.UseStatusCodePages();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=People}/{action=Index}/{id?}");

                routes.MapRoute(
                    "NotFound",
                    "{*url}",
                    new { controller = "People", action = "Error" }
                );
            });
        }
    }
}
