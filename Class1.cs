using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Wheater_data_Analysis_API.Models;

namespace Wheater_data_Analysis_API.Models
{
    public class WeatherData
    {
        [Name("Data.Precipitation")]
        public float DataPrecipitation { get; set; }

        [Name("Date.Full")]
        public string DateFull { get; set; }

        [Name("Date.Month")]
        public int DateMonth { get; set; }

        [Name("Date.Week of")]
        public int DateWeekOf { get; set; }

        [Name("Date.Year")]
        public int DateYear { get; set; }

        [Name("Station.City")]
        public string StationCity { get; set; }

        [Name("Station.Code")]
        public string StationCode { get; set; }

        [Name("Station.Location")]
        public string StationLocation { get; set; }

        [Name("Station.State")]
        public string StationState { get; set; }

        [Name("Data.Temperature.Avg Temp")]
        public int DataTemperatureAvgTemp { get; set; }

        [Name("Data.Temperature.Max Temp")]
        public int DataTemperatureMaxTemp { get; set; }

        [Name("Data.Temperature.Min Temp")]
        public int DataTemperatureMinTemp { get; set; }

        [Name("Data.Wind.Direction")]
        public int DataWindDirection { get; set; }

        [Name("Data.Wind.Speed")]
        public float DataWindSpeed { get; set; }
    }
}
    

    public class WeatherDataMap : ClassMap<WeatherData>
    {
        public WeatherDataMap()
        {
        Map(m => m.DataPrecipitation).Name("Data.Precipitation");
        Map(m => m.DateFull).Name("Date.Full");
        Map(m => m.DateMonth).Name("Date.Month");
        Map(m => m.DateWeekOf).Name("Date.Week of");
        Map(m => m.DateYear).Name("Date.Year");
        Map(m => m.StationCity).Name("Station.City");
        Map(m => m.StationCode).Name("Station.Code");
        Map(m => m.StationLocation).Name("Station.Location");
        Map(m => m.StationState).Name("Station.State");
        Map(m => m.DataTemperatureAvgTemp).Name("Data.Temperature.Avg Temp");
        Map(m => m.DataTemperatureMaxTemp).Name("Data.Temperature.Max Temp");
        Map(m => m.DataTemperatureMinTemp).Name("Data.Temperature.Min Temp");
        Map(m => m.DataWindDirection).Name("Data.Wind.Direction");
        Map(m => m.DataWindSpeed).Name("Data.Wind.Speed");
        }
    }


