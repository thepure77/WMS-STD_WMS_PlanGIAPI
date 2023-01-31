using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterBusiness.PlanGoodsIssue;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlanGIBusiness.PlanGoodIssue;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlanGIAPI.Controllers
{
    [Route("api/AutoPlanGoodIssue")]
    [ApiController]
    public class AutoPlanGoodsIssueController : ControllerBase
    {
        #region AutobasicSuggestion
        [HttpPost("autobasicSuggestion")]
        public IActionResult autobasicSuggestion([FromBody]JObject body)

        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autobasicSuggestion(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region autoPlanGoodIssueNo
        [HttpPost("autoPlanGoodIssueNo")]
        public IActionResult autoPlanGoodIssueNo([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoPlanGoodIssueNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoOwnerfilter
        [HttpPost("autoOwnerfilter")]
        public IActionResult autoOwnerfilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoOwnerfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoUser
        [HttpPost("autoUser")]
        public IActionResult autoUser([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoUser(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoSkufilter
        [HttpPost("autoSkufilter")]
        public IActionResult autoSkufilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoSkufilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoProductfilter
        [HttpPost("autoProductfilter")]
        public IActionResult autoProdutfilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoProductfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoSoldTo
        [HttpPost("autoSoldTofilter")]
        public IActionResult autoSoldTo([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoSoldTofilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoShiptofilter
        [HttpPost("autoShipTofilter")]
        public IActionResult autoShipTo([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoShipTofilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region autoMovementTypefilter
        [HttpPost("autoMovementTypefilter")]
        public IActionResult autoMovementTypefilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoMovementTypefilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion


        #region autoCostCenterFullfilter
        [HttpPost("autoCostCenterFullfilter")]
        public IActionResult autoCostCenterFullfilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoCostCenterFullfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoBilling_No
        [HttpPost("AutoBilling_No")]
        public IActionResult AutoBilling_No([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.AutoBilling_No(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoMatdoc_No
        [HttpPost("AutoMatdoc_No")]
        public IActionResult AutoMatdoc_No([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.AutoMatdoc_No(Models);
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
