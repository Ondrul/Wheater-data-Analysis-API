using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WebApi.Helpers;
using Wheater_data_Analysis_API.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

    public class FormModel
    {
        public IFormFile File { get; set; }
    }
}

