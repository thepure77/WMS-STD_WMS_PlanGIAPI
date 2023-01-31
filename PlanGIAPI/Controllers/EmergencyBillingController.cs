using DataAccess;
using MasterDataBusiness.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using planGIBusiness.PlanGoodsIssue;
using PlanGIBusiness.PlanGoodIssue;
using PlanGIBusiness.Reports;
using planGoodsIssueBusiness.GoodsReceive;
using PTTPL.OMS.Business.Documents;
using PTTPL.TMS.Business.Common;
using PTTPL.TMS.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using static planGIBusiness.PlanGoodsIssue.PopupGIViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlanGIAPI.Controllers
{
    //[Authorize]
    [Route("api/EmergencyBilling")]
    public class EmergencyBillingController : Controller
    {
        //private PlanGIDbContext context;

        private readonly IHostingEnvironment _hostingEnvironment;
        public EmergencyBillingController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region filter
        //[AllowAnonymous]
        [HttpPost("filter")]
        public IActionResult filter([FromBody]JObject body)
        {
            try
            {
                var service = new EmergencyBillingService();
                var Models = new SearchDetailModel();
                Models = JsonConvert.DeserializeObject<SearchDetailModel>(body.ToString());
                var result = service.filter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #endregion
        
        #region printOutDeliveryNote

        #region printOutDeliveryNoteemergency
        [HttpPost("printOutDeliveryNoteEmergency")]
        public IActionResult printOutDeliveryNoteemergency([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new EmergencyBillingService();
                ReportPlanGoodsIssueViewModel Models = JsonConvert.DeserializeObject<ReportPlanGoodsIssueViewModel>(body.ToString());
                localFilePath = service.printOutDeliveryNote_emergency(Models, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                System.IO.File.Delete(localFilePath);
            }
        }
        #endregion
        
        #endregion

       
    }
}
