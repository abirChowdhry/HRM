﻿// <auto-generated />
using System;
using HRM.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HRM.Migrations
{
    [DbContext(typeof(HRMContext))]
    [Migration("20240210155126_empbasicCreated")]
    partial class empbasicCreated
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HRM.Models.EmpBasicInfo", b =>
                {
                    b.Property<long>("IntEmployeeBasicInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IntEmployeeBasicInfoId"));

                    b.Property<DateTime?>("DteConfirmationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DteCreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DteDateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DteInternCloseDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DteJoiningDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DteLastWorkingDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DteProbationaryCloseDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DteUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("IntBusinessUnitId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntCreatedBy")
                        .HasColumnType("bigint");

                    b.Property<long>("IntDepartmentId")
                        .HasColumnType("bigint");

                    b.Property<long>("IntDesignationId")
                        .HasColumnType("bigint");

                    b.Property<long>("IntEmploymentTypeId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntLineManagerId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntSupervisorId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntUpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("StrBloodGroup")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StrEmployeeCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StrEmployeeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StrGender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StrMaritalStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StrReligion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IntEmployeeBasicInfoId");

                    b.ToTable("empBasicInfos");
                });
#pragma warning restore 612, 618
        }
    }
}
