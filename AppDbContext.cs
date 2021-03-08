using BlazorApp1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<WeatherForecast> WeatherForecast { get; set; }
        //public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        //{ }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=BlazorDB;Integrated Security=True");
                                
            }

        }

        


            
    }

    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                // auto migration
                context = new AppDbContext();
                context.Database.Migrate();

                seeds(context);

                // Seed the database.
                InitializeUserAndRoles(context);



            }
        }

        public static void seeds(AppDbContext context)
        {

            if (context.WeatherForecast.Count() == 0)
            {
                var rng = new Random();
                var startDate = DateTime.Now;
                var result = Task.FromResult(Enumerable.Range(1, 1000).Select(index => new WeatherForecast
                {
                    Date = startDate.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                }).ToArray());


                foreach (var item in result.Result)
                    context.AddAsync(item);

                context.SaveChanges();


            }
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        private static void InitializeUserAndRoles(AppDbContext context)
        {
            // init user and roles  
        }
    }
}
