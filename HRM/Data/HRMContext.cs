using HRM.Models;
using HRM.Models.Bonus;
using HRM.Models.Payroll;
using HRM.Models.Salary;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRM.Data
{
    public class HRMContext : DbContext
    {
        public HRMContext(DbContextOptions<HRMContext> options) : base(options) { }
        public DbSet<EmpBasicInfo> empBasicInfos { get; set; }
        public DbSet<BusinessUnit> businessUnits { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<Designation> designations { get; set; }
        public DbSet<EmployementType> employementTypes { get; set; }
        public DbSet<PayrollPolicy> payrollPolicies { get; set; }
        public DbSet<PayrollElement> payrollElements { get; set; }
        public DbSet<PayrollGroupHeader> payrollGroupHeaders { get; set; }
        public DbSet<PayrollGroupRow> payrollGroupRows { get; set; }
        public DbSet<SalaryAssignHeader> salaryAssignHeaders { get; set; }
        public DbSet<SalaryAssignRow> salaryAssignRows { get; set; }
        public DbSet<SalaryAdditionNDeduction> salaryAdditionNDeductions { get; set; }
        public DbSet<EmpTransferNPromotion> empTransferNPromotions { get; set; }
        public DbSet<BonusSetup> bonusSetups { get; set; }
        public DbSet<BonusGenerate> bonusGenerates { get; set; }
    }
}
