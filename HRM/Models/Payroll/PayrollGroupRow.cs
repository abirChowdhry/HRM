using System.ComponentModel.DataAnnotations;

namespace HRM.Models.Payroll
{
    public class PayrollGroupRow
    {
        [Key]
        public long IntPayrollGroupRowId { get; set; }
        public long IntPayrollHeaderId { get; set; }
        public long IntPayrollElementTypeId { get; set; }
        public string StrPayrollElementName { get; set; }
        public decimal? NumNumberOfPercent { get; set; }
        public bool IsActive { get; set; }
        public DateTime DteCreatedAt { get; set; }
        public long IntCreatedBy { get; set; }
        public DateTime? DteUpdatedAt { get; set; }
        public long? IntUpdatedBy { get; set; }
    }
}
