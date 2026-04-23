using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT15_LabExam_Farase.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Employees",
            columns: table => new
            {
                EmployeeId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Position = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                Department = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                DailyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Employees", x => x.EmployeeId);
            });

        migrationBuilder.CreateTable(
            name: "Payrolls",
            columns: table => new
            {
                PayrollId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                EmployeeId = table.Column<int>(type: "int", nullable: false),
                PayrollDate = table.Column<DateTime>(type: "date", nullable: false),
                DaysWorked = table.Column<int>(type: "int", nullable: false),
                GrossPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Deduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                NetPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Payrolls", x => x.PayrollId);
                table.ForeignKey(
                    name: "FK_Payrolls_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "EmployeeId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "Employees",
            columns: new[] { "EmployeeId", "DailyRate", "Department", "FirstName", "LastName", "Position" },
            values: new object[,]
            {
                { 1, 750m, "Human Resources", "Maria", "Santos", "HR Officer" },
                { 2, 680m, "Technology", "James", "Cruz", "IT Support" }
            });

        migrationBuilder.InsertData(
            table: "Payrolls",
            columns: new[] { "PayrollId", "DaysWorked", "Deduction", "EmployeeId", "GrossPay", "NetPay", "PayrollDate" },
            values: new object[,]
            {
                { 1, 20, 500m, 1, 15000m, 14500m, new DateTime(2026, 4, 1) },
                { 2, 18, 240m, 2, 12240m, 12000m, new DateTime(2026, 4, 1) }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Payrolls_EmployeeId",
            table: "Payrolls",
            column: "EmployeeId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Payrolls");

        migrationBuilder.DropTable(
            name: "Employees");
    }
}