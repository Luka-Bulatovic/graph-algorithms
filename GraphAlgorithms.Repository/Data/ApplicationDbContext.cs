using GraphAlgorithms.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphAlgorithms.Repository.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { }

        public DbSet<Graph> Graphs { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<GraphClass> GraphClasses { get; set; }
        public DbSet<Models.Action> Actions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Graph>()
                .HasMany(g => g.GraphClasses)
                .WithMany(gc => gc.Graphs);

            modelBuilder.Entity<Models.Action>()
                .HasMany(a => a.Graphs)
                .WithMany(g => g.Actions);
        }
    }
}
