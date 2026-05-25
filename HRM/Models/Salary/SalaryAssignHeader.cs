using System.ComponentModel.DataAnnotations;

using HRM.Models;

namespace HRM.Models.Salary
{
    public class SalaryAssignHeader : IUserOwned
    {
        [Key]
        public long IntSalaryAssignHeaderId { get; set; }
        public long IntUserId { get; set; }
        public long? IntPayrollGroupHeaderId { get; set; }
        public string StrPayrollGroupHeaderTitle { get; set; }
        public long IntEmployeeId { get; set; }
        public long IntBusinessUnitId { get; set; }
        public decimal NumGrossSalary { get; set; }
        public decimal NumNetGrossSalary { get; set; }
        public bool? IsActive { get; set; }
        public long? IntCreateBy { get; set; }
        public DateTime? DteCreateDateTime { get; set; }
        public long? IntUpdateBy { get; set; }
        public DateTime? DteUpdateDateTime { get; set; }
    }
}
