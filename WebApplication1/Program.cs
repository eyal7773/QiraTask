using Dal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Dal.Maps;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var map = new InvoiceMap();
            var x = new CsvRepo<Invoice,InvoiceMap>($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}Invoices.csv",map);
            
            x.ReadAll().ForEach(x => Console.WriteLine($"{x.Id} {x.Status} {x.Amount} {x.PaymentMethod} {x.CreatedAt} {x.UpdatedAt}"));
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
