using HRM.DTOs;

namespace HRM.Interfaces
{
    public interface IPayrollService
    {
        #region================== Payroll Policy ========================================
        Task<bool> CreatePayrollPolicy(PayrollPolicyCreateVM payrollPolicyCreateVM);
        Task<List<PayrollPolicyLanding>> PayrollPolicyLanding(long businessUnitId);
        Task<bool> UpdatePolicy(PayrollPolicyCreateVM payrollPolicyCreateVM);
        Task<bool> DeletePolicy(long policyId);
        #endregion=============== Payroll Policy ========================================


        #region================== Payroll Element =========================================
        Task<bool> CreatePayrollElement(PayrollElementCreateVM payrollElementCreateVM);
        Task<List<PayrollElementLanding>> PayrollElementLanding(long businessUnitId);
        Task<bool> UpdatePayrollElement(PayrollElementCreateVM payrollElementCreateVM);
        Task<bool> DeletePayrollElement(long elementId);
        #endregion=============== Payroll Element ============================================


        #region================== Payroll Group ====================================================================
        Task<bool> CreatePayrollGroupHeaderNRow(PayrollGroupHeaderNRowCreateVM payrollGroupHeaderNRowCreateVM);
        Task<List<PayrollHeaderLanding>> PayrollHeaderLanding(long businessUnitId);
        Task<List<PayrollRowLanding>> PayrollRowLanding(long headerId);
        Task<bool> UpdatePayrollGroupHeaderNRow(PayrollGroupHeaderNRowCreateVM payrollGroupHeaderNRowCreateVM);
        Task<bool> DeletePayrollGroupHeaderNRow(long headerId);
        #endregion============== Payroll Group =====================================================================
    }
}
