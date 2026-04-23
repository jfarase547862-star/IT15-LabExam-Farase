using IT15_LabExam_Farase.Models;
using Microsoft.EntityFrameworkCore;

namespace IT15_LabExam_Farase.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Employees> Employees => Set<Employees>();

    public DbSet<Payroll> Payrolls => Set<Payroll>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employees>(entity =>
        {
            entity.Property(employee => employee.DailyRate).HasPrecision(18, 2);

            entity.HasData(
                new Employees
                {
                    EmployeeId = 1,
                    FirstName = "Maria",
                    LastName = "Santos",
                    Position = "HR Officer",
                    Department = "Human Resources",
                    DailyRate = 750m
                },
                new Employees
                {
                    EmployeeId = 2,
                    FirstName = "James",
                    LastName = "Cruz",
                    Position = "IT Support",
                    Department = "Technology",
                    DailyRate = 680m
                }
            );
        });

        modelBuilder.Entity<Payroll>(entity =>
        {
            entity.Property(payroll => payroll.GrossPay).HasPrecision(18, 2);
            entity.Property(payroll => payroll.Deduction).HasPrecision(18, 2);
            entity.Property(payroll => payroll.NetPay).HasPrecision(18, 2);

            entity.HasOne(payroll => payroll.Employee)
                .WithMany(employee => employee.Payrolls)
                .HasForeignKey(payroll => payroll.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasData(
                new Payroll
                {
                    PayrollId = 1,
                    EmployeeId = 1,
                    PayrollDate = new DateTime(2026, 4, 1),
                    DaysWorked = 20,
                    GrossPay = 15000m,
                    Deduction = 500m,
                    NetPay = 14500m
                },
                new Payroll
                {
                    PayrollId = 2,
                    EmployeeId = 2,
                    PayrollDate = new DateTime(2026, 4, 1),
                    DaysWorked = 18,
                    GrossPay = 12240m,
                    Deduction = 240m,
                    NetPay = 12000m
                }
            );
        });
    }
}