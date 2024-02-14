using HRM.Models;

namespace HRM.Interfaces
{
    public interface IBasicService
    {
        public Task<List<BusinessUnit>> getAllBusinessunit();
        public Task<bool> createBusinessunit(string businessUnitName);
        public Task<List<Department>> getAllDepartment();
        public Task<bool> createDepartment(string DepartmentName);
        public Task<List<Designation>> getAllDesignations();
        public Task<bool> createDesignations(string DesignationName);
        public Task<List<EmployementType>> getAllEmployementType();
        public Task<bool> createEmployementType(string employementTypeName);
    }
}
