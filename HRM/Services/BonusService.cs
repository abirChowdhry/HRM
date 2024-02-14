using HRM.Data;
using HRM.DTOs;
using HRM.Interfaces;
using HRM.Migrations;
using HRM.Models;
using HRM.Models.Bonus;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace HRM.Services
{
    public class BonusService : IBonusService
    {
        HRMContext _context;
        public BonusService(HRMContext context) { _context = context; }


        #region=============================== Bonus Setup ================================================
        public async Task<bool> CreateBonusSetup(BonusSetupCreateVM bonusSetupCreateVM)
        {
            try
            {
                var businessUnit = await _context.businessUnits.Where(x => x.IntBusinessUnitId == bonusSetupCreateVM.IntBusinessUnitId).FirstOrDefaultAsync();
                var dept = await _context.departments.Where(x => x.IntDepartmentId == bonusSetupCreateVM.IntDepartmentId).FirstOrDefaultAsync();
                var empType = await _context.employementTypes.Where(x => x.IntEmployementId == bonusSetupCreateVM.IntEmployementTypeId).FirstOrDefaultAsync();
                var nameExst = await _context.bonusSetups.Where(x => x.StrBonusSetupName == bonusSetupCreateVM.StrBonusSetupName).FirstOrDefaultAsync();

                if (businessUnit != null && (dept != null || empType != null || bonusSetupCreateVM.StrReligion != null || bonusSetupCreateVM.IntServiceLengthMonths != null) && nameExst == null)
                {
                    BonusSetup bonusSetup = new BonusSetup
                    {
                        IntBonusSetypId = bonusSetupCreateVM.IntBonusSetypId,
                        StrBonusSetupName = bonusSetupCreateVM.StrBonusSetupName,
                        IntBusinessUnitId = bonusSetupCreateVM.IntBusinessUnitId,
                        IntDepartmentId = bonusSetupCreateVM.IntDepartmentId,
                        IntServiceLengthMonths = bonusSetupCreateVM.IntServiceLengthMonths,
                        StrReligion = bonusSetupCreateVM.StrReligion,
                        NumPercentage = bonusSetupCreateVM.NumPercentage,
                        IsActive = true,
                        IntCreatedBy = bonusSetupCreateVM.IntCreatedBy,
                        DteCreatedAt = DateTime.Now,
                        IntUpdatedBy = bonusSetupCreateVM.IntUpdatedBy
                    };

                    await _context.bonusSetups.AddAsync(bonusSetup);
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
        
        
        public async Task<bool> UpdateBonusSetup(BonusSetupCreateVM bonusSetupCreateVM)
        {
            try
            {
                var businessUnit = await _context.businessUnits.Where(x => x.IntBusinessUnitId == bonusSetupCreateVM.IntBusinessUnitId).FirstOrDefaultAsync();
                var dept = await _context.departments.Where(x => x.IntDepartmentId == bonusSetupCreateVM.IntDepartmentId).FirstOrDefaultAsync();
                var empType = await _context.employementTypes.Where(x => x.IntEmployementId == bonusSetupCreateVM.IntEmployementTypeId).FirstOrDefaultAsync();
                var exst = await _context.bonusSetups.Where(x => x.IntBonusSetypId != bonusSetupCreateVM.IntBonusSetypId && x.StrBonusSetupName == bonusSetupCreateVM.StrBonusSetupName).FirstOrDefaultAsync();

                var data = await _context.bonusSetups.Where(x => x.IntBonusSetypId == bonusSetupCreateVM.IntBonusSetypId).FirstOrDefaultAsync();

                if (businessUnit != null && (dept != null || empType != null || bonusSetupCreateVM.StrReligion != null || bonusSetupCreateVM.IntServiceLengthMonths != null) && data != null && exst == null)
                {
                    data.StrBonusSetupName = bonusSetupCreateVM.StrBonusSetupName;
                    data.IntBusinessUnitId = bonusSetupCreateVM.IntBusinessUnitId;
                    data.IntDepartmentId = bonusSetupCreateVM.IntDepartmentId;
                    data.IntServiceLengthMonths = bonusSetupCreateVM.IntServiceLengthMonths;
                    data.StrReligion = bonusSetupCreateVM.StrReligion;
                    data.NumPercentage = bonusSetupCreateVM.NumPercentage;
                    data.IntCreatedBy = bonusSetupCreateVM.IntCreatedBy;
                    data.IntUpdatedBy = bonusSetupCreateVM.IntUpdatedBy;
                    data.DteUpdatedAt = DateTime.Now;

                    _context.bonusSetups.Update(data);
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

        public async Task<List<BonusLanding>> BonusLanding(long intBusinessUnitId)
        {
            try 
            {
                IQueryable<BonusLanding> bonusLandings = (from bn in _context.bonusSetups
                                                          join bus in _context.businessUnits on bn.IntBusinessUnitId equals bus.IntBusinessUnitId into bus2
                                                          from business in bus2.DefaultIfEmpty()
                                                          join dep in _context.departments on bn.IntDepartmentId equals dep.IntDepartmentId into dep2
                                                          from dept in dep2.DefaultIfEmpty()
                                                          join empType in _context.employementTypes on bn.IntEmployementTypeId equals empType.IntEmployementId into empType2
                                                          from emp in empType2.DefaultIfEmpty()

                                                          where bn.IntBusinessUnitId == intBusinessUnitId

                                                          select new BonusLanding
                                                          {
                                                              IntBonusSetypId = bn.IntBonusSetypId,
                                                              StrBonusSetupName = bn.StrBonusSetupName,
                                                              IntBusinessUnitId = bn.IntBusinessUnitId,
                                                              StrBusinessUnitName = business.StrBusinessUnitName,
                                                              IntDepartmentId = bn.IntDepartmentId,
                                                              StrDepartmentName = dept.StrDepartmentName,
                                                              IntEmployementTypeId = bn.IntEmployementTypeId,
                                                              StrEmployementType = emp.StrEmployementName,
                                                              StrReligion = bn.StrReligion,
                                                              IntServiceLengthMonths = bn.IntServiceLengthMonths,
                                                              NumPercentage = bn.NumPercentage,
                                                              IsACTIVE = bn.IsActive,
                                                              IntCreatedBy = bn.IntCreatedBy,
                                                              StrCreatedAt = bn.DteCreatedAt.ToString("dd MMM yyyy"),
                                                              IntUpdatedBy = bn.IntUpdatedBy,
                                                              StrUpdatedAt = bn.DteUpdatedAt.Value.ToString("dd MMM yyyy")
                                                          });

                return await bonusLandings.ToListAsync();
            }
            catch (Exception ex) 
            { 
                throw ex; 
            }
        }

        #endregion=============================== Bonus Setup ================================================


        #region================================== Bonus Generate ================================================
        public async Task<bool> CreateBonusGenerate(BonusGenerateCreate bonusGenerateCreate)
        {
            try
            {
                var exstBonus = await _context.bonusSetups.Where(x => x.IntBonusSetypId == bonusGenerateCreate.IntBonusSetupId).FirstOrDefaultAsync();

                if (exstBonus != null)
                {

                    BonusGenerate bonusGenerate = new BonusGenerate
                    {
                        IntBonusGenerateId = bonusGenerateCreate.IntBonusGenerateId,
                        IntBonusSetupId = bonusGenerateCreate.IntBonusSetupId,
                        IntYearId = bonusGenerateCreate.IntYearId,
                        IntMonthId = bonusGenerateCreate.IntMonthId,
                        IsActive = true,
                        IntCreatedBy = bonusGenerateCreate.IntCreatedBy,
                        DteCreatedAt = DateTime.Now
                    };

                    await _context.bonusGenerates.AddAsync(bonusGenerate);
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


        public async Task<bool> UpdateBonusGenerate(BonusGenerateCreate bonusGenerateCreate)
        {
            try
            {
                var data = await _context.bonusGenerates.Where(x => x.IntBonusGenerateId == bonusGenerateCreate.IntBonusSetupId).FirstOrDefaultAsync();

                if (data != null)
                {
                    data.IntBonusGenerateId = bonusGenerateCreate.IntBonusGenerateId;
                    data.IntBonusSetupId = bonusGenerateCreate.IntBonusSetupId;
                    data.IntYearId = bonusGenerateCreate.IntYearId;
                    data.IntMonthId = bonusGenerateCreate.IntMonthId;
                    data.IntCreatedBy = bonusGenerateCreate.IntCreatedBy;
                    data.IntUpdatedBy = bonusGenerateCreate.IntUpdatedBy;
                    data.DteUpdatedAt = DateTime.Now;

                    _context.bonusGenerates.Update(data);
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

        public async Task<List<BonusGenerateLanding>> BonusGenerateLanding(long intBusinessUnitId, long intYearId, long intMonthId)
        {
            try
            {
                IQueryable<BonusGenerateLanding> bonusGenerateLandings = (from bn in _context.bonusGenerates
                                                                          join bon in _context.bonusSetups on bn.IntBonusSetupId equals bon.IntBonusSetypId into bon2
                                                                          from bonus in bon2.DefaultIfEmpty()
                                                                          join bus in _context.businessUnits on bonus.IntBusinessUnitId equals bus.IntBusinessUnitId into bus2
                                                                          from business in bus2.DefaultIfEmpty()
                                                                          
                                                                          where bonus.IntBusinessUnitId == intBusinessUnitId && bn.IntYearId == intYearId && bn.IntMonthId == intMonthId

                                                                          select new BonusGenerateLanding
                                                                          {
                                                                              IntBonusGenerateId = bn.IntBonusGenerateId,
                                                                              IntBonusSetupId = bn.IntBonusSetupId,
                                                                              StrBonusSetupName = bonus.StrBonusSetupName,
                                                                              IntBusinessUnitId = bonus.IntBusinessUnitId,
                                                                              StrBusinessUnitName = business.StrBusinessUnitName,
                                                                              IntYearId = bn.IntYearId,
                                                                              IntMonthId = bn.IntMonthId,
                                                                              IntCreatedBy = bn.IntCreatedBy,
                                                                              StrCreatedAt = bn.DteCreatedAt.ToString("dd MMM yyyy"),
                                                                              IntUpdatedBy = bn.IntUpdatedBy,
                                                                              StrUpdatedAt = bn.DteUpdatedAt.Value.ToString("dd MMM yyyy")
                                                                          });

                return await bonusGenerateLandings.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion================================== Bonus Generate ================================================
    }
}
