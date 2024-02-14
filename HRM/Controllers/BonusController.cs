using HRM.DTOs;
using HRM.Interfaces;
using HRM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonusController : ControllerBase
    {
        IBonusService _bonusService;
        public BonusController(IBonusService bonusService) { _bonusService = bonusService; }


        #region================================ Bonus Setup ==========================================

        [HttpPost]
        [Route("CreateNUpdateBonus")]
        public async Task<IActionResult> CreateNUpdateBonus(BonusSetupCreateVM bonusSetupCreateVM)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            MessageHelperUpdate resUpdate = new MessageHelperUpdate();

            try
            {
                if (bonusSetupCreateVM.IntBonusSetypId > 0)
                {
                    if (await _bonusService.UpdateBonusSetup(bonusSetupCreateVM) == true)
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
                    if (await _bonusService.CreateBonusSetup(bonusSetupCreateVM) == true)
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
        [Route("BonusLanding")]
        public async Task<IActionResult> BonusLanding(long intBusinessUnitId)
        {
            MessageHelperCreate res = new MessageHelperCreate();

            try
            {

                var data = await _bonusService.BonusLanding(intBusinessUnitId);

                if (data == null)
                {
                    res.StatusCode = 401;
                    res.Message = "No Bonus Found !!!";
                    return BadRequest(res);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageHelperError { Message = ex.Message });
            }
        }

        #endregion========================== Bonus Setup ==============================================


        #region================================ Bonus Generate ==========================================

        [HttpPost]
        [Route("CreateNUpdateBonusGenerate")]
        public async Task<IActionResult> CreateNUpdateBonusGenerate(BonusGenerateCreate bonusGenerateCreate)
        {
            MessageHelperCreate res = new MessageHelperCreate();
            MessageHelperUpdate resUpdate = new MessageHelperUpdate();

            try
            {
                if (bonusGenerateCreate.IntBonusGenerateId > 0)
                {
                    if (await _bonusService.UpdateBonusGenerate(bonusGenerateCreate) == true)
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
                    if (await _bonusService.CreateBonusGenerate(bonusGenerateCreate) == true)
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
        [Route("BonusGenerateLanding")]
        public async Task<IActionResult> BonusGenerateLanding(long intBusinessUnitid, long intyearId, long intMonthId)
        {
            MessageHelperCreate res = new MessageHelperCreate();

            try
            {

                var data = await _bonusService.BonusGenerateLanding(intBusinessUnitid, intyearId, intMonthId);

                if (data == null)
                {
                    res.StatusCode = 401;
                    res.Message = "No Generated Bonus Found !!!";
                    return BadRequest(res);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageHelperError { Message = ex.Message });
            }
        }

        #endregion========================== Bonus Generate ==============================================
    }
}
