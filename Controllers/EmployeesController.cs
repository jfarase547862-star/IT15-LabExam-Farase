using IT15_LabExam_Farase.Data;
using IT15_LabExam_Farase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IT15_LabExam_Farase.Controllers;

public class EmployeesController : Controller
{
    private readonly AppDbContext _context;

    public EmployeesController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _context.Employees
            .AsNoTracking()
            .OrderBy(employee => employee.FirstName)
            .ThenBy(employee => employee.LastName)
            .ToListAsync();

        return View(employees);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employees employee)
    {
        if (!ModelState.IsValid)
        {
            return View(employee);
        }

        _context.Add(employee);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var employee = await _context.Employees.FindAsync(id.Value);
        if (employee is null)
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Employees employee)
    {
        if (id != employee.EmployeeId)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(employee);
        }

        _context.Update(employee);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await EmployeeExists(employee.EmployeeId))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var employee = await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(model => model.EmployeeId == id.Value);

        if (employee is null)
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee is not null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> EmployeeExists(int id)
    {
        return await _context.Employees.AnyAsync(employee => employee.EmployeeId == id);
    }
}