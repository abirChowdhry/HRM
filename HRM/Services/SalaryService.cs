using HRM.Data;
using HRM.DTOs;
using HRM.Interfaces;
using HRM.Migrations;
using HRM.Models;
using HRM.Models.Payroll;
using HRM.Models.Salary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace HRM.Services
{
    // Salary service handles assignment, adjustments, and generated monthly payable summaries.
    public class SalaryService : ISalaryService
    {
        private readonly HRMContext _context;
        public SalaryService(HRMContext context)
        {
            _context = context;
        }


        #region======================= Salary Assign ======================================================
        public async Task<List<SalaryAssignLanding>> SalaryAssignLanding(long businessUnitId)
        {
            try
            {
                IQueryable<SalaryAssignLanding> salaryAssignLandings = (from emp in _context.empBasicInfos
                                                                        join bus in _context.businessUnits on emp.IntBusinessUnitId equals bus.IntBusinessUnitId into b2
                                                                        from business in b2.DefaultIfEmpty()
                                                                        join head in _context.salaryAssignHeaders.Where(x => x.IsActive == true) on emp.IntEmployeeBasicInfoId equals head.IntEmployeeId into h2
                                                                        from header in h2.DefaultIfEmpty()
                                                                        join dep in _context.departments on emp.IntDepartmentId equals dep.IntDepartmentId into d2
                                                                        from dept in d2.DefaultIfEmpty()
                                                                        join des in _context.designations on emp.IntDesignationId equals des.IntDesignationId into d3
                                                                        from desig in d3.DefaultIfEmpty()

                                                                        where emp.IsActive == true && emp.IntBusinessUnitId == businessUnitId && header == null
                                                                        select new SalaryAssignLanding
                                                                        {
                                                                            IntEmployeeId = emp.IntEmployeeBasicInfoId,
                                                                            IntBusinessUnitId = emp.IntBusinessUnitId,
                                                                            StrBusinessUnitName = business.StrBusinessUnitName,
                                                                            StrEmployeeName = emp.StrEmployeeName,
                                                                            StrDepartmentName = dept.StrDepartmentName,
                                                                            StrDesignationName = desig.StrDesignationName,
                                                                        }).OrderBy(x => x.IntEmployeeId).AsNoTracking().AsQueryable();

                return await salaryAssignLandings.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        
        public async Task<List<SalaryDetailsLanding>> SalaryDetailsLanding(long businessUnitId)
        {
            try
            {
                IQueryable<SalaryDetailsLanding> salaryDetailsLandings = (from emp in _context.empBasicInfos
                                                                        join bus in _context.businessUnits on emp.IntBusinessUnitId equals bus.IntBusinessUnitId into b2
                                                                        from business in b2.DefaultIfEmpty()
                                                                        join head in _context.salaryAssignHeaders.Where(x => x.IsActive == true) on emp.IntEmployeeBasicInfoId equals head.IntEmployeeId into h2
                                                                        from header in h2.DefaultIfEmpty()
                                                                        join pg in _context.payrollGroupHeaders on header.IntPayrollGroupHeaderId equals pg.IntPayrollGroupHeaderId into pg2
                                                                        from payroll in pg2.DefaultIfEmpty()
                                                                        join dep in _context.departments on emp.IntDepartmentId equals dep.IntDepartmentId into d2
                                                                        from dept in d2.DefaultIfEmpty()
                                                                        join des in _context.designations on emp.IntDesignationId equals des.IntDesignationId into d3
                                                                        from desig in d3.DefaultIfEmpty()

                                                                        where emp.IsActive == true && emp.IntBusinessUnitId == businessUnitId && header != null
                                                                        select new SalaryDetailsLanding
                                                                        {
                                                                            IntSalaryAssignHeaderId = header.IntSalaryAssignHeaderId,
                                                                            IntEmployeeId = emp.IntEmployeeBasicInfoId,
                                                                            IntBusinessUnitId = emp.IntBusinessUnitId,
                                                                            StrBusinessUnitName = business.StrBusinessUnitName,
                                                                            StrEmployeeName = emp.StrEmployeeName,
                                                                            StrDepartmentName = dept.StrDepartmentName,
                                                                            StrDesignationName = desig.StrDesignationName,
                                                                            NumGrossSalary = header.NumGrossSalary,
                                                                            NumNetGrossSalary = header.NumNetGrossSalary,
                                                                            IntPayrollGroupHeader = payroll.IntPayrollGroupHeaderId,
                                                                            StrPayrollGroupHeader = payroll.StrPayrollGroupHeaderTitle

                                                                        }).OrderBy(x => x.IntEmployeeId).AsNoTracking().AsQueryable();

                return await salaryDetailsLandings.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SalaryAssign(SalaryAssignVM salaryAssignVM)
        {
            try
            {

                var data = await _context.salaryAssignHeaders.Where(x => x.IntEmployeeId == salaryAssignVM.IntEmployeeId && x.IsActive == true).FirstOrDefaultAsync();
                var businessUnit = await _context.businessUnits.Where(x => x.IntBusinessUnitId == salaryAssignVM.IntBusinessUnitId).FirstOrDefaultAsync();
                var payrollGroupExst = await _context.payrollGroupHeaders.Where(x => x.IntPayrollGroupHeaderId == salaryAssignVM.IntPayrollGroupHeaderId && x.IntBusinessUnitId == salaryAssignVM.IntBusinessUnitId).FirstOrDefaultAsync();
                var empExst = await _context.empBasicInfos.Where(x => x.IntEmployeeBasicInfoId == salaryAssignVM.IntEmployeeId).FirstOrDefaultAsync();

                // Each active employee can have one active salary assignment at a time.
                if (data == null && businessUnit != null && payrollGroupExst != null && empExst != null)
                {
                    SalaryAssignHeader salaryAssignHeader = new SalaryAssignHeader
                    {
                        IntSalaryAssignHeaderId = salaryAssignVM.IntSalaryAssignHeaderId,
                        IntPayrollGroupHeaderId = salaryAssignVM.IntPayrollGroupHeaderId,
                        StrPayrollGroupHeaderTitle = _context.payrollGroupHeaders.Where(x => x.IntPayrollGroupHeaderId == salaryAssignVM.IntPayrollGroupHeaderId).Select(x => x.StrPayrollGroupHeaderTitle).FirstOrDefault() ?? "",
                        IntEmployeeId = salaryAssignVM.IntEmployeeId,
                        IntBusinessUnitId = salaryAssignVM.IntBusinessUnitId,
                        NumGrossSalary = salaryAssignVM.NumGrossSalary,
                        NumNetGrossSalary = salaryAssignVM.NumNetGrossSalary,
                        IsActive = true,
                        IntCreateBy = salaryAssignVM.IntCreateBy,
                        DteCreateDateTime = DateTime.Now,
                    };

                    await _context.salaryAssignHeaders.AddAsync(salaryAssignHeader);
                    await _context.SaveChangesAsync();

                    var elementRows = await _context.payrollGroupRows.Where(x => x.IntPayrollHeaderId == salaryAssignHeader.IntPayrollGroupHeaderId).ToListAsync();

                    List<SalaryAssignRow> salaryAssignRows = new List<SalaryAssignRow>();
                    bool check = true;

                    // Split the gross salary into component rows using the selected salary structure.
                    elementRows.ForEach(er =>
                    {
                        salaryAssignRows.Add(new SalaryAssignRow
                        {
                          IntSalaryAssignHeaderId = salaryAssignHeader.IntSalaryAssignHeaderId,
                          IntEmployeeId = salaryAssignVM.IntEmployeeId,
                          IntPayrollElementId = er.IntPayrollElementTypeId,
                          StrPayrollElement = er.StrPayrollElementName,
                          NumNumberOfPercent = er.NumNumberOfPercent,
                          NumAmount =(decimal)((salaryAssignVM.NumNetGrossSalary*er.NumNumberOfPercent)/100),
                          IsActive = true,
                          IntCreateBy = salaryAssignVM.IntCreateBy,
                          DteCreatedAt = DateTime.Now
                        });
                    });


                        await _context.salaryAssignRows.AddRangeAsync(salaryAssignRows);
                        await _context.SaveChangesAsync();
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SalaryAssignUpdate(SalaryAssignVM salaryAssignVM)
        {
            try
            {

                var header = await _context.salaryAssignHeaders.Where(x => x.IntSalaryAssignHeaderId == salaryAssignVM.IntSalaryAssignHeaderId).FirstOrDefaultAsync();
                var businessUnit = await _context.businessUnits.Where(x => x.IntBusinessUnitId == salaryAssignVM.IntBusinessUnitId).FirstOrDefaultAsync();
                var payrollGroupExst = await _context.payrollGroupHeaders.Where(x => x.IntPayrollGroupHeaderId == salaryAssignVM.IntPayrollGroupHeaderId && x.IntBusinessUnitId == salaryAssignVM.IntBusinessUnitId).FirstOrDefaultAsync();
                var empExst = await _context.empBasicInfos.Where(x => x.IntEmployeeBasicInfoId == salaryAssignVM.IntEmployeeId).FirstOrDefaultAsync();

                if (header != null && businessUnit != null && payrollGroupExst != null && empExst != null)
                {
                    header.IntSalaryAssignHeaderId = salaryAssignVM.IntSalaryAssignHeaderId;
                    header.IntPayrollGroupHeaderId = salaryAssignVM.IntPayrollGroupHeaderId;
                    header.StrPayrollGroupHeaderTitle = _context.payrollGroupHeaders.Where(x => x.IntPayrollGroupHeaderId == salaryAssignVM.IntPayrollGroupHeaderId).Select(x => x.StrPayrollGroupHeaderTitle).FirstOrDefault() ?? "";
                    header.IntEmployeeId = salaryAssignVM.IntEmployeeId;
                    header.IntBusinessUnitId = salaryAssignVM.IntBusinessUnitId;
                    header.NumGrossSalary = salaryAssignVM.NumGrossSalary;
                    header.NumNetGrossSalary = salaryAssignVM.NumNetGrossSalary;
                    header.IntCreateBy = salaryAssignVM.IntCreateBy;
                    header.IntUpdateBy = salaryAssignVM.IntUpdateBy;
                    header.DteUpdateDateTime = DateTime.Now;

                    _context.salaryAssignHeaders.Update(header);
                    await _context.SaveChangesAsync();

                    var oldRows = await _context.salaryAssignRows.Where(x => x.IntSalaryAssignHeaderId == header.IntSalaryAssignHeaderId).ToListAsync();
                    _context.salaryAssignRows.RemoveRange(oldRows);
                    await _context.SaveChangesAsync();

                    var elementRows = await _context.payrollGroupRows.Where(x => x.IntPayrollHeaderId == header.IntPayrollGroupHeaderId).ToListAsync();

                    List<SalaryAssignRow> salaryAssignRows = new List<SalaryAssignRow>();

                    elementRows.ForEach(er =>
                    {
                        salaryAssignRows.Add(new SalaryAssignRow
                        {
                            IntSalaryAssignHeaderId = header.IntSalaryAssignHeaderId,
                            IntEmployeeId = header.IntEmployeeId,
                            IntPayrollElementId = er.IntPayrollElementTypeId,
                            StrPayrollElement = er.StrPayrollElementName,
                            NumNumberOfPercent = er.NumNumberOfPercent,
                            NumAmount = (decimal)((header.NumNetGrossSalary * er.NumNumberOfPercent) / 100),
                            IsActive = true,
                            IntCreateBy = (long)header.IntCreateBy,
                            DteCreatedAt = DateTime.Now
                        });
                    });

                    await _context.salaryAssignRows.AddRangeAsync(salaryAssignRows);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteSalaryAssign(long salaryAssignHeaderId)
        {
            try
            {
                var header = await _context.salaryAssignHeaders.Where(x => x.IntSalaryAssignHeaderId == salaryAssignHeaderId).FirstOrDefaultAsync();
                var rows = await _context.salaryAssignRows.Where(x => x.IntSalaryAssignHeaderId == salaryAssignHeaderId).ToListAsync();

                if (header == null)
                {
                    return false;
                }

                header.IsActive = false;
                header.DteUpdateDateTime = DateTime.Now;
                foreach (var row in rows)
                {
                    row.IsActive = false;
                }

                _context.salaryAssignHeaders.Update(header);
                _context.salaryAssignRows.UpdateRange(rows);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion======================= Salary Assign ======================================================


        #region======================= Salary Addition & Deduction ======================================================

        public async Task<bool> CreateSalaryAdditionNDeduction(SalaryAdditionNDeductionVM salaryAdditionNDeductionVM)
        {
                try
                {
                    var businessUnit = await _context.businessUnits.Where(x => x.IntBusinessUnitId == salaryAdditionNDeductionVM.IntBusinessUnitId).FirstOrDefaultAsync();
                    var empExst = await _context.empBasicInfos.Where(x => x.IntEmployeeBasicInfoId == salaryAdditionNDeductionVM.IntEmployeeId).FirstOrDefaultAsync();
                    var grossSalary = await _context.salaryAssignHeaders.Where(x => x.IntEmployeeId == salaryAdditionNDeductionVM.IntEmployeeId).Select(x => x.NumGrossSalary).FirstOrDefaultAsync();
                    var amount = salaryAdditionNDeductionVM.NumAmount ?? 0;
                    var hasSingleDirection = salaryAdditionNDeductionVM.IsAddition == true ^ salaryAdditionNDeductionVM.IsDeduction == true;
                    var hasValidAmount = amount > 0 && (salaryAdditionNDeductionVM.IsAddition == true || grossSalary > amount);

                    if (empExst != null && businessUnit != null && hasSingleDirection && hasValidAmount)
                    {
                        SalaryAdditionNDeduction salaryAdditionNDeduction = new SalaryAdditionNDeduction
                        {
                            IntSalaryAdditionAndDeductionId = salaryAdditionNDeductionVM.IntSalaryAdditionAndDeductionId,
                            IntBusinessUnitId = salaryAdditionNDeductionVM.IntBusinessUnitId,
                            IntEmployeeId = salaryAdditionNDeductionVM.IntEmployeeId,
                            IntYear = salaryAdditionNDeductionVM.IntYear,
                            IntMonth = salaryAdditionNDeductionVM.IntMonth,
                            IsAddition = salaryAdditionNDeductionVM.IsAddition,
                            IsDeduction = salaryAdditionNDeductionVM.IsDeduction,
                            IntAdditionNdeductionTypeId = salaryAdditionNDeductionVM.IntAdditionNdeductionTypeId,
                            NumAmount = salaryAdditionNDeductionVM.NumAmount,
                            IsActive = true,
                            IntCreatedBy = salaryAdditionNDeductionVM.IntCreatedBy,
                            DteCreatedAt = DateTime.Now
                        };

                        await _context.salaryAdditionNDeductions.AddAsync(salaryAdditionNDeduction);
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

        public async Task<bool> UpdateSalaryAdditionNDeduction(SalaryAdditionNDeductionVM salaryAdditionNDeductionVM)
        {
            try
            {
                var data = await _context.salaryAdditionNDeductions
                    .Where(x => x.IntSalaryAdditionAndDeductionId == salaryAdditionNDeductionVM.IntSalaryAdditionAndDeductionId)
                    .FirstOrDefaultAsync();
                var businessUnit = await _context.businessUnits.Where(x => x.IntBusinessUnitId == salaryAdditionNDeductionVM.IntBusinessUnitId).FirstOrDefaultAsync();
                var empExst = await _context.empBasicInfos.Where(x => x.IntEmployeeBasicInfoId == salaryAdditionNDeductionVM.IntEmployeeId).FirstOrDefaultAsync();
                var grossSalary = await _context.salaryAssignHeaders.Where(x => x.IntEmployeeId == salaryAdditionNDeductionVM.IntEmployeeId && x.IsActive == true).Select(x => x.NumGrossSalary).FirstOrDefaultAsync();
                var amount = salaryAdditionNDeductionVM.NumAmount ?? 0;
                var hasSingleDirection = salaryAdditionNDeductionVM.IsAddition == true ^ salaryAdditionNDeductionVM.IsDeduction == true;
                var hasValidAmount = amount > 0 && (salaryAdditionNDeductionVM.IsAddition == true || grossSalary > amount);

                if (data == null || businessUnit == null || empExst == null || !hasSingleDirection || !hasValidAmount)
                {
                    return false;
                }

                data.IntBusinessUnitId = salaryAdditionNDeductionVM.IntBusinessUnitId;
                data.IntEmployeeId = salaryAdditionNDeductionVM.IntEmployeeId;
                data.IntYear = salaryAdditionNDeductionVM.IntYear;
                data.IntMonth = salaryAdditionNDeductionVM.IntMonth;
                data.IsAddition = salaryAdditionNDeductionVM.IsAddition;
                data.IsDeduction = salaryAdditionNDeductionVM.IsDeduction;
                data.IntAdditionNdeductionTypeId = salaryAdditionNDeductionVM.IntAdditionNdeductionTypeId;
                data.NumAmount = salaryAdditionNDeductionVM.NumAmount;
                data.IntUpdatedBy = salaryAdditionNDeductionVM.IntUpdatedBy;
                data.DteUpdatedAt = DateTime.Now;

                _context.salaryAdditionNDeductions.Update(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteSalaryAdditionNDeduction(long adjustmentId)
        {
            try
            {
                var data = await _context.salaryAdditionNDeductions.Where(x => x.IntSalaryAdditionAndDeductionId == adjustmentId).FirstOrDefaultAsync();
                if (data == null)
                {
                    return false;
                }

                data.IsActive = false;
                data.DteUpdatedAt = DateTime.Now;
                _context.salaryAdditionNDeductions.Update(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<SalaryAdjustmentLanding>> SalaryAdjustmentLanding(long businessUnitId, long employeeId, long yearId, long monthId)
        {
            try
            {
                IQueryable<SalaryAdjustmentLanding> query = from adjustment in _context.salaryAdditionNDeductions
                                                            join emp in _context.empBasicInfos on adjustment.IntEmployeeId equals emp.IntEmployeeBasicInfoId into e2
                                                            from employee in e2.DefaultIfEmpty()
                                                            join bus in _context.businessUnits on adjustment.IntBusinessUnitId equals bus.IntBusinessUnitId into b2
                                                            from business in b2.DefaultIfEmpty()
                                                            where adjustment.IsActive == true
                                                            select new SalaryAdjustmentLanding
                                                            {
                                                                IntSalaryAdditionAndDeductionId = adjustment.IntSalaryAdditionAndDeductionId,
                                                                IntBusinessUnitId = adjustment.IntBusinessUnitId,
                                                                StrBusinessUnitName = business.StrBusinessUnitName,
                                                                IntEmployeeId = adjustment.IntEmployeeId,
                                                                StrEmployeeName = employee.StrEmployeeName,
                                                                IntYear = adjustment.IntYear,
                                                                IntMonth = adjustment.IntMonth,
                                                                IsAddition = adjustment.IsAddition,
                                                                IsDeduction = adjustment.IsDeduction,
                                                                IntAdditionNdeductionTypeId = adjustment.IntAdditionNdeductionTypeId,
                                                                NumAmount = adjustment.NumAmount
                                                            };

                if (businessUnitId > 0)
                {
                    query = query.Where(x => x.IntBusinessUnitId == businessUnitId);
                }

                if (employeeId > 0)
                {
                    query = query.Where(x => x.IntEmployeeId == employeeId);
                }

                if (yearId > 0)
                {
                    query = query.Where(x => x.IntYear == yearId);
                }

                if (monthId > 0)
                {
                    query = query.Where(x => x.IntMonth == monthId);
                }

                return await query.OrderByDescending(x => x.IntYear)
                    .ThenByDescending(x => x.IntMonth)
                    .ThenBy(x => x.StrEmployeeName)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<EmpSalAddNDeductionVM>> EmpSalAddNDeductionLanding(long employeeId)
        {
            try
            {
                IQueryable<EmpSalAddNDeductionVM> empSalAddNDeductionVMs = (from ad in _context.salaryAdditionNDeductions
                                                                            join basic in _context.empBasicInfos on ad.IntEmployeeId equals basic.IntEmployeeBasicInfoId into b2
                                                                            from emp in b2.DefaultIfEmpty()
                                                                            join type in _context.payrollElements on ad.IntAdditionNdeductionTypeId equals type.IntPayrollElementId into t2
                                                                            from element in t2.DefaultIfEmpty()

                                                                            where emp.IsActive == true && ad.IntEmployeeId == employeeId
                                                                            select new EmpSalAddNDeductionVM
                                                                            {
                                                                                IntEmployeeId = emp.IntEmployeeBasicInfoId,
                                                                                StrEmployeeName = emp.StrEmployeeName,
                                                                                IntYear = ad.IntYear,
                                                                                IntMonth = ad.IntMonth,
                                                                                Addition = (from a in _context.salaryAdditionNDeductions
                                                                                            where a.IsAddition == true && a.IntAdditionNdeductionTypeId == ad.IntAdditionNdeductionTypeId
                                                                                            select new SalAddition
                                                                                            {
                                                                                                IntAddDeductTypeId = a.IntAdditionNdeductionTypeId,
                                                                                                StrAdddeductionName = element.StrPayrollElementName,
                                                                                                NumAmount = (decimal)ad.NumAmount
                                                                                            }).ToList(),
                                                                                Deduction = (from d in _context.salaryAdditionNDeductions
                                                                                             where d.IsDeduction == true && d.IntAdditionNdeductionTypeId == ad.IntAdditionNdeductionTypeId
                                                                                             select new SalDeduction
                                                                                             {
                                                                                                 IntAddDeductTypeId = d.IntAdditionNdeductionTypeId,
                                                                                                 StrAdddeductionName = element.StrPayrollElementName,
                                                                                                 NumAmount = (decimal)ad.NumAmount
                                                                                             }).ToList()
                                                                            }).OrderBy(x => x.IntEmployeeId).AsNoTracking().AsQueryable();

                return await empSalAddNDeductionVMs.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion======================= Salary Addition & Deduction ===================================================


        #region========================= Salary Generate ================================================================
        public async Task<List<SalaryGenerateLandingVM>> SalaryGenerateLanding(long intYearId, long intMonthId)
        {
            try
            {
                var salaryNotHoldEmps = await _context.empBasicInfos.Where(x => x.IsSalaryHold == false && x.IsActive == true).Select(x => x.IntEmployeeBasicInfoId).ToListAsync();

                var totalEmployee = _context.salaryAssignHeaders
                                            .Where(x => x.IsActive == true && salaryNotHoldEmps.Contains(x.IntEmployeeId))
                                            .GroupBy(x => x.IntBusinessUnitId)
                                            .Select(g => new
                                            {
                                                BusinessUnitId = g.Key,
                                                TotalEmployees = g.Select(x => x.IntEmployeeId).Distinct().Count()
                                            })
                                            .ToList();

                var totalGrossSalary = _context.salaryAssignHeaders
                                                .Where(x => x.IsActive == true && salaryNotHoldEmps.Contains(x.IntEmployeeId))
                                                .GroupBy(x => x.IntBusinessUnitId)
                                                .Select(g => new
                                                {
                                                    BusinessUnitId = g.Key,
                                                    TotalGrossSalary = g.Sum(x => x.NumGrossSalary)
                                                })
                                                .ToList();

                var totalAddition = _context.salaryAdditionNDeductions
                                            .Where(x => x.IsActive == true && x.IsAddition == true && x.IntYear == intYearId && x.IntMonth == intMonthId)
                                            .GroupBy(x => x.IntBusinessUnitId)
                                            .Select(a => new
                                            {
                                                BusinessUnitId = a.Key,
                                                TotalAddition = a.Sum(x => x.NumAmount)
                                            })
                                            .ToList();

                var totalDeduction = _context.salaryAdditionNDeductions
                                            .Where(x => x.IsActive == true && x.IsDeduction == true && x.IntYear == intYearId && x.IntMonth == intMonthId)
                                            .GroupBy(x => x.IntBusinessUnitId)
                                            .Select(a => new
                                            {
                                                BusinessUnitId = a.Key,
                                                TotalDeduction = a.Sum(x => x.NumAmount)
                                            })
                                            .ToList();

                var salaryGenerateLandingVMs = new List<SalaryGenerateLandingVM>();

                foreach (var gross in totalGrossSalary)
                {
                    var emp = totalEmployee.FirstOrDefault(x => x.BusinessUnitId == gross.BusinessUnitId);
                    var add = totalAddition.FirstOrDefault(x => x.BusinessUnitId == gross.BusinessUnitId);
                    var ded = totalDeduction.FirstOrDefault(x => x.BusinessUnitId == gross.BusinessUnitId);

                    SalaryGenerateLandingVM salaryGenerateLandingVM = new SalaryGenerateLandingVM
                    {
                        IntBusinessUnitId = gross.BusinessUnitId,
                        StrBusinessUnitName = _context.businessUnits.FirstOrDefault(x => x.IntBusinessUnitId == gross.BusinessUnitId)?.StrBusinessUnitName ?? "",
                        IntTotalEmployee = emp?.TotalEmployees ?? 0,
                        NumTotalPaybleSalary = gross.TotalGrossSalary + (add?.TotalAddition ?? 0) - (ded?.TotalDeduction ?? 0)
                    };
                    salaryGenerateLandingVMs.Add(salaryGenerateLandingVM);
                }

                return salaryGenerateLandingVMs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion========================= Salary Generate ================================================================

    }
}
