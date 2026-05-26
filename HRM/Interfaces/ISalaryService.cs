using HRM.DTOs;
using HRM.Models.Salary;

namespace HRM.Interfaces
{
    public interface ISalaryService
    {
        #region================== Salary Assign ========================================

        Task<List<SalaryAssignLanding>> SalaryAssignLanding(long businessUnitId);
        Task<bool> SalaryAssign(SalaryAssignVM salaryAssignVM);
        Task<bool> SalaryAssignUpdate(SalaryAssignVM salaryAssignVM);
        Task<bool> DeleteSalaryAssign(long salaryAssignHeaderId);
        Task<List<SalaryDetailsLanding>> SalaryDetailsLanding(long businessUnitId);

        #endregion=============== Salary Asssign =======================================


        #region================== Salary Addition & Deduction ==========================

        Task<bool> CreateSalaryAdditionNDeduction(SalaryAdditionNDeductionVM salaryAdditionNDeductionVM);
        Task<bool> UpdateSalaryAdditionNDeduction(SalaryAdditionNDeductionVM salaryAdditionNDeductionVM);
        Task<bool> DeleteSalaryAdditionNDeduction(long adjustmentId);
        Task<List<SalaryAdjustmentLanding>> SalaryAdjustmentLanding(long businessUnitId, long employeeId, long yearId, long monthId);
        Task<List<EmpSalAddNDeductionVM>> EmpSalAddNDeductionLanding(long employeeId);

        #endregion=============== Salary Addition & Deduction ==========================


        #region================== Salary Generate ==========================

        Task<List<SalaryGenerateLandingVM>> SalaryGenerateLanding(long yearId, long monthId);

        #endregion=============== Salary Generate ==========================
    }
}
