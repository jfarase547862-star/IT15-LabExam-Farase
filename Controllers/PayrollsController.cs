using IT15_LabExam_Farase.Data;
using IT15_LabExam_Farase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IT15_LabExam_Farase.Controllers;

public class PayrollsController : Controller
{
    private readonly AppDbContext _context;

    public PayrollsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? employeeId)
    {
        var payrolls = _context.Payrolls
            .Include(payroll => payroll.Employee)
            .AsNoTracking()
            .OrderByDescending(payroll => payroll.PayrollDate)
            .ThenByDescending(payroll => payroll.PayrollId)
            .AsQueryable();

        if (employeeId.HasValue)
        {
            payrolls = payrolls.Where(payroll => payroll.EmployeeId == employeeId.Value);

            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(model => model.EmployeeId == employeeId.Value);

            ViewBag.EmployeeName = employee?.FullName;
            ViewBag.SelectedEmployeeId = employeeId.Value;
        }

        ViewBag.EmployeeId = await GetEmployeeSelectListAsync(employeeId);

        return View(await payrolls.ToListAsync());
    }

    public async Task<IActionResult> Create(int? employeeId)
    {
        await PopulateEmployeeSelectListAsync(employeeId);

        return View(new Payroll
        {
            PayrollDate = DateTime.Today,
            EmployeeId = employeeId ?? 0
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Payroll payroll)
    {
        var employee = await _context.Employees.FindAsync(payroll.EmployeeId);
        if (employee is null)
        {
            ModelState.AddModelError(nameof(Payroll.EmployeeId), "Select a valid employee.");
        }

        payroll.GrossPay = payroll.DaysWorked * (employee?.DailyRate ?? 0m);
        payroll.NetPay = payroll.GrossPay - payroll.Deduction;

        if (!ModelState.IsValid)
        {
            await PopulateEmployeeSelectListAsync(payroll.EmployeeId);
            return View(payroll);
        }

        _context.Add(payroll);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { employeeId = payroll.EmployeeId });
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var payroll = await _context.Payrolls.FindAsync(id.Value);
        if (payroll is null)
        {
            return NotFound();
        }

        await PopulateEmployeeSelectListAsync(payroll.EmployeeId);
        return View(payroll);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Payroll payroll)
    {
        if (id != payroll.PayrollId)
        {
            return NotFound();
        }

        var employee = await _context.Employees.FindAsync(payroll.EmployeeId);
        if (employee is null)
        {
            ModelState.AddModelError(nameof(Payroll.EmployeeId), "Select a valid employee.");
        }

        payroll.GrossPay = payroll.DaysWorked * (employee?.DailyRate ?? 0m);
        payroll.NetPay = payroll.GrossPay - payroll.Deduction;

        if (!ModelState.IsValid)
        {
            await PopulateEmployeeSelectListAsync(payroll.EmployeeId);
            return View(payroll);
        }

        try
        {
            _context.Update(payroll);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await PayrollExists(payroll.PayrollId))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index), new { employeeId = payroll.EmployeeId });
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var payroll = await _context.Payrolls
            .Include(model => model.Employee)
            .AsNoTracking()
            .FirstOrDefaultAsync(model => model.PayrollId == id.Value);

        if (payroll is null)
        {
            return NotFound();
        }

        return View(payroll);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var payroll = await _context.Payrolls.FindAsync(id);
        if (payroll is not null)
        {
            _context.Payrolls.Remove(payroll);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateEmployeeSelectListAsync(int? selectedEmployeeId = null)
    {
        ViewBag.EmployeeId = await GetEmployeeSelectListAsync(selectedEmployeeId);
    }

    private async Task<SelectList> GetEmployeeSelectListAsync(int? selectedEmployeeId = null)
    {
        var employees = await _context.Employees
            .AsNoTracking()
            .OrderBy(employee => employee.FirstName)
            .ThenBy(employee => employee.LastName)
            .ToListAsync();

        return new SelectList(employees, nameof(Employees.EmployeeId), nameof(Employees.FullName), selectedEmployeeId);
    }

    private async Task<bool> PayrollExists(int id)
    {
        return await _context.Payrolls.AnyAsync(payroll => payroll.PayrollId == id);
    }
}