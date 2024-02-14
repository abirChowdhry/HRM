using HRM.Data;
using HRM.Interfaces;
using HRM.Models;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services
{
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
