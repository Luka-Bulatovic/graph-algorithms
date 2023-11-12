using GraphAlgorithms.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Text;

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

            // Seeders
            SeedTables(modelBuilder);
        }

        private void SeedTables(ModelBuilder modelBuilder)
        {
            // Seed for GraphClass
            modelBuilder.Entity<GraphClass>().HasData(
                Enum.GetValues(typeof(GraphClassEnum))
                    .Cast<GraphClassEnum>()
                    .Select(e => new GraphClass() { ID = (int)e, Name = AddSpacesToEnumName(e.ToString()) })
            );

            // Seed for ActionType
            modelBuilder.Entity<ActionType>().HasData(
                Enum.GetValues(typeof(ActionTypeEnum))
                    .Cast<ActionTypeEnum>()
                    .Select(e => new ActionType() { ID = (int)e, Name = AddSpacesToEnumName(e.ToString()) })
            );
        }

        private string AddSpacesToEnumName(string enumName)
        {
            if (string.IsNullOrEmpty(enumName))
                return string.Empty;

            var newText = new StringBuilder(enumName.Length * 2);
            newText.Append(enumName[0]);

            for (int i = 1; i < enumName.Length; i++)
            {
                if (char.IsUpper(enumName[i]))
                    newText.Append(' ');
                newText.Append(enumName[i]);
            }

            return newText.ToString();
        }
    }
}
