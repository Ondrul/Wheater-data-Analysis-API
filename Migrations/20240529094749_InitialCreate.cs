using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheater_data_Analysis_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataPrecipitation = table.Column<float>(type: "REAL", nullable: false),
                    DateFull = table.Column<string>(type: "TEXT", nullable: false),
                    DateMonth = table.Column<int>(type: "INTEGER", nullable: false),
                    DateWeekOf = table.Column<int>(type: "INTEGER", nullable: false),
                    DateYear = table.Column<int>(type: "INTEGER", nullable: false),
                    StationCity = table.Column<string>(type: "TEXT", nullable: false),
                    StationCode = table.Column<string>(type: "TEXT", nullable: false),
                    StationLocation = table.Column<string>(type: "TEXT", nullable: false),
                    StationState = table.Column<string>(type: "TEXT", nullable: false),
                    DataTemperatureAvgTemp = table.Column<int>(type: "INTEGER", nullable: false),
                    DataTemperatureMaxTemp = table.Column<int>(type: "INTEGER", nullable: false),
                    DataTemperatureMinTemp = table.Column<int>(type: "INTEGER", nullable: false),
                    DataWindDirection = table.Column<int>(type: "INTEGER", nullable: false),
                    DataWindSpeed = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherData");
        }
    }
}
