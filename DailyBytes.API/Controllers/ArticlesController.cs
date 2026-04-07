using DailyBytes.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DailyBytes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly NewsDbContext _db;
        public ArticlesController(NewsDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _db.Articles
                .Include(a => a.Category)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _db.Articles.Include(a => a.Category).FirstOrDefaultAsync(a => a.ArticleId == id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Article dto)
        {
            dto.CreatedDate = dto.CreatedDate == default ? DateTime.UtcNow : dto.CreatedDate;
            _db.Articles.Add(dto);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = dto.ArticleId }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Article dto)
        {
            var existing = await _db.Articles.FindAsync(id);
            if (existing == null) return NotFound();
            existing.HeadLine = dto.HeadLine;
            existing.SubHeading = dto.SubHeading;
            existing.Content = dto.Content;
            existing.CategoryId = dto.CategoryId;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _db.Articles.FindAsync(id);
            if (existing == null) return NotFound();
            _db.Articles.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
