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
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using static planGIBusiness.PlanGoodsIssue.PopupGIViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlanGIAPI.Controllers
{
    //[Authorize]
    [Route("api/CheckStrock")]
    public class CheckStockController : Controller
    {
        //private PlanGIDbContext context;

        private readonly IHostingEnvironment _hostingEnvironment;
        public CheckStockController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region CheckSctock_filter
        [HttpPost("CheckSctock_filter")]
        public IActionResult CheckSctock_filter([FromBody]JObject body)
        {
            try
            {
                var service = new CheckStockService();
                var Models = new CheckStockModel();
                Models = JsonConvert.DeserializeObject<CheckStockModel>(body.ToString());
                var result = service.filter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region getSctock_filter
        [HttpPost("getSctock_filter")]
        public IActionResult getSctock_filter([FromBody]JObject body)
        {
            try
            {
                var service = new CheckStockService();
                var Models = new CheckStockModel();
                Models = JsonConvert.DeserializeObject<CheckStockModel>(body.ToString());
                var result = service.getSctock_filter(Models);
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
