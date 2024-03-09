﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        public DbSet<Post> Posts { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<BecomeAdminApplication> adminApplications { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }
        public DbSet<PostCommentReaction> PostCommentReactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
         builder.Entity<UserProfile>().HasMany(up => up.Posts).WithOne().HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Post>().HasMany(p => p.Comments).WithOne(c => c.Post).HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.Restrict);
          builder.Entity<PostReaction>().HasOne(r => r.User).WithMany(u => u.Reactions).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<PostComment>().HasOne(c => c.Post).WithMany(u => u.Comments).HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<PostComment>().HasMany(pc => pc.Reactions).WithOne(pcr => pcr.Comment).HasForeignKey(pc => pc.CommentId).OnDelete(DeleteBehavior.Restrict);
         builder.Entity<PostReaction>().ToTable("Reactions").HasOne(r => r.Post).WithMany(c => c.Reactions).HasForeignKey(r => r.PostId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<UserProfile>().HasMany(up => up.Comments).WithOne(pc => pc.Commenter);
            builder.Entity<Image>().ToTable("Images").HasOne(i => i.Content).WithMany(c => c.Images).HasForeignKey(i => i.ContentId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<BecomeAdminApplication>().ToTable("AdminApplications").HasOne(ada => ada.Applicant).WithOne(a=> a.AdminApplication).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<UserProfile>().HasMany(up => up.Followers).WithOne(f => f.Profile);
            builder.Entity<UserProfile>().HasMany(up => up.Friends).WithOne(f => f.Profile);
            builder.Entity<User>().HasOne(u => u.CurrentProfile).WithOne(cp => cp.User).OnDelete(DeleteBehavior.SetNull);
        }
    }
}