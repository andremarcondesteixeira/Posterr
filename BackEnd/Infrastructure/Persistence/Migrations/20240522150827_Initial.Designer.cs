﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Posterr.Infrastructure.Persistence;

#nullable disable

namespace Posterr.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240522150827_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Posterr.Infrastructure.Persistence.DbEntities.PostDbEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Posterr.Infrastructure.Persistence.DbEntities.RepostDbEntity", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("PostId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("UserId", "PostId");

                    b.HasIndex("PostId");

                    b.ToTable("Reposts");
                });

            modelBuilder.Entity("Posterr.Infrastructure.Persistence.DbEntities.UserDbEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedAt = new DateTime(2024, 5, 22, 15, 8, 25, 921, DateTimeKind.Utc).AddTicks(4058),
                            Username = "simba"
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2024, 5, 22, 15, 8, 25, 921, DateTimeKind.Utc).AddTicks(4060),
                            Username = "nala"
                        },
                        new
                        {
                            Id = 3L,
                            CreatedAt = new DateTime(2024, 5, 22, 15, 8, 25, 921, DateTimeKind.Utc).AddTicks(4061),
                            Username = "timon"
                        },
                        new
                        {
                            Id = 4L,
                            CreatedAt = new DateTime(2024, 5, 22, 15, 8, 25, 921, DateTimeKind.Utc).AddTicks(4062),
                            Username = "pumbaa"
                        });
                });

            modelBuilder.Entity("Posterr.Infrastructure.Persistence.DbEntities.PostDbEntity", b =>
                {
                    b.HasOne("Posterr.Infrastructure.Persistence.DbEntities.UserDbEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Posterr.Infrastructure.Persistence.DbEntities.RepostDbEntity", b =>
                {
                    b.HasOne("Posterr.Infrastructure.Persistence.DbEntities.PostDbEntity", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Posterr.Infrastructure.Persistence.DbEntities.UserDbEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}