using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyBytes.DAL
{
    public record NewsItem
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Summary { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public DateTime PublishedAt { get; init; }
    }

    public interface IDailyBytesRepository
    {
        Task<IEnumerable<NewsItem>> GetAllAsync();
        Task<NewsItem?> GetByIdAsync(int id);
        Task<NewsItem> AddAsync(NewsItem item);
    }

    public class DailyBytesRepository : IDailyBytesRepository
    {
        private readonly List<NewsItem> _items = new();
        private int _nextId = 1;

        public DailyBytesRepository()
        {
            // Seed with sample data
            AddAsync(new NewsItem
            {
                Title = "Welcome to DailyBytes",
                Summary = "DailyBytes launches a lightweight news service.",
                Content = "This is a sample news item created during project setup.",
                PublishedAt = DateTime.UtcNow.AddHours(-1)
            }).GetAwaiter().GetResult();

            AddAsync(new NewsItem
            {
                Title = "Tech Update",
                Summary = "New C# features arrive.",
                Content = "C# continues to evolve with pattern matching and records.",
                PublishedAt = DateTime.UtcNow.AddMinutes(-30)
            }).GetAwaiter().GetResult();
        }

        public Task<NewsItem> AddAsync(NewsItem item)
        {
            var newItem = item with { Id = _nextId++ };
            _items.Add(newItem);
            return Task.FromResult(newItem);
        }

        public Task<IEnumerable<NewsItem>> GetAllAsync()
        {
            var list = _items.OrderByDescending(i => i.PublishedAt).ToList();
            return Task.FromResult<IEnumerable<NewsItem>>(list);
        }

        public Task<NewsItem?> GetByIdAsync(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            return Task.FromResult(item);
        }
    }
}
