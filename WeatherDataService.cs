using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wheater_data_Analysis_API.Models;
using WebApi.Helpers;

public class WeatherDataService
{
    private readonly DataContext _context;

    public WeatherDataService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<string>> GetCitiesAsync()
    {
        return await _context.WeatherData.Select(w => w.StationCity).Distinct().ToListAsync();
    }

    public async Task<List<string>> GetStatesAsync()
    {
        return await _context.WeatherData.Select(w => w.StationState).Distinct().ToListAsync();
    }
}
