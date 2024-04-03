﻿// <auto-generated />
using System;
using Hospital.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Hospital.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DepartmentDoctor", b =>
                {
                    b.Property<string>("DepartmentsDepartmentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DoctorsId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DepartmentsDepartmentId", "DoctorsId");

                    b.HasIndex("DoctorsId");

                    b.ToTable("DoctorDepartments", (string)null);
                });

            modelBuilder.Entity("Hospital.Data.Models.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Hospital.Data.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

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

                    b.HasIndex("IsDeleted");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.Department", b =>
                {
                    b.Property<string>("DepartmentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BossId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("HospitalId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DepartmentId");

                    b.HasIndex("BossId")
                        .IsUnique()
                        .HasFilter("[BossId] IS NOT NULL");

                    b.HasIndex("HospitalId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.Hospital", b =>
                {
                    b.Property<string>("HospitalId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DirectorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("HospitalId");

                    b.HasIndex("DirectorId")
                        .IsUnique()
                        .HasFilter("[DirectorId] IS NOT NULL");

                    b.ToTable("Hospitals");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.Illness", b =>
                {
                    b.Property<string>("IllnessId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IllnessId");

                    b.ToTable("Illnesses");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.IllnessPatient", b =>
                {
                    b.Property<string>("PatientId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IllnessId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PatientId", "IllnessId");

                    b.HasIndex("IllnessId");

                    b.ToTable("IllnessPatient");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.People.Director", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Adress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HospitalId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Directors");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.People.Doctor", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Adress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BossDepartmentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Qualification")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BossDepartmentId")
                        .IsUnique()
                        .HasFilter("[BossDepartmentId] IS NOT NULL");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.People.Patient", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Adress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DaysStayCount")
                        .HasColumnType("int");

                    b.Property<string>("DepartmentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DoctorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoomId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("DoctorId");

                    b.HasIndex("RoomId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.Room", b =>
                {
                    b.Property<string>("RoomId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DepartmentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoomType")
                        .HasColumnType("int");

                    b.Property<int>("RoomTypeId")
                        .HasColumnType("int");

                    b.HasKey("RoomId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Hospital.Data.Models.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted");

                    b.ToTable("Settings");
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

            modelBuilder.Entity("DepartmentDoctor", b =>
                {
                    b.HasOne("Hospital.Data.Models.Hospitals.Department", null)
                        .WithMany()
                        .HasForeignKey("DepartmentsDepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hospital.Data.Models.Hospitals.People.Doctor", null)
                        .WithMany()
                        .HasForeignKey("DoctorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.Department", b =>
                {
                    b.HasOne("Hospital.Data.Models.Hospitals.People.Doctor", "Boss")
                        .WithOne()
                        .HasForeignKey("Hospital.Data.Models.Hospitals.Department", "BossId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Hospital.Data.Models.Hospitals.Hospital", "Hospital")
                        .WithMany("Departments")
                        .HasForeignKey("HospitalId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("Boss");

                    b.Navigation("Hospital");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.Hospital", b =>
                {
                    b.HasOne("Hospital.Data.Models.Hospitals.People.Director", "Director")
                        .WithOne("Hospital")
                        .HasForeignKey("Hospital.Data.Models.Hospitals.Hospital", "DirectorId");

                    b.Navigation("Director");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.IllnessPatient", b =>
                {
                    b.HasOne("Hospital.Data.Models.Hospitals.Illness", "Illness")
                        .WithMany()
                        .HasForeignKey("IllnessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hospital.Data.Models.Hospitals.People.Patient", "Patient")
                        .WithMany("IllnessPatient")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Illness");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.People.Director", b =>
                {
                    b.HasOne("Hospital.Data.Models.ApplicationUser", null)
                        .WithOne("Director")
                        .HasForeignKey("Hospital.Data.Models.Hospitals.People.Director", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.People.Doctor", b =>
                {
                    b.HasOne("Hospital.Data.Models.Hospitals.Department", "BossDepartment")
                        .WithOne()
                        .HasForeignKey("Hospital.Data.Models.Hospitals.People.Doctor", "BossDepartmentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Hospital.Data.Models.ApplicationUser", null)
                        .WithOne("Doctor")
                        .HasForeignKey("Hospital.Data.Models.Hospitals.People.Doctor", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BossDepartment");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.People.Patient", b =>
                {
                    b.HasOne("Hospital.Data.Models.Hospitals.Department", null)
                        .WithMany("Patients")
                        .HasForeignKey("DepartmentId");

                    b.HasOne("Hospital.Data.Models.Hospitals.People.Doctor", "Doctor")
                        .WithMany("Patients")
                        .HasForeignKey("DoctorId");

                    b.HasOne("Hospital.Data.Models.ApplicationUser", null)
                        .WithOne("Patient")
                        .HasForeignKey("Hospital.Data.Models.Hospitals.People.Patient", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hospital.Data.Models.Hospitals.Room", "Room")
                        .WithMany("Patients")
                        .HasForeignKey("RoomId");

                    b.Navigation("Doctor");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.Room", b =>
                {
                    b.HasOne("Hospital.Data.Models.Hospitals.Department", "Department")
                        .WithMany("Rooms")
                        .HasForeignKey("DepartmentId");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Hospital.Data.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Hospital.Data.Models.ApplicationUser", null)
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Hospital.Data.Models.ApplicationUser", null)
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Hospital.Data.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Hospital.Data.Models.ApplicationUser", null)
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Hospital.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Hospital.Data.Models.ApplicationUser", b =>
                {
                    b.Navigation("Claims");

                    b.Navigation("Director");

                    b.Navigation("Doctor");

                    b.Navigation("Logins");

                    b.Navigation("Patient");

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.Department", b =>
                {
                    b.Navigation("Patients");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.Hospital", b =>
                {
                    b.Navigation("Departments");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.People.Director", b =>
                {
                    b.Navigation("Hospital");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.People.Doctor", b =>
                {
                    b.Navigation("Patients");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.People.Patient", b =>
                {
                    b.Navigation("IllnessPatient");
                });

            modelBuilder.Entity("Hospital.Data.Models.Hospitals.Room", b =>
                {
                    b.Navigation("Patients");
                });
#pragma warning restore 612, 618
        }
    }
}
