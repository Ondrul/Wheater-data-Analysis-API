using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Helpers;
using Wheater_data_Analysis_API.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

[ApiController]
[Route("api/[controller]")]
public class WeatherDataController : ControllerBase
{
    private readonly DataContext _context;
    private readonly WeatherDataService _service;

    public WeatherDataController(DataContext context, WeatherDataService service)
    {
        _context = context;
        _service = service;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadWeatherData(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

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
                _context.WeatherData.Add(record);
            }
            await _context.SaveChangesAsync();
        }

        return Ok("File uploaded and processed.");
    }

    [HttpGet("analysis")]
    public async Task<IActionResult> GetWeatherDataAnalysis(Aggregation aggregationType, int? year = null, int? month = null, int? day = null, string city = null, string state = null)
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

        var result = aggregationType switch
        {
            Aggregation.Max => $"Maximum temperature is: {await query.MaxAsync(w => w.DataTemperatureAvgTemp)}",
            Aggregation.Min => $"Minimum temperature is: {await query.MinAsync(w => w.DataTemperatureAvgTemp)}",
            Aggregation.Avg => $"Average temperature is: {await query.AverageAsync(w => w.DataTemperatureAvgTemp)}",
            Aggregation.Sum => $"Sum of temperatures is: {await query.SumAsync(w => w.DataTemperatureAvgTemp)}",
            _ => "Invalid aggregation type."
        };

        return Ok(result);
    }

    [HttpGet("cities")]
    public async Task<IActionResult> GetCities()
    {
        var cities = await _service.GetCitiesAsync();
        return Ok(cities);
    }

    [HttpGet("states")]
    public async Task<IActionResult> GetStates()
    {
        var states = await _service.GetStatesAsync();
        return Ok(states);
    }

    public class FormModel
    {
        public IFormFile File { get; set; }
    }
}

public enum Aggregation
{
    Max,
    Min,
    Avg,
    Sum,
}
