using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlanGIBusiness.Demo;

namespace PlanGIAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : Controller
    {
        #region SO
        [HttpPost("SO")]
        public IActionResult CreateSO(DemoSORequestViewModel model)
        {
            try
            {
                var service = new DemoService();
                var Result = service.CreateSO(model);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region Shipment
        [HttpPost("Shipment")]
        public IActionResult CreateShipment(DemoShipmentRequestViewModel model)
        {
            try
            {
                var service = new DemoService();
                var Result = service.CreateShipment(model);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion
    }
}