using HRM.DTOs;
using HRM.Models.Salary;

namespace HRM.Interfaces
{
    public interface ISalaryService
    {
        #region================== Salary Assign ========================================

        Task<List<SalaryAssignLanding>> SalaryAssignLanding(long businessUnitId);
        Task<bool> SalaryAssign(SalaryAssignVM salaryAssignVM);
        Task<List<SalaryDetailsLanding>> SalaryDetailsLanding(long businessUnitId);

        #endregion=============== Salary Asssign =======================================


        #region================== Salary Addition & Deduction ==========================

        Task<bool> CreateSalaryAdditionNDeduction(SalaryAdditionNDeductionVM salaryAdditionNDeductionVM);
        Task<List<EmpSalAddNDeductionVM>> EmpSalAddNDeductionLanding(long employeeId);

        #endregion=============== Salary Addition & Deduction ==========================


        #region================== Salary Generate ==========================

        Task<List<SalaryGenerateLandingVM>> SalaryGenerateLanding(long yearId, long monthId);

        #endregion=============== Salary Generate ==========================
    }
}
