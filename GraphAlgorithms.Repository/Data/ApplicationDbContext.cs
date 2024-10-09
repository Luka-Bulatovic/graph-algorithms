using GraphAlgorithms.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        public DbSet<CustomGraphSetEntity> CustomGraphSets { get; set; }
        public DbSet<GraphPropertyEntity> GraphProperties { get; set; }

        #region Seeders
        private void SeedGraphClasses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GraphClassEntity>().HasData(
                Enum.GetValues(typeof(GraphClassEnum))
                    .Cast<GraphClassEnum>()
                    .Select(e => new GraphClassEntity() { ID = (int)e, Name = AddSpacesToEnumName(e.ToString()) })
            );
        }
        private void SeedActionTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionTypeEntity>().HasData(
                Enum.GetValues(typeof(ActionTypeEnum))
                    .Cast<ActionTypeEnum>()
                    .Select(e => new ActionTypeEntity() { ID = (int)e, Name = AddSpacesToEnumName(e.ToString()) })
            );
        }
        private void SeedGraphProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GraphPropertyEntity>().HasData(
                new List<GraphPropertyEntity>
                {
                    new GraphPropertyEntity() {
                        ID = (int)GraphPropertyEnum.Order,
                        Name = "Order",
                        Description = "",
                        IsGeneralDisplayProperty = true
                    },
                    new GraphPropertyEntity() {
                        ID = (int)GraphPropertyEnum.MinSizeCoef,
                        Name = "Min. Edges Coefficient",
                        Description = "Minimum percentage of all possible edges",
                        IsGeneralDisplayProperty = false
                    },
                    new GraphPropertyEntity() {
                        ID = (int)GraphPropertyEnum.Diameter,
                        Name = "Diameter",
                        Description = "",
                        IsGeneralDisplayProperty = true
                    },
                    new GraphPropertyEntity() {
                        ID = (int)GraphPropertyEnum.CycleLength,
                        Name = "Cycle Length",
                        Description = "Length of cycle in unicyclic graph",
                        IsGeneralDisplayProperty = false
                    },
                    new GraphPropertyEntity() {
                        ID = (int)GraphPropertyEnum.FirstBipartitionSize,
                        Name = "First Partition Size",
                        Description = "",
                        IsGeneralDisplayProperty = false
                    },
                    new GraphPropertyEntity() {
                        ID = (int)GraphPropertyEnum.SecondBipartitionSize,
                        Name = "Second Partition Size",
                        Description = "",
                        IsGeneralDisplayProperty = false
                    },
                    new GraphPropertyEntity() {
                        ID = (int)GraphPropertyEnum.Radius,
                        Name = "Radius",
                        Description = "",
                        IsGeneralDisplayProperty = true
                    },
                    new GraphPropertyEntity() {
                        ID = (int)GraphPropertyEnum.SizeToOrderRatio,
                        Name = "Size/Order Ratio",
                        Description = "",
                        IsGeneralDisplayProperty = true
                    },
                }
            );
        }
        
        private void SeedRandomGenerationGraphClassPropertyRelationship(EntityTypeBuilder j)
        {
            j.HasData(
                // Random Connected Graph
                new { GraphPropertyID = (int)GraphPropertyEnum.Order, GraphClassID = (int)GraphClassEnum.ConnectedGraph },
                new { GraphPropertyID = (int)GraphPropertyEnum.MinSizeCoef, GraphClassID = (int)GraphClassEnum.ConnectedGraph },

                // Random Unicyclic Bipartite Graph
                new { GraphPropertyID = (int)GraphPropertyEnum.FirstBipartitionSize, GraphClassID = (int)GraphClassEnum.UnicyclicBipartiteGraph },
                new { GraphPropertyID = (int)GraphPropertyEnum.SecondBipartitionSize, GraphClassID = (int)GraphClassEnum.UnicyclicBipartiteGraph },
                new { GraphPropertyID = (int)GraphPropertyEnum.CycleLength, GraphClassID = (int)GraphClassEnum.UnicyclicBipartiteGraph },

                // Random Acyclic Graph with fixed diameter
                new { GraphPropertyID = (int)GraphPropertyEnum.Order, GraphClassID = (int)GraphClassEnum.AcyclicGraphWithFixedDiameter },
                new { GraphPropertyID = (int)GraphPropertyEnum.Diameter, GraphClassID = (int)GraphClassEnum.AcyclicGraphWithFixedDiameter }
            );
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureEntityToTableMappings(modelBuilder);
            
            SeedTables(modelBuilder);

            ConfigureManyToManyMappings(modelBuilder);
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

            //// Graph - CustomGraphSet
            modelBuilder.Entity<GraphEntity>()
                .HasMany(g => g.CustomGraphSets)
                .WithMany(cgs => cgs.Graphs)
                .UsingEntity<Dictionary<string, object>>(
                    "CustomGraphSetGraphXRef", // Specify the join table name
                    j => j.HasOne<CustomGraphSetEntity>() // Configure the relationship to CustomGraphSetEntity
                        .WithMany()
                        .HasForeignKey("CustomGraphSetID") // Specify the foreign key column name in the join table
                        .OnDelete(DeleteBehavior.Cascade), // Configure cascade delete as per your migration
                    j => j.HasOne<GraphEntity>() // Configure the relationship to GraphEntity
                        .WithMany()
                        .HasForeignKey("GraphID") // Specify the foreign key column name in the join table
                        .OnDelete(DeleteBehavior.Cascade), // Configure cascade delete as per your migration
                    j =>
                    {
                        j.HasKey("CustomGraphSetID", "GraphID"); // Configure the composite primary key for the join table
                    });

            //// GraphProperty - GraphClass (Properties whose entry is required for Random Graph Generation)
            modelBuilder.Entity<GraphPropertyEntity>()
                .HasMany(g => g.RandomGenerationGraphClasses)
                .WithMany(gc => gc.RandomGenerationGraphProperties)
                .UsingEntity<Dictionary<string, object>>(
                    "RandomGenerationGraphClassPropertyXRef", // Specify the join table name
                    j => j.HasOne<GraphClassEntity>() // Configure the relationship to GraphClassEntity
                        .WithMany()
                        .HasForeignKey("GraphClassID")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<GraphPropertyEntity>() // Configure the relationship to GraphPropertyEntity
                        .WithMany()
                        .HasForeignKey("GraphPropertyID")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("GraphClassID", "GraphPropertyID"); // Configure the composite primary key for the join table

                        SeedRandomGenerationGraphClassPropertyRelationship(j);
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
            SeedGraphClasses(modelBuilder);
            SeedActionTypes(modelBuilder);
            SeedGraphProperties(modelBuilder);
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
