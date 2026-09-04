using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayrollSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class AddOvertimeHoursToAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OvertimeHours",
                table: "Attendances",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OvertimeHours",
                table: "Attendances");
        }
    }
}
