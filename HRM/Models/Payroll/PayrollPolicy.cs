using System.ComponentModel.DataAnnotations;

namespace HRM.Models.Payroll
{
    public class PayrollPolicy
    {
        [Key]
        public long IntPayrollPolicyId { get; set; }
        public long IntBusinessUnitId { get; set; }
        public string StrPayrollPolicyName { get; set; }
        public bool? IsSalaryDivideByActualMonthDays { get; set; }
        public long? IntGrossSalaryDevidedByDays { get; set; }
        public long? IntGrossSalaryRoundDigits { get; set; }
        public bool? IsGrossSalaryRoundUp { get; set; }
        public bool? IsGrossSalaryRoundDown { get; set; }
        public long? IntNetPayableSalaryRoundDigits { get; set; }
        public bool? IsNetPayableSalaryRoundUp { get; set; }
        public bool? IsNetPayableSalaryRoundDown { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DteCreatedAt { get; set; }
        public long? IntCreatedBy { get; set; }
        public DateTime? DteUpdatedAt { get; set; }
        public long? IntUpdatedBy { get; set; }
    }
}
