using Azure;
using HRM.Data;
using HRM.DTOs;
using HRM.Interfaces;
using HRM.Migrations;
using HRM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;

namespace HRM.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HRMContext _context;
        public EmployeeService(HRMContext context) 
        {
            _context = context;
        }

        #region============================== Employee Basic Functions ===============================================================================
        public async Task<bool> CreateEmployee(EmployeeCreateVM employeeCreateVM)
        {
            try
            {
                var data = await _context.empBasicInfos.Where(x => x.IntEmployeeBasicInfoId == employeeCreateVM.IntEmployeeBasicInfoId).FirstOrDefaultAsync();
                var codeExists = await _context.empBasicInfos.AnyAsync(x => x.StrEmployeeCode == employeeCreateVM.StrEmployeeCode);
                var businessUnit = await _context.businessUnits.Where(x => x.IntBusinessUnitId == employeeCreateVM.IntBusinessUnitId).FirstOrDefaultAsync();
                var dept = await _context.departments.Where(x => x.IntDepartmentId == employeeCreateVM.IntDepartmentId).FirstOrDefaultAsync();
                var desig = await _context.designations.Where(x => x.IntDesignationId == employeeCreateVM.IntDesignationId).FirstOrDefaultAsync();
                var empType = await _context.employementTypes.Where(x => x.IntEmployementId == employeeCreateVM.IntEmploymentTypeId).FirstOrDefaultAsync();

                if (data == null && codeExists == false && businessUnit != null && dept != null && desig != null && empType != null)
                {
                    EmpBasicInfo empBasicInfo = new EmpBasicInfo
                    {
                        StrEmployeeCode = employeeCreateVM.StrEmployeeCode,
                        StrEmployeeName = employeeCreateVM.StrEmployeeName,
                        IntDepartmentId = employeeCreateVM.IntDepartmentId,
                        IntDesignationId = employeeCreateVM.IntDesignationId,
                        StrGender = employeeCreateVM.StrGender,
                        StrMaritalStatus = employeeCreateVM.StrMaritalStatus,
                        StrBloodGroup = employeeCreateVM.StrBloodGroup,
                        DteDateOfBirth = employeeCreateVM.DteDateOfBirth,
                        DteJoiningDate = employeeCreateVM.DteJoiningDate,
                        DteInternCloseDate = employeeCreateVM.DteInternCloseDate, 
                        DteProbationaryCloseDate = employeeCreateVM.DteProbationaryCloseDate,
                        DteConfirmationDate = employeeCreateVM.DteConfirmationDate,
                        DteLastWorkingDate = employeeCreateVM.DteLastWorkingDate,
                        IntSupervisorId = employeeCreateVM.IntSupervisorId,
                        IntLineManagerId = employeeCreateVM.IntLineManagerId,
                        IsActive = true,
                        IntBusinessUnitId = employeeCreateVM.IntBusinessUnitId,
                        IntEmploymentTypeId = employeeCreateVM.IntEmploymentTypeId,
                        IsSalaryHold = employeeCreateVM.IsSalaryHold,
                        DteCreatedAt = DateTime.Now,
                        IntCreatedBy = employeeCreateVM.IntCreatedBy
                    };

                    await _context.empBasicInfos.AddAsync(empBasicInfo);
                    await _context.SaveChangesAsync();
                    return true;
                }

                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateEmployee(EmployeeCreateVM employeeCreateVM)
        {
            try
            {
                var exst = await _context.empBasicInfos.Where(x => x.IntEmployeeBasicInfoId != employeeCreateVM.IntEmployeeBasicInfoId && x.StrEmployeeCode == employeeCreateVM.StrEmployeeCode).FirstOrDefaultAsync();
                var businessUnit = await _context.businessUnits.Where(x => x.IntBusinessUnitId == employeeCreateVM.IntBusinessUnitId).FirstOrDefaultAsync();
                var dept = await _context.departments.Where(x => x.IntDepartmentId == employeeCreateVM.IntDepartmentId).FirstOrDefaultAsync();
                var desig = await _context.designations.Where(x => x.IntDesignationId == employeeCreateVM.IntDesignationId).FirstOrDefaultAsync();
                var empType = await _context.employementTypes.Where(x => x.IntEmployementId == employeeCreateVM.IntEmploymentTypeId).FirstOrDefaultAsync();

                if (exst == null && businessUnit != null && dept != null && desig != null && empType != null)
                {
                    EmpBasicInfo data = await _context.empBasicInfos.Where(emp => emp.IntEmployeeBasicInfoId == employeeCreateVM.IntEmployeeBasicInfoId).FirstOrDefaultAsync();
                    if (data != null)
                    {
                        data.StrEmployeeCode = employeeCreateVM.StrEmployeeCode;
                        data.StrEmployeeName = employeeCreateVM.StrEmployeeName;
                        data.IntDepartmentId = employeeCreateVM.IntDepartmentId;
                        data.IntDesignationId = employeeCreateVM.IntDesignationId;
                        data.StrGender = employeeCreateVM.StrGender;
                        data.StrMaritalStatus = employeeCreateVM.StrMaritalStatus;
                        data.StrBloodGroup = employeeCreateVM.StrBloodGroup;
                        data.DteDateOfBirth = employeeCreateVM.DteDateOfBirth;
                        data.DteJoiningDate = employeeCreateVM.DteJoiningDate;
                        data.DteInternCloseDate = employeeCreateVM.DteInternCloseDate;
                        data.DteProbationaryCloseDate = employeeCreateVM.DteProbationaryCloseDate;
                        data.DteConfirmationDate = employeeCreateVM.DteConfirmationDate;
                        data.DteLastWorkingDate = employeeCreateVM.DteLastWorkingDate;
                        data.IntSupervisorId = employeeCreateVM.IntSupervisorId;
                        data.IntLineManagerId = employeeCreateVM.IntLineManagerId;
                        data.IntBusinessUnitId = employeeCreateVM.IntBusinessUnitId;
                        data.IntEmploymentTypeId = employeeCreateVM.IntEmploymentTypeId;
                        data.IsSalaryHold = employeeCreateVM.IsSalaryHold;
                        data.IntCreatedBy = employeeCreateVM.IntCreatedBy;
                        data.IntUpdatedBy = employeeCreateVM.IntUpdatedBy;
                        data.DteUpdatedAt = DateTime.Now;
                    }
                    _context.empBasicInfos.Update(data);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteEmployee(long employeeId)
        {
            try
            {
                EmpBasicInfo data = await _context.empBasicInfos.Where(x => x.IntEmployeeBasicInfoId == employeeId).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.IsActive = false;

                    _context.empBasicInfos.Update(data);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<EmployeeLandingPagination> EmployeeLanding(EmployeeLandingFilter employeeLandingFilter)
        {
            try
            {
                employeeLandingFilter.searchTxt = string.IsNullOrEmpty(employeeLandingFilter.searchTxt) ? "" : employeeLandingFilter.searchTxt.ToLower();

                IQueryable<EmployeeLandingVM> employeeLandings = (from emp in _context.empBasicInfos
                                                                 join bus in _context.businessUnits on emp.IntBusinessUnitId equals bus.IntBusinessUnitId into b2
                                                                 from business in b2.DefaultIfEmpty()
                                                                 join dep in _context.departments on emp.IntDepartmentId equals dep.IntDepartmentId into d2
                                                                 from dept in d2.DefaultIfEmpty()
                                                                 join des in _context.designations on emp.IntDesignationId equals des.IntDesignationId into d3
                                                                 from desig in d3.DefaultIfEmpty()
                                                                 join empT in _context.employementTypes on emp.IntEmploymentTypeId equals empT.IntEmployementId into t2
                                                                 from type in t2.DefaultIfEmpty()
                                                                 
                                                                 where emp.IsActive == true && emp.IntBusinessUnitId == employeeLandingFilter.IntBusinessunitId
                                                                 && (!string.IsNullOrEmpty(employeeLandingFilter.searchTxt) ? (emp.StrEmployeeName.ToLower().Contains(employeeLandingFilter.searchTxt) || emp.StrEmployeeCode.ToLower().Contains(employeeLandingFilter.searchTxt)
                                                                 || desig.StrDesignationName.ToLower().Contains(employeeLandingFilter.searchTxt) || dept.StrDepartmentName.ToLower().Contains(employeeLandingFilter.searchTxt)) : true)

                                                                 && ((employeeLandingFilter.DepartmentList != null && employeeLandingFilter.DepartmentList.Count > 0 ? employeeLandingFilter.DepartmentList.Contains((long)emp.IntDepartmentId) : true)
                                                                 && (employeeLandingFilter.DesignationList != null && employeeLandingFilter.DesignationList.Count > 0 ? employeeLandingFilter.DesignationList.Contains((long)emp.IntDesignationId) : true)
                                                                 && (employeeLandingFilter.EmployementTypeList != null && employeeLandingFilter.EmployementTypeList.Count > 0 ? employeeLandingFilter.EmployementTypeList.Contains((long)emp.IntEmploymentTypeId) : true))
                                                                 select new EmployeeLandingVM
                                                                 {
                                                                     IntEmployeeBasicInfoId = emp.IntEmployeeBasicInfoId,
                                                                     StrEmployeeCode = emp.StrEmployeeCode,
                                                                     StrEmployeeName = emp.StrEmployeeName,
                                                                     IntBusinessUnitId = emp.IntBusinessUnitId,
                                                                     IntDepartmentId = emp.IntDepartmentId,
                                                                     StrDepartmentName = dept.StrDepartmentName,
                                                                     IntDesignationId = emp.IntDesignationId,
                                                                     StrDesignationName = desig.StrDesignationName,
                                                                     StrGender = emp.StrGender ?? "",
                                                                     StrMaritalStatus = emp.StrMaritalStatus ?? "",
                                                                     StrBloodGroup = emp.StrBloodGroup ?? "",
                                                                     DteDateOfBirth = emp.DteDateOfBirth.HasValue ? emp.DteDateOfBirth.Value.ToString("dd MMM yyyy") : "",
                                                                     DteJoiningDate = emp.DteJoiningDate,
                                                                     DteConfirmationDate = emp.DteConfirmationDate,
                                                                     DteLastWorkingDate = emp.DteLastWorkingDate,
                                                                     IsActive = emp.IsActive,
                                                                     StrBusinessUnitName = business.StrBusinessUnitName,
                                                                     IntEmployementTyperId = emp.IntEmploymentTypeId,
                                                                     StrEmploymentTypeName = type.StrEmployementName,
                                                                 }).OrderBy(x => x.IntEmployeeBasicInfoId).AsNoTracking().AsQueryable();

                EmployeeLandingPagination retObj = new();

                if (employeeLandingFilter.IsHeaderNeed)
                {
                    EmployeeInfoHeader eh = new();

                    var datas = employeeLandings.Select(x => new
                    {
                        DepartmentId = x.IntDepartmentId,
                        Department = x.StrDepartmentName,
                        DesignationId = x.IntDesignationId,
                        Designation = x.StrDesignationName,
                        EmployementTypeId = x.IntEmployementTyperId,
                        EmployementType = x.StrEmploymentTypeName
                    }).Distinct().ToList();

                    eh.DepartmentList = datas.Where(x => !string.IsNullOrEmpty(x.Department)).Select(x => new CommonDDL { Value = (long)x.DepartmentId, Label = (string)x.Department }).DistinctBy(x => x.Value).ToList();
                    eh.DesignationList = datas.Where(x => !string.IsNullOrEmpty(x.Designation)).Select(x => new CommonDDL { Value = (long)x.DesignationId, Label = (string)x.Designation }).DistinctBy(x => x.Value).ToList();
                    eh.EmployementTypeList = datas.Where(x => !string.IsNullOrEmpty(x.EmployementType)).Select(x => new CommonDDL { Value = (long)x.EmployementTypeId, Label = (string)x.EmployementType }).DistinctBy(x => x.Value).ToList();

                    retObj.employeeInfoHeader = eh;
                }

                if (employeeLandingFilter.IsPaginated == false)
                {
                    retObj.Data = employeeLandings.ToList();
                }
                else
                {
                    int maxSize = 1000;
                    employeeLandingFilter.PageSize = employeeLandingFilter.PageSize > maxSize ? maxSize : employeeLandingFilter.PageSize;
                    employeeLandingFilter.PageNo = employeeLandingFilter.PageNo < 1 ? 1 : employeeLandingFilter.PageNo;

                    retObj.TotalCount = employeeLandings.Count();
                    retObj.Data = employeeLandings.Skip(employeeLandingFilter.PageSize * (employeeLandingFilter.PageNo - 1)).Take(employeeLandingFilter.PageSize).ToList();
                    retObj.PageSize = employeeLandingFilter.PageSize;
                    retObj.CurrentPage = employeeLandingFilter.PageNo;
                }

                return Task.FromResult(retObj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<EmployeeLandingVM> GetEmployeeById(long employeeId)
        {
            try
            {

                EmployeeLandingVM? employeeLanding = (from emp in _context.empBasicInfos
                                                                  join bus in _context.businessUnits on emp.IntBusinessUnitId equals bus.IntBusinessUnitId into b2
                                                                  from business in b2.DefaultIfEmpty()
                                                                  join dep in _context.departments on emp.IntDepartmentId equals dep.IntDepartmentId into d2
                                                                  from dept in d2.DefaultIfEmpty()
                                                                  join des in _context.designations on emp.IntDesignationId equals des.IntDesignationId into d3
                                                                  from desig in d3.DefaultIfEmpty()
                                                                  join empT in _context.employementTypes on emp.IntEmploymentTypeId equals empT.IntEmployementId into t2
                                                                  from type in t2.DefaultIfEmpty()

                                                                  where emp.IntEmployeeBasicInfoId == employeeId
                                                                 select new EmployeeLandingVM
                                                                  {
                                                                      IntEmployeeBasicInfoId = emp.IntEmployeeBasicInfoId,
                                                                      StrEmployeeCode = emp.StrEmployeeCode,
                                                                      StrEmployeeName = emp.StrEmployeeName,
                                                                      IntBusinessUnitId = emp.IntBusinessUnitId,
                                                                      IntDepartmentId = emp.IntDepartmentId,
                                                                      IntDesignationId = emp.IntDesignationId,
                                                                      StrDepartmentName = dept.StrDepartmentName,
                                                                      StrDesignationName = desig.StrDesignationName,
                                                                      StrGender = emp.StrGender ?? "",
                                                                      StrMaritalStatus = emp.StrMaritalStatus ?? "",
                                                                      StrBloodGroup = emp.StrBloodGroup ?? "",
                                                                      DteDateOfBirth = emp.DteDateOfBirth.HasValue ? emp.DteDateOfBirth.Value.ToString("dd MMM yyyy") : "",
                                                                      DteJoiningDate = emp.DteJoiningDate,
                                                                      DteConfirmationDate = emp.DteConfirmationDate,
                                                                      DteLastWorkingDate = emp.DteLastWorkingDate,
                                                                      IsActive = emp.IsActive,
                                                                      StrBusinessUnitName = business.StrBusinessUnitName,
                                                                      StrEmploymentTypeName = type.StrEmployementName,
                                                                  }).FirstOrDefault();


                return Task.FromResult(employeeLanding);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion============================== Employee Basic Functions ===============================================================================


        #region================================ Employee Transfer And Promtion ==========================================================================
        public async Task<bool> CreateEmployeeTrnasferNPromotion(TransferNPromotionCreateVM transferNPromotionCreateVM)
        {
            try
            {
                var empExst = await _context.empBasicInfos.Where(x => x.IntEmployeeBasicInfoId == transferNPromotionCreateVM.IntEmployeeId && x.IntBusinessUnitId == transferNPromotionCreateVM.IntBusinessUnitId).FirstOrDefaultAsync();
                if (empExst == null) { return false; }

                var businessUnit = await _context.businessUnits.Where(x => x.IntBusinessUnitId == transferNPromotionCreateVM.IntBusinessUnitId).FirstOrDefaultAsync();

                var oldDepartment = await _context.departments.Where(x => x.IntDepartmentId == transferNPromotionCreateVM.IntOldDepartmentId).FirstOrDefaultAsync();
                var newDepartment = await _context.departments.Where(x => x.IntDepartmentId == transferNPromotionCreateVM.IntNewDepartmenId).FirstOrDefaultAsync();
                bool isValidTransfer = oldDepartment != null && newDepartment != null;
                
                var oldDesignation = await _context.designations.Where(x => x.IntDesignationId == transferNPromotionCreateVM.IntOldDesignationId).FirstOrDefaultAsync();
                var newDesignation = await _context.designations.Where(x => x.IntDesignationId == transferNPromotionCreateVM.IntNewDesignationId).FirstOrDefaultAsync();
                bool isValidPromotion = oldDesignation != null && newDesignation != null;

                if (businessUnit != null && (isValidTransfer || isValidPromotion))
                {
                    EmpTransferNPromotion empTransferNPromotion = new EmpTransferNPromotion
                    {
                        IntEmpTransferNPromotionId = transferNPromotionCreateVM.IntEmpTransferNPromotionId,
                        IntEmployeeId = transferNPromotionCreateVM.IntEmployeeId,
                        IntBusinessUnitId = transferNPromotionCreateVM.IntBusinessUnitId,
                        IntOldDepartmentId = transferNPromotionCreateVM.IntOldDepartmentId ?? null,
                        IntNewDepartmenId = transferNPromotionCreateVM.IntNewDepartmenId ?? null,
                        IntOldDesignationId = transferNPromotionCreateVM.IntOldDesignationId ?? null,
                        IntNewDesignationId = transferNPromotionCreateVM.IntNewDesignationId ?? null,
                        IsTransfer = transferNPromotionCreateVM.IsTransfer,
                        IsPromotion = transferNPromotionCreateVM.IsPromotion,
                        IntCreatedBy = transferNPromotionCreateVM.IntCreatedBy,
                        DteCreatedAt = DateTime.Now
                    };

                    await _context.empTransferNPromotions.AddAsync(empTransferNPromotion);
                    
                    if (isValidTransfer)
                    {
                        empExst.IntDepartmentId = (long)transferNPromotionCreateVM.IntNewDepartmenId;
                    }
                    
                    if (isValidPromotion)
                    {
                        empExst.IntDesignationId = (long)transferNPromotionCreateVM.IntNewDesignationId;
                    }

                    _context.empBasicInfos.Update(empExst);
                    await _context.SaveChangesAsync();
                    return true;
                }

                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateEmployeeTrnasferNPromotion(TransferNPromotionCreateVM transferNPromotionCreateVM)
        {
            try
            {
                var data = await _context.empTransferNPromotions.Where(x => x.IntEmpTransferNPromotionId == transferNPromotionCreateVM.IntEmpTransferNPromotionId).FirstOrDefaultAsync();
                var empExst = await _context.empBasicInfos.Where(x => x.IntEmployeeBasicInfoId == transferNPromotionCreateVM.IntEmployeeId && x.IntBusinessUnitId == transferNPromotionCreateVM.IntBusinessUnitId).FirstOrDefaultAsync();
                var businessUnit = await _context.businessUnits.Where(x => x.IntBusinessUnitId == transferNPromotionCreateVM.IntBusinessUnitId).FirstOrDefaultAsync();

                if (data == null || empExst == null || businessUnit == null)
                {
                    return false;
                }

                bool isValidTransfer = transferNPromotionCreateVM.IntOldDepartmentId != null
                                       && transferNPromotionCreateVM.IntNewDepartmenId != null
                                       && await _context.departments.AnyAsync(x => x.IntDepartmentId == transferNPromotionCreateVM.IntOldDepartmentId)
                                       && await _context.departments.AnyAsync(x => x.IntDepartmentId == transferNPromotionCreateVM.IntNewDepartmenId);

                bool isValidPromotion = transferNPromotionCreateVM.IntOldDesignationId != null
                                        && transferNPromotionCreateVM.IntNewDesignationId != null
                                        && await _context.designations.AnyAsync(x => x.IntDesignationId == transferNPromotionCreateVM.IntOldDesignationId)
                                        && await _context.designations.AnyAsync(x => x.IntDesignationId == transferNPromotionCreateVM.IntNewDesignationId);

                if (isValidTransfer == false && isValidPromotion == false)
                {
                    return false;
                }

                data.IntEmployeeId = transferNPromotionCreateVM.IntEmployeeId;
                data.IntBusinessUnitId = transferNPromotionCreateVM.IntBusinessUnitId;
                data.IntOldDepartmentId = transferNPromotionCreateVM.IntOldDepartmentId;
                data.IntNewDepartmenId = transferNPromotionCreateVM.IntNewDepartmenId;
                data.IntOldDesignationId = transferNPromotionCreateVM.IntOldDesignationId;
                data.IntNewDesignationId = transferNPromotionCreateVM.IntNewDesignationId;
                data.IsTransfer = transferNPromotionCreateVM.IsTransfer;
                data.IsPromotion = transferNPromotionCreateVM.IsPromotion;
                data.IntUpdatedBy = transferNPromotionCreateVM.IntUpdatedBy;
                data.DteUpdatedAt = DateTime.Now;

                if (isValidTransfer)
                {
                    empExst.IntDepartmentId = (long)transferNPromotionCreateVM.IntNewDepartmenId;
                }

                if (isValidPromotion)
                {
                    empExst.IntDesignationId = (long)transferNPromotionCreateVM.IntNewDesignationId;
                }

                _context.empTransferNPromotions.Update(data);
                _context.empBasicInfos.Update(empExst);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<EmpTransferNPromotionLandingVM>> EmpTransferNPromotionLandingByDate(DateTime dteFromDate, DateTime dteToDate, long intBusinessUnitId, bool? isTransfer, bool? isPromotion) 
        {
            try
            {
                IQueryable<EmpTransferNPromotionLandingVM> empTransferNPromotionLandingVMs = (  from tp in _context.empTransferNPromotions
                                                                                                join basic in _context.empBasicInfos on tp.IntEmployeeId equals basic.IntEmployeeBasicInfoId into b2
                                                                                                from emp in b2.DefaultIfEmpty()
                                                                                                join bus in _context.businessUnits on tp.IntBusinessUnitId equals bus.IntBusinessUnitId into bus2
                                                                                                from business in bus2.DefaultIfEmpty()
                                                                                                join oldDep in _context.departments on tp.IntOldDepartmentId equals oldDep.IntDepartmentId into oldDep2
                                                                                                from oldDept in oldDep2.DefaultIfEmpty()
                                                                                                join newDep in _context.departments on tp.IntNewDepartmenId equals newDep.IntDepartmentId into newDep2
                                                                                                from newDept in newDep2.DefaultIfEmpty()
                                                                                                join oldDes in _context.designations on tp.IntOldDesignationId equals oldDes.IntDesignationId into oldDes2
                                                                                                from oldDesig in oldDes2.DefaultIfEmpty()
                                                                                                join newDes in _context.designations on tp.IntNewDesignationId equals newDes.IntDesignationId into newDes2
                                                                                                from newDesig in newDes2.DefaultIfEmpty()

                                                                                                where tp.DteCreatedAt>= dteFromDate && tp.DteCreatedAt<= dteToDate && tp.IntBusinessUnitId == intBusinessUnitId
                                                                                                select new EmpTransferNPromotionLandingVM
                                                                                                {
                                                                                                    IntEmpTransferNPromotionId = tp.IntEmpTransferNPromotionId,
                                                                                                    IntEmployeeId = tp.IntEmployeeId,
                                                                                                    StrEmployeeName = emp.StrEmployeeName,
                                                                                                    IntBusinessUnitId = tp.IntBusinessUnitId,
                                                                                                    StrBusinessUnitName = business.StrBusinessUnitName ?? "",
                                                                                                    IntOldDepartmentId = tp.IntOldDepartmentId,
                                                                                                    StrOldDepartmentName = oldDept.StrDepartmentName ?? "",
                                                                                                    IntNewDepartmenId = tp.IntNewDepartmenId,
                                                                                                    StrNewDepartmentName = newDept.StrDepartmentName ?? "",
                                                                                                    IntOldDesignationId = tp.IntOldDesignationId,
                                                                                                    StrOldDesignationName = oldDesig.StrDesignationName ?? "",
                                                                                                    IntNewDesignationId = tp.IntNewDesignationId,
                                                                                                    StrNewDesignationName = newDesig.StrDesignationName ?? "",
                                                                                                    IsTransfer = tp.IsTransfer,
                                                                                                    IsPromotion = tp.IsPromotion,
                                                                                                    IntCreatedBy = tp.IntCreatedBy,
                                                                                                    StrCreatedAt = tp.DteCreatedAt.ToString("dd MMM yyyy"),
                                                                                                    IntUpdatedBy = tp.IntUpdatedBy,
                                                                                                    StrUpdatedAt = tp.DteUpdatedAt.HasValue ? tp.DteUpdatedAt.Value.ToString("dd MMM yyyy") : ""
                                                                                                }).OrderByDescending(x => x.IntEmpTransferNPromotionId).AsNoTracking().AsQueryable();
                if (isTransfer == true && isPromotion == false) 
                {
                    return await empTransferNPromotionLandingVMs.Where(x => x.IsTransfer == true && x.IsPromotion == false).ToListAsync();
                }
                
                if (isTransfer == false && isPromotion == true) 
                {
                    return await empTransferNPromotionLandingVMs.Where(x => x.IsTransfer == false && x.IsPromotion == true).ToListAsync();
                }            

                return await empTransferNPromotionLandingVMs.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  async Task<List<EmployeeLandingVM>> EmployeeSearchByAddress(string employeeAdress)
        {
            IQueryable<EmployeeLandingVM> employeeLandings = (from emp in _context.empBasicInfos
                                                              where emp.IsActive == true && emp.StrAddress != null
                                                              && emp.StrAddress.Contains(employeeAdress)
                                                              select new EmployeeLandingVM
                                                              {
                                                                  IntEmployeeBasicInfoId = emp.IntEmployeeBasicInfoId,
                                                                  StrEmployeeCode = emp.StrEmployeeCode,
                                                                  StrEmployeeName = emp.StrEmployeeName,
                                                                  IntBusinessUnitId = emp.IntBusinessUnitId,
                                                                  IntDepartmentId = emp.IntDepartmentId,
                                                                  StrDepartmentName = "",
                                                                  IntDesignationId = emp.IntDesignationId,
                                                                  StrDesignationName = "",
                                                                  StrGender = emp.StrGender ?? "",
                                                                  StrMaritalStatus = emp.StrMaritalStatus ?? "",
                                                                  StrBloodGroup = emp.StrBloodGroup ?? "",
                                                                  DteDateOfBirth = emp.DteDateOfBirth.HasValue ? emp.DteDateOfBirth.Value.ToString("dd MMM yyyy") : "",
                                                                  DteJoiningDate = emp.DteJoiningDate,
                                                                  DteConfirmationDate = emp.DteConfirmationDate,
                                                                  DteLastWorkingDate = emp.DteLastWorkingDate,
                                                                  IsActive = emp.IsActive,
                                                                  StrBusinessUnitName = "",
                                                                  IntEmployementTyperId = emp.IntEmploymentTypeId,
                                                                  StrEmploymentTypeName = "",
                                                              }).AsNoTracking().AsQueryable();
                                                              
            return await employeeLandings.ToListAsync();
        }

        #endregion================================ Employee Transfer And Promtion ==========================================================================

    }
}
