using HRM.DTOs;

namespace HRM.Interfaces
{
    public interface IEmployeeService
    {

        #region======================= Basic Employee Functions =========================================
        Task<bool> CreateEmployee(EmployeeCreateVM employeeCreateVM);
        Task<EmployeeLandingPagination> EmployeeLanding(EmployeeLandingFilter employeeLandingFilter);
        Task<EmployeeLandingVM> GetEmployeeById(long employeeId);
        Task<bool> UpdateEmployee(EmployeeCreateVM employeeCreateVM);
        Task<bool> DeleteEmployee(long  employeeId);
        Task<List<EmployeeLandingVM>> EmployeeSearchByAddress(string  employeeAdress);
        #endregion===================== Basic Employee Functions =========================================

        #region======================== Employee Transfer And Promotion ==================================
        Task<bool> CreateEmployeeTrnasferNPromotion(TransferNPromotionCreateVM transferNPromotionCreateVM);
        Task<bool> UpdateEmployeeTrnasferNPromotion(TransferNPromotionCreateVM transferNPromotionCreateVM);
        Task<List<EmpTransferNPromotionLandingVM>> EmpTransferNPromotionLandingByDate(DateTime dteFromDate, DateTime dteToDate, long intBusinessUnitId, bool? isTransfer, bool? isPromotion);
        #endregion===================== Employee Trnasfer And Promotion ==================================
    }
}
