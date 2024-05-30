using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;
using Wheater_data_Analysis_API.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;


[ApiController]
[Route("api/[controller]")]
public class WeatherDataController : ControllerBase
{
    private readonly DataContext _context;

    public WeatherDataController(DataContext context)
    {
        _context = context;
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
        {
            query = query.Where(w => w.DateYear == year.Value);
        }

        if (month.HasValue)
        {
            query = query.Where(w => w.DateMonth == month.Value);
        }

        if (day.HasValue)
        {
            query = query.Where(w => w.DateWeekOf == day.Value);
        }

        if (!string.IsNullOrEmpty(city))
        {
            query = query.Where(w => w.StationCity == city);
        }

        if (!string.IsNullOrEmpty(state))
        {
            query = query.Where(w => w.StationState == state);
        }

        if (!query.Any())
        {
            return NotFound("No data found for the specified filters.");
        }

        var result = aggregationType switch
        {
            Aggregation.Max => await query.MaxAsync(w => w.DataTemperatureAvgTemp),
            Aggregation.Min => await query.MinAsync(w => w.DataTemperatureAvgTemp),
            Aggregation.Avg => await query.AverageAsync(w => w.DataTemperatureAvgTemp),
            Aggregation.Sum => await query.SumAsync(w => w.DataTemperatureAvgTemp),
            _ => throw new ArgumentOutOfRangeException()
        };

        string responseMessage = aggregationType switch
        {
            Aggregation.Max => $"The maximum temperature is: {result}",
            Aggregation.Min => $"The minimum temperature is: {result}",
            Aggregation.Avg => $"The average temperature is: {result}",
            Aggregation.Sum => $"The total temperature sum is: {result}",
            _ => "Unknown aggregation type"
        };

        return Ok(responseMessage);
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