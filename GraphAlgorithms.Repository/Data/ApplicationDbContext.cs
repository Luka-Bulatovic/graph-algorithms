using GraphAlgorithms.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Text;
using static GraphAlgorithms.Shared.Shared;

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
            ConfigureManyToManyMappings(modelBuilder);
            
            // Seeders
            SeedTables(modelBuilder);
        }

        private void ConfigureManyToManyMappings(ModelBuilder modelBuilder)
        {
            //// Graph - GraphClass
            modelBuilder.Entity<GraphEntity>()
                .HasMany(g => g.GraphClasses)
                .WithMany(gc => gc.Graphs)
                .UsingEntity<Dictionary<string, object>>(
                    "GraphClassGraphXRef", // Specify the join table name
                    j => j.HasOne<GraphClassEntity>() // Configure the relationship to GraphClassEntity
                        .WithMany()
                        .HasForeignKey("GraphClassID") // Specify the foreign key column name in the join table
                        .OnDelete(DeleteBehavior.Cascade), // Configure cascade delete as per your migration
                    j => j.HasOne<GraphEntity>() // Configure the relationship to GraphEntity
                        .WithMany()
                        .HasForeignKey("GraphID") // Specify the foreign key column name in the join table
                        .OnDelete(DeleteBehavior.Cascade), // Configure cascade delete as per your migration
                    j =>
                    {
                        j.HasKey("GraphClassID", "GraphID"); // Configure the composite primary key for the join table
                    });


        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => (e.Entity is GraphEntity /* Add more here if needed */)
                            && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var now = DateTime.UtcNow; // Use UtcNow for consistency across time zones
                var entity = (GraphEntity)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    // Ensure CreatedDate is not modified
                    entityEntry.Property("CreatedDate").IsModified = false;
                    entity.UpdatedDate = now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ConfigureEntityToTableMappings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionTypeEntity>().ToTable("ActionTypes");
            modelBuilder.Entity<GraphClassEntity>().ToTable("GraphClasses");
            modelBuilder.Entity<GraphEntity>().ToTable("Graphs");
            modelBuilder.Entity<ActionEntity>().ToTable("Actions");
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
