using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlanGIBusiness.PlanGoodIssue;
using PlanGIBusiness.Planwave;
using static planGIBusiness.PlanGoodsIssue.PopupGIViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlanGIAPI.Controllers
{
    [Route("api/PlanGoodsIssueItem")]
    [ApiController]
    public class PlanGoodsIssueItemController : ControllerBase
    {
        #region find
        [HttpGet("find/{id}")]
        public IActionResult find(Guid id)
        {
            try
            {
                var service = new PlanGoodsIssueItemService();
                var result = service.find(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region find_with_wave
        [HttpPost("find_with_wave")]
        public IActionResult find_with_wave([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsIssueItemService();
                var Models = new Planwave();
                Models = JsonConvert.DeserializeObject<Planwave>(body.ToString());
                var result = service.find_with_wave(Models.planGoodsIssue_Index, Models.wave_status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion


        #region UpdateStatusPGII
        [HttpPost("UpdateStatusPGII")]
        public IActionResult UpdateStatusPGII([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsIssueItemService();
                var Models = JsonConvert.DeserializeObject<DocumentViewModel>(body.ToString());
                var result = service.UpdateStatusPGII(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region returnmatdoc
        [HttpPost("returnmatdoc")]
        public IActionResult returnmatdoc([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsIssueItemService();
                PlanGoodIssueDocViewModelItem Models = JsonConvert.DeserializeObject<PlanGoodIssueDocViewModelItem>(body.ToString());
                var result = service.returnmatdoc(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

    }
}
