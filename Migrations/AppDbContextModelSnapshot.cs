using System;
using IT15_LabExam_Farase.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace IT15_LabExam_Farase.Migrations;

[DbContext(typeof(AppDbContext))]
public partial class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "9.0.0");

        modelBuilder.Entity("IT15_LabExam_Farase.Models.Employees", b =>
        {
            b.Property<int>("EmployeeId")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            b.Property<decimal>("DailyRate")
                .HasColumnType("decimal(18,2)");

            b.Property<string>("Department")
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnType("nvarchar(60)");

            b.Property<string>("FirstName")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            b.Property<string>("LastName")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            b.Property<string>("Position")
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnType("nvarchar(60)");

            b.HasKey("EmployeeId");

            b.ToTable("Employees");

            b.HasData(
                new
                {
                    EmployeeId = 1,
                    DailyRate = 750m,
                    Department = "Human Resources",
                    FirstName = "Maria",
                    LastName = "Santos",
                    Position = "HR Officer"
                },
                new
                {
                    EmployeeId = 2,
                    DailyRate = 680m,
                    Department = "Technology",
                    FirstName = "James",
                    LastName = "Cruz",
                    Position = "IT Support"
                });
        });

        modelBuilder.Entity("IT15_LabExam_Farase.Models.Payroll", b =>
        {
            b.Property<int>("PayrollId")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            b.Property<decimal>("Deduction")
                .HasColumnType("decimal(18,2)");

            b.Property<int>("DaysWorked")
                .HasColumnType("int");

            b.Property<int>("EmployeeId")
                .HasColumnType("int");

            b.Property<decimal>("GrossPay")
                .HasColumnType("decimal(18,2)");

            b.Property<decimal>("NetPay")
                .HasColumnType("decimal(18,2)");

            b.Property<DateTime>("PayrollDate")
                .HasColumnType("date");

            b.HasKey("PayrollId");

            b.HasIndex("EmployeeId");

            b.ToTable("Payrolls");

            b.HasData(
                new
                {
                    PayrollId = 1,
                    DaysWorked = 20,
                    Deduction = 500m,
                    EmployeeId = 1,
                    GrossPay = 15000m,
                    NetPay = 14500m,
                    PayrollDate = new DateTime(2026, 4, 1)
                },
                new
                {
                    PayrollId = 2,
                    DaysWorked = 18,
                    Deduction = 240m,
                    EmployeeId = 2,
                    GrossPay = 12240m,
                    NetPay = 12000m,
                    PayrollDate = new DateTime(2026, 4, 1)
                });
        });

        modelBuilder.Entity("IT15_LabExam_Farase.Models.Payroll", b =>
        {
            b.HasOne("IT15_LabExam_Farase.Models.Employees", "Employee")
                .WithMany("Payrolls")
                .HasForeignKey("EmployeeId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Employee");
        });

        modelBuilder.Entity("IT15_LabExam_Farase.Models.Employees", b =>
        {
            b.Navigation("Payrolls");
        });
    }
}