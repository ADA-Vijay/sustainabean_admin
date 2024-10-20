using Microsoft.EntityFrameworkCore;
using sustainbean_api.Models;

namespace sustainbean_api
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Blog> blogs { get; set; }
        public DbSet<Tag> tbl_tag { get; set; }
        public DbSet<Feature> features { get; set; }
        public DbSet<Category> categories { get; set; }

    }
}
