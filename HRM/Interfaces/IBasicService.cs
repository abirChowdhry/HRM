using HRM.Models;

namespace HRM.Interfaces
{
    public interface IBasicService
    {
        public Task<List<BusinessUnit>> getAllBusinessunit();
        public Task<bool> createBusinessunit(string businessUnitName);
        public Task<bool> updateBusinessunit(long businessUnitId, string businessUnitName);
        public Task<bool> deleteBusinessunit(long businessUnitId);
        public Task<List<Department>> getAllDepartment();
        public Task<bool> createDepartment(string DepartmentName);
        public Task<bool> updateDepartment(long departmentId, string departmentName);
        public Task<bool> deleteDepartment(long departmentId);
        public Task<List<Designation>> getAllDesignations();
        public Task<bool> createDesignations(string DesignationName);
        public Task<bool> updateDesignation(long designationId, string designationName);
        public Task<bool> deleteDesignation(long designationId);
        public Task<List<EmployementType>> getAllEmployementType();
        public Task<bool> createEmployementType(string employementTypeName);
        public Task<bool> updateEmployementType(long employementTypeId, string employementTypeName);
        public Task<bool> deleteEmployementType(long employementTypeId);
    }
}
