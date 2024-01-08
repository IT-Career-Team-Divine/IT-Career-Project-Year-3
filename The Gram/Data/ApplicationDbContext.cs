using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using The_Gram.Data.Models;

namespace The_Gram.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Image> Images { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           builder.Entity<Content>().HasOne(c => c.User).WithMany(u => u.Contents).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Reaction>().HasOne(r => r.Content).WithMany(c => c.Reactions).HasForeignKey(r => r.ContentId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Reaction>().HasOne(r => r.User).WithMany(u => u.Reactions).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Message>().HasOne(m => m.Reciever).WithMany(u => u.Messages).HasForeignKey(m => m.RecieverId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Image>().HasOne(i => i.Content).WithMany(c => c.Images).HasForeignKey(i => i.ContentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}