﻿// <auto-generated />
using System;
using GraphAlgorithms.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GraphAlgorithms.Repository.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231211075830_ChangedSeederForGraphClasses")]
    partial class ChangedSeederForGraphClasses
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

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

                    b.HasKey("ID");

                    b.HasIndex("ActionTypeID");

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

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.GraphClassEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("GraphClasses", (string)null);

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "Connected Graph"
                        },
                        new
                        {
                            ID = 2,
                            Name = "Unicyclic Bipartite Graph"
                        },
                        new
                        {
                            ID = 3,
                            Name = "Tree"
                        },
                        new
                        {
                            ID = 4,
                            Name = "Unicyclic Graph"
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

            modelBuilder.Entity("GraphClassEntityGraphEntity", b =>
                {
                    b.Property<int>("GraphClassesID")
                        .HasColumnType("int");

                    b.Property<int>("GraphsID")
                        .HasColumnType("int");

                    b.HasKey("GraphClassesID", "GraphsID");

                    b.HasIndex("GraphsID");

                    b.ToTable("GraphClassEntityGraphEntity");
                });

            modelBuilder.Entity("GraphAlgorithms.Repository.Entities.ActionEntity", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Entities.ActionTypeEntity", "ActionType")
                        .WithMany("Actions")
                        .HasForeignKey("ActionTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActionType");
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

            modelBuilder.Entity("GraphClassEntityGraphEntity", b =>
                {
                    b.HasOne("GraphAlgorithms.Repository.Entities.GraphClassEntity", null)
                        .WithMany()
                        .HasForeignKey("GraphClassesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GraphAlgorithms.Repository.Entities.GraphEntity", null)
                        .WithMany()
                        .HasForeignKey("GraphsID")
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
#pragma warning restore 612, 618
        }
    }
}
