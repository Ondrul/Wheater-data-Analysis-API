using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApi.Helpers;
using WebApi.Services;
using Wheater_data_Analysis_API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase")));


builder.Services.AddTransient<IWeatherDataService, WeatherDataService>();
builder.Services.AddTransient<IWheatherDataRepository, WheatherDataRepository>();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.UseAuthorization();

app.Run();
