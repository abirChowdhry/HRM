using System.ComponentModel.DataAnnotations;

namespace HRM.Models.Salary
{
    public class SalaryAssignRow
    {
        [Key]
        public long IntSalaryAssignRowId { get; set; }
        public long IntSalaryAssignHeaderId { get; set; }
        public long IntEmployeeId { get; set; }
        public long IntPayrollElementId { get; set; }
        public string StrPayrollElement { get; set; }
        public decimal? NumNumberOfPercent { get; set; }
        public decimal NumAmount { get; set; }
        public bool? IsActive { get; set; }
        public long IntCreateBy { get; set; }
        public DateTime DteCreatedAt { get; set; }
    }
}
