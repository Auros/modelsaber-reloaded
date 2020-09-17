using ModelSaber.API.Models;
using Microsoft.EntityFrameworkCore;
using ModelSaber.Common;

namespace ModelSaber.API
{
    public class ModelSaberContext : DbContext
    {
        private readonly DatabaseSettings _databaseSettings;

        public DbSet<User> Users { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Collection> Collections { get; set; }

        public ModelSaberContext(DatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_databaseSettings.ConnectionString).UseSnakeCaseNamingConvention();
        }
    }
}