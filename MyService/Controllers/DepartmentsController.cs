using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyService;
using MyService.Entities;
using System.Text.Json;

[ApiController]
[Route("departments")]
public class DepartmentsController : ControllerBase
{
    private readonly ApplicationContext _context;

    public DepartmentsController(ApplicationContext context)
    {
        _context = context;
    }


    [HttpGet("{departmentId}")]
    public async Task<ActionResult<Department>> GetDepartment(int departmentId)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == departmentId);
        if (department == null)
            return NotFound();

        SaveJsonToFile(new List<Department> { department }, "department.json");

        return Ok(department);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
    {
        var departments = await _context.Departments.ToListAsync();

        if (departments == null || departments.Count == 0)
            return NotFound();


        SaveJsonToFile(departments, "departments.json");

        return Ok(departments);
    }

    private void SaveJsonToFile<T>(List<T> data, string fileName)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        System.IO.File.WriteAllText(fileName, json);
    }

    [HttpPost]
    public async Task<ActionResult<Department>> CreateDepartment(Department department)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDepartment), new { departmentId = department.Id }, department);
    }

    [HttpDelete("{departmentId}")]
    public async Task<IActionResult> DeleteDepartment(int departmentId)
    {
        var department = await _context.Departments.FindAsync(departmentId);
        if (department == null)
            return NotFound();

        var posts = _context.Posts.Where(p => p.DepartmentId == departmentId);
        var employees = _context.Employees.Where(e => e.Post.DepartmentId == departmentId);

        _context.Posts.RemoveRange(posts);
        _context.Employees.RemoveRange(employees);
        _context.Departments.Remove(department);

        await _context.SaveChangesAsync();
        return NoContent();
    }
}