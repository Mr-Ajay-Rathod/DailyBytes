using DailyBytes.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DailyBytes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly NewsDbContext _db;
        public FavoritesController(NewsDbContext db) => _db = db;

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] FavouriteArticle fav)
        {
            if (!await _db.Users.AnyAsync(u => u.UserId == fav.UserId)) return BadRequest("User not found");
            if (!await _db.Articles.AnyAsync(a => a.ArticleId == fav.ArticleId)) return BadRequest("Article not found");
            var exists = await _db.FavouriteArticles.AnyAsync(f => f.UserId == fav.UserId && f.ArticleId == fav.ArticleId);
            if (exists) return Conflict("Already in favourites");
            _db.FavouriteArticles.Add(fav);
            await _db.SaveChangesAsync();
            return Ok(1);
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromQuery] int userId, [FromQuery] int articleId)
        {
            var fav = await _db.FavouriteArticles.FindAsync(userId, articleId);
            if (fav == null) return NotFound();
            _db.FavouriteArticles.Remove(fav);
            await _db.SaveChangesAsync();
            return Ok(1);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var items = await _db.FavouriteArticles
                .Where(f => f.UserId == userId)
                .Include(f => f.Article)
                .ThenInclude(a => a.Category)
                .Select(f => f.Article)
                .ToListAsync();
            return Ok(items);
        }
    }
}
