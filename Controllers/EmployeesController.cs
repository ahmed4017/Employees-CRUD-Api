using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Employees.Data;
using Employees.Models;
using Employees.Data;
using Employees.Models;
using Employees.Dtos;

namespace EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }


     [HttpGet]
    public async Task<ActionResult<PagedResultDto<Employee>>> GetEmployee([FromQuery] EmployeeQueryParametersDto parameters)
    {
        var query = _context.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            query = query.Where(e =>
                e.FirstName.Contains(parameters.Search) ||
                e.LastName.Contains(parameters.Search) ||
                e.Email.Contains(parameters.Search) ||
                e.Position.Contains(parameters.Search)
            );
        }

        var totalCount = await query.CountAsync();

        var employees = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        var result = new PagedResultDto<Employee>
        {
            TotalCount = totalCount,
            Data = employees
        };

        return Ok(result);
    }



    [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(CreateEmployeeDto dto)
        {
            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Position = dto.Position
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Email = dto.Email;
            employee.Position = dto.Position;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
