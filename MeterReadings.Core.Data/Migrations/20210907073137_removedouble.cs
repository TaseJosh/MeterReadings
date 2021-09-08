using Microsoft.EntityFrameworkCore.Migrations;

namespace MeterReadings.Core.Data.Migrations
{
    public partial class removedouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MeterReadValue",
                table: "MeterReadings",
                type: "int",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldMaxLength: 5);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "MeterReadValue",
                table: "MeterReadings",
                type: "float",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 5);
        }
    }
}
