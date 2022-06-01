using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task4.Models;

namespace Task4.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(UserConfigure);
        }

        private void UserConfigure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users").Property(p => p.Name).IsRequired().HasMaxLength(255);
            builder.ToTable("Users").Property(p => p.Email).IsRequired().HasMaxLength(255);
            builder.ToTable("Users").Property(p => p.Password).IsRequired().HasMaxLength(255);
            builder.ToTable("Users").Property(p => p.Status).HasMaxLength(1);
        }
    }
}
