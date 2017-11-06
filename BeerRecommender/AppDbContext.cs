using System.Data.Entity;

namespace BeerRecommender
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("BeerDb") {
            Database.SetInitializer(new DataInitializer(this));
        }
        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }
    }
}
