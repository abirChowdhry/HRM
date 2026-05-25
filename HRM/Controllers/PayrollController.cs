using HRM.DTOs;
using HRM.Interfaces;
using HRM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollService _payrollService;
        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }


        #region================================= Payroll Policy ==================================================
        [HttpPost]
        [Route("CreateNUpdatePayrollPolicy")]
        public async Task<IActionResult> CreateNUpdatePayrollPolicy(PayrollPolicyCreateVM payrollPolicyCreateVM)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            MessageHelperUpdate resUpdate = new MessageHelperUpdate();

            try
            {
                if (payrollPolicyCreateVM.IntPayrollPolicyId > 0)
                {
                    if (await _payrollService.UpdatePolicy(payrollPolicyCreateVM) == true)
                    {
                        resUpdate.StatusCode = 200;
                        resUpdate.Message = "Updated Successfully !!!";
                        return Ok(resUpdate);
                    }

                    res.StatusCode = 401;
                    res.Message = "Update Was Unsuccessful !!!";
                    return BadRequest(res);

                }
                else
                {
                    if (await _payrollService.CreatePayrollPolicy(payrollPolicyCreateVM) == true)
                    {
                        res.StatusCode = 200;
                        res.Message = "Created Successfully !!!";
                        return Ok(res);
                    }
                    res.StatusCode = 401;
                    res.Message = "Create Was Unsuccessful !!!";
                    return BadRequest(res);
                }
            }
            catch (Exception ex)
            {
                res.StatusCode = 500;
                res.Message = ex.Message;
                return BadRequest(res);

            }
        }

        [HttpPost]
        [Route("DeletePayrollPolicy")]
        public async Task<IActionResult> DeletePayrollPolicy(long policyId)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            MessageHelperUpdate resUpdate = new MessageHelperUpdate();

            try
            {
                if (await _payrollService.DeletePolicy(policyId) == true)
                {
                    resUpdate.StatusCode = 200;
                    resUpdate.Message = "Deleted Successfully !!!";
                    return Ok(resUpdate);
                }

                res.StatusCode = 401;
                res.Message = "Delete Was Unsuccessful !!!";
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
        [Route("PayrollPolicyLanding")]
        public async Task<IActionResult> PayrollPolicyLanding(long businessUnitId)
        {
            MessageHelperCreate res = new MessageHelperCreate();

            try
            {

                var data = await _payrollService.PayrollPolicyLanding(businessUnitId);

                if (data == null)
                {
                    res.StatusCode = 401;
                    res.Message = "No Payroll Policy Found !!!";
                    return BadRequest(res);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageHelperError { Message = ex.Message });
            }
        }

        #endregion============================ Payroll Policy ====================================================



        #region=============================== Payroll Element ===================================================

        [HttpPost]
        [Route("CreateNUpdatePayrollElement")]
        public async Task<IActionResult> CreateNUpdatePayrollElement(PayrollElementCreateVM payrollElementCreateVM)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            MessageHelperUpdate resUpdate = new MessageHelperUpdate();

            try
            {
                if (payrollElementCreateVM.IntPayrollElementId > 0)
                {
                    if (await _payrollService.UpdatePayrollElement(payrollElementCreateVM) == true)
                    {
                        resUpdate.StatusCode = 200;
                        resUpdate.Message = "Updated Successfully !!!";
                        return Ok(resUpdate);
                    }

                    res.StatusCode = 401;
                    res.Message = "Update Was Unsuccessful !!!";
                    return BadRequest(res);

                }
                else
                {
                    if (await _payrollService.CreatePayrollElement(payrollElementCreateVM) == true)
                    {
                        res.StatusCode = 200;
                        res.Message = "Created Successfully !!!";
                        return Ok(res);
                    }
                    res.StatusCode = 401;
                    res.Message = "Create Was Unsuccessful !!!";
                    return BadRequest(res);
                }
            }
            catch (Exception ex)
            {
                res.StatusCode = 500;
                res.Message = ex.Message;
                return BadRequest(res);

            }
        }

        [HttpPost]
        [Route("DeletePayrollElement")]
        public async Task<IActionResult> DeletePayrollElement(long elementId)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            MessageHelperUpdate resUpdate = new MessageHelperUpdate();

            try
            {
                if (await _payrollService.DeletePayrollElement(elementId) == true)
                {
                    resUpdate.StatusCode = 200;
                    resUpdate.Message = "Deleted Successfully !!!";
                    return Ok(resUpdate);
                }

                res.StatusCode = 401;
                res.Message = "Delete Was Unsuccessful !!!";
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
        [Route("PayrollElementLanding")]
        public async Task<IActionResult> PayrollElementLanding(long businessUnitId)
        {
            MessageHelperCreate res = new MessageHelperCreate();

            try
            {

                var data = await _payrollService.PayrollElementLanding(businessUnitId);

                if (data == null)
                {
                    res.StatusCode = 401;
                    res.Message = "No Payroll Element Found !!!";
                    return BadRequest(res);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageHelperError { Message = ex.Message });
            }
        }

        #endregion============================ Payroll Element ================================================


        #region=============================== Payroll Group ==================================================

        [HttpPost]
        [Route("PayrollGroupHeaderNRowCreateNUpdate")]
        public async Task<IActionResult> PayrollGroupHeaderNRowCreateNUpdate(PayrollGroupHeaderNRowCreateVM payrollGroupHeaderNRowCreateVM) 
        {
            MessageHelperCreate res = new MessageHelperCreate();
            MessageHelperUpdate resUpdate = new MessageHelperUpdate();

            try
            {
                if (payrollGroupHeaderNRowCreateVM.IntPayrollGroupHeaderId > 0)
                {
                    if (await _payrollService.UpdatePayrollGroupHeaderNRow(payrollGroupHeaderNRowCreateVM) == true)
                    {
                        resUpdate.StatusCode = 200;
                        resUpdate.Message = "Updated Successfully !!!";
                        return Ok(resUpdate);
                    }

                    res.StatusCode = 401;
                    res.Message = "Update Was Unsuccessful !!!";
                    return BadRequest(res);

                }
                else
                {
                    if (await _payrollService.CreatePayrollGroupHeaderNRow(payrollGroupHeaderNRowCreateVM) == true)
                    {
                        res.StatusCode = 200;
                        res.Message = "Created Successfully !!!";
                        return Ok(res);
                    }
                    res.StatusCode = 401;
                    res.Message = "Create Was Unsuccessful !!!";
                    return BadRequest(res);
                }
            }
            catch (Exception ex)
            {
                res.StatusCode = 500;
                res.Message = ex.Message;
                return BadRequest(res);

            }
        }

        [HttpPost]
        [Route("DeletePayrollHeaderNRow")]
        public async Task<IActionResult> DeletePayrollHeaderNRow(long headerId)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            MessageHelperUpdate resUpdate = new MessageHelperUpdate();

            try
            {
                if (await _payrollService.DeletePayrollGroupHeaderNRow(headerId) == true)
                {
                    resUpdate.StatusCode = 200;
                    resUpdate.Message = "Deleted Successfully !!!";
                    return Ok(resUpdate);
                }

                res.StatusCode = 401;
                res.Message = "Delete Was Unsuccessful !!!";
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
        [Route("PayrollHeaderLanding")]
        public async Task<IActionResult> PayrollHeaderLanding(long businessUnitId)
        {
            MessageHelperCreate res = new MessageHelperCreate();

            try
            {

                var data = await _payrollService.PayrollHeaderLanding(businessUnitId);

                if (data == null)
                {
                    res.StatusCode = 401;
                    res.Message = "No Payroll Group Found !!!";
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
        [Route("PayrollRowLanding")]
        public async Task<IActionResult> PayrollRowLanding(long headerId)
        {
            MessageHelperCreate res = new MessageHelperCreate();

            try
            {

                var data = await _payrollService.PayrollRowLanding(headerId);

                if (data == null)
                {
                    res.StatusCode = 401;
                    res.Message = "No Payroll Group Details Found !!!";
                    return BadRequest(res);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageHelperError { Message = ex.Message });
            }
        }

        #endregion============================ Payroll Group ==================================================
    }
}
