using System;
using DataAccess;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlanGIBusiness.PlanGoodIssue;

namespace PlanGIAPI.Controllers
{
    [Route("api/FilterTable")]
    [ApiController]
    public class FilterTableController : ControllerBase
    {
        private PlanGIDbContext context;



        #region im_PlanGoodsIssue
        [HttpPost("im_PlanGoodsIssue")]
        public IActionResult im_PlanGoodsReceive([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new DocumentViewModel();
                Models = JsonConvert.DeserializeObject<DocumentViewModel>(body.ToString());
                var result = service.im_PlanGoodsIssue(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region im_PlanGoodsIssue
        [HttpPost("im_PlanGoodsIssueItem")]
        public IActionResult im_PlanGoodsReceiveItem([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsIssueItemService();
                var Models = new DocumentViewModel();
                Models = JsonConvert.DeserializeObject<DocumentViewModel>(body.ToString());
                var result = service.im_PlanGoodsIssueItem(Models);
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