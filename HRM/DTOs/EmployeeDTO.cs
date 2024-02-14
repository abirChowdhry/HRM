using System.ComponentModel.DataAnnotations;

namespace HRM.DTOs
{
    public class EmployeeDTO
    {
        
    }

    public class EmployeeLandingFilter
    {
        public long IntBusinessunitId { get; set; }
        public bool IsHeaderNeed { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public bool IsPaginated { get; set; }
        public string? searchTxt { get; set; }
        public List<long>? DesignationList { get; set; }
        public List<long>? DepartmentList { get; set; }
        public List<long>? EmployementTypeList { get; set; }
    }

    public class EmployeeCreateVM 
    {
        public long IntEmployeeBasicInfoId { get; set; }
        public string StrEmployeeCode { get; set; }
        public string StrEmployeeName { get; set; }
        public long IntDepartmentId { get; set; }
        public long IntDesignationId { get; set; }
        public string? StrGender { get; set; }
        public string? StrReligion { get; set; }
        public string? StrMaritalStatus { get; set; }
        public string? StrBloodGroup { get; set; }
        public DateTime? DteDateOfBirth { get; set; }
        public DateTime DteJoiningDate { get; set; }
        public DateTime? DteInternCloseDate { get; set; }
        public DateTime? DteProbationaryCloseDate { get; set; }
        public DateTime? DteConfirmationDate { get; set; }
        public DateTime? DteLastWorkingDate { get; set; }
        public long? IntSupervisorId { get; set; }
        public long? IntLineManagerId { get; set; }
        public long IntBusinessUnitId { get; set; }
        public long IntEmploymentTypeId { get; set; }
        public bool? IsSalaryHold { get; set; }
        public long? IntCreatedBy { get; set; }
        public long? IntUpdatedBy { get; set; }
    }

    public class EmployeeLandingPagination
    {
        public List<EmployeeLandingVM> Data { get; set; }
        public EmployeeInfoHeader employeeInfoHeader { get; set; }
        public long CurrentPage { get; set; }
        public long TotalCount { get; set; }
        public long PageSize { get; set; }

    }

    public class EmployeeLandingVM
    {
        public string StrEmployeeCode { get; set; }
        public string StrEmployeeName { get; set; }
        public long IntDepartmentId { get; set; }
        public string StrDepartmentName { get; set; }
        public long IntDesignationId { get; set; }
        public string StrDesignationName { get; set; }
        public string? StrGender { get; set; }
        public string? StrReligion { get; set; }
        public string? StrMaritalStatus { get; set; }
        public string? StrBloodGroup { get; set; }
        public string? DteDateOfBirth { get; set; }
        public DateTime DteJoiningDate { get; set; }
        public DateTime? DteConfirmationDate { get; set; }
        public DateTime? DteLastWorkingDate { get; set; }
        public string? StrSupervisorName { get; set; }
        public string? StrLineManagerName { get; set; }
        public bool? IsActive { get; set; }
        public string StrBusinessUnitName { get; set; }
        public long IntEmployementTyperId { get; set; }
        public string StrEmploymentTypeName { get; set; }
    }

    public class EmployeeInfoHeader 
    {
        public List<CommonDDL> DepartmentList { get; set; }
        public List<CommonDDL> DesignationList { get; set; }
        public List<CommonDDL> EmployementTypeList { get; set; }
    }

    public class CommonDDL 
    {
        public long Value { get; set; }
        public string Label { get; set; }
    }

    public class TransferNPromotionCreateVM 
    {
        public long IntEmpTransferNPromotionId { get; set; }
        public long IntEmployeeId { get; set; }
        public long IntBusinessUnitId { get; set; }
        public long? IntOldDepartmentId { get; set; }
        public long? IntNewDepartmenId { get; set; }
        public long? IntOldDesignationId { get; set; }
        public long? IntNewDesignationId { get; set; }
        public bool? IsTransfer { get; set; }
        public bool? IsPromotion { get; set; }
        public long IntCreatedBy { get; set; }
        public long? IntUpdatedBy { get; set; }
    }

    public class EmpTransferNPromotionLandingVM 
    {
        public long IntEmpTransferNPromotionId { get; set; }
        public long IntEmployeeId { get; set; }
        public string StrEmployeeName { get; set; }
        public long IntBusinessUnitId { get; set; }
        public string StrBusinessUnitName { get; set; }
        public long? IntOldDepartmentId { get; set; }
        public string? StrOldDepartmentName { get; set; }
        public long? IntNewDepartmenId { get; set; }
        public string? StrNewDepartmentName { get; set; }
        public long? IntOldDesignationId { get; set; }
        public string? StrOldDesignationName { get; set; }
        public long? IntNewDesignationId { get; set; }
        public string? StrNewDesignationName { get; set; }
        public bool? IsTransfer { get; set; }
        public bool? IsPromotion { get; set; }
        public long IntCreatedBy { get; set; }
        public string StrCreatedAt { get; set; }
        public long? IntUpdatedBy { get; set; }
        public string StrUpdatedAt { get; set;}
    }
}
