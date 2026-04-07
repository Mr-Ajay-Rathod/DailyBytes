using DailyBytes.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DailyBytes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly NewsDbContext _db;

        public NewsController(NewsDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _db.News.OrderByDescending(n => n.PublishedAt).ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _db.News.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewsItemDto dto)
        {
            var item = new NewsItem
            {
                Title = dto.Title,
                Summary = dto.Summary,
                Content = dto.Content,
                PublishedAt = dto.PublishedAt == default ? DateTime.UtcNow : dto.PublishedAt
            };

            _db.News.Add(item);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }
    }

    public record NewsItemDto(string Title, string Summary, string Content, DateTime PublishedAt);
}
