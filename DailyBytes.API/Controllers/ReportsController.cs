using DailyBytes.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DailyBytes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly NewsDbContext _db;
        public ReportsController(NewsDbContext db) => _db = db;

        [HttpPost]
        public async Task<IActionResult> Report([FromBody] ReportRequest req)
        {
            if (!await _db.Users.AnyAsync(u => u.UserId == req.UserId)) return BadRequest("User not found");
            if (!await _db.Articles.AnyAsync(a => a.ArticleId == req.ArticleId)) return BadRequest("Article not found");

            var report = new ArticleReport
            {
                ArticleId = req.ArticleId,
                UserId = req.UserId,
                Reason = req.Reason,
                Comments = req.Comments,
                ReportedDate = DateTime.UtcNow,
                Status = "Pending"
            };
            _db.ArticleReports.Add(report);
            await _db.SaveChangesAsync();
            return Ok(1);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.ArticleReports.Include(r => r.Article).Include(r => r.User).ToListAsync();
            return Ok(list);
        }
    }

    public record ReportRequest(int ArticleId, int UserId, string Reason, string? Comments);
}
