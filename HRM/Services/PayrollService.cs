using HRM.Data;
using HRM.DTOs;
using HRM.Interfaces;
using HRM.Models;
using HRM.Models.Payroll;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly HRMContext _context;
        public PayrollService(HRMContext context)
        {
            _context = context;
        }


        #region======================= Payroll Policy =====================================================
        public async Task<bool> CreatePayrollPolicy(PayrollPolicyCreateVM payrollPolicyCreateVM)
        {
            try
            {
                var data = await _context.payrollPolicies.Where(x => x.IntPayrollPolicyId == payrollPolicyCreateVM.IntPayrollPolicyId).FirstOrDefaultAsync();
                var businessUnit = await _context.businessUnits.Where(x => x.IntBusinessUnitId == payrollPolicyCreateVM.IntBusinessUnitId).FirstOrDefaultAsync();
                var nameExst = await _context.payrollPolicies.Where(x => x.StrPayrollPolicyName == payrollPolicyCreateVM.StrPayrollPolicyName).FirstOrDefaultAsync();

                if (data == null && businessUnit != null && nameExst == null)
                {
                    PayrollPolicy payrollPolicy = new PayrollPolicy
                    {
                        IntPayrollPolicyId = payrollPolicyCreateVM.IntPayrollPolicyId,
                        IntBusinessUnitId = payrollPolicyCreateVM.IntBusinessUnitId,
                        StrPayrollPolicyName = payrollPolicyCreateVM.StrPayrollPolicyName,
                        IsSalaryDivideByActualMonthDays = payrollPolicyCreateVM.IsSalaryDivideByActualMonthDays,
                        IntGrossSalaryDevidedByDays = payrollPolicyCreateVM.IntGrossSalaryDevidedByDays,
                        IntGrossSalaryRoundDigits = payrollPolicyCreateVM.IntGrossSalaryRoundDigits,
                        IsGrossSalaryRoundUp = payrollPolicyCreateVM.IsGrossSalaryRoundUp,
                        IsGrossSalaryRoundDown = payrollPolicyCreateVM.IsGrossSalaryRoundDown,
                        IntNetPayableSalaryRoundDigits = payrollPolicyCreateVM.IntNetPayableSalaryRoundDigits,
                        IsNetPayableSalaryRoundUp = payrollPolicyCreateVM.IsNetPayableSalaryRoundUp,
                        IsNetPayableSalaryRoundDown = payrollPolicyCreateVM.IsNetPayableSalaryRoundDown,
                        IsActive = true,
                        IntCreatedBy = payrollPolicyCreateVM.IntCreatedBy,
                        DteCreatedAt = DateTime.Now
                    };

                    await _context.payrollPolicies.AddAsync(payrollPolicy);
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

        public async Task<bool> UpdatePolicy(PayrollPolicyCreateVM payrollPolicyCreateVM)
        {
            try
            {
                var exst = await _context.payrollPolicies.Where(x => x.IntPayrollPolicyId == payrollPolicyCreateVM.IntPayrollPolicyId && x.StrPayrollPolicyName != payrollPolicyCreateVM.StrPayrollPolicyName).FirstOrDefaultAsync();
                var businessUnit = await _context.businessUnits.Where(x => x.IntBusinessUnitId == payrollPolicyCreateVM.IntBusinessUnitId).FirstOrDefaultAsync();

                if (exst == null && businessUnit != null)
                {
                    PayrollPolicy data = await _context.payrollPolicies.Where(x => x.IntPayrollPolicyId == payrollPolicyCreateVM.IntPayrollPolicyId).FirstOrDefaultAsync();
                    if (data != null)
                    {
                        data.IntBusinessUnitId = payrollPolicyCreateVM.IntBusinessUnitId;
                        data.StrPayrollPolicyName = payrollPolicyCreateVM.StrPayrollPolicyName;
                        data.IsSalaryDivideByActualMonthDays = payrollPolicyCreateVM.IsSalaryDivideByActualMonthDays;
                        data.IntGrossSalaryDevidedByDays = payrollPolicyCreateVM.IntGrossSalaryDevidedByDays;
                        data.IntGrossSalaryRoundDigits = payrollPolicyCreateVM.IntGrossSalaryRoundDigits;
                        data.IsGrossSalaryRoundUp = payrollPolicyCreateVM?.IsGrossSalaryRoundUp;
                        data.IsGrossSalaryRoundDown = payrollPolicyCreateVM.IsGrossSalaryRoundDown;
                        data.IntNetPayableSalaryRoundDigits = payrollPolicyCreateVM.IntNetPayableSalaryRoundDigits;
                        data.IsNetPayableSalaryRoundUp = payrollPolicyCreateVM.IsNetPayableSalaryRoundUp;
                        data.IsNetPayableSalaryRoundDown = payrollPolicyCreateVM.IsNetPayableSalaryRoundDown;
                        data.IntCreatedBy = payrollPolicyCreateVM.IntCreatedBy;
                        data.DteUpdatedAt = DateTime.Now;
                        data.IntUpdatedBy = payrollPolicyCreateVM.IntUpdatedBy;
                    }
                    _context.payrollPolicies.Update(data);
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

        public async Task<bool> DeletePolicy(long policyId)
        {
            try
            {
                PayrollPolicy data = await _context.payrollPolicies.Where(x => x.IntPayrollPolicyId == policyId).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.IsActive = false;

                    _context.payrollPolicies.Update(data);
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

        public Task<List<PayrollPolicyLanding>> PayrollPolicyLanding(long businessUnitId)
        {
            try
            {

                IQueryable<PayrollPolicyLanding> payrollPolicyLandings = (from payroll in _context.payrollPolicies
                                                                  join bus in _context.businessUnits on payroll.IntBusinessUnitId equals bus.IntBusinessUnitId into b2
                                                                  from business in b2.DefaultIfEmpty()

                                                                  where payroll.IsActive == true && payroll.IntBusinessUnitId == businessUnitId
                                                                  select new PayrollPolicyLanding
                                                                  {
                                                                    IntPayrollPolicyId = payroll.IntPayrollPolicyId,
                                                                    StrPayrollPolicyName = payroll.StrPayrollPolicyName,
                                                                    StrBusinessUnitName = business.StrBusinessUnitName,
                                                                    IsSalaryDivideByActualMonthDays = payroll.IsSalaryDivideByActualMonthDays,
                                                                    IntGrossSalaryDevidedByDays = payroll.IntGrossSalaryDevidedByDays,
                                                                    IsGrossSalaryRoundUp = payroll.IsGrossSalaryRoundUp,
                                                                    IsGrossSalaryRoundDown = payroll.IsGrossSalaryRoundDown,
                                                                    IntNetPayableSalaryRoundDigits = payroll.IntNetPayableSalaryRoundDigits,
                                                                    IsNetPayableSalaryRoundUp = payroll.IsNetPayableSalaryRoundUp,
                                                                    IsNetPayableSalaryRoundDown = payroll.IsNetPayableSalaryRoundDown,
                                                                    IntCreatedBy = payroll.IntCreatedBy,
                                                                    StrCreatedAt = payroll.DteCreatedAt.Value.ToString("dd MMM yyyy"),
                                                                    IntUpdatedBy = payroll.IntUpdatedBy,
                                                                    StrUpdatedBy = payroll.DteUpdatedAt.Value.ToString("dd MMM yyyy") ?? ""
                                                                  }).OrderBy(x => x.IntPayrollPolicyId).AsNoTracking().AsQueryable();

                return Task.FromResult(payrollPolicyLandings.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion================================== Payroll Policy ==============================================================



        #region===================================== Payroll Element =============================================================

        public async Task<bool> CreatePayrollElement(PayrollElementCreateVM payrollElementCreateVM)
        {
            try
            {
                var data = await _context.payrollElements.Where(x => x.IntPayrollElementId == payrollElementCreateVM.IntPayrollElementId).FirstOrDefaultAsync();
                var businessUnit = await _context.businessUnits.Where(x => x.IntBusinessUnitId == payrollElementCreateVM.IntBusinessUnitId).FirstOrDefaultAsync();
                var nameExst = await _context.payrollElements.Where(x => x.StrPayrollElementName == payrollElementCreateVM.StrPayrollElementName).FirstOrDefaultAsync();

                if (data == null && businessUnit != null && nameExst == null)
                {
                    PayrollElement payrollElement = new PayrollElement
                    {
                        IntPayrollElementId = payrollElementCreateVM.IntPayrollElementId,
                        StrPayrollElementName = payrollElementCreateVM.StrPayrollElementName,
                        IntBusinessUnitId = payrollElementCreateVM.IntBusinessUnitId,
                        IsBasicElement = payrollElementCreateVM.IsBasicElement,
                        IsSalaryElement = payrollElementCreateVM.IsSalaryElement,
                        IsActive = true,
                        DteCreatedAt = DateTime.Now,
                        IntCreatedBy = payrollElementCreateVM.IntCreatedBy
                    };

                    await _context.payrollElements.AddAsync(payrollElement);
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

        public async Task<bool> UpdatePayrollElement(PayrollElementCreateVM payrollElementCreateVM)
        {
            try
            {
                var nameExst = await _context.payrollElements.Where(x => x.IntPayrollElementId == payrollElementCreateVM.IntPayrollElementId && x.StrPayrollElementName != payrollElementCreateVM.StrPayrollElementName).FirstOrDefaultAsync();
                var businessUnit = await _context.businessUnits.Where(x => x.IntBusinessUnitId == payrollElementCreateVM.IntBusinessUnitId).FirstOrDefaultAsync();

                if (nameExst == null && businessUnit != null)
                {
                    PayrollElement data = await _context.payrollElements.Where(x => x.IntPayrollElementId == payrollElementCreateVM.IntPayrollElementId).FirstOrDefaultAsync();
                    if (data != null)
                    {
                        data.IntBusinessUnitId = payrollElementCreateVM.IntBusinessUnitId;
                        data.StrPayrollElementName = payrollElementCreateVM.StrPayrollElementName;
                        data.IsBasicElement = payrollElementCreateVM.IsBasicElement;
                        data.IsSalaryElement = payrollElementCreateVM.IsSalaryElement;
                        data.IntCreatedBy = payrollElementCreateVM.IntCreatedBy;
                        data.DteUpdatedAt = DateTime.Now;
                        data.IntUpdatedBy = payrollElementCreateVM.IntUpdatedBy;
                    }
                    _context.payrollElements.Update(data);
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

        public async Task<bool> DeletePayrollElement(long elementId)
        {
            try
            {
                PayrollElement data = await _context.payrollElements.Where(x => x.IntPayrollElementId == elementId).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.IsActive = false;

                    _context.payrollElements.Update(data);
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
        public Task<List<PayrollElementLanding>> PayrollElementLanding(long businessUnitId)
        {
            try
            {

                IQueryable<PayrollElementLanding> payrollElementLandings = (from payrollElement in _context.payrollElements
                                                                          join bus in _context.businessUnits on payrollElement.IntBusinessUnitId equals bus.IntBusinessUnitId into b2
                                                                          from business in b2.DefaultIfEmpty()

                                                                          where payrollElement.IsActive == true && payrollElement.IntBusinessUnitId == businessUnitId
                                                                          select new PayrollElementLanding
                                                                          {
                                                                              IntPayrollElementId = payrollElement.IntPayrollElementId,
                                                                              StrPayrollElementName = payrollElement.StrPayrollElementName,
                                                                              StrBusinessUnitName = business.StrBusinessUnitName,
                                                                              IsBasicElement = payrollElement.IsBasicElement,
                                                                              IsSalaryElement = payrollElement.IsSalaryElement,
                                                                              IsActive = payrollElement.IsActive,
                                                                              DteCreatedAt = payrollElement.DteCreatedAt.ToString("dd MMM yyyy"),
                                                                              IntCreatedBy = payrollElement.IntCreatedBy,
                                                                              DteUpdatedAt = payrollElement.DteUpdatedAt.Value.ToString("dd MMM yyyy") ?? "",
                                                                              IntUpdatedBy = payrollElement.IntUpdatedBy

                                                                          }).OrderBy(x => x.IntPayrollElementId).AsNoTracking().AsQueryable();

                return Task.FromResult(payrollElementLandings.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion================================== Payroll Element =============================================================


        #region===================================== Payroll Group ===============================================================

        public async Task<bool> CreatePayrollGroupHeaderNRow(PayrollGroupHeaderNRowCreateVM payrollGroupHeaderNRowCreateVM)
        {
            try 
            {

                var data = await _context.payrollGroupHeaders.Where(x => x.IntPayrollGroupHeaderId == payrollGroupHeaderNRowCreateVM.IntPayrollGroupHeaderId).FirstOrDefaultAsync();
                var businessUnit = await _context.businessUnits.Where(x => x.IntBusinessUnitId == payrollGroupHeaderNRowCreateVM.IntBusinessUnitId).FirstOrDefaultAsync();
                var nameExst = await _context.payrollGroupHeaders.Where(x => x.StrPayrollGroupHeaderTitle == payrollGroupHeaderNRowCreateVM.StrPayrollGroupHeaderTitle).FirstOrDefaultAsync();
                var policyExst = await _context.payrollPolicies.Where(x => x.IntPayrollPolicyId == payrollGroupHeaderNRowCreateVM.IntPayrollPolicyId && x.IntBusinessUnitId == payrollGroupHeaderNRowCreateVM.IntBusinessUnitId).FirstOrDefaultAsync();

                //Payroll Element Type Check
                var elementTypeIds = await _context.payrollElements.Where(x => x.IntBusinessUnitId == payrollGroupHeaderNRowCreateVM.IntBusinessUnitId && x.IsSalaryElement == true).Select(x => x.IntPayrollElementId).ToListAsync();

                if (data == null && businessUnit != null && nameExst == null && policyExst != null)
                {
                    PayrollGroupHeader payrollGroupHeader = new PayrollGroupHeader
                    {
                        IntPayrollGroupHeaderId = payrollGroupHeaderNRowCreateVM.IntPayrollGroupHeaderId,
                        StrPayrollGroupHeaderTitle = payrollGroupHeaderNRowCreateVM.StrPayrollGroupHeaderTitle,
                        IntBusinessUnitId = payrollGroupHeaderNRowCreateVM.IntBusinessUnitId,
                        IntPayrollPolicyId = payrollGroupHeaderNRowCreateVM.IntPayrollPolicyId,
                        IsActive = true,
                        DteCreatedAt = DateTime.Now,
                        IntCreatedBy = payrollGroupHeaderNRowCreateVM.IntCreatedBy
                    };

                    await _context.payrollGroupHeaders.AddAsync(payrollGroupHeader);
                    await _context.SaveChangesAsync();

                    List<PayrollGroupRow> payrollGroupRows = new List<PayrollGroupRow>();
                    bool check = true;

                    payrollGroupHeaderNRowCreateVM.payrollGroupRowList.ForEach(prg =>
                    {
                        if (!elementTypeIds.Contains(prg.IntPayrollElementTypeId))
                        {
                            check = false;
                            return;
                        }
                            payrollGroupRows.Add(new PayrollGroupRow
                            {
                                IntPayrollGroupRowId = prg.IntPayrollGroupRowId,
                                IntPayrollHeaderId = payrollGroupHeader.IntPayrollGroupHeaderId,
                                IntPayrollElementTypeId = prg.IntPayrollElementTypeId,
                                StrPayrollElementName = prg.StrPayrollElementName,
                                NumNumberOfPercent = prg.NumNumberOfPercent,
                                IsActive = true,
                                DteCreatedAt = DateTime.Now,
                                IntCreatedBy = payrollGroupHeaderNRowCreateVM.IntCreatedBy
                            });
                    });


                    if (check == true)
                    {
                        if (payrollGroupRows.Select(x => x.NumNumberOfPercent).Sum() != 100) 
                        {
                            var header = _context.payrollGroupHeaders.Where(x => x.IntPayrollGroupHeaderId == payrollGroupHeader.IntPayrollGroupHeaderId).FirstOrDefault();
                            _context.payrollGroupHeaders.Remove(header);
                            return false;
                        }

                        await _context.payrollGroupRows.AddRangeAsync(payrollGroupRows);
                        await _context.SaveChangesAsync();
                        return true;
                    }

                    var header2 = _context.payrollGroupHeaders.Where(x => x.IntPayrollGroupHeaderId == payrollGroupHeader.IntPayrollGroupHeaderId).FirstOrDefault();
                    _context.payrollGroupHeaders.Remove(header2);
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> UpdatePayrollGroupHeaderNRow(PayrollGroupHeaderNRowCreateVM payrollGroupHeaderNRowCreateVM)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeletePayrollGroupHeaderNRow(long headerId)
        {
            try
            {
                PayrollGroupHeader payrollGroupHeader = await _context.payrollGroupHeaders.Where(x => x.IntPayrollGroupHeaderId == headerId).FirstOrDefaultAsync();
                List<PayrollGroupRow> payrollGroupRows = await _context.payrollGroupRows.Where(x => x.IntPayrollHeaderId == headerId).ToListAsync();
                if (payrollGroupHeader != null)
                {
                    payrollGroupHeader.IsActive = false;

                    _context.payrollGroupHeaders.Update(payrollGroupHeader);
                    await _context.SaveChangesAsync();

                    foreach (var item in payrollGroupRows) 
                    {
                        item.IsActive = false;

                        _context.payrollGroupRows.Update(item);
                        await _context.SaveChangesAsync();
                    }
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

        public async Task<List<PayrollHeaderLanding>> PayrollHeaderLanding(long businessUnitId)
        {
            try
            {

                IQueryable<PayrollHeaderLanding> payrollHeaderLandings = (from header in _context.payrollGroupHeaders
                                                                          join bus in _context.businessUnits on header.IntBusinessUnitId equals bus.IntBusinessUnitId into b2
                                                                          from business in b2.DefaultIfEmpty()
                                                                          join pa in _context.payrollPolicies on header.IntPayrollPolicyId equals pa.IntPayrollPolicyId into p2
                                                                          from policy in p2.DefaultIfEmpty()

                                                                          where header.IsActive == true && header.IntBusinessUnitId == businessUnitId
                                                                            select new PayrollHeaderLanding
                                                                            {
                                                                                IntPayrollGroupHeaderId = header.IntPayrollGroupHeaderId,
                                                                                StrPayrollGroupHeaderTitle = header.StrPayrollGroupHeaderTitle,
                                                                                StrBusinessUnitName = business.StrBusinessUnitName,
                                                                                StrPayrollPolicyName = policy.StrPayrollPolicyName,
                                                                                IsActive = header.IsActive,
                                                                                StrCreatedAt = header.DteCreatedAt.ToString("dd MMM yyyy"),
                                                                                IntCreatedBy = header.IntCreatedBy,
                                                                                StrUpdatedAt = header.DteUpdatedAt.Value.ToString("dd MMM yyyy") ?? "",
                                                                                IntUpdatedBy = header.IntUpdatedBy
                                                                            }).OrderBy(x => x.IntPayrollGroupHeaderId).AsNoTracking().AsQueryable();

                return await payrollHeaderLandings.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<List<PayrollRowLanding>> PayrollRowLanding(long headerId)
        {
            try
            {

                IQueryable<PayrollRowLanding> payrollRowLandings = (from row in _context.payrollGroupRows
                                                                          join head in _context.payrollGroupHeaders on row.IntPayrollHeaderId equals head.IntPayrollGroupHeaderId into h2
                                                                          from header in h2.DefaultIfEmpty()

                                                                          where row.IsActive == true && header.IntPayrollGroupHeaderId == headerId 
                                                                            select new PayrollRowLanding
                                                                            {
                                                                                IntPayrollGroupRowId = row.IntPayrollGroupRowId,
                                                                                IntPayrollElementTypeId = row.IntPayrollElementTypeId,
                                                                                StrPayrollElementName = row.StrPayrollElementName,
                                                                                NumNumberOfPercent = row.NumNumberOfPercent,
                                                                                IsActive = row.IsActive,
                                                                                StrCreatedAt = row.DteCreatedAt.ToString("dd MMM yyyy"),
                                                                                IntCreatedBy = row.IntCreatedBy,
                                                                                StrUpdatedAt = row.DteUpdatedAt.Value.ToString("dd MMM yyyy"),
                                                                                IntUpdatedBy = row.IntUpdatedBy
                                                                            }).OrderBy(x => x.IntPayrollGroupRowId).AsNoTracking().AsQueryable();

                return await payrollRowLandings.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion================================== Payroll Group ===============================================================
    }
}
