namespace HRM.DTOs
{
    public class BonusDTO
    {
    }

    public class BonusSetupCreateVM 
    {
        public long IntBonusSetypId { get; set; }
        public string StrBonusSetupName { get; set; }
        public long IntBusinessUnitId { get; set; }
        public long? IntDepartmentId { get; set; }
        public long? IntServiceLengthMonths { get; set; }
        public long? IntEmployementTypeId { get; set; }
        public string? StrReligion { get; set; }
        public decimal NumPercentage { get; set; }
        public long IntCreatedBy { get; set; }
        public long? IntUpdatedBy { get; set; }
    }

    public class BonusLanding 
    {
        public long IntBonusSetypId { get; set; }
        public string StrBonusSetupName { get; set; }
        public long IntBusinessUnitId { get; set; }
        public string StrBusinessUnitName { get; set; }
        public long? IntDepartmentId { get; set; }
        public string? StrDepartmentName { get; set; }
        public long? IntServiceLengthMonths { get; set; }
        public long? IntEmployementTypeId { get; set; }
        public string? StrEmployementType { get; set; }
        public string? StrReligion { get; set; }
        public decimal NumPercentage { get; set; }
        public bool IsACTIVE { get; set; }
        public long IntCreatedBy { get; set; }
        public string StrCreatedAt { get; set; }
        public long? IntUpdatedBy { get; set; }
        public string? StrUpdatedAt { get; set; }
    }


    public class BonusGenerateCreate 
    {
        public long IntBonusGenerateId { get; set; }
        public long IntBonusSetupId { get; set; }
        public long IntYearId { get; set; }
        public long IntMonthId { get; set; }
        public long IntCreatedBy { get; set; }
        public long? IntUpdatedBy { get; set; }
    }
    
    public class BonusGenerateLanding
    {
        public long IntBonusGenerateId { get; set; }
        public long IntBonusSetupId { get; set; }
        public string StrBonusSetupName { get; set; }
        public long IntBusinessUnitId { get; set; }
        public string StrBusinessUnitName { get; set; }
        public long IntYearId { get; set; }
        public long IntMonthId { get; set; }
        public long IntCreatedBy { get; set; }
        public string StrCreatedAt { get; set; }
        public long? IntUpdatedBy { get; set; }
        public string? StrUpdatedAt { get; set; }
    }

    public class TotalBonus 
    {
        public long IntBusinessUnitId { get; set; }
        public decimal NumBonus { get; set; }
    }
}
