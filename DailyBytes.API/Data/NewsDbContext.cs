using Microsoft.EntityFrameworkCore;

namespace DailyBytes.API.Data
{
    public class NewsDbContext : DbContext
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Article> Articles { get; set; } = null!;
        public DbSet<Membership> Memberships { get; set; } = null!;
        public DbSet<FavouriteArticle> FavouriteArticles { get; set; } = null!;
        public DbSet<ArticleReport> ArticleReports { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(eb =>
            {
                eb.HasKey(u => u.UserId);
                eb.Property(u => u.UserId).HasColumnName("UserId");
                eb.Property(u => u.EmailId).IsRequired();
                eb.HasIndex(u => u.EmailId).IsUnique();
                eb.HasIndex(u => u.UserName).IsUnique();
            });

            modelBuilder.Entity<Category>(eb =>
            {
                eb.HasKey(c => c.CategoryId);
                eb.Property(c => c.CategoryName).IsRequired();
            });

            modelBuilder.Entity<Article>(eb =>
            {
                eb.HasKey(a => a.ArticleId);
                eb.Property(a => a.HeadLine).IsRequired();
                eb.Property(a => a.SubHeading).IsRequired();
                eb.Property(a => a.Content).IsRequired();
                eb.Property(a => a.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                eb.HasOne(a => a.Category).WithMany(c => c.Articles).HasForeignKey(a => a.CategoryId);
            });

            modelBuilder.Entity<Membership>(eb => eb.HasKey(m => m.MembershipId));

            modelBuilder.Entity<FavouriteArticle>(eb =>
            {
                eb.HasKey(f => new { f.UserId, f.ArticleId });
                eb.HasOne(f => f.User).WithMany(u => u.Favourites).HasForeignKey(f => f.UserId);
                eb.HasOne(f => f.Article).WithMany(a => a.FavouritedBy).HasForeignKey(f => f.ArticleId);
            });

            modelBuilder.Entity<ArticleReport>(eb =>
            {
                eb.HasKey(r => r.ReportId);
                eb.HasOne(r => r.Article).WithMany(a => a.Reports).HasForeignKey(r => r.ArticleId);
                eb.HasOne(r => r.User).WithMany(u => u.Reports).HasForeignKey(r => r.UserId);
            });
        }
    }

    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;

        public ICollection<FavouriteArticle> Favourites { get; set; } = new List<FavouriteArticle>();
        public ICollection<ArticleReport> Reports { get; set; } = new List<ArticleReport>();
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }

    public class Article
    {
        public int ArticleId { get; set; }
        public string HeadLine { get; set; } = string.Empty;
        public string SubHeading { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        public ICollection<FavouriteArticle> FavouritedBy { get; set; } = new List<FavouriteArticle>();
        public ICollection<ArticleReport> Reports { get; set; } = new List<ArticleReport>();
    }

    public class Membership
    {
        public int MembershipId { get; set; }
        public string MembershipName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int DurationTime { get; set; }
    }

    public class FavouriteArticle
    {
        public int UserId { get; set; }
        public User? User { get; set; }
        public int ArticleId { get; set; }
        public Article? Article { get; set; }
    }

    public class ArticleReport
    {
        public int ReportId { get; set; }
        public int ArticleId { get; set; }
        public Article? Article { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? Comments { get; set; }
        public DateTime ReportedDate { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
