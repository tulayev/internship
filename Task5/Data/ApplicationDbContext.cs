using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task5.Models;

namespace Task5.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)  
        {

        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MessageUser> MessageUser { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Message>(MessageConfigure);
            builder.Entity<User>(UserConfigure);
            builder.Entity<MessageUser>(MessageUserConfigure);
        }

        private void MessageConfigure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(p => p.Title).IsRequired().HasMaxLength(255);
        }
        
        private void UserConfigure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(255);
        }

        private void MessageUserConfigure(EntityTypeBuilder<MessageUser> builder)
        {
            builder.ToTable("MessageUser").HasKey(mu => new { mu.Id });
            builder.ToTable("MessageUser").HasOne(mu => mu.Message).WithMany(m => m.MessageUser).HasForeignKey(mu => mu.MessageId);
            builder.ToTable("MessageUser").HasOne(mu => mu.User).WithMany(u => u.MessageUser).HasForeignKey(mu => mu.UserId);
        }
    }
}
