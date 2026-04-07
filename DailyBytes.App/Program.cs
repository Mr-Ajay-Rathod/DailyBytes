using System;
using System.Threading.Tasks;
using DailyBytes.DAL;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var repo = new DailyBytesRepository();

        if (args.Length == 0)
        {
            Console.WriteLine("DailyBytes - simple news console app");
            Console.WriteLine("Usage:");
            Console.WriteLine("  list           - list recent news");
            Console.WriteLine("  view <id>      - view news item by id");
            Console.WriteLine("  add            - add a sample news item");
            return 0;
        }

        var cmd = args[0].ToLowerInvariant();
        switch (cmd)
        {
            case "list":
                var items = await repo.GetAllAsync();
                foreach (var it in items)
                {
                    Console.WriteLine($"{it.Id}: {it.Title} ({it.PublishedAt:u})");
                    Console.WriteLine($"  {it.Summary}");
                }
                return 0;

            case "view":
                if (args.Length < 2 || !int.TryParse(args[1], out var id))
                {
                    Console.WriteLine("Please specify a numeric id: view <id>");
                    return 1;
                }
                var item = await repo.GetByIdAsync(id);
                if (item is null)
                {
                    Console.WriteLine("Not found");
                    return 1;
                }
                Console.WriteLine($"{item.Id}: {item.Title} ({item.PublishedAt:u})\n");
                Console.WriteLine(item.Content);
                return 0;

            case "add":
                var news = new NewsItem
                {
                    Title = "Manual item " + DateTime.UtcNow.ToString("s"),
                    Summary = "Added from CLI",
                    Content = "This news item was added by the console application.",
                    PublishedAt = DateTime.UtcNow
                };
                var added = await repo.AddAsync(news);
                Console.WriteLine($"Added item with id {added.Id}");
                return 0;

            default:
                Console.WriteLine("Unknown command");
                return 1;
        }
    }
}
