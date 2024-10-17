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

                    b.Property<int>("CreatedByID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ForGraphClassID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ActionTypeID");

                    b.HasIndex("ForGraphClassID");

                    b.ToTable("Actions", (string)null);
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

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("GraphClasses", (string)null);

                    b.HasData(
                        new
                        {
                            ID = 1,
                            CanGenerateRandomGraphs = false,
                            Name = "Connected Graph"
                        },
                        new
                        {
                            ID = 2,
                            CanGenerateRandomGraphs = false,
                            Name = "Unicyclic Bipartite Graph"
                        },
                        new
                        {
                            ID = 3,
                            CanGenerateRandomGraphs = false,
                            Name = "Tree"
                        },
                        new
                        {
                            ID = 4,
                            CanGenerateRandomGraphs = false,
                            Name = "Unicyclic Graph"
                        },
                        new
                        {
                            ID = 5,
                            CanGenerateRandomGraphs = false,
                            Name = "Bipartite Graph"
                        },
                        new
                        {
                            ID = 6,
                            CanGenerateRandomGraphs = false,
                            Name = "Acyclic Graph With Fixed Diameter"
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

                    b.HasOne("GraphAlgorithms.Repository.Entities.GraphClassEntity", "ForGraphClass")
                        .WithMany()
                        .HasForeignKey("ForGraphClassID");

                    b.Navigation("ActionType");

                    b.Navigation("ForGraphClass");
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
                    b.Navigation("GraphPropertyValues");
                });
#pragma warning restore 612, 618
        }
    }
}
