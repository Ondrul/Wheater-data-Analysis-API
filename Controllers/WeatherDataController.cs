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
    public async Task<IActionResult> GetWeatherDataAnalysis()
    {
   
        var analysis = await _context.WeatherData
            .GroupBy(w => w.DateYear)
            .Select(g => new
            {
                Year = g.Key,
                AvgTemperature = g.Average(w => w.DataTemperatureAvgTemp)
            })
            .ToListAsync();

        return Ok(analysis);
    }

    public class FormModel
    {
        public IFormFile File { get; set; }
    }
}
