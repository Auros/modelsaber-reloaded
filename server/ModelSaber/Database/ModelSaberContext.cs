using System;
using ModelSaber.Models.Game;
using ModelSaber.Models.User;
using ModelSaber.Models.Model;
using ModelSaber.Models.Settings;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ModelSaber.Models.Localization;

namespace ModelSaber.Database
{
    public class ModelSaberContext : DbContext
    {
        private readonly IDatabaseSettings _databaseSettings;

        public ModelSaberContext(IDatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<ModelCollection> ModelCollections { get; set; }
        public DbSet<LocalizationDatum> LocalizationTable { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Game>().HasKey(g => g.ID);
            builder.Entity<Game>().Property(g => g.Title).IsRequired();
            builder.Entity<Game>().Property(g => g.Description).IsRequired();
            builder.Entity<Game>().Property(g => g.Created).IsRequired();

            builder.Entity<User>().HasKey(u => u.ID);
            builder.Entity<User>().Property(u => u.Permissions).HasDefaultValue(new string[] { "*.upload" });
            builder.Entity<User>().Property(u => u.ExternalProfiles).HasDefaultValue(new string[0]);

            builder.Entity<Model>().HasKey(m => m.ID);
            builder.Entity<Model>().Property(m => m.Name).IsRequired();
            builder.Entity<Model>().Property(m => m.Hash).IsRequired();
            builder.Entity<Model>().Property(m => m.Tags).HasDefaultValue(new string[0]);
            builder.Entity<Model>().Property(m => m.Preview).IsRequired();
            builder.Entity<Model>().Property(m => m.Created).IsRequired();
            builder.Entity<Model>().HasOne(m => m.Collection).WithMany(c => c.Models).HasForeignKey(m => m.CollectionID);
            builder.Entity<Model>().Property(m => m.DownloadURL).IsRequired();
            builder.Entity<Model>().Property(m => m.Type).IsRequired().HasDefaultValue(DownloadFileType.Single);

            builder.Entity<ModelCollection>().HasKey(c => c.ID);
            builder.Entity<ModelCollection>().Property(c => c.Name).IsRequired();
            builder.Entity<ModelCollection>().Property(c => c.Created).IsRequired();
            builder.Entity<ModelCollection>().Property(c => c.Description).IsRequired();
            builder.Entity<ModelCollection>().HasMany(c => c.Models).WithOne(m => m.Collection);

            builder.Entity<LocalizationDatum>().HasKey(l => l.ID);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(_databaseSettings.ConnectionString);
            options.UseSnakeCaseNamingConvention();
        }
    }
}