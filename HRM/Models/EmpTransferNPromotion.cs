using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class EmpTransferNPromotion
    {
        [Key]
        public long IntEmpTransferNPromotionId { get; set; }
        public long IntEmployeeId { get; set; }
        public long IntBusinessUnitId { get; set; }
        public long? IntOldDepartmentId { get; set; }
        public long? IntNewDepartmenId { get; set;}
        public long? IntOldDesignationId { get; set; }
        public long? IntNewDesignationId { get; set ; }
        public bool? IsTransfer { get; set; }
        public bool? IsPromotion { get; set; }
        public long IntCreatedBy { get; set; }
        public DateTime DteCreatedAt { get; set; }
        public long? IntUpdatedBy { get; set;}
        public DateTime? DteUpdatedAt { get; set; }
    }
}
