﻿using ModelSaber.Common;
using ModelSaber.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ModelSaber.API
{
    public class ModelSaberContext : DbContext
    {
        private readonly DatabaseSettings _databaseSettings;

        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Collection> Collections { get; set; }

        public ModelSaberContext(DatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vote>().HasOne(v => v.Voter).WithMany();
            modelBuilder.Entity<Audit>().HasOne(a => a.User).WithMany();
            modelBuilder.Entity<Model>().HasOne(m => m.Uploader).WithMany();
            modelBuilder.Entity<Model>().HasOne(m => m.Collection).WithMany();
            modelBuilder.Entity<Comment>().HasOne(c => c.Commenter).WithMany();
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Vote>().Property(v => v.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Audit>().Property(a => a.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Model>().Property(m => m.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Comment>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Playlist>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Collection>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(u => u.Profile).HasColumnType("jsonb");
            modelBuilder.Entity<Model>().Property(m => m.License).HasColumnType("jsonb");
            modelBuilder.Entity<Model>().HasMany(m => m.Playlists).WithMany(p => p.Models);
            modelBuilder.Entity<Playlist>().HasMany(p => p.Models).WithMany(m => m.Playlists);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_databaseSettings.ConnectionString);
            optionsBuilder.UseSnakeCaseNamingConvention();
        }
    }
}