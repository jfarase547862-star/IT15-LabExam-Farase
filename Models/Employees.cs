using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IT15_LabExam_Farase.Models;

public class Employees
{
	[Key]
	[Display(Name = "Employee ID")]
	public int EmployeeId { get; set; }

	[Required]
	[StringLength(50)]
	[Display(Name = "First Name")]
	public string FirstName { get; set; } = string.Empty;

	[Required]
	[StringLength(50)]
	[Display(Name = "Last Name")]
	public string LastName { get; set; } = string.Empty;

	[Required]
	[StringLength(60)]
	public string Position { get; set; } = string.Empty;

	[Required]
	[StringLength(60)]
	public string Department { get; set; } = string.Empty;

	[Range(typeof(decimal), "0", "999999999.99")]
	[Column(TypeName = "decimal(18,2)")]
	[Display(Name = "Daily Rate")]
	public decimal DailyRate { get; set; }

	[NotMapped]
	public string FullName => $"{FirstName} {LastName}".Trim();

	public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
}
