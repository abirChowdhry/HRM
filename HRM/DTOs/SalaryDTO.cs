using System.Globalization;

namespace HRM.DTOs
{
    public class SalaryDTO
    {
    }

    public class SalaryAssignLanding 
    {
        public long IntEmployeeId { get; set; }
        public string StrBusinessUnitName { get; set; }
        public string StrEmployeeName { get; set; }
        public string StrDepartmentName { get; set;}
        public string StrDesignationName { get; set;}

    }
    
    public class SalaryDetailsLanding 
    {
        public long IntEmployeeId { get; set; }
        public string StrBusinessUnitName { get; set; }
        public string StrEmployeeName { get; set; }
        public string StrDepartmentName { get; set;}
        public string StrDesignationName { get; set;}
        public decimal NumGrossSalary { get; set;}
        public long IntPayrollGroupHeader { get; set;}
        public string StrPayrollGroupHeader { get; set; }

    }

    public class SalaryAssignVM 
    {
        public long IntSalaryAssignHeaderId { get; set; }
        public long? IntPayrollGroupHeaderId { get; set; }
        public long IntEmployeeId { get; set; }
        public long IntBusinessUnitId { get; set; }
        public decimal NumGrossSalary { get; set; }
        public decimal NumNetGrossSalary { get; set; }
        public long IntCreateBy { get; set; }
        public long? IntUpdateBy { get; set; }
    }

    public class SalaryAdditionNDeductionVM 
    {
        public long IntSalaryAdditionAndDeductionId { get; set; }
        public long IntBusinessUnitId { get; set; }
        public long IntEmployeeId { get; set; }
        public long? IntYear { get; set; }
        public long? IntMonth { get; set; }
        public bool? IsAddition { get; set; }
        public bool? IsDeduction { get; set; }
        public long? IntAdditionNdeductionTypeId { get; set; }
        public decimal? NumAmount { get; set; }
        public long IntCreatedBy { get; set; }
        public long? IntUpdatedBy { get; set; }
    }

    public class EmpSalAddNDeductionVM 
    {
        public long IntEmployeeId { get; set;}
        public string StrEmployeeName { get; set; }
        public List<SalAddition> Addition { get; set; }
        public List<SalDeduction> Deduction { get; set; }
        public long? IntYear { get; set;}
        public long? IntMonth { get; set;}
    }

    public class SalAddition 
    {
        public long? IntAddDeductTypeId { get; set; }
        public string StrAdddeductionName { get; set;}
        public decimal NumAmount { get; set; }
    }
    
    public class SalDeduction
    {
        public long? IntAddDeductTypeId { get; set; }
        public string StrAdddeductionName { get; set;}
        public decimal NumAmount { get; set; }
    }

    public class SalaryGenerateLandingVM 
    {
        public long IntBusinessUnitId { get; set; }
        public string StrBusinessUnitName { get; set;}
        public long? IntTotalEmployee { get; set; }
        public decimal? NumTotalPaybleSalary { get; set;}
    }
}
