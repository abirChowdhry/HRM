using HRM.DTOs;
using HRM.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Buffers.Text;

namespace HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        #region=============================== Employee Basic Functions =======================================
        [HttpPost]
        [Route("CreateNUpdateEmployee")]
        public async Task<IActionResult> CreateNUpdateEmployee(EmployeeCreateVM employeeCreateVM)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            MessageHelperUpdate resUpdate = new MessageHelperUpdate();

            try
            {
                if (employeeCreateVM.IntEmployeeBasicInfoId > 0)
                {
                    if (await _employeeService.UpdateEmployee(employeeCreateVM) == true)
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
                    if (await _employeeService.CreateEmployee(employeeCreateVM) == true)
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
        [Route("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(long employeeId)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            MessageHelperUpdate resUpdate = new MessageHelperUpdate();

            try
            {
                if (await _employeeService.DeleteEmployee(employeeId) == true)
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
        [Route("EmployeeLanding")]
        public async Task<IActionResult> EmployeeLanding(EmployeeLandingFilter employeeLandingFilter)
        {
            MessageHelperCreate res = new MessageHelperCreate();

            try
            {

                var data = await _employeeService.EmployeeLanding(employeeLandingFilter);

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
        [Route("EmployeeSearchByAdress")]
        public async Task<IActionResult> EmployeeSearchByAdress(string employeeAdress)
        {
            MessageHelperCreate res = new MessageHelperCreate();

            try
            {

                var data = await _employeeService.EmployeeSearchByAddress(employeeAdress);

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

        [HttpGet]
        [Route("EmployeeById")]
        public async Task<IActionResult> EmployeeById(long employeeId)
        {
            MessageHelperCreate res = new MessageHelperCreate();

            try
            {

                var data = await _employeeService.GetEmployeeById(employeeId);

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


        #endregion=============================== Employee Basic Functions =======================================


        #region================================== Employee Transfer And Promotion ================================

        [HttpPost]
        [Route("CreateNUpdateEmpTransferNPromoton")]
        public async Task<IActionResult> CreateNUpdateEmpTransferNPromoton(TransferNPromotionCreateVM transferNPromotionCreateVM)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            MessageHelperUpdate resUpdate = new MessageHelperUpdate();

            try
            {
                if (transferNPromotionCreateVM.IntEmpTransferNPromotionId > 0)
                {
                    if (await _employeeService.UpdateEmployeeTrnasferNPromotion(transferNPromotionCreateVM) == true)
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
                    if (await _employeeService.CreateEmployeeTrnasferNPromotion(transferNPromotionCreateVM) == true)
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
        [Route("EmployeeTransferNPromotionHistory")]
        public async Task<IActionResult> EmployeeTransferNPromotionHistory(DateTime dteFromDate, DateTime dteToDate, long intBusinessUnitId, bool? isTransfer, bool? IsPromotion)
        {
            MessageHelperCreate res = new MessageHelperCreate();

            try
            {

                var data = await _employeeService.EmpTransferNPromotionLandingByDate(dteFromDate,dteToDate, intBusinessUnitId, isTransfer, IsPromotion);

                if (data == null)
                {
                    res.StatusCode = 401;
                    res.Message = "No Employee Transfer And Promotion History Found !!!";
                    return BadRequest(res);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageHelperError { Message = ex.Message });
            }
        }

        #endregion================================== Employee Transfer And Promotion =============================
    }
}
