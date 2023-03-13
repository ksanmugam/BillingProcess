using BillingProcess.Billing.DllApi;
using BillingProcess.Billing.Services;
using BillingProcess.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BillingProcess
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
            services.AddControllers();

            services.AddScoped<BillingApi>();

            services.AddScoped<BillingService>();

            services.AddControllers().AddNewtonsoftJson();

            services.AddDbContext<DatabaseContext>(
                ob => ob.UseSqlServer(Configuration["ConnectionString"],
                sso => sso.MigrationsAssembly(
                    Assembly.GetExecutingAssembly().GetName().Name)
                    ));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bill Process API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSwagger();
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Billing Process API");
            });
        }

        private static void AddCompanyData(DatabaseContext context)
        {
            //var lines = File.ReadLines(@"C:\Users\ksanmugam\source\repos\BillingProcess\BillingProcess\Companies.csv").Select(a => a.Split(','));
            //var CSV = from line in lines select (line.Split(',')).toA
            //var product = new Company("Coca-Cola 600ml", "Best mixer for spirits", 1.25);

            //context.Products.Add(product);

            context.SaveChanges();
        }
    }
}
