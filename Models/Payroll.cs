using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IT15_LabExam_Farase.Models;

public class Payroll
{
    [Key]
    [Display(Name = "Payroll ID")]
    public int PayrollId { get; set; }

    [Required]
    [Display(Name = "Employee ID")]
    public int EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employees? Employee { get; set; }

    [DataType(DataType.Date)]
    [Column(TypeName = "date")]
    [Display(Name = "Payroll Date")]
    public DateTime PayrollDate { get; set; } = DateTime.Today;

    [Range(0, 31)]
    [Display(Name = "Days Worked")]
    public int DaysWorked { get; set; }

    [Range(typeof(decimal), "0", "999999999.99")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal GrossPay { get; set; }

    [Range(typeof(decimal), "0", "999999999.99")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Deduction { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal NetPay { get; set; }
}