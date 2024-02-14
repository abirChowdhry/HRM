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
    [Migration("20240212093300_salaryGenerateCreated")]
    partial class salaryGenerateCreated
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HRM.Models.BusinessUnit", b =>
                {
                    b.Property<long>("IntBusinessUnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IntBusinessUnitId"));

                    b.Property<string>("StrBusinessUnitName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IntBusinessUnitId");

                    b.ToTable("businessUnits");
                });

            modelBuilder.Entity("HRM.Models.Department", b =>
                {
                    b.Property<long>("IntDepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IntDepartmentId"));

                    b.Property<string>("StrDepartmentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IntDepartmentId");

                    b.ToTable("departments");
                });

            modelBuilder.Entity("HRM.Models.Designation", b =>
                {
                    b.Property<long>("IntDesignationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IntDesignationId"));

                    b.Property<string>("StrDesignationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IntDesignationId");

                    b.ToTable("designations");
                });

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

            modelBuilder.Entity("HRM.Models.EmployementType", b =>
                {
                    b.Property<long>("IntEmployementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IntEmployementId"));

                    b.Property<string>("StrEmployementName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IntEmployementId");

                    b.ToTable("employementTypes");
                });

            modelBuilder.Entity("HRM.Models.Payroll.PayrollElement", b =>
                {
                    b.Property<long>("IntPayrollElementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IntPayrollElementId"));

                    b.Property<DateTime>("DteCreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DteUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("IntBusinessUnitId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntCreatedBy")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntUpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsBasicElement")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSalaryElement")
                        .HasColumnType("bit");

                    b.Property<string>("StrPayrollElementName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IntPayrollElementId");

                    b.ToTable("payrollElements");
                });

            modelBuilder.Entity("HRM.Models.Payroll.PayrollGroupHeader", b =>
                {
                    b.Property<long>("IntPayrollGroupHeaderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IntPayrollGroupHeaderId"));

                    b.Property<DateTime>("DteCreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DteUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("IntBusinessUnitId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntCreatedBy")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntPayrollPolicyId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntUpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("StrPayrollGroupHeaderTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IntPayrollGroupHeaderId");

                    b.ToTable("payrollGroupHeaders");
                });

            modelBuilder.Entity("HRM.Models.Payroll.PayrollGroupRow", b =>
                {
                    b.Property<long>("IntPayrollGroupRowId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IntPayrollGroupRowId"));

                    b.Property<DateTime>("DteCreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DteUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("IntCreatedBy")
                        .HasColumnType("bigint");

                    b.Property<long>("IntPayrollElementTypeId")
                        .HasColumnType("bigint");

                    b.Property<long>("IntPayrollHeaderId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntUpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<decimal?>("NumNumberOfPercent")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("StrPayrollElementName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IntPayrollGroupRowId");

                    b.ToTable("payrollGroupRows");
                });

            modelBuilder.Entity("HRM.Models.Payroll.PayrollPolicy", b =>
                {
                    b.Property<long>("IntPayrollPolicyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IntPayrollPolicyId"));

                    b.Property<DateTime?>("DteCreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DteUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("IntBusinessUnitId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntCreatedBy")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntGrossSalaryDevidedByDays")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntGrossSalaryRoundDigits")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntNetPayableSalaryRoundDigits")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntUpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsGrossSalaryRoundDown")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsGrossSalaryRoundUp")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsNetPayableSalaryRoundDown")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsNetPayableSalaryRoundUp")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSalaryDivideByActualMonthDays")
                        .HasColumnType("bit");

                    b.Property<string>("StrPayrollPolicyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IntPayrollPolicyId");

                    b.ToTable("payrollPolicies");
                });

            modelBuilder.Entity("HRM.Models.Salary.SalaryAdditionNDeduction", b =>
                {
                    b.Property<long>("IntSalaryAdditionAndDeductionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IntSalaryAdditionAndDeductionId"));

                    b.Property<DateTime?>("DteCreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DteUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("IntAdditionNdeductionTypeId")
                        .HasColumnType("bigint");

                    b.Property<long>("IntBusinessUnitId")
                        .HasColumnType("bigint");

                    b.Property<long>("IntCreatedBy")
                        .HasColumnType("bigint");

                    b.Property<long>("IntEmployeeId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntMonth")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntUpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntYear")
                        .HasColumnType("bigint");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsAddition")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsDeduction")
                        .HasColumnType("bit");

                    b.Property<decimal?>("NumAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IntSalaryAdditionAndDeductionId");

                    b.ToTable("salaryAdditionNDeductions");
                });

            modelBuilder.Entity("HRM.Models.Salary.SalaryAssignHeader", b =>
                {
                    b.Property<long>("IntSalaryAssignHeaderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IntSalaryAssignHeaderId"));

                    b.Property<DateTime?>("DteCreateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DteUpdateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("IntBusinessUnitId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntCreateBy")
                        .HasColumnType("bigint");

                    b.Property<long>("IntEmployeeId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntPayrollGroupHeaderId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IntUpdateBy")
                        .HasColumnType("bigint");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<decimal>("NumGrossSalary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("NumNetGrossSalary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("StrPayrollGroupHeaderTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IntSalaryAssignHeaderId");

                    b.ToTable("salaryAssignHeaders");
                });

            modelBuilder.Entity("HRM.Models.Salary.SalaryAssignRow", b =>
                {
                    b.Property<long>("IntSalaryAssignRowId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IntSalaryAssignRowId"));

                    b.Property<DateTime>("DteCreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("IntCreateBy")
                        .HasColumnType("bigint");

                    b.Property<long>("IntEmployeeId")
                        .HasColumnType("bigint");

                    b.Property<long>("IntPayrollElementId")
                        .HasColumnType("bigint");

                    b.Property<long>("IntSalaryAssignHeaderId")
                        .HasColumnType("bigint");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<decimal>("NumAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("NumNumberOfPercent")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("StrPayrollElement")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IntSalaryAssignRowId");

                    b.ToTable("salaryAssignRows");
                });

            modelBuilder.Entity("HRM.Models.Salary.SalaryGenerate", b =>
                {
                    b.Property<long>("IntSalaryGenerateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IntSalaryGenerateId"));

                    b.Property<long>("IntBusinessUnitId")
                        .HasColumnType("bigint");

                    b.Property<long>("IntMonthId")
                        .HasColumnType("bigint");

                    b.Property<long>("IntYearId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<decimal>("NumTotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IntSalaryGenerateId");

                    b.ToTable("salaryGenerates");
                });
#pragma warning restore 612, 618
        }
    }
}
