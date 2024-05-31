using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Services;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Http;

[ApiController]
[Route("api/[controller]")]
public class WeatherDataController : ControllerBase
{
    private readonly IWeatherDataService _service;

    public WeatherDataController(IWeatherDataService service)
    {
        _service = service;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadWeatherData(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        await _service.UploadWeatherDataAsync(file);
        return Ok("File uploaded and processed.");
    }
   [HttpGet("analysis")]
public async Task<IActionResult> GetWeatherDataAnalysis(Aggregation aggregationType, int? year, int? month, int? day, string city = null, string state = null)
{
    var result = await _service.GetWeatherDataAnalysis(aggregationType, year, month, day, city, state);
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
}
