using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApiTodoApp.Contexts;
using WebApiTodoApp.Dto.Assignment;
using WebApiTodoApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiTodoApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        public readonly TodoDbContext _dbContext;
        public AssignmentsController(TodoDbContext context) {
            _dbContext = context;
        }

        // GET: api/<AssignmentsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAssignments()
        {
            var assignments = await _dbContext.Assignments.ToListAsync();
            var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimValue) || !int.TryParse(claimValue, out int userId))
            {
                return Unauthorized("No se pudo obtener el identificador del usuario.");
            }

            var assignmentsDto = assignments.Where(a => a.UserId == userId).ToList();

            return Ok(assignmentsDto);
        }

        // GET api/<AssignmentsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssignmentDto>> GetAssignment(int id)
        {
            var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimValue) || !int.TryParse(claimValue, out int userId))
            {
                return Unauthorized("No se pudo obtener el identificador del usuario.");
            }

            var assignment = await _dbContext.Assignments
                                    .Where(a => a.Id == id && a.UserId == userId)
                                    .FirstOrDefaultAsync();

            if (assignment == null)
                return NotFound();

            var dto = new AssignmentDto
            {
                Id = assignment.Id,
                Name = assignment.Name,
                Description = assignment.Description,
                StatusId = assignment.StateId,
                UserId = assignment.UserId
            };

            return Ok(dto);
        }

        // GET api/<AssignmentsController>/5
        [Authorize(Roles = "Admin")]
        [HttpGet("user/{id}")]
        public async Task<ActionResult<AssignmentDto>> GetAssignmentWithUserId(int id)
        {
            //var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var assignment = await _dbContext.Assignments
                                    .Where(a => a.UserId == id)
                                    .FirstOrDefaultAsync();

            if (assignment == null)
                return NotFound();

            var dto = new AssignmentDto
            {
                Id = assignment.Id,
                Name = assignment.Name,
                Description = assignment.Description,
                StatusId = assignment.StateId,
                CategoryId = assignment.CategoryId,
                UserId = assignment.UserId
            };

            return Ok(dto);
        }

        // POST api/<AssignmentsController>
        [HttpPost]
        public async Task<ActionResult<AssignmentDto>> CreateAssignment(AssignmentDto dto)
        {
            var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimValue) || !int.TryParse(claimValue, out int userId))
            {
                return Unauthorized("No se pudo obtener el identificador del usuario.");
            }

            var assignment = new Assignment
            {
                Name = dto.Name,
                Description = dto.Description,
                StateId = dto.StatusId,
                CategoryId = dto.CategoryId,
                UserId = userId
            };

            _dbContext.Assignments.Add(assignment);
            await _dbContext.SaveChangesAsync();

            dto.Id = assignment.Id;
            dto.UserId = userId;

            return CreatedAtAction(nameof(GetAssignment), new { id = assignment.Id }, dto);
        }

        // PUT api/<AssignmentsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssignment(int id, AssignmentDto dto)
        {
            var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(claimValue, out int userId))
                return Unauthorized("No se pudo obtener el identificador del usuario.");

            if (id != dto.Id)
                return BadRequest("ID no coincide.");

            var assignment = await _dbContext.Assignments
                                    .Where(a => a.Id == id && a.UserId == userId)
                                    .FirstOrDefaultAsync();

            if (assignment == null)
                return NotFound();

            assignment.Name = dto.Name;
            assignment.Description = dto.Description;
            assignment.CategoryId = dto.CategoryId;
            assignment.StateId = dto.StatusId;

            _dbContext.Entry(assignment).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(id))
                    return NotFound();
                else
                    throw;
            }

            return Ok(new { message = "Asignación actualizada correctamente." });
        }

        // DELETE api/<AssignmentsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(claimValue, out int userId))
                return Unauthorized("No se pudo obtener el identificador del usuario.");

            var assignment = await _dbContext.Assignments
                                    .Where(a => a.Id == id && a.UserId == userId)
                                    .FirstOrDefaultAsync();

            if (assignment == null)
                return NotFound();

            _dbContext.Assignments.Remove(assignment);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Asignación eliminada correctamente." });
        }

        private bool AssignmentExists(int id)
        {
            return _dbContext.Assignments.Any(e => e.Id == id);
        }

        private int? GetUserId()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return int.TryParse(claim?.Value, out int id) ? id : null;
        }

    }
}
