using Dal;
using Dal.Maps;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Db;

namespace WebApplication1
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "QiraTask", Version = "v1" });
            });


            switch (GetDbType())
            {
                case "EF":
                    services.AddDbContext<DbContext,ApiContext>(options => options.UseInMemoryDatabase(databaseName: "QiraTask"));
                    services.AddTransient<IRepo<Invoice>, GenericRepository<Invoice>>();
                    break;
                case "CSV":
                default:
                    services.AddScoped<IRepo<Invoice>,CsvRepo<Invoice,InvoiceMap>>((serviceProvider) =>
                    {
                        var map = new InvoiceMap();
                        var repo = new CsvRepo<Invoice, InvoiceMap>($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Invoices.csv", map);
                        return repo;
                    });
                    break;
            }

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private string GetDbType()
        {
            return (Environment.GetEnvironmentVariable("DB_TYPE") == null ? "CSV" : Environment.GetEnvironmentVariable("DB_TYPE"));
        }
    }
}
