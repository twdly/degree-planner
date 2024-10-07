﻿// <auto-generated />
using System;
using DegreePlanner.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DegreePlanner.Migrations
{
    [DbContext(typeof(Database))]
    [Migration("20241007065004_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DegreePlanner.Data.Degree", b =>
                {
                    b.Property<int>("DegreeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DegreeId"));

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DegreeId");

                    b.ToTable("Degrees");
                });

            modelBuilder.Entity("DegreePlanner.Data.DegreeSubject", b =>
                {
                    b.Property<int>("DegreeId")
                        .HasColumnType("int");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("DegreeId", "SubjectId");

                    b.HasIndex("SubjectId");

                    b.ToTable("DegreeSubjects");
                });

            modelBuilder.Entity("DegreePlanner.Data.Major", b =>
                {
                    b.Property<int>("MajorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MajorId"));

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<int?>("DegreeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MajorId");

                    b.HasIndex("DegreeId");

                    b.ToTable("Majors");
                });

            modelBuilder.Entity("DegreePlanner.Data.MajorSubject", b =>
                {
                    b.Property<int>("MajorId")
                        .HasColumnType("int");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.HasKey("MajorId", "SubjectId");

                    b.HasIndex("SubjectId");

                    b.ToTable("MajorSubjects");
                });

            modelBuilder.Entity("DegreePlanner.Data.Prerequisite", b =>
                {
                    b.Property<int>("PrerequisiteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PrerequisiteId"));

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.HasKey("PrerequisiteId");

                    b.ToTable("Prerequisites");
                });

            modelBuilder.Entity("DegreePlanner.Data.Subject", b =>
                {
                    b.Property<int>("SubjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubjectId"));

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubjectId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("DegreePlanner.Data.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<int?>("DegreeId")
                        .HasColumnType("int");

                    b.Property<int?>("MajorId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("DegreeId");

                    b.HasIndex("MajorId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DegreePlanner.Data.UserSubject", b =>
                {
                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Mark")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("SubjectId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSubjects");
                });

            modelBuilder.Entity("PrerequisiteSubject", b =>
                {
                    b.Property<int>("PrerequisitesPrerequisiteId")
                        .HasColumnType("int");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.HasKey("PrerequisitesPrerequisiteId", "SubjectId");

                    b.HasIndex("SubjectId");

                    b.ToTable("PrerequisiteSubject");
                });

            modelBuilder.Entity("DegreePlanner.Data.DegreeSubject", b =>
                {
                    b.HasOne("DegreePlanner.Data.Degree", null)
                        .WithMany()
                        .HasForeignKey("DegreeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DegreePlanner.Data.Subject", null)
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DegreePlanner.Data.Major", b =>
                {
                    b.HasOne("DegreePlanner.Data.Degree", null)
                        .WithMany("Majors")
                        .HasForeignKey("DegreeId");
                });

            modelBuilder.Entity("DegreePlanner.Data.MajorSubject", b =>
                {
                    b.HasOne("DegreePlanner.Data.Major", null)
                        .WithMany()
                        .HasForeignKey("MajorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DegreePlanner.Data.Subject", null)
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DegreePlanner.Data.User", b =>
                {
                    b.HasOne("DegreePlanner.Data.Degree", "Degree")
                        .WithMany("Students")
                        .HasForeignKey("DegreeId");

                    b.HasOne("DegreePlanner.Data.Major", "Major")
                        .WithMany("Students")
                        .HasForeignKey("MajorId");

                    b.Navigation("Degree");

                    b.Navigation("Major");
                });

            modelBuilder.Entity("DegreePlanner.Data.UserSubject", b =>
                {
                    b.HasOne("DegreePlanner.Data.Subject", null)
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DegreePlanner.Data.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PrerequisiteSubject", b =>
                {
                    b.HasOne("DegreePlanner.Data.Prerequisite", null)
                        .WithMany()
                        .HasForeignKey("PrerequisitesPrerequisiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DegreePlanner.Data.Subject", null)
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DegreePlanner.Data.Degree", b =>
                {
                    b.Navigation("Majors");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("DegreePlanner.Data.Major", b =>
                {
                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}