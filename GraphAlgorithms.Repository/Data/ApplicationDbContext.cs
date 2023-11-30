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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureEntityToTableMappings(modelBuilder);

            modelBuilder.Entity<GraphEntity>()
                .HasMany(g => g.GraphClasses)
                .WithMany(gc => gc.Graphs);

            modelBuilder.Entity<GraphEntity>()
                .HasOne(g => g.ActionType) // GraphEntity has one ActionType
                .WithMany() // ActionType can be associated with many GraphEntities
                .HasForeignKey(g => g.ActionTypeID);

            // Seeders
            SeedTables(modelBuilder);
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
