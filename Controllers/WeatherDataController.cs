using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Wheater_data_Analysis_API.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class WeatherDataController : ControllerBase
{
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
                Console.WriteLine($"Date: {record.DateFull}, City: {record.StationCity}, Temp: {record.DataTemperatureAvgTemp}");
            }

            return Ok(records);
        }
    }

    public class FormModel
    {
        public IFormFile File { get; set; }
    }
}