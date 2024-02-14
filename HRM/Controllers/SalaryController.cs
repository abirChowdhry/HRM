using HRM.DTOs;
using HRM.Interfaces;
using HRM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryService _salaryService;
        public SalaryController(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }


        #region============================== Salary Assign ===============================================
        [HttpPost]
        [Route("SalaryAssignLanding")]
        public async Task<IActionResult> SalaryAssignLanding(long businessUnitId)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            try
            {

                var data = await _salaryService.SalaryAssignLanding(businessUnitId);

                if (data == null)
                {
                    res.StatusCode = 401;
                    res.Message = "No Employee Found !!!";
                    return BadRequest(res);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageHelperError { Message = ex.Message });
            }
        }
        
        [HttpPost]
        [Route("SalaryDetailsLanding")]
        public async Task<IActionResult> SalaryDetailsLanding(long businessUnitId)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            try
            {

                var data = await _salaryService.SalaryDetailsLanding(businessUnitId);

                if (data == null)
                {
                    res.StatusCode = 401;
                    res.Message = "No Employee Salary Assigned !!!";
                    return BadRequest(res);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageHelperError { Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("SalaryAssign")]
        public async Task<IActionResult> SalaryAssign(SalaryAssignVM salaryAssignVM)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            MessageHelperUpdate resUpdate = new MessageHelperUpdate();

            try
            {
                if (salaryAssignVM.IntSalaryAssignHeaderId == 0)
                {
                    if (await _salaryService.SalaryAssign(salaryAssignVM) == true)
                    {
                        res.StatusCode = 200;
                        res.Message = "Created Successfully !!!";
                        return Ok(res);
                    }
                }
                res.StatusCode = 401;
                res.Message = "Create Was Unsuccessful !!!";
                return BadRequest(res);
            }
            catch (Exception ex)
            {
                res.StatusCode = 500;
                res.Message = ex.Message;
                return BadRequest(res);

            }
        }

        [HttpPost]
        [Route("CreateSalaryAdditionNDecduction")]
        public async Task<IActionResult> CreateSalaryAdditionNDecduction(SalaryAdditionNDeductionVM salaryAdditionNDeductionVM)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            MessageHelperUpdate resUpdate = new MessageHelperUpdate();

            try
            {
                if (salaryAdditionNDeductionVM.IntSalaryAdditionAndDeductionId == 0)
                {
                    if (await _salaryService.CreateSalaryAdditionNDeduction(salaryAdditionNDeductionVM) == true)
                    {
                        res.StatusCode = 200;
                        res.Message = "Created Successfully !!!";
                        return Ok(res);
                    }
                }
                res.StatusCode = 401;
                res.Message = "Create Was Unsuccessful !!!";
                return BadRequest(res);
            }
            catch (Exception ex)
            {
                res.StatusCode = 500;
                res.Message = ex.Message;
                return BadRequest(res);

            }
        }


        [HttpPost]
        [Route("EmpSalAddNDeductLanding")]
        public async Task<IActionResult> EmpSalAddNDeductLanding(long employeeId)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            try
            {

                var data = await _salaryService.EmpSalAddNDeductionLanding(employeeId);

                if (data == null)
                {
                    res.StatusCode = 401;
                    res.Message = "No Employee Addition & Deduction Data Found !!!";
                    return BadRequest(res);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageHelperError { Message = ex.Message });
            }
        }

        #endregion================================= Salary Assign ============================================


        #region==================================== Salary Generate ==========================================

        [HttpPost]
        [Route("SalaryGenerate")]
        public async Task<IActionResult> SalaryGenerate(long intYearId, long intMonthId)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            try
            {

                var data = await _salaryService.SalaryGenerateLanding(intYearId, intMonthId);

                if (data == null || data.Count == 0)
                {
                    res.StatusCode = 401;
                    res.Message = "No Salary Data To Generate!!!";
                    return BadRequest(res);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageHelperError { Message = ex.Message });
            }
        }

        #endregion================================= Salary Generate ==========================================
    }
}
