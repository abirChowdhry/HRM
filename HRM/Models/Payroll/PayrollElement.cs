using System.ComponentModel.DataAnnotations;

namespace HRM.Models.Payroll
{
    public class PayrollElement
    {
        [Key]
        public long IntPayrollElementId { get; set; }
        public string? StrPayrollElementName { get; set; }
        [Required]
        public long IntBusinessUnitId { get; set; }
        public bool? IsBasicElement { get; set; }
        public bool? IsSalaryElement { get; set; }
        public bool? IsActive { get; set; }
        public DateTime DteCreatedAt { get; set; }
        public long? IntCreatedBy { get; set; }
        public DateTime? DteUpdatedAt { get; set; }
        public long? IntUpdatedBy { get; set; }
    }
}
