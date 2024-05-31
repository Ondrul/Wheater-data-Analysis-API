using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Helpers;
using Wheater_data_Analysis_API.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Http;
using Wheater_data_Analysis_API;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApi.Services
{
    public interface IWeatherDataService
    {
        Task<List<string>> GetCitiesAsync();
        Task<List<string>> GetStatesAsync();
        Task UploadWeatherDataAsync(IFormFile file);
        Task<string> GetWeatherDataAnalysis(Aggregation aggregationType, int? year, int? month, int? day, string city, string state);
    }

    public class WeatherDataService : IWeatherDataService
    {
        private readonly IWheatherDataRepository repository;

        public WeatherDataService(IWheatherDataRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<string>> GetCitiesAsync()
        {
            return await repository.GetCitiesAsync();
        }

        public async Task<List<string>> GetStatesAsync()
        {
            return await repository.GetStatesAsync();
        }

        public async Task UploadWeatherDataAsync(IFormFile file)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, csvConfig))
            {
                csv.Context.RegisterClassMap<WeatherDataMap>();
                var records = csv.GetRecords<WeatherData>().ToList();

                foreach (var record in records)
                {
                    await repository.AddWeatherDataAsync(record);
                }

                await repository.SaveChangesAsync();
        
            
            }
        }

        public async Task<string> GetWeatherDataAnalysis(Aggregation aggregationType, int? year, int? month, int? day, string city, string state)
        {
            var result = aggregationType switch
            {
                Aggregation.Max => $"Maximum temperature is: {await repository.MaxAsync(year, month, day, city, state)}",
                Aggregation.Min => $"Minimum temperature is: {await repository.MinAsync(year, month, day, city, state)}",
                Aggregation.Avg => $"Average temperature is: {await repository.AvgAsync(year, month, day, city, state)}",
                Aggregation.Sum => $"Sum of temperatures is: {await repository.SumAsync(year, month, day, city, state)}",
                _ => "Invalid aggregation type."
            };

            return result;
        }
    }
   
}
