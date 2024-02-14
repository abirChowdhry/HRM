using System.ComponentModel.DataAnnotations;

namespace HRM.Models.Bonus
{
    public class BonusSetup
    {
        [Key]
        public long IntBonusSetypId { get; set; }
        public string StrBonusSetupName { get; set; }
        public long IntBusinessUnitId { get; set; }
        public long? IntDepartmentId { get; set; }
        public long? IntServiceLengthMonths { get; set; }
        public long? IntEmployementTypeId { get; set; }
        public string? StrReligion { get; set; }
        public decimal NumPercentage { get; set; }
        public bool IsActive { get; set; }
        public long IntCreatedBy { get; set; }
        public DateTime DteCreatedAt { get; set; }
        public long? IntUpdatedBy { get; set; }
        public DateTime? DteUpdatedAt { get; set; }
    }
}
