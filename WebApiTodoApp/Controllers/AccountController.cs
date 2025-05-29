using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApiTodoApp.Contexts;
using WebApiTodoApp.Dto.Account;
using WebApiTodoApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiTodoApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly TodoDbContext _dbContext;
        public AccountController(TodoDbContext context) 
        {
            _dbContext = context;
        }

        [Authorize(Roles="User")]
        [HttpGet("me")]
        public async Task<ActionResult<AccountDto>> GetMyAccount()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return Unauthorized(new { message = "No se pudo obtener el ID del usuario desde el token." });
            }

            var user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
                return NotFound();

            var userDto = new AccountDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.Role
            };

            return Ok(userDto);
        }

        // GET: api/<AccountController>
        [Authorize(Roles="Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts()
        {
            // Usa el método asincrónico ToListAsync() para evitar bloqueos
            var users = await _dbContext.Users.ToListAsync();

            // Mapear a AccountDto
            var usersDto = users.Select(u => new AccountDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Role = u.Role
            }).ToList();

            return Ok(usersDto);
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetAccounts(int id)
        {
            var account = await _dbContext.Users.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            var accountDto = new AccountDto
            {
                Id = account.Id,
                UserName = account.UserName,
                Role = account.Role
            };

            return Ok(accountDto);
        }

        [HttpPost]
        public async Task<ActionResult<AccountDto>> PostAccount(RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName))
            {
                return BadRequest("UserName no puede ser nulo o vacío");
            }

            if (string.IsNullOrWhiteSpace(dto.PasswordHash))
            {
                return BadRequest("Password no puede ser nulo o vacío");
            }

            // Crear el usuario con la contraseña encriptada
            var user = new User
            {
                UserName = dto.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash),
                Role = dto.Role
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var userDto = new AccountDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.Role
            };

            return CreatedAtAction(nameof(PostAccount), new { id = user.Id }, userDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, [FromBody] UpdateAccountDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.UserName))
                return BadRequest("El nombre de usuario es obligatorio.");

            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            user.UserName = dto.UserName;
            user.Role = dto.Role;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Cuenta actualizada correctamente." });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _dbContext.Users.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(account);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Cuenta eliminada correctamente." });
        }

        private bool UserExists(int id)
        {
            return _dbContext.Users.Any(e => e.Id == id);
        }
    }
}
