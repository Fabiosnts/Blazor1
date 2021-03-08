using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1.Data
{
    public class WeatherForecastService
    {

        public List<WeatherForecast> GetForecastAsync(DateTime startDate)
        {

            List<WeatherForecast> list;
            using (var _context = new AppDbContext())
            {
                 list =
                _context.WeatherForecast
                .Where(x => x.TemperatureC > 50).ToList();
            }
                       

            return list;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<WeatherForecast[]> PostForecastAsync(DateTime startDate)
        {

            var rng = new Random();
            var result = Task.FromResult(Enumerable.Range(1, 1000).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());


            var _context = new AppDbContext();

            foreach (var item in result.Result)
                    _context.AddAsync(item);
            
            _context.SaveChanges();

            return result;
        }
    }
}
