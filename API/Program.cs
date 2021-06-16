using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
           var host = CreateHostBuilder(args).Build();
           using var scope = host.Services.CreateScope();
           var services = scope.ServiceProvider;

           try 
           {
               services.GetRequiredService<ILogger<Program>>().LogInformation("Dopefunk");
               var context = services.GetService<DataContext>();
               await context.Database.MigrateAsync();
               await Seed.SeedData(context);
           }
           catch(Exception e) 
           {
               services.GetRequiredService<ILogger<Program>>().LogError(e.Message);
           }

           await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}