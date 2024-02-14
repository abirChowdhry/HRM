using System.ComponentModel.DataAnnotations;

namespace HRM.Models.Bonus
{
    public class BonusGenerate
    {
        [Key]
        public long IntBonusGenerateId { get; set; }
        public long IntBonusSetupId { get; set; }
        public long IntYearId { get; set; }
        public long IntMonthId { get; set; }
        public bool IsActive { get; set; }
        public long IntCreatedBy { get; set; }
        public DateTime DteCreatedAt { get; set; }
        public long? IntUpdatedBy { get; set; }
        public DateTime? DteUpdatedAt { get; set; }
    }
}
