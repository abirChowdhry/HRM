using HRM.DTOs;
using HRM.Interfaces;
using HRM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
