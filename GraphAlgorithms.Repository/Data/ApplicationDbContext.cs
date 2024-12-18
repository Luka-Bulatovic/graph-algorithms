using GraphAlgorithms.Repository.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System.Text;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.Repository.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserEntity>
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
        public DbSet<GraphPropertyValueEntity> GraphPropertyValues { get; set; }
        public DbSet<ActionPropertyValueEntity> ActionPropertyValues { get; set; }

        #region Seeders
        private void SeedGraphClasses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GraphClassEntity>().HasData(
                new List<GraphClassEntity>()
                {
                    new GraphClassEntity()
                    {
                        ID = (int)GraphClassEnum.Connected,
                        Name = "Connected",
                        CanGenerateRandomGraphs = true,
                        HasClassifier = true,
                    },
                    new GraphClassEntity()
                    {
                        ID = (int)GraphClassEnum.UnicyclicBipartite,
                        Name = "Unicyclic Bipartite",
                        CanGenerateRandomGraphs = true,
                        HasClassifier = false,
                    },
                    new GraphClassEntity()
                    {
                        ID = (int)GraphClassEnum.Tree,
                        Name = "Tree",
                        CanGenerateRandomGraphs = false,
                        HasClassifier = true,
                    },
                    new GraphClassEntity()
                    {
                        ID = (int)GraphClassEnum.Unicyclic,
                        Name = "Unicyclic",
                        CanGenerateRandomGraphs = false,
                        HasClassifier = true,
                    },
                    new GraphClassEntity()
                    {
                        ID = (int)GraphClassEnum.Bipartite,
                        Name = "Bipartite",
                        CanGenerateRandomGraphs = false,
                        HasClassifier = true,
                    },
                    new GraphClassEntity()
                    {
                        ID = (int)GraphClassEnum.AcyclicWithFixedDiameter,
                        Name = "Acyclic With Fixed Diameter",
                        CanGenerateRandomGraphs = true,
                        HasClassifier = false,
                    },

                }
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
                    new GraphPropertyEntity() {
                        ID = (int)GraphPropertyEnum.Size,
                        Name = "Size",
                        Description = "Number of edges in graph",
                        IsGeneralDisplayProperty = true
                    },
                    new GraphPropertyEntity() {
                        ID = (int)GraphPropertyEnum.WienerIndex,
                        Name = "Wiener Index",
                        Description = "Wiener Index of a graph",
                        IsGeneralDisplayProperty = true
                    },
                    new GraphPropertyEntity() {
                        ID = (int)GraphPropertyEnum.MinNodeDegree,
                        Name = "Min. Node Degree",
                        Description = "",
                        IsGeneralDisplayProperty = true
                    },
                    new GraphPropertyEntity() {
                        ID = (int)GraphPropertyEnum.MaxNodeDegree,
                        Name = "Max. Node Degree",
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
                new { GraphPropertyID = (int)GraphPropertyEnum.Order, GraphClassID = (int)GraphClassEnum.Connected },
                new { GraphPropertyID = (int)GraphPropertyEnum.MinSizeCoef, GraphClassID = (int)GraphClassEnum.Connected },

                // Random Unicyclic Bipartite Graph
                new { GraphPropertyID = (int)GraphPropertyEnum.FirstBipartitionSize, GraphClassID = (int)GraphClassEnum.UnicyclicBipartite },
                new { GraphPropertyID = (int)GraphPropertyEnum.SecondBipartitionSize, GraphClassID = (int)GraphClassEnum.UnicyclicBipartite },
                new { GraphPropertyID = (int)GraphPropertyEnum.CycleLength, GraphClassID = (int)GraphClassEnum.UnicyclicBipartite },

                // Random Acyclic Graph with fixed diameter
                new { GraphPropertyID = (int)GraphPropertyEnum.Order, GraphClassID = (int)GraphClassEnum.AcyclicWithFixedDiameter },
                new { GraphPropertyID = (int)GraphPropertyEnum.Diameter, GraphClassID = (int)GraphClassEnum.AcyclicWithFixedDiameter }
            );
        }

        private void SeedTestUsers(ModelBuilder modelBuilder)
        {
            //AQAAAAIAAYagAAAAEPywUphZYSOThwHJm+zMnei4mrv0rF+QKd91ePCk+CeC0k3yBPDCtcYvf56LzC7fBQ==
            var user1ID = "c9b1c1ae-76aa-45cf-93e1-7b54c6446a01"; // Fixed GUID
            var user2ID = "b05a5e47-8a72-4838-a53a-2b04222858fb"; // Fixed GUID

            var user1 = new UserEntity
            {
                Id = user1ID,
                UserName = "luka",
                NormalizedUserName = "LUKA",
                Email = "luka@graphs.com",
                NormalizedEmail = "LUKA@GRAPHS.COM",
                EmailConfirmed = true,
                SecurityStamp = string.Empty,
                PasswordHash = "AQAAAAIAAYagAAAAEPywUphZYSOThwHJm+zMnei4mrv0rF+QKd91ePCk+CeC0k3yBPDCtcYvf56LzC7fBQ==" // Pre-hashed password
            };

            var user2 = new UserEntity
            {
                Id = user2ID,
                UserName = "zana",
                NormalizedUserName = "ZANA",
                Email = "zana@graphs.com",
                NormalizedEmail = "ZANA@GRAPHS.COM",
                EmailConfirmed = true,
                SecurityStamp = string.Empty,
                PasswordHash = "AQAAAAIAAYagAAAAEOsVGxeoyKOkqR1GbtTt66R1Y7sZjXvvXvh1pjjwVlzxlLQBU3NZpPWffgauQPGPYw==" // Pre-hashed password
            };

            modelBuilder.Entity<UserEntity>().HasData(new UserEntity[] { user1, user2 });
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureEntityToTableMappings(modelBuilder);
            
            SeedTables(modelBuilder);

            ConfigureManyToManyMappings(modelBuilder);

            ConfigureSpecificManyToOneMappings(modelBuilder);

            modelBuilder
                .HasDbFunction(typeof(CustomDBFunctions).GetMethod(nameof(CustomDBFunctions.ConvertToInt)))
                .HasName("ConvertToInt")
                .HasSchema("dbo");
        }

        private void ConfigureSpecificManyToOneMappings(ModelBuilder modelBuilder)
        {
            // ActionEntity to User
            modelBuilder.Entity<ActionEntity>()
                .HasOne(a => a.CreatedBy) // Navigational property for user
                .WithMany() // One user can have multiple actions
                .HasForeignKey(a => a.CreatedByID)
                .OnDelete(DeleteBehavior.Restrict); // Restrict deleting user that has actions
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


            //// Graph - GraphProperty (GraphPropertyValues table)
            modelBuilder.Entity<GraphPropertyValueEntity>(entity =>
            {
                entity.HasKey(gpv => new { gpv.GraphID, gpv.GraphPropertyID});

                entity.HasOne(gpv => gpv.Graph)
                      .WithMany(g => g.GraphPropertyValues)
                      .HasForeignKey(gpv => gpv.GraphID);

                entity.HasOne(gpv => gpv.GraphProperty)
                      .WithMany(gp => gp.GraphPropertyValues)
                      .HasForeignKey(gpv => gpv.GraphPropertyID);
            });

            //// Action - GraphProperty (for storing Properties used for random generation Action)
            modelBuilder.Entity<ActionPropertyValueEntity>(entity =>
            {
                entity.HasKey(apv => new { apv.ActionID, apv.GraphPropertyID });

                entity.HasOne(apv => apv.Action)
                      .WithMany(a => a.ActionPropertyValues)
                      .HasForeignKey(apv => apv.ActionID);

                entity.HasOne(apv => apv.GraphProperty)
                      .WithMany(gp => gp.ActionPropertyValues)
                      .HasForeignKey(apv => apv.GraphPropertyID);
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
            SeedTestUsers(modelBuilder);
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
