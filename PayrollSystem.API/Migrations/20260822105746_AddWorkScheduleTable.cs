using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayrollSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkScheduleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_WorkSchedule_WorkScheduleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkScheduleDay_WorkSchedule_WorkScheduleId",
                table: "WorkScheduleDay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkScheduleDay",
                table: "WorkScheduleDay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkSchedule",
                table: "WorkSchedule");

            migrationBuilder.RenameTable(
                name: "WorkScheduleDay",
                newName: "WorkScheduleDays");

            migrationBuilder.RenameTable(
                name: "WorkSchedule",
                newName: "WorkSchedules");

            migrationBuilder.RenameIndex(
                name: "IX_WorkScheduleDay_WorkScheduleId_DayOfWeek",
                table: "WorkScheduleDays",
                newName: "IX_WorkScheduleDays_WorkScheduleId_DayOfWeek");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkScheduleDays",
                table: "WorkScheduleDays",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkSchedules",
                table: "WorkSchedules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_WorkSchedules_WorkScheduleId",
                table: "Employees",
                column: "WorkScheduleId",
                principalTable: "WorkSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkScheduleDays_WorkSchedules_WorkScheduleId",
                table: "WorkScheduleDays",
                column: "WorkScheduleId",
                principalTable: "WorkSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_WorkSchedules_WorkScheduleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkScheduleDays_WorkSchedules_WorkScheduleId",
                table: "WorkScheduleDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkSchedules",
                table: "WorkSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkScheduleDays",
                table: "WorkScheduleDays");

            migrationBuilder.RenameTable(
                name: "WorkSchedules",
                newName: "WorkSchedule");

            migrationBuilder.RenameTable(
                name: "WorkScheduleDays",
                newName: "WorkScheduleDay");

            migrationBuilder.RenameIndex(
                name: "IX_WorkScheduleDays_WorkScheduleId_DayOfWeek",
                table: "WorkScheduleDay",
                newName: "IX_WorkScheduleDay_WorkScheduleId_DayOfWeek");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkSchedule",
                table: "WorkSchedule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkScheduleDay",
                table: "WorkScheduleDay",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_WorkSchedule_WorkScheduleId",
                table: "Employees",
                column: "WorkScheduleId",
                principalTable: "WorkSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkScheduleDay_WorkSchedule_WorkScheduleId",
                table: "WorkScheduleDay",
                column: "WorkScheduleId",
                principalTable: "WorkSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
