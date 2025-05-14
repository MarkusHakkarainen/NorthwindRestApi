using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly NorthwindOriginalContext _context;

        public EmployeesController(NorthwindOriginalContext context)
        {
            _context = context;
        }

        // Hae kaikki työntekijät
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            try
            {
                return Ok(await _context.Employees.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe: " + ex.Message);
            }
        }

        // Hae työntekijä pääavaimella
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound($"Työntekijää id:llä {id} ei löydy");
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe: " + ex.Message);
            }
        }

        // Hae työntekijät, joilla on tietty esihenkilö
        [HttpGet("reportsTo/{reportsToId}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesByReportsTo(int reportsToId)
        {
            try
            {
                var employees = await _context.Employees
                    .Where(e => e.ReportsTo == reportsToId)
                    .ToListAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe: " + ex.Message);
            }
        }

        // Lisää uusi työntekijä
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee([FromBody] Employee employee)
        {
            try
            {
                employee.ReportsTo = null; // Nollataan ReportsTo kenttä
                employee.Photo = null; // Nollataan Photo kenttä
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, employee);
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe: " + ex.Message);
            }
        }

        // Päivitä työntekijä
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, [FromBody] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest("Työntekijä-ID ei täsmää.");
            }

            try
            {
                _context.Entry(employee).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Työntekijä päivitetty onnistuneesti.");
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe: " + ex.Message);
            }
        }

        // Poista työntekijä
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound($"Työntekijää id:llä {id} ei löydy");
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return Ok($"Työntekijä {employee.FirstName} {employee.LastName} poistettu onnistuneesti.");
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe: " + ex.Message);
            }
        }
    }
}

