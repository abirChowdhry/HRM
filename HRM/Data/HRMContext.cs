using HRM.Models;
using HRM.Models.Bonus;
using HRM.Models.Payroll;
using HRM.Models.Salary;
using HRM.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRM.Data
{
    // Central EF Core context. Global filters keep each logged-in user's HR data isolated.
    public class HRMContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public HRMContext(DbContextOptions<HRMContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<AppUser> appUsers { get; set; }
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().HasIndex(x => x.StrEmail).IsUnique();

            // Every user-owned table is filtered automatically by the current JWT user id.
            modelBuilder.Entity<BusinessUnit>().HasQueryFilter(x => x.IntUserId == _currentUserService.UserId);
            modelBuilder.Entity<Department>().HasQueryFilter(x => x.IntUserId == _currentUserService.UserId);
            modelBuilder.Entity<Designation>().HasQueryFilter(x => x.IntUserId == _currentUserService.UserId);
            modelBuilder.Entity<EmployementType>().HasQueryFilter(x => x.IntUserId == _currentUserService.UserId);
            modelBuilder.Entity<EmpBasicInfo>().HasQueryFilter(x => x.IntUserId == _currentUserService.UserId);
            modelBuilder.Entity<EmpTransferNPromotion>().HasQueryFilter(x => x.IntUserId == _currentUserService.UserId);
            modelBuilder.Entity<PayrollPolicy>().HasQueryFilter(x => x.IntUserId == _currentUserService.UserId);
            modelBuilder.Entity<PayrollElement>().HasQueryFilter(x => x.IntUserId == _currentUserService.UserId);
            modelBuilder.Entity<PayrollGroupHeader>().HasQueryFilter(x => x.IntUserId == _currentUserService.UserId);
            modelBuilder.Entity<PayrollGroupRow>().HasQueryFilter(x => x.IntUserId == _currentUserService.UserId);
            modelBuilder.Entity<SalaryAssignHeader>().HasQueryFilter(x => x.IntUserId == _currentUserService.UserId);
            modelBuilder.Entity<SalaryAssignRow>().HasQueryFilter(x => x.IntUserId == _currentUserService.UserId);
            modelBuilder.Entity<SalaryAdditionNDeduction>().HasQueryFilter(x => x.IntUserId == _currentUserService.UserId);
            modelBuilder.Entity<BonusSetup>().HasQueryFilter(x => x.IntUserId == _currentUserService.UserId);
            modelBuilder.Entity<BonusGenerate>().HasQueryFilter(x => x.IntUserId == _currentUserService.UserId);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            SetOwnedUserIds();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetOwnedUserIds();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetOwnedUserIds()
        {
            // New records receive ownership here, so services do not need to set IntUserId manually.
            if (_currentUserService.IsAuthenticated == false)
            {
                return;
            }

            foreach (var entry in ChangeTracker.Entries<IUserOwned>().Where(x => x.State == EntityState.Added && x.Entity.IntUserId == 0))
            {
                entry.Entity.IntUserId = _currentUserService.UserId;
            }
        }
    }
}
