using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterDataBusiness.CostCenter;
using MasterDataBusiness.StorageLoc;
using MasterDataBusiness.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlanGIBusiness.PlanGoodIssue;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlanGIAPI.Controllers
{
    [Route("api/DropdownPlanGoodsIssue")]
    [ApiController]
    public class DropdownPlanGoodsIssueController : ControllerBase
    {
        #region DropdownDocumentType
        [HttpPost("dropdownDocumentType")]
        public IActionResult dropdownDocumentType([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new DocumentTypeViewModel();
                Models = JsonConvert.DeserializeObject<DocumentTypeViewModel>(body.ToString());
                var result = service.DropdownDocumentType(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region DropdownStatus
        [HttpPost("dropdownStatus")]
        public IActionResult DropdownStatus([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ProcessStatusViewModel();
                Models = JsonConvert.DeserializeObject<ProcessStatusViewModel>(body.ToString());
                var result = service.dropdownStatus(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region DropdownWarehouse
        [HttpPost("dropdownWarehouse")]
        public IActionResult DropdownWarehouse([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new warehouseDocViewModel();
                Models = JsonConvert.DeserializeObject<warehouseDocViewModel>(body.ToString());
                var result = service.dropdownWarehouse(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region DropdownRound
        [HttpPost("dropdownRound")]
        public IActionResult dropdownRound([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new roundDocViewModel();
                Models = JsonConvert.DeserializeObject<roundDocViewModel>(body.ToString());
                var result = service.dropdownRound(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownProductconversion
        [HttpPost("dropdownProductconversion")]
        public IActionResult dropdownProductconversion([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ProductConversionViewModelDoc();
                Models = JsonConvert.DeserializeObject<ProductConversionViewModelDoc>(body.ToString());
                var result = service.dropdownProductconversion(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownItemStatus
        [HttpPost("dropdownItemStatus")]
        public IActionResult dropdownItemStatus([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ItemStatusDocViewModel();
                Models = JsonConvert.DeserializeObject<ItemStatusDocViewModel>(body.ToString());
                var result = service.dropdownItemStatus(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownTypeCar
        [HttpPost("dropdownTypeCar")]
        public IActionResult dropdownTypeCar([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new TypeCarViewModel();
                Models = JsonConvert.DeserializeObject<TypeCarViewModel>(body.ToString());
                var result = service.dropdownTypeCar(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownTransport
        [HttpPost("dropdownTransport")]
        public IActionResult dropdownTransport([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new TransportViewModel();
                Models = JsonConvert.DeserializeObject<TransportViewModel>(body.ToString());
                var result = service.dropdownTransport(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownCostCenter
        [HttpPost("dropdownCostCenter")]
        public IActionResult dropdownCostCenter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new CostCenterViewModel();
                Models = JsonConvert.DeserializeObject<CostCenterViewModel>(body.ToString());
                var result = service.dropdownCostCenter(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownStorageLoc
        [HttpPost("dropdownStorageLoc")]
        public IActionResult dropdownStorageLoc([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new StorageLocViewModel();
                Models = JsonConvert.DeserializeObject<StorageLocViewModel>(body.ToString());
                var result = service.dropdownStorageLoc(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownWeight
        [HttpPost("dropdownWeight")]
        public IActionResult dropdownWeight([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new WeightViewModel();
                Models = JsonConvert.DeserializeObject<WeightViewModel>(body.ToString());
                var result = service.dropdownWeight(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownVolume
        [HttpPost("dropdownVolume")]
        public IActionResult dropdownVolume([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new VolumeViewModel();
                Models = JsonConvert.DeserializeObject<VolumeViewModel>(body.ToString());
                var result = service.dropdownVolume(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownCurrency
        [HttpPost("dropdownCurrency")]
        public IActionResult dropdownCurrency([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new CurrencyViewModel();
                Models = JsonConvert.DeserializeObject<CurrencyViewModel>(body.ToString());
                var result = service.dropdownCurrency(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownRoute
        [HttpPost("dropdownRoute")]
        public IActionResult dropdownRoute([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new RouteViewModel();
                Models = JsonConvert.DeserializeObject<RouteViewModel>(body.ToString());
                var result = service.dropdownRoute(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownSubRoute
        [HttpPost("dropdownSubRoute")]
        public IActionResult dropdownSubRoute([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new SubRouteViewModel();
                Models = JsonConvert.DeserializeObject<SubRouteViewModel>(body.ToString());
                var result = service.dropdownSubRoute(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region shippingMethoddropdown
        [HttpPost("shippingMethoddropdown")]
        public IActionResult shippingMethoddropdown([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ShippingMethodViewModel();
                Models = JsonConvert.DeserializeObject<ShippingMethodViewModel>(body.ToString());
                var result = service.shippingMethoddropdown(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region shippingTermsdropdown
        [HttpPost("shippingTermsdropdown")]
        public IActionResult shippingTermsdropdown([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ShippingTermsViewModel();
                Models = JsonConvert.DeserializeObject<ShippingTermsViewModel>(body.ToString());
                var result = service.shippingTermsdropdown(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region paymentTypedropdown
        [HttpPost("paymentTypedropdown")]
        public IActionResult paymentTypedropdown([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new PaymentTypeViewModel();
                Models = JsonConvert.DeserializeObject<PaymentTypeViewModel>(body.ToString());
                var result = service.paymentTypedropdown(Models);
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
