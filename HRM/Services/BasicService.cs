using HRM.Data;
using HRM.Interfaces;
using HRM.Models;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services
{
    // CRUD rules for setup/master data used across the HRM workspace.
    public class BasicService : IBasicService
    {
        HRMContext _context;
        public BasicService(HRMContext context) { _context = context; }

        public async Task<bool> createBusinessunit(string businessUnitName)
        {
            try 
            {
                var data = await _context.businessUnits.Where(x => x.StrBusinessUnitName == businessUnitName).FirstOrDefaultAsync();
                if (data == null) 
                {
                    _context.businessUnits.Add(new BusinessUnit { StrBusinessUnitName = businessUnitName });
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> updateBusinessunit(long businessUnitId, string businessUnitName)
        {
            try
            {
                var data = await _context.businessUnits.Where(x => x.IntBusinessUnitId == businessUnitId).FirstOrDefaultAsync();
                var duplicate = await _context.businessUnits.Where(x => x.IntBusinessUnitId != businessUnitId && x.StrBusinessUnitName == businessUnitName).FirstOrDefaultAsync();

                if (data != null && duplicate == null)
                {
                    data.StrBusinessUnitName = businessUnitName;
                    _context.businessUnits.Update(data);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> deleteBusinessunit(long businessUnitId)
        {
            try
            {
                var data = await _context.businessUnits.Where(x => x.IntBusinessUnitId == businessUnitId).FirstOrDefaultAsync();
                // Do not delete setup records that are already connected to employees or payroll data.
                var isUsed = await _context.empBasicInfos.AnyAsync(x => x.IntBusinessUnitId == businessUnitId)
                          || await _context.payrollPolicies.AnyAsync(x => x.IntBusinessUnitId == businessUnitId)
                          || await _context.payrollElements.AnyAsync(x => x.IntBusinessUnitId == businessUnitId)
                          || await _context.payrollGroupHeaders.AnyAsync(x => x.IntBusinessUnitId == businessUnitId)
                          || await _context.salaryAssignHeaders.AnyAsync(x => x.IntBusinessUnitId == businessUnitId)
                          || await _context.salaryAdditionNDeductions.AnyAsync(x => x.IntBusinessUnitId == businessUnitId)
                          || await _context.empTransferNPromotions.AnyAsync(x => x.IntBusinessUnitId == businessUnitId)
                          || await _context.bonusSetups.AnyAsync(x => x.IntBusinessUnitId == businessUnitId);

                if (data != null && isUsed == false)
                {
                    _context.businessUnits.Remove(data);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> createDepartment(string DepartmentName)
        {
            try
            {
                var data = await _context.departments.Where(x => x.StrDepartmentName == DepartmentName).FirstOrDefaultAsync();
                if (data == null)
                {
                    _context.departments.Add(new Department { StrDepartmentName = DepartmentName });
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> updateDepartment(long departmentId, string departmentName)
        {
            try
            {
                var data = await _context.departments.Where(x => x.IntDepartmentId == departmentId).FirstOrDefaultAsync();
                var duplicate = await _context.departments.Where(x => x.IntDepartmentId != departmentId && x.StrDepartmentName == departmentName).FirstOrDefaultAsync();

                if (data != null && duplicate == null)
                {
                    data.StrDepartmentName = departmentName;
                    _context.departments.Update(data);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> deleteDepartment(long departmentId)
        {
            try
            {
                var data = await _context.departments.Where(x => x.IntDepartmentId == departmentId).FirstOrDefaultAsync();
                var isUsed = await _context.empBasicInfos.AnyAsync(x => x.IntDepartmentId == departmentId)
                          || await _context.empTransferNPromotions.AnyAsync(x => x.IntOldDepartmentId == departmentId || x.IntNewDepartmenId == departmentId)
                          || await _context.bonusSetups.AnyAsync(x => x.IntDepartmentId == departmentId);

                if (data != null && isUsed == false)
                {
                    _context.departments.Remove(data);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> createDesignations(string DesignationName)
        {
            try
            {
                var data = await _context.designations.Where(x => x.StrDesignationName == DesignationName).FirstOrDefaultAsync();
                if (data == null)
                {
                    _context.designations.Add(new Designation { StrDesignationName = DesignationName });
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> updateDesignation(long designationId, string designationName)
        {
            try
            {
                var data = await _context.designations.Where(x => x.IntDesignationId == designationId).FirstOrDefaultAsync();
                var duplicate = await _context.designations.Where(x => x.IntDesignationId != designationId && x.StrDesignationName == designationName).FirstOrDefaultAsync();

                if (data != null && duplicate == null)
                {
                    data.StrDesignationName = designationName;
                    _context.designations.Update(data);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> deleteDesignation(long designationId)
        {
            try
            {
                var data = await _context.designations.Where(x => x.IntDesignationId == designationId).FirstOrDefaultAsync();
                var isUsed = await _context.empBasicInfos.AnyAsync(x => x.IntDesignationId == designationId)
                          || await _context.empTransferNPromotions.AnyAsync(x => x.IntOldDesignationId == designationId || x.IntNewDesignationId == designationId);

                if (data != null && isUsed == false)
                {
                    _context.designations.Remove(data);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> createEmployementType(string employementTypeName)
        {
            try
            {
                var data = await _context.employementTypes.Where(x => x.StrEmployementName == employementTypeName).FirstOrDefaultAsync();
                if (data == null)
                {
                    _context.employementTypes.Add(new EmployementType { StrEmployementName = employementTypeName });
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> updateEmployementType(long employementTypeId, string employementTypeName)
        {
            try
            {
                var data = await _context.employementTypes.Where(x => x.IntEmployementId == employementTypeId).FirstOrDefaultAsync();
                var duplicate = await _context.employementTypes.Where(x => x.IntEmployementId != employementTypeId && x.StrEmployementName == employementTypeName).FirstOrDefaultAsync();

                if (data != null && duplicate == null)
                {
                    data.StrEmployementName = employementTypeName;
                    _context.employementTypes.Update(data);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> deleteEmployementType(long employementTypeId)
        {
            try
            {
                var data = await _context.employementTypes.Where(x => x.IntEmployementId == employementTypeId).FirstOrDefaultAsync();
                var isUsed = await _context.empBasicInfos.AnyAsync(x => x.IntEmploymentTypeId == employementTypeId)
                          || await _context.bonusSetups.AnyAsync(x => x.IntEmployementTypeId == employementTypeId);

                if (data != null && isUsed == false)
                {
                    _context.employementTypes.Remove(data);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<BusinessUnit>> getAllBusinessunit()
        {
            return await _context.businessUnits.ToListAsync();
        }

        public async Task<List<Department>> getAllDepartment()
        {
            return await _context.departments.ToListAsync();
        }

        public async Task<List<Designation>> getAllDesignations()
        {
            return await _context.designations.ToListAsync();
        }

        public async Task<List<EmployementType>> getAllEmployementType()
        {
            return await _context.employementTypes.ToListAsync();
        }
    }
}
