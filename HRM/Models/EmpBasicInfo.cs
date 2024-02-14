using System.ComponentModel.DataAnnotations;

namespace HRM.Models
{
    public class EmpBasicInfo
    {
        [Key]
        public long IntEmployeeBasicInfoId { get; set; }
        [Required]
        public string StrEmployeeCode { get; set; }
        [Required]
        public string StrEmployeeName { get; set; }
        [Required]
        public long IntDepartmentId { get; set; }
        [Required]
        public long IntDesignationId { get; set; }
        public string? StrGender { get; set; }
        public string? StrReligion { get; set; }
        public string? StrMaritalStatus { get; set; }
        public string? StrBloodGroup { get; set; }
        public DateTime? DteDateOfBirth { get; set; }
        [Required]
        public DateTime DteJoiningDate { get; set; }
        public DateTime? DteInternCloseDate { get; set; }
        public DateTime? DteProbationaryCloseDate { get; set; }
        public DateTime? DteConfirmationDate { get; set; }
        public DateTime? DteLastWorkingDate { get; set; }
        public long? IntSupervisorId { get; set; }
        public long? IntLineManagerId { get; set; }
        public bool? IsActive { get; set; }
        [Required]
        public long IntBusinessUnitId { get; set; }
        [Required]
        public long IntEmploymentTypeId { get; set; }
        public bool? IsSalaryHold { get; set; }

        public string? StrAddress { get; set; }
        [Required]
        public DateTime DteCreatedAt { get; set; }
        public long? IntCreatedBy { get; set; }
        public DateTime? DteUpdatedAt { get; set; }
        public long? IntUpdatedBy { get; set; }
    }
}
