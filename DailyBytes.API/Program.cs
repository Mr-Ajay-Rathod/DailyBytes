using DailyBytes.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allow Angular dev server to access API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("Default") ?? "Server=(localdb)\\mssqllocaldb;Database=DailyBytesDB;Trusted_Connection=True;MultipleActiveResultSets=true";
builder.Services.AddDbContext<DailyBytes.API.Data.NewsDbContext>(opt => opt.UseSqlServer(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DailyBytes.API.Data.NewsDbContext>();
    // If using SQL Server with an existing schema, do not overwrite. Ensure DB exists and seed only when empty.
    try
    {
        db.Database.EnsureCreated();
    }
    catch
    {
        // Ignore EnsureCreated errors for existing SQL Server schemas; assume DB already provisioned.
    }
    if (!db.News.Any())
    {
        db.News.AddRange(new[]
        {
            new DailyBytes.API.Data.NewsItem
            {
                Title = "Welcome to DailyBytes",
                Summary = "DailyBytes launches a modern news platform.",
                Content = "This is a seeded news item for DailyBytes. Build your app and replace seeding with real content.",
                PublishedAt = DateTime.UtcNow.AddHours(-2)
            },
            new DailyBytes.API.Data.NewsItem
            {
                Title = "Tech Trends 2026",
                Summary = "AI, cloud and edge computing continue to grow.",
                Content = "An overview of the major trends shaping the software industry in 2026.",
                PublishedAt = DateTime.UtcNow.AddHours(-1)
            }
        });
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost4200");
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
