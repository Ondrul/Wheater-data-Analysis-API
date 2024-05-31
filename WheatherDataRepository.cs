using Microsoft.EntityFrameworkCore;
using WebApi.Helpers;
using Wheater_data_Analysis_API.Models;

namespace Wheater_data_Analysis_API;

public interface IWheatherDataRepository
{
    Task<List<string>> GetCitiesAsync();
    Task<List<string>> GetStatesAsync();

    Task<decimal> MaxAsync(int? year, int? month, int? day, string city, string state);
    Task<decimal> MinAsync(int? year, int? month, int? day, string city, string state);
    Task<decimal> SumAsync(int? year, int? month, int? day, string city, string state);
    Task<decimal> AvgAsync(int? year, int? month, int? day, string city, string state);
    Task SaveChangesAsync();
    Task AddWeatherDataAsync(WeatherData weatherData);

}


public class WheatherDataRepository : GenericRepository<WeatherData>, IWheatherDataRepository
{
    private IQueryable<WeatherData> GetFilterQuery(int? year, int? month, int? day, string city, string state)
    {
        var query = _context.WeatherData.AsQueryable();

        if (year.HasValue)
            query = query.Where(w => w.DateYear == year.Value);
        if (month.HasValue)
            query = query.Where(w => w.DateMonth == month.Value);
        if (day.HasValue)
            query = query.Where(w => w.DateWeekOf == day.Value);
        if (!string.IsNullOrEmpty(city))
            query = query.Where(w => w.StationCity == city);
        if (!string.IsNullOrEmpty(state))
            query = query.Where(w => w.StationState == state);

        return query;
    }
    public WheatherDataRepository(DataContext context) : base(context)
    {
    }

    public async Task<List<string>> GetCitiesAsync()
    {
        return await _context.WeatherData.Select(w => w.StationCity).Distinct().ToListAsync();
    }
    public async Task<List<string>> GetStatesAsync()
    {
        return await _context.WeatherData.Select(w => w.StationState).Distinct().ToListAsync();
    }
    public async Task<decimal> MaxAsync(int? year, int? month, int? day, string city, string state)
    {
        return await GetFilterQuery(year, month, day, city, state)
                        .MaxAsync(w => w.DataTemperatureAvgTemp);
    }

    public async Task<decimal> MinAsync(int? year, int? month, int? day, string city, string state)
    {
        return await GetFilterQuery(year, month, day, city, state)
                        .MinAsync(w => w.DataTemperatureAvgTemp);
    }

    public async Task<decimal> SumAsync(int? year, int? month, int? day, string city, string state)
    {

        return await GetFilterQuery(year, month, day, city, state)
                        .SumAsync(w => w.DataTemperatureAvgTemp);
    }

    public async Task<decimal> AvgAsync(int? year, int? month, int? day, string city, string state)
    {
        return (decimal) await GetFilterQuery(year, month, day, city, state)
                                .AverageAsync(w => w.DataTemperatureAvgTemp);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task AddWeatherDataAsync(WeatherData weatherData)
    {
        await _context.WeatherData.AddAsync(weatherData);
    }
 
}
