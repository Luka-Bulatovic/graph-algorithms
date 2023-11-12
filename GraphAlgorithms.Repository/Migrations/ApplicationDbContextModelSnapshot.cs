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

            modelBuilder.Entity("ActionGraph", b =>
                {
                    b.Property<int>("ActionsID")
                        .HasColumnType("int");

                    b.Property<int>("GraphsID")
                        .HasColumnType("int");

                    b.HasKey("ActionsID", "GraphsID");

                    b.HasIndex("GraphsID");

                    b.ToTable("ActionGraph");
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Models.Action", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("ActionTypeID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ActionTypeID");

                    b.ToTable("Actions");
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Models.ActionType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("ActionTypes");

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

            modelBuilder.Entity("GraphAlgorithms.Repository.Models.Graph", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

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

                    b.Property<int>("WienerIndex")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Graphs");
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Models.GraphClass", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("GraphClasses");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "Tree"
                        },
                        new
                        {
                            ID = 2,
                            Name = "Unicyclic Graph"
                        });
                });

            modelBuilder.Entity("GraphGraphClass", b =>
                {
                    b.Property<int>("GraphClassesID")
                        .HasColumnType("int");

                    b.Property<int>("GraphsID")
                        .HasColumnType("int");

                    b.HasKey("GraphClassesID", "GraphsID");

                    b.HasIndex("GraphsID");

                    b.ToTable("GraphGraphClass");
                });

            modelBuilder.Entity("ActionGraph", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Models.Action", null)
                        .WithMany()
                        .HasForeignKey("ActionsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GraphAlgorithms.Repository.Models.Graph", null)
                        .WithMany()
                        .HasForeignKey("GraphsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Models.Action", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Models.ActionType", "ActionType")
                        .WithMany()
                        .HasForeignKey("ActionTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActionType");
                });

            modelBuilder.Entity("GraphGraphClass", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Models.GraphClass", null)
                        .WithMany()
                        .HasForeignKey("GraphClassesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GraphAlgorithms.Repository.Models.Graph", null)
                        .WithMany()
                        .HasForeignKey("GraphsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
