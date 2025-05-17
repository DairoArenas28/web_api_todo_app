using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiTodoApp.Contexts;
using WebApiTodoApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiTodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly TodoDbContext _dbContext;
        public AccountController(TodoDbContext context) 
        {
            _dbContext = context;
        }

        // GET: api/<AccountController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAccounts()
        {
            return await _dbContext.Users.ToListAsync();
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetAccounts(int id)
        {
            var account = await _dbContext.Users.FindAsync(id);

            if (account == null) 
            { 
                return NotFound();
            }

            return account;
        }

        // POST api/<AccountController>
        [HttpPost]
        public async Task<ActionResult<User>> PostAccount(User user)
        {
            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(PostAccount), new {id = user.Id}, user);
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(user).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) 
            {
                if (!UserExists(id))
                {
                    return NotFound();
                } else
                {
                    throw;
                }
            }

            return Ok(new { message = "Cuenta actualizada correctamente." });
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult>  DeleteAccount(int id)
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
