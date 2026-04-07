using DailyBytes.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DailyBytes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly NewsDbContext _db;
        public UsersController(NewsDbContext db) => _db = db;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User dto)
        {
            if (string.IsNullOrWhiteSpace(dto.EmailId)) return BadRequest("Email required");
            if (await _db.Users.AnyAsync(u => u.EmailId == dto.EmailId)) return Conflict("Email already exists");
            if (await _db.Users.AnyAsync(u => u.UserName == dto.UserName)) return Conflict("Username already exists");

            _db.Users.Add(dto);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = dto.UserId }, dto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.EmailId == req.Email && u.Password == req.Password);
            if (user == null) return Unauthorized();
            return Ok(new { user.UserId, user.EmailId, user.UserName, user.FirstName, user.LastName });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.EmailId == email);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }

    public record LoginRequest(string Email, string Password);
}
