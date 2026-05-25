using System.ComponentModel.DataAnnotations;

using HRM.Models;

namespace HRM.Models.Payroll
{
    public class PayrollGroupHeader : IUserOwned
    {
        [Key]
        public long IntPayrollGroupHeaderId { get; set; }
        public long IntUserId { get; set; }
        public string StrPayrollGroupHeaderTitle { get; set; }
        public long IntBusinessUnitId { get; set; }
        public long? IntPayrollPolicyId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DteCreatedAt { get; set; }
        public long? IntCreatedBy { get; set; }
        public DateTime? DteUpdatedAt { get; set; }
        public long? IntUpdatedBy { get; set; }
    }
}
