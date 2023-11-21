using GraphAlgorithms.Repository.Entities;
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

        public DbSet<GraphEntity> Graphs { get; set; }
        public DbSet<ActionTypeEntity> ActionTypes { get; set; }
        public DbSet<GraphClassEntity> GraphClasses { get; set; }
        public DbSet<ActionEntity> Actions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureEntityToTableMappings(modelBuilder);

            modelBuilder.Entity<GraphEntity>()
                .HasMany(g => g.GraphClasses)
                .WithMany(gc => gc.Graphs);

            modelBuilder.Entity<ActionEntity>()
                .HasMany(a => a.Graphs)
                .WithMany(g => g.Actions);

            // Seeders
            SeedTables(modelBuilder);
        }

        private void ConfigureEntityToTableMappings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionEntity>().ToTable("Actions");
            modelBuilder.Entity<ActionTypeEntity>().ToTable("ActionTypes");
            modelBuilder.Entity<GraphClassEntity>().ToTable("GraphClasses");
            modelBuilder.Entity<GraphEntity>().ToTable("Graphs");
        }

        private void SeedTables(ModelBuilder modelBuilder)
        {
            // Seed for GraphClass
            modelBuilder.Entity<GraphClassEntity>().HasData(
                Enum.GetValues(typeof(GraphClassEnum))
                    .Cast<GraphClassEnum>()
                    .Select(e => new GraphClassEntity() { ID = (int)e, Name = AddSpacesToEnumName(e.ToString()) })
            );

            // Seed for ActionType
            modelBuilder.Entity<ActionTypeEntity>().HasData(
                Enum.GetValues(typeof(ActionTypeEnum))
                    .Cast<ActionTypeEnum>()
                    .Select(e => new ActionTypeEntity() { ID = (int)e, Name = AddSpacesToEnumName(e.ToString()) })
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
