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
    [Route("api/PlanGoodsIssue")]
    public class PlanGoodsIssueController : Controller
    {
        //private PlanGIDbContext context;

        private readonly IHostingEnvironment _hostingEnvironment;
        public PlanGoodsIssueController(IHostingEnvironment hostingEnvironment)
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
                var service = new PlanGoodIssueService();
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

        [HttpPost("filter_in")]
        public IActionResult FilterInClause([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                SearchPlanGoodsIssueInClauseViewModel Models = JsonConvert.DeserializeObject<SearchPlanGoodsIssueInClauseViewModel>(body is null ? string.Empty : body.ToString());
                var result = service.FilterInClause(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region CreateOrUpdate
        //[Authorize]
        //[AllowAnonymous]
        [HttpPost("createOrUpdate")]
        public IActionResult CreateOrUpdate([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new PlanGoodDocIssueViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodDocIssueViewModel>(body.ToString());
                var result = service.CreateOrUpdate(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region find
        [HttpGet("find/{id}")]
        public IActionResult find(Guid id)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var result = service.find(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region Delete
        [HttpPost("delete")]
        public IActionResult Delete([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new PlanGoodDocIssueViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodDocIssueViewModel>(body.ToString());
                var result = service.Delete(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region confirmStatus
        [HttpPost("confirmStatus")]
        public IActionResult confirmStatus([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new PlanGoodDocIssueViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodDocIssueViewModel>(body.ToString());
                var result = service.confirmStatus(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region closeDocument
        [HttpPost("closeDocument")]
        public IActionResult closeDocument([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new PlanGoodDocIssueViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodDocIssueViewModel>(body.ToString());
                var result = service.closeDocument(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region confirmFirstOrder
        [HttpPost("confirmFirstOrder")]
        public IActionResult confirmFirstOrder([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new PlanGoodDocIssueViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodDocIssueViewModel>(body.ToString());
                var result = service.confirmFirstOrder(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion


        #region updateUserAssign
        [HttpPost("updateUserAssign")]
        public IActionResult updateUserAssign([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new PlanGoodDocIssueViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodDocIssueViewModel>(body.ToString());
                var result = service.updateUserAssign(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region deleteUserAssign
        [HttpPost("deleteUserAssign")]
        public IActionResult deleteUserAssign([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new PlanGoodDocIssueViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodDocIssueViewModel>(body.ToString());
                var result = service.deleteUserAssign(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion


        #region PopupGIRunWave
        [HttpPost("PopupGIRunWave")]
        public IActionResult PopupGI([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new SearchDetailModel();
                Models = JsonConvert.DeserializeObject<SearchDetailModel>(body.ToString());
                var result = service.PopupGIRunWave(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region fillterWave
        [HttpPost("fillterWave")]
        public IActionResult fillterWave([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new planGoodsIssueAlreadyWave();
                Models = JsonConvert.DeserializeObject<planGoodsIssueAlreadyWave>(body.ToString());
                var result = service.fillterWave(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region updateStatusPlanGIRunWave
        [HttpPost("updateStatusPlanGIRunWave")]
        public IActionResult updateStatusPlanGIRunWave([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new plangoodsissueitemViewModel();
                Models = JsonConvert.DeserializeObject<plangoodsissueitemViewModel>(body.ToString());
                var result = service.updateStatusPlanGIRunWave(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        # region PrintPlanGoodsIssue
        [HttpPost("PrintPlanGoodsIssue")]
        public IActionResult PrintPlanGoodsIssue([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ReportPlanGoodsIssueViewModel();
                Models = JsonConvert.DeserializeObject<ReportPlanGoodsIssueViewModel>(body.ToString());
                localFilePath = service.PrintPlanGoodsIssue(Models, _hostingEnvironment.ContentRootPath);
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

        #region printOutDeliveryNote

        #region printOutDeliveryNote
        [HttpPost("printOutDeliveryNote")]
        public IActionResult printOutDeliveryNote([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new PlanGoodIssueService();
                ReportPlanGoodsIssueViewModel Models = JsonConvert.DeserializeObject<ReportPlanGoodsIssueViewModel>(body.ToString());
                localFilePath = service.printOutDeliveryNote(Models, _hostingEnvironment.ContentRootPath);
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

        #region printOutDeliveryNoteemergency
        [HttpPost("printOutDeliveryNoteemergency")]
        public IActionResult printOutDeliveryNoteemergency([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new PlanGoodIssueService();
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

        #region Old
        //[HttpPost("printOutDeliveryNote")]
        //public IActionResult printOutDeliveryNote([FromBody]JObject body)
        //{
        //    string localFilePath = "";
        //    try
        //    {
        //        var service = new PlanGoodIssueService();
        //        var Models = new ReportPlanGoodsIssueViewModel();
        //        Models = JsonConvert.DeserializeObject<ReportPlanGoodsIssueViewModel>(body.ToString());
        //        //localFilePath = service.ReportPrintOutShipment(Models, _hostingEnvironment.ContentRootPath);
        //        localFilePath = service.printOutDeliveryNote(Models, _hostingEnvironment.ContentRootPath);
        //        if (!System.IO.File.Exists(localFilePath))
        //        {
        //            return NotFound();
        //        }
        //        return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
        //        //return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //    finally
        //    {
        //        System.IO.File.Delete(localFilePath);
        //    }
        //}
        #endregion

        #endregion

        #region PrintOutPGI
        [HttpPost("PrintOutPGI")]
        public IActionResult PrintOutPGI([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new PrintOutPGIViewModel();
                Models = JsonConvert.DeserializeObject<PrintOutPGIViewModel>(body.ToString());
                localFilePath = service.PrintOutPGI(Models, _hostingEnvironment.ContentRootPath);
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

        #region GetPath
        [HttpPost("GetPath")]
        public IActionResult GetPath([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var result = _hostingEnvironment.ContentRootPath;
                return Ok(result);
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

        #region findUser
        [HttpPost("findUser")]
        public IActionResult findUser([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new im_Signatory_logViewModel();
                Models = JsonConvert.DeserializeObject<im_Signatory_logViewModel>(body.ToString());
                var result = service.findUser(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region printReportPO
        [HttpPost("printReportPO")]
        public IActionResult printReportPO([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ReportPOViewModel();
                Models = JsonConvert.DeserializeObject<ReportPOViewModel>(body.ToString());
                localFilePath = service.printReportPO(Models, _hostingEnvironment.ContentRootPath);
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

        #region printReportBilling
        [HttpPost("printReportBilling")]
        public IActionResult printReportBilling([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new ReportPOViewModel();
                Models = JsonConvert.DeserializeObject<ReportPOViewModel>(body.ToString());
                localFilePath = service.printReportBilling(Models, _hostingEnvironment.ContentRootPath);
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
        }
        #endregion

        #region importDocument
        [HttpPost("importFileOutbound")]
        public async Task<IActionResult> Index(EngineerVM engineerVM)
        {
            try
            {
                resultViewModel items = new resultViewModel();
                if (engineerVM.File != null)
                {
                    //upload files to wwwroot
                    var fileName = Path.GetFileName(engineerVM.File.FileName);
                    var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads", fileName);

                    string memoryPath = AppsInfo.upload;
                    string memoryDocument = AppsInfo.document_upload;
                    string virtualDocument = AppsInfo.document_path;

                    string root = _hostingEnvironment.ContentRootPath + memoryPath;

                    DirectoryInfo dir = new DirectoryInfo(root);
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }

                    // path = root;
                    var provider = new MultipartFormDataStreamProvider(root);

                    var path = _hostingEnvironment.ContentRootPath + memoryDocument;

                    string guidName = "";
                    using (var fileSteam = new FileStream(filePath, FileMode.Create))
                    {
                        await engineerVM.File.CopyToAsync(fileSteam);
                        var item = new fileViewModel();
                        guidName = Guid.NewGuid().ToString().ToUpper().Replace("-", "_");
                        string extension = fileAppService.getExtension(engineerVM.File.ContentType);

                        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                        {
                            fileName = fileName.Trim('"');
                        }
                        if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                        {
                            fileName = Path.GetFileName(fileName);
                        }

                        string newPath = path + guidName + "\\" + fileName;
                        string thumbPath = path + guidName + "\\" + "thumb\\" + fileName;

                        if (fileAppService.getTypeImage(engineerVM.File.ContentType))
                            fileAppService.setThumbnail(newPath, thumbPath);
                        else
                            thumbPath = "";

                        item.name = fileName;
                        item.extension = extension;
                        item.virtualPath = virtualDocument;
                        item.path = virtualDocument + "/" + fileName;
                        item.orginal = fileName;
                        item.thumb = virtualDocument + guidName + "thumb/" + fileName;
                        if (thumbPath == "")
                            item.fileType = "document";
                        else
                            item.fileType = "image";
                        path = item.path;
                        if (path.StartsWith("~"))
                        {
                            path = path.Substring(1);
                        }
                    }
                    //your logic to save filePath to database, for example

                    items.result = true;
                    items.value = filePath;
                    string baseUrl = AppsInfo.upload_host;
                    items.url = baseUrl + path;
                    return this.Ok(items);
                }
                else
                {

                }
                return View();
            }
            catch (System.Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region comfirmPostDate
        [HttpPost("comfirmPostDate")]
        public IActionResult confirmPostDate([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodIssueService();
                var Models = new PlanGoodDocIssueViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodDocIssueViewModel>(body.ToString());
                var result = service.comfirmPostDate(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region printProductReturn
        [HttpPost("printProductReturn")]
        public IActionResult printProductReturn([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new PlanGoodIssueService();
                ReportPlanGoodsIssueViewModel Models = JsonConvert.DeserializeObject<ReportPlanGoodsIssueViewModel>(body.ToString());
                localFilePath = service.printProductReturnService(Models, _hostingEnvironment.ContentRootPath);
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
    }
}
