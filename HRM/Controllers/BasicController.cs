using HRM.DTOs;
using HRM.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    // Master-data endpoints used by the Setup screen.
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BasicController : ControllerBase
    {
        IBasicService _basicService;
        public BasicController(IBasicService basicService) { _basicService = basicService; }

        [HttpPost]
        [Route("CreateBusinessUnit")]
        public async Task<IActionResult> CreateBusinessUnit(string businessUnitName)
        {
            MessageHelperCreate res = new MessageHelperCreate();

            if (await _basicService.createBusinessunit(businessUnitName) == true)
            {
                res.StatusCode = 200;
                res.Message = "Created Successfully !!!";
                return Ok(res);
            }
            res.StatusCode = 401;
            res.Message = "Create Was Unsuccessful !!!";
            return BadRequest(res);
        }

        [HttpPost]
        [Route("UpdateBusinessUnit")]
        public async Task<IActionResult> UpdateBusinessUnit(long businessUnitId, string businessUnitName)
        {
            MessageHelperUpdate res = new MessageHelperUpdate();

            if (await _basicService.updateBusinessunit(businessUnitId, businessUnitName) == true)
            {
                res.StatusCode = 200;
                res.Message = "Updated Successfully !!!";
                return Ok(res);
            }
            res.StatusCode = 401;
            res.Message = "Update Was Unsuccessful !!!";
            return BadRequest(res);
        }

        [HttpPost]
        [Route("DeleteBusinessUnit")]
        public async Task<IActionResult> DeleteBusinessUnit(long businessUnitId)
        {
            MessageHelper res = new MessageHelper();

            if (await _basicService.deleteBusinessunit(businessUnitId) == true)
            {
                res.StatusCode = 200;
                res.Message = "Deleted Successfully !!!";
                return Ok(res);
            }
            res.StatusCode = 401;
            res.Message = "Delete Was Unsuccessful. This business unit may already be in use.";
            return BadRequest(res);
        }

        [HttpPost]
        [Route("CreateDeparment")]
        public async Task<IActionResult> CreateDeparment(string depatmentName)
        {
            MessageHelperCreate res = new MessageHelperCreate();

            if (await _basicService.createDepartment(depatmentName) == true)
            {
                res.StatusCode = 200;
                res.Message = "Created Successfully !!!";
                return Ok(res);
            }
            res.StatusCode = 401;
            res.Message = "Create Was Unsuccessful !!!";
            return BadRequest(res);
        }

        [HttpPost]
        [Route("UpdateDeparment")]
        public async Task<IActionResult> UpdateDeparment(long departmentId, string departmentName)
        {
            MessageHelperUpdate res = new MessageHelperUpdate();

            if (await _basicService.updateDepartment(departmentId, departmentName) == true)
            {
                res.StatusCode = 200;
                res.Message = "Updated Successfully !!!";
                return Ok(res);
            }
            res.StatusCode = 401;
            res.Message = "Update Was Unsuccessful !!!";
            return BadRequest(res);
        }

        [HttpPost]
        [Route("DeleteDeparment")]
        public async Task<IActionResult> DeleteDeparment(long departmentId)
        {
            MessageHelper res = new MessageHelper();

            if (await _basicService.deleteDepartment(departmentId) == true)
            {
                res.StatusCode = 200;
                res.Message = "Deleted Successfully !!!";
                return Ok(res);
            }
            res.StatusCode = 401;
            res.Message = "Delete Was Unsuccessful. This department may already be in use.";
            return BadRequest(res);
        }

        [HttpPost]
        [Route("CreateDesignation")]
        public async Task<IActionResult> CreateDesignation(string designationName)
        {
            MessageHelperCreate res = new MessageHelperCreate();

            if (await _basicService.createDesignations(designationName) == true)
            {
                res.StatusCode = 200;
                res.Message = "Created Successfully !!!";
                return Ok(res);
            }
            res.StatusCode = 401;
            res.Message = "Create Was Unsuccessful !!!";
            return BadRequest(res);
        }

        [HttpPost]
        [Route("UpdateDesignation")]
        public async Task<IActionResult> UpdateDesignation(long designationId, string designationName)
        {
            MessageHelperUpdate res = new MessageHelperUpdate();

            if (await _basicService.updateDesignation(designationId, designationName) == true)
            {
                res.StatusCode = 200;
                res.Message = "Updated Successfully !!!";
                return Ok(res);
            }
            res.StatusCode = 401;
            res.Message = "Update Was Unsuccessful !!!";
            return BadRequest(res);
        }

        [HttpPost]
        [Route("DeleteDesignation")]
        public async Task<IActionResult> DeleteDesignation(long designationId)
        {
            MessageHelper res = new MessageHelper();

            if (await _basicService.deleteDesignation(designationId) == true)
            {
                res.StatusCode = 200;
                res.Message = "Deleted Successfully !!!";
                return Ok(res);
            }
            res.StatusCode = 401;
            res.Message = "Delete Was Unsuccessful. This designation may already be in use.";
            return BadRequest(res);
        }

        [HttpPost]
        [Route("CreateEmployementType")]
        public async Task<IActionResult> CreateEmployementType(string employementTypeName)
        {
            MessageHelperCreate res = new MessageHelperCreate();

            if (await _basicService.createEmployementType(employementTypeName) == true)
            {
                res.StatusCode = 200;
                res.Message = "Created Successfully !!!";
                return Ok(res);
            }
            res.StatusCode = 401;
            res.Message = "Create Was Unsuccessful !!!";
            return BadRequest(res);
        }

        [HttpPost]
        [Route("UpdateEmployementType")]
        public async Task<IActionResult> UpdateEmployementType(long employementTypeId, string employementTypeName)
        {
            MessageHelperUpdate res = new MessageHelperUpdate();

            if (await _basicService.updateEmployementType(employementTypeId, employementTypeName) == true)
            {
                res.StatusCode = 200;
                res.Message = "Updated Successfully !!!";
                return Ok(res);
            }
            res.StatusCode = 401;
            res.Message = "Update Was Unsuccessful !!!";
            return BadRequest(res);
        }

        [HttpPost]
        [Route("DeleteEmployementType")]
        public async Task<IActionResult> DeleteEmployementType(long employementTypeId)
        {
            MessageHelper res = new MessageHelper();

            if (await _basicService.deleteEmployementType(employementTypeId) == true)
            {
                res.StatusCode = 200;
                res.Message = "Deleted Successfully !!!";
                return Ok(res);
            }
            res.StatusCode = 401;
            res.Message = "Delete Was Unsuccessful. This employment type may already be in use.";
            return BadRequest(res);
        }


        [HttpGet]
        [Route("GetBusinessUnits")]
        public async Task<IActionResult> GetBusinessUnits() 
        {
            var data = await _basicService.getAllBusinessunit();
            return Ok(data);
        }
        
        [HttpGet]
        [Route("GetDepartments")]
        public async Task<IActionResult> GetDepartments() 
        {
            var data = await _basicService.getAllDepartment();
            return Ok(data);
        }
        
        [HttpGet]
        [Route("GetDesignations")]
        public async Task<IActionResult> GetDesignations() 
        {
            var data = await _basicService.getAllDesignations();
            return Ok(data);
        }
        
        [HttpGet]
        [Route("GetEmployementTypes")]
        public async Task<IActionResult> GetEmployementTypes() 
        {
            var data = await _basicService.getAllEmployementType();
            return Ok(data);
        }
    }
}
