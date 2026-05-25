using System.ComponentModel.DataAnnotations;

using HRM.Models;

namespace HRM.Models.Salary
{
    public class SalaryAdditionNDeduction : IUserOwned
    {
        [Key]
        public long IntSalaryAdditionAndDeductionId { get; set; }
        public long IntUserId { get; set; }
        public long IntBusinessUnitId { get; set; }
        public long IntEmployeeId { get; set; }
        public long? IntYear { get; set; }
        public long? IntMonth { get; set; }
        public bool? IsAddition { get; set; }
        public bool? IsDeduction { get; set; }
        public long? IntAdditionNdeductionTypeId { get; set; }
        public decimal? NumAmount { get; set; }
        public bool? IsActive { get; set; }
        public long IntCreatedBy { get; set; }
        public DateTime? DteCreatedAt { get; set; }
        public long? IntUpdatedBy { get; set; }
        public DateTime? DteUpdatedAt { get; set; }
    }
}
