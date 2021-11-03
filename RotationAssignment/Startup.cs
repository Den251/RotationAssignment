using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RotationAssignment.Models;
using System.Web.Http;
using System.Collections.Specialized;

namespace RotationAssignment
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
            //services.AddSingleton()
            services.AddControllers();
            services.AddSingleton<List<Terminal>>();
            services.AddSingleton<List<Cargo>>();
            services.AddSingleton<Terminal>();
            services.AddSingleton<Rotation>();    
            services.AddSingleton<Prospect>(); 
            services.AddSingleton<List<TimeStamp>>();
            services.AddSingleton<OrderedDictionary>();
            //services.AddSingleton<Cargo>(); Rotation
            //services.AddDbContext<RotationContext>(opt =>
            //                                   opt.UseInMemoryDatabase("Rotation"));
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RotationAssignment", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RotationAssignment v1"));
            }
            //var config = new HttpConfiguration();
            //config.MapHttpAttributeRoutes();


            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
