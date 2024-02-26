﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TeamsMaker.Api.DataAccess.Context;

#nullable disable

namespace TeamsMaker.Api.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20240225140555_update_file_types")]
    partial class update_file_types
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

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

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Department", "lookups");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "CS",
                            IsActive = true,
                            Name = "Computer Science",
                            OrganizationId = 1
                        },
                        new
                        {
                            Id = 2,
                            Code = "IS",
                            IsActive = true,
                            Name = "Information System",
                            OrganizationId = 1
                        });
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.DepartmentStaff", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StaffId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("StaffId");

                    b.ToTable("DepartmentStaff", "dbo");
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.ImportedStaff", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<string>("SSN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ImportedStaff", "lookups");

                    b.HasData(
                        new
                        {
                            Id = new Guid("3e9f4430-2927-41eb-a8a5-099248d1e6ba"),
                            OrganizationId = 1,
                            SSN = "553-35-8652"
                        },
                        new
                        {
                            Id = new Guid("9266966b-fa8e-461a-bd61-0a1a15d5c234"),
                            OrganizationId = 1,
                            SSN = "622-45-0646"
                        });
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.ImportedStudent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CollegeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("GPA")
                        .HasColumnType("real");

                    b.Property<DateOnly>("GraduationYear")
                        .HasColumnType("date");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<string>("SSN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ImportedStudent", "lookups");

                    b.HasData(
                        new
                        {
                            Id = new Guid("5cba5edb-d6f0-4dee-85df-7f23fcbf86d3"),
                            CollegeId = "College-123",
                            Department = "CS",
                            GPA = 3.5f,
                            GraduationYear = new DateOnly(2026, 2, 17),
                            OrganizationId = 1,
                            SSN = "600-68-1014"
                        },
                        new
                        {
                            Id = new Guid("86281c15-127d-4c91-9dff-dcc24164f79b"),
                            CollegeId = "College-456",
                            Department = "IS",
                            GPA = 3.3f,
                            GraduationYear = new DateOnly(2024, 2, 17),
                            OrganizationId = 1,
                            SSN = "776-11-4808"
                        });
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.Link", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(2083)
                        .HasColumnType("nvarchar(2083)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Link", "dbo");
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Organization", "dbo");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Kafr-Elsheikh City",
                            IsActive = true
                        });
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AddedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsInvoked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshToken", "dbo");
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOrganizationAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.HasIndex("OrganizationId");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("About")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Bio")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("City")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Gender")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

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

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SSN")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("OrganizationId");

                    b.ToTable("AspNetUsers", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.Staff", b =>
                {
                    b.HasBaseType("TeamsMaker.Api.DataAccess.Models.User");

                    b.Property<int>("Classification")
                        .HasColumnType("int");

                    b.ToTable("Staff", "dbo");
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.Student", b =>
                {
                    b.HasBaseType("TeamsMaker.Api.DataAccess.Models.User");

                    b.Property<string>("CollegeId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<float>("GPA")
                        .HasColumnType("real");

                    b.Property<DateOnly?>("GraduationYear")
                        .HasColumnType("date");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Student", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("TeamsMaker.Api.DataAccess.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TeamsMaker.Api.DataAccess.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TeamsMaker.Api.DataAccess.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("TeamsMaker.Api.DataAccess.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TeamsMaker.Api.DataAccess.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TeamsMaker.Api.DataAccess.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.Department", b =>
                {
                    b.HasOne("TeamsMaker.Api.DataAccess.Models.Organization", "Organization")
                        .WithMany("Departments")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.DepartmentStaff", b =>
                {
                    b.HasOne("TeamsMaker.Api.DataAccess.Models.Department", "Department")
                        .WithMany("DepartmentStaff")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TeamsMaker.Api.DataAccess.Models.Staff", "Staff")
                        .WithMany("DepartmentStaff")
                        .HasForeignKey("StaffId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.Link", b =>
                {
                    b.HasOne("TeamsMaker.Api.DataAccess.Models.User", "User")
                        .WithMany("Links")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.Organization", b =>
                {
                    b.OwnsOne("Core.ValueObjects.TranslatableString", "Name", b1 =>
                        {
                            b1.Property<int>("OrganizationId")
                                .HasColumnType("int");

                            b1.Property<string>("Eng")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Loc")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("OrganizationId");

                            b1.ToTable("Organization", "dbo");

                            b1.WithOwner()
                                .HasForeignKey("OrganizationId");

                            b1.HasData(
                                new
                                {
                                    OrganizationId = 1,
                                    Eng = "Computers and Information Systems Kafr-Elsheikh University",
                                    Loc = "الحاسبات ونظم المعلومات جامعة كفر الشيخ"
                                });
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.RefreshToken", b =>
                {
                    b.HasOne("TeamsMaker.Api.DataAccess.Models.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.Role", b =>
                {
                    b.HasOne("TeamsMaker.Api.DataAccess.Models.Organization", "Organization")
                        .WithMany("Roles")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.User", b =>
                {
                    b.HasOne("TeamsMaker.Api.DataAccess.Models.Organization", "Organization")
                        .WithMany("Users")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("Core.ValueObjects.FileData", "Avatar", b1 =>
                        {
                            b1.Property<string>("UserId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("ContentType")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("UserId");

                            b1.ToTable("AspNetUsers");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("Core.ValueObjects.FileData", "Header", b1 =>
                        {
                            b1.Property<string>("UserId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("ContentType")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("UserId");

                            b1.ToTable("AspNetUsers");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Avatar");

                    b.Navigation("Header");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.Staff", b =>
                {
                    b.HasOne("TeamsMaker.Api.DataAccess.Models.User", null)
                        .WithOne()
                        .HasForeignKey("TeamsMaker.Api.DataAccess.Models.Staff", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.Student", b =>
                {
                    b.HasOne("TeamsMaker.Api.DataAccess.Models.Department", "Department")
                        .WithMany("Students")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TeamsMaker.Api.DataAccess.Models.User", null)
                        .WithOne()
                        .HasForeignKey("TeamsMaker.Api.DataAccess.Models.Student", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Core.ValueObjects.FileData", "CV", b1 =>
                        {
                            b1.Property<string>("StudentId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("ContentType")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("StudentId");

                            b1.ToTable("Student", "dbo");

                            b1.WithOwner()
                                .HasForeignKey("StudentId");
                        });

                    b.Navigation("CV");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.Department", b =>
                {
                    b.Navigation("DepartmentStaff");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.Organization", b =>
                {
                    b.Navigation("Departments");

                    b.Navigation("Roles");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.User", b =>
                {
                    b.Navigation("Links");

                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("TeamsMaker.Api.DataAccess.Models.Staff", b =>
                {
                    b.Navigation("DepartmentStaff");
                });
#pragma warning restore 612, 618
        }
    }
}
