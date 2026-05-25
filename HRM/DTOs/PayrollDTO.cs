using System.ComponentModel.DataAnnotations;

namespace HRM.DTOs
{
    public class PayrollDTO
    {

    }

    public class PayrollPolicyCreateVM 
    {
        public long IntPayrollPolicyId { get; set; }
        public long IntBusinessUnitId { get; set; }
        public string StrPayrollPolicyName { get; set; }
        public bool? IsSalaryDivideByActualMonthDays { get; set; }
        public long? IntGrossSalaryDevidedByDays { get; set; }
        public long? IntGrossSalaryRoundDigits { get; set; }
        public bool? IsGrossSalaryRoundUp { get; set; }
        public bool? IsGrossSalaryRoundDown { get; set; }
        public long? IntNetPayableSalaryRoundDigits { get; set; }
        public bool? IsNetPayableSalaryRoundUp { get; set; }
        public bool? IsNetPayableSalaryRoundDown { get; set; }
        public long? IntCreatedBy { get; set; }
        public long? IntUpdatedBy { get; set; }
    }

    public class PayrollPolicyLanding 
    {
        public long IntPayrollPolicyId { get; set; }
        public string StrBusinessUnitName { get; set; }
        public string StrPayrollPolicyName { get; set; }
        public bool? IsSalaryDivideByActualMonthDays { get; set; }
        public long? IntGrossSalaryDevidedByDays { get; set; }
        public long? IntGrossSalaryRoundDigits { get; set; }
        public bool? IsGrossSalaryRoundUp { get; set; }
        public bool? IsGrossSalaryRoundDown { get; set; }
        public long? IntNetPayableSalaryRoundDigits { get; set; }
        public bool? IsNetPayableSalaryRoundUp { get; set; }
        public bool? IsNetPayableSalaryRoundDown { get; set; }
        public long? IntCreatedBy { get; set; }
        public string? StrCreatedAt { get; set; }
        public long? IntUpdatedBy { get; set; }
        public string? StrUpdatedBy { get; set; }
    }

    public class PayrollElementCreateVM 
    {
        public long IntPayrollElementId { get; set; }
        public string? StrPayrollElementName { get; set; }
        public long IntBusinessUnitId { get; set; }
        public bool? IsBasicElement { get; set; }
        public bool? IsSalaryElement { get; set; }
        public long? IntCreatedBy { get; set; }
        public long? IntUpdatedBy { get; set; }
    }

    public class PayrollElementLanding 
    {
        public long IntPayrollElementId { get; set; }
        public string? StrPayrollElementName { get; set; }
        public string StrBusinessUnitName { get; set; }
        public bool? IsBasicElement { get; set; }
        public bool? IsSalaryElement { get; set; }
        public bool? IsActive { get; set; }
        public string? DteCreatedAt { get; set; }
        public long? IntCreatedBy { get; set; }
        public string? DteUpdatedAt { get; set; }
        public long? IntUpdatedBy { get; set; }
    }

    public class PayrollGroupHeaderNRowCreateVM
    {
        public long IntPayrollGroupHeaderId { get; set; }
        public string StrPayrollGroupHeaderTitle { get; set; }
        public long IntBusinessUnitId { get; set; }
        public long? IntPayrollPolicyId { get; set; }
        public long IntCreatedBy { get; set; }
        public long? IntUpdatedBy { get; set; }
        public List<PayrollGroupRowViewModel> payrollGroupRowList { get; set; }
    }

    public class PayrollGroupRowViewModel
    {
        public long IntPayrollGroupRowId { get; set; }
        public long IntPayrollElementTypeId { get; set; }
        public string StrPayrollElementName { get; set; }
        public decimal? NumNumberOfPercent { get; set; }
        public bool IsActive { get; set; }
    }

    public class PayrollHeaderLanding 
    {
        public long IntPayrollGroupHeaderId { get; set; }
        public string StrPayrollGroupHeaderTitle { get; set; }
        public string StrBusinessUnitName { get; set; }
        public string? StrPayrollPolicyName { get; set; }
        public bool IsActive { get; set; }
        public string StrCreatedAt { get; set; }
        public long? IntCreatedBy { get; set; }
        public string? StrUpdatedAt { get; set; }
        public long? IntUpdatedBy { get; set; }
    }

    public class PayrollRowLanding 
    {
        public long IntPayrollGroupRowId { get; set; }
        public long IntPayrollElementTypeId { get; set; }
        public string StrPayrollElementName { get; set; }
        public decimal? NumNumberOfPercent { get; set; }
        public bool IsActive { get; set; }
        public string StrCreatedAt { get; set; }
        public long IntCreatedBy { get; set; }
        public string? StrUpdatedAt { get; set; }
        public long? IntUpdatedBy { get; set; }
    }
}
