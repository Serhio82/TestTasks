using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyService.Entities;
using System.Text.Json;

namespace MyService.Controllers
{
    [ApiController]
    [Route("employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public EmployeesController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<Employee>> GetEmployee(int employeeId)
        {
            var employee = await _context.Employees
                .Include(e => e.Post)
                .ThenInclude(p => p.Department)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                return NotFound();

            
            SaveJsonToFile(new List<Employee> { employee }, "employee.json");

            return Ok(employee);
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _context.Employees
                .Include(e => e.Post)
                .ThenInclude(p => p.Department)
                .ToListAsync();

            if (employees == null || employees.Count == 0)
                return NotFound();

      
            SaveJsonToFile(employees, "employees.json");

            return Ok(employees);
        }

    
        private void SaveJsonToFile<T>(List<T> data, string fileName)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(fileName, json);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { employeeId = employee.Id }, employee);
        }

        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, JsonPatchDocument<Employee> patchDoc)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                return NotFound();

            patchDoc.ApplyTo(employee, error =>
            {
       
                ModelState.AddModelError(error.Operation.path, error.ErrorMessage);
            });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{employeeId}/promote")]
        public async Task<IActionResult> PromoteEmployee(int employeeId)
        {
            var employee = await _context.Employees.Include(e => e.Post).FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee == null)
                return NotFound();

            var nextPost = await _context.Posts.FirstOrDefaultAsync(p => p.Id == employee.Post.PromotionId);
            if (nextPost == null)
                return BadRequest("Promotion not available for the current position.");

            employee.PostId = nextPost.Id;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
