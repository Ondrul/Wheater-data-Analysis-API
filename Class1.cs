namespace Wheater_data_Analysis_API.Models
{
    public class WeatherData
    {
        public float DataPrecipitation { get; set; }
        public string DateFull { get; set; }
        public int DateMonth { get; set; }
        public int DateWeekOf { get; set; }
        public int DateYear { get; set; }
        public string StationCity { get; set; }
        public string StationCode { get; set; }
        public string StationLocation { get; set; }
        public string StationState { get; set; }
        public int DataTemperatureAvgTemp { get; set; }
        public int DataTemperatureMaxTemp { get; set; }
        public int DataTemperatureMinTemp { get; set; }
        public int DataWindDirection { get; set; }
        public float DataWindSpeed { get; set; }
    }
}

