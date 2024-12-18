﻿// <auto-generated />
using System;
using GraphAlgorithms.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CustomGraphSetGraphXRef", b =>
                {
                    b.Property<int>("CustomGraphSetID")
                        .HasColumnType("int");

                    b.Property<int>("GraphID")
                        .HasColumnType("int");

                    b.HasKey("CustomGraphSetID", "GraphID");

                    b.HasIndex("GraphID");

                    b.ToTable("CustomGraphSetGraphXRef");
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.ActionEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("ActionTypeID")
                        .HasColumnType("int");

                    b.Property<string>("CreatedByID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ForGraphClassID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ActionTypeID");

                    b.HasIndex("CreatedByID");

                    b.HasIndex("ForGraphClassID");

                    b.ToTable("Actions", (string)null);
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.ActionPropertyValueEntity", b =>
                {
                    b.Property<int>("ActionID")
                        .HasColumnType("int");

                    b.Property<int>("GraphPropertyID")
                        .HasColumnType("int");

                    b.Property<string>("PropertyValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ActionID", "GraphPropertyID");

                    b.HasIndex("GraphPropertyID");

                    b.ToTable("ActionPropertyValues");
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.ActionTypeEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("ActionTypes", (string)null);

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "Draw"
                        },
                        new
                        {
                            ID = 2,
                            Name = "Import"
                        },
                        new
                        {
                            ID = 3,
                            Name = "Generate Random"
                        });
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.CustomGraphSetEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("CreatedByID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CreatedByID");

                    b.ToTable("CustomGraphSets");
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.GraphClassEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<bool>("CanGenerateRandomGraphs")
                        .HasColumnType("bit");

                    b.Property<bool>("HasClassifier")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("GraphClasses", (string)null);

                    b.HasData(
                        new
                        {
                            ID = 1,
                            CanGenerateRandomGraphs = true,
                            HasClassifier = true,
                            Name = "Connected"
                        },
                        new
                        {
                            ID = 2,
                            CanGenerateRandomGraphs = true,
                            HasClassifier = false,
                            Name = "Unicyclic Bipartite"
                        },
                        new
                        {
                            ID = 3,
                            CanGenerateRandomGraphs = false,
                            HasClassifier = true,
                            Name = "Tree"
                        },
                        new
                        {
                            ID = 4,
                            CanGenerateRandomGraphs = false,
                            HasClassifier = true,
                            Name = "Unicyclic"
                        },
                        new
                        {
                            ID = 5,
                            CanGenerateRandomGraphs = false,
                            HasClassifier = true,
                            Name = "Bipartite"
                        },
                        new
                        {
                            ID = 6,
                            CanGenerateRandomGraphs = true,
                            HasClassifier = false,
                            Name = "Acyclic With Fixed Diameter"
                        });
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.GraphEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("ActionID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DataXML")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("WienerIndex")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ActionID");

                    b.ToTable("Graphs", (string)null);
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.GraphPropertyEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsGeneralDisplayProperty")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("GraphProperties");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Description = "",
                            IsGeneralDisplayProperty = true,
                            Name = "Order"
                        },
                        new
                        {
                            ID = 2,
                            Description = "Minimum percentage of all possible edges",
                            IsGeneralDisplayProperty = false,
                            Name = "Min. Edges Coefficient"
                        },
                        new
                        {
                            ID = 3,
                            Description = "",
                            IsGeneralDisplayProperty = true,
                            Name = "Diameter"
                        },
                        new
                        {
                            ID = 4,
                            Description = "Length of cycle in unicyclic graph",
                            IsGeneralDisplayProperty = false,
                            Name = "Cycle Length"
                        },
                        new
                        {
                            ID = 5,
                            Description = "",
                            IsGeneralDisplayProperty = false,
                            Name = "First Partition Size"
                        },
                        new
                        {
                            ID = 6,
                            Description = "",
                            IsGeneralDisplayProperty = false,
                            Name = "Second Partition Size"
                        },
                        new
                        {
                            ID = 7,
                            Description = "",
                            IsGeneralDisplayProperty = true,
                            Name = "Radius"
                        },
                        new
                        {
                            ID = 8,
                            Description = "",
                            IsGeneralDisplayProperty = true,
                            Name = "Size/Order Ratio"
                        },
                        new
                        {
                            ID = 9,
                            Description = "Number of edges in graph",
                            IsGeneralDisplayProperty = true,
                            Name = "Size"
                        },
                        new
                        {
                            ID = 10,
                            Description = "Wiener Index of a graph",
                            IsGeneralDisplayProperty = true,
                            Name = "Wiener Index"
                        },
                        new
                        {
                            ID = 11,
                            Description = "",
                            IsGeneralDisplayProperty = true,
                            Name = "Min. Node Degree"
                        },
                        new
                        {
                            ID = 12,
                            Description = "",
                            IsGeneralDisplayProperty = true,
                            Name = "Max. Node Degree"
                        });
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.GraphPropertyValueEntity", b =>
                {
                    b.Property<int>("GraphID")
                        .HasColumnType("int");

                    b.Property<int>("GraphPropertyID")
                        .HasColumnType("int");

                    b.Property<string>("PropertyValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GraphID", "GraphPropertyID");

                    b.HasIndex("GraphPropertyID");

                    b.ToTable("GraphPropertyValues");
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.UserEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "c9b1c1ae-76aa-45cf-93e1-7b54c6446a01",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "73e1ae7a-3f32-44d3-bde2-e21103cada84",
                            Email = "luka@graphs.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "LUKA@GRAPHS.COM",
                            NormalizedUserName = "LUKA",
                            PasswordHash = "AQAAAAIAAYagAAAAEPywUphZYSOThwHJm+zMnei4mrv0rF+QKd91ePCk+CeC0k3yBPDCtcYvf56LzC7fBQ==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "",
                            TwoFactorEnabled = false,
                            UserName = "luka"
                        },
                        new
                        {
                            Id = "b05a5e47-8a72-4838-a53a-2b04222858fb",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "25bcb6ec-43f2-4184-9634-36dccf83fdc9",
                            Email = "zana@graphs.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "ZANA@GRAPHS.COM",
                            NormalizedUserName = "ZANA",
                            PasswordHash = "AQAAAAIAAYagAAAAEOsVGxeoyKOkqR1GbtTt66R1Y7sZjXvvXvh1pjjwVlzxlLQBU3NZpPWffgauQPGPYw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "",
                            TwoFactorEnabled = false,
                            UserName = "zana"
                        });
                });

            modelBuilder.Entity("GraphClassGraphXRef", b =>
                {
                    b.Property<int>("GraphClassID")
                        .HasColumnType("int");

                    b.Property<int>("GraphID")
                        .HasColumnType("int");

                    b.HasKey("GraphClassID", "GraphID");

                    b.HasIndex("GraphID");

                    b.ToTable("GraphClassGraphXRef");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("RandomGenerationGraphClassPropertyXRef", b =>
                {
                    b.Property<int>("GraphClassID")
                        .HasColumnType("int");

                    b.Property<int>("GraphPropertyID")
                        .HasColumnType("int");

                    b.HasKey("GraphClassID", "GraphPropertyID");

                    b.HasIndex("GraphPropertyID");

                    b.ToTable("RandomGenerationGraphClassPropertyXRef");

                    b.HasData(
                        new
                        {
                            GraphClassID = 1,
                            GraphPropertyID = 1
                        },
                        new
                        {
                            GraphClassID = 1,
                            GraphPropertyID = 2
                        },
                        new
                        {
                            GraphClassID = 2,
                            GraphPropertyID = 5
                        },
                        new
                        {
                            GraphClassID = 2,
                            GraphPropertyID = 6
                        },
                        new
                        {
                            GraphClassID = 2,
                            GraphPropertyID = 4
                        },
                        new
                        {
                            GraphClassID = 6,
                            GraphPropertyID = 1
                        },
                        new
                        {
                            GraphClassID = 6,
                            GraphPropertyID = 3
                        });
                });

            modelBuilder.Entity("CustomGraphSetGraphXRef", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Entities.CustomGraphSetEntity", null)
                        .WithMany()
                        .HasForeignKey("CustomGraphSetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GraphAlgorithms.Repository.Entities.GraphEntity", null)
                        .WithMany()
                        .HasForeignKey("GraphID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.ActionEntity", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Entities.ActionTypeEntity", "ActionType")
                        .WithMany("Actions")
                        .HasForeignKey("ActionTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GraphAlgorithms.Repository.Entities.UserEntity", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedByID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("GraphAlgorithms.Repository.Entities.GraphClassEntity", "ForGraphClass")
                        .WithMany()
                        .HasForeignKey("ForGraphClassID");

                    b.Navigation("ActionType");

                    b.Navigation("CreatedBy");

                    b.Navigation("ForGraphClass");
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.ActionPropertyValueEntity", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Entities.ActionEntity", "Action")
                        .WithMany("ActionPropertyValues")
                        .HasForeignKey("ActionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GraphAlgorithms.Repository.Entities.GraphPropertyEntity", "GraphProperty")
                        .WithMany("ActionPropertyValues")
                        .HasForeignKey("GraphPropertyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Action");

                    b.Navigation("GraphProperty");
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.CustomGraphSetEntity", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Entities.UserEntity", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedByID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.GraphEntity", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Entities.ActionEntity", "Action")
                        .WithMany("Graphs")
                        .HasForeignKey("ActionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Action");
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.GraphPropertyValueEntity", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Entities.GraphEntity", "Graph")
                        .WithMany("GraphPropertyValues")
                        .HasForeignKey("GraphID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GraphAlgorithms.Repository.Entities.GraphPropertyEntity", "GraphProperty")
                        .WithMany("GraphPropertyValues")
                        .HasForeignKey("GraphPropertyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Graph");

                    b.Navigation("GraphProperty");
                });

            modelBuilder.Entity("GraphClassGraphXRef", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Entities.GraphClassEntity", null)
                        .WithMany()
                        .HasForeignKey("GraphClassID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GraphAlgorithms.Repository.Entities.GraphEntity", null)
                        .WithMany()
                        .HasForeignKey("GraphID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GraphAlgorithms.Repository.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RandomGenerationGraphClassPropertyXRef", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Entities.GraphClassEntity", null)
                        .WithMany()
                        .HasForeignKey("GraphClassID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GraphAlgorithms.Repository.Entities.GraphPropertyEntity", null)
                        .WithMany()
                        .HasForeignKey("GraphPropertyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.ActionEntity", b =>
                {
                    b.Navigation("ActionPropertyValues");

                    b.Navigation("Graphs");
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.ActionTypeEntity", b =>
                {
                    b.Navigation("Actions");
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.GraphEntity", b =>
                {
                    b.Navigation("GraphPropertyValues");
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.GraphPropertyEntity", b =>
                {
                    b.Navigation("ActionPropertyValues");

                    b.Navigation("GraphPropertyValues");
                });
#pragma warning restore 612, 618
        }
    }
}
