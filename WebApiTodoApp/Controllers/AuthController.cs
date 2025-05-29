using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiTodoApp.Contexts;
using WebApiTodoApp.Dto.Auth;
using WebApiTodoApp.Models;
using WebApiTodoApp.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiTodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TodoDbContext _dbContext;
        private readonly TokenService _tokenService;

        public AuthController(TodoDbContext dbContext, TokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == loginDto.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            var token = _tokenService.CreateToken(user);

            return Ok(token);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            // Validar si el usuario ya existe
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == dto.UserName);
            if (existingUser != null)
            {
                return BadRequest("El nombre de usuario ya está en uso.");
            }

            // Hashear la contraseña (reemplaza esto por una función más segura si deseas)
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                UserName = dto.UserName,
                PasswordHash = passwordHash,
                Role = dto.Role
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new { user.Id, user.UserName, user.Role });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUserById(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(new { user.Id, user.UserName, user.Role });
        }

    }
}
