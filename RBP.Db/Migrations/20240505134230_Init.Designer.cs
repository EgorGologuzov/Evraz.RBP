﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RBP.Db;

#nullable disable

namespace RBP.Db.Migrations
{
    [DbContext(typeof(PostgresContext))]
    [Migration("20240505134230_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RBP.Services.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RoleDataJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("RBP.Services.Models.Defect", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Defects");
                });

            modelBuilder.Entity("RBP.Services.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProfileId")
                        .HasColumnType("integer");

                    b.Property<string>("PropertiesJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SteelId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("ProfileId");

                    b.HasIndex("SteelId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("RBP.Services.Models.RailProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("RailProfiles");
                });

            modelBuilder.Entity("RBP.Services.Models.Statement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ResponsibleId")
                        .HasColumnType("uuid");

                    b.Property<int>("SegmentId")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<int>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ResponsibleId");

                    b.HasIndex("SegmentId");

                    b.HasIndex("Date", "Type", "SegmentId");

                    b.ToTable("Statements");
                });

            modelBuilder.Entity("RBP.Services.Models.StatementDefect", b =>
                {
                    b.Property<Guid>("StatementId")
                        .HasColumnType("uuid");

                    b.Property<int>("DefectId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Size")
                        .HasColumnType("numeric");

                    b.HasKey("StatementId", "DefectId");

                    b.HasIndex("DefectId");

                    b.ToTable("StatementDefects");
                });

            modelBuilder.Entity("RBP.Services.Models.SteelGrade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("SteelGrades");
                });

            modelBuilder.Entity("RBP.Services.Models.WorkshopSegment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("WorkshopSegments");
                });

            modelBuilder.Entity("RBP.Services.Models.Product", b =>
                {
                    b.HasOne("RBP.Services.Models.RailProfile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RBP.Services.Models.SteelGrade", "Steel")
                        .WithMany()
                        .HasForeignKey("SteelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Profile");

                    b.Navigation("Steel");
                });

            modelBuilder.Entity("RBP.Services.Models.Statement", b =>
                {
                    b.HasOne("RBP.Services.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RBP.Services.Models.Account", "Responsible")
                        .WithMany()
                        .HasForeignKey("ResponsibleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RBP.Services.Models.WorkshopSegment", "Segment")
                        .WithMany()
                        .HasForeignKey("SegmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Responsible");

                    b.Navigation("Segment");
                });

            modelBuilder.Entity("RBP.Services.Models.StatementDefect", b =>
                {
                    b.HasOne("RBP.Services.Models.Defect", "Defect")
                        .WithMany()
                        .HasForeignKey("DefectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RBP.Services.Models.Statement", null)
                        .WithMany("Defects")
                        .HasForeignKey("StatementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Defect");
                });

            modelBuilder.Entity("RBP.Services.Models.Statement", b =>
                {
                    b.Navigation("Defects");
                });
#pragma warning restore 612, 618
        }
    }
}
