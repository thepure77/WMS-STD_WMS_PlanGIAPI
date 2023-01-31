using AspNetCore.Reporting;
using Business.Library;
using Comone.Utils;
using DataAccess;
using GIBusiness.GoodIssue;
using GIDataAccess.Models;
using GRDataAccess.Models;
using MasterBusiness.PlanGoodsIssue;
using MasterDataBusiness.CostCenter;
using MasterDataBusiness.StorageLoc;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using planGIBusiness.AutoNumber;
using planGIBusiness.PlanGoodsIssue;
using PlanGIBusiness.Libs;
using PlanGIBusiness.Reports;
using PlanGIBusiness.Reports.DeliveryNote;
using PlanGIBusiness.Reports.Pack;
using PlanGIBusiness.Reports.ProductReturn;
using PlanGIBusiness.ViewModels;
using PlanGIDataAccess.Models;
using planGoodsIssueBusiness.GoodsReceive;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using static planGIBusiness.PlanGoodsIssue.PopupGIViewModel;
using static planGIBusiness.PlanGoodsIssue.SearchDetailModel;
using static PlanGIBusiness.PlanGoodIssue.PlanGoodDocIssueViewModel;

namespace PlanGIBusiness.PlanGoodIssue
{
    public class EmergencyBillingService
    {
        #region EmergencyBillingService
        private PlanGIDbContext db;

        public EmergencyBillingService()
        {
            db = new PlanGIDbContext();
        }

        public EmergencyBillingService(PlanGIDbContext db)
        {
            this.db = db;
        }

        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));

            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
        #endregion

        #region filter
        public actionResultViewModel filter(SearchDetailModel model)
        {
            try
            {
                var query = db.VIEW_PlanGoodsIssue.AsQueryable();

                
                if (model.status.Count == 0)
                {
                    query = query.Where(c => c.Document_Status != -1);
                }

                #region advanceSearch
                if (model.advanceSearch == true)
                {
                    if (!string.IsNullOrEmpty(model.planGoodsIssue_No))
                    {
                        query = query.Where(c => c.PlanGoodsIssue_No == (model.planGoodsIssue_No));
                    }

                    if (!string.IsNullOrEmpty(model.owner_Name))
                    {
                        query = query.Where(c => c.Owner_Name.Contains(model.owner_Name));
                    }


                    if (!string.IsNullOrEmpty(model.warehouse_Name))
                    {
                        query = query.Where(c => c.Warehouse_Name.Contains(model.warehouse_Name));
                    }

                    if (!string.IsNullOrEmpty(model.warehouse_Name_To))
                    {
                        query = query.Where(c => c.Warehouse_Name_To.Contains(model.warehouse_Name_To));
                    }

                    if (!string.IsNullOrEmpty(model.document_Status.ToString()))
                    {
                        query = query.Where(c => c.Document_Status == (model.document_Status));
                    }

                    if (!string.IsNullOrEmpty(model.round_Index.ToString()))
                    {
                        query = query.Where(c => c.Round_Index == (model.round_Index));
                    }

                    if (!string.IsNullOrEmpty(model.processStatus_Name))
                    {
                        int DocumentStatue = 0;

                        var StatusName = new List<ProcessStatusViewModel>();

                        var StatusModel = new ProcessStatusViewModel();

                        StatusModel.process_Index = new Guid("80E8E627-6A2D-4075-9BA6-04B7178C1203");

                        StatusModel.processStatus_Name = model.processStatus_Name;

                        //GetConfig
                        StatusName = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), StatusModel.sJson());

                        if (StatusName.Count > 0)
                        {
                            DocumentStatue = StatusName.FirstOrDefault().processStatus_Id.sParse<int>();
                        }

                        query = query.Where(c => c.Document_Status == DocumentStatue);
                    }

                    if (!string.IsNullOrEmpty(model.documentType_Index.ToString()) && model.documentType_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        query = query.Where(c => c.DocumentType_Index == (model.documentType_Index));
                    }

                    if (!string.IsNullOrEmpty(model.planGoodsIssue_Date) && !string.IsNullOrEmpty(model.planGoodsIssue_Date_To))
                    {
                        var dateStart = model.planGoodsIssue_Date.toBetweenDate();
                        var dateEnd = model.planGoodsIssue_Date_To.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsIssue_Date >= dateStart.start && c.PlanGoodsIssue_Date <= dateEnd.end);
                    }
                    else if (!string.IsNullOrEmpty(model.planGoodsIssue_Date))
                    {
                        var planGoodsIssue_date_From = model.planGoodsIssue_Date.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsIssue_Date >= planGoodsIssue_date_From.start);
                    }
                    else if (!string.IsNullOrEmpty(model.planGoodsIssue_Date_To))
                    {
                        var planGoodsIssue_date_To = model.planGoodsIssue_Date_To.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsIssue_Date <= planGoodsIssue_date_To.start);
                    }

                    //if (!string.IsNullOrEmpty(model.planGoodsIssue_Due_Date) && !string.IsNullOrEmpty(model.planGoodsIssue_Due_Date_To))
                    //{
                    //    var dateStart = model.planGoodsIssue_Due_Date.toBetweenDate();
                    //    var dateEnd = model.planGoodsIssue_Due_Date_To.toBetweenDate();
                    //    query = query.Where(c => c.PlanGoodsIssue_Due_Date >= dateStart.start && c.PlanGoodsIssue_Due_Date <= dateEnd.end);
                    //}

                    //else if (!string.IsNullOrEmpty(model.planGoodsIssue_Due_Date))
                    //{
                    //    var planGoodsIssue_due_date_From = model.planGoodsIssue_Due_Date.toBetweenDate();
                    //    query = query.Where(c => c.PlanGoodsIssue_Due_Date >= planGoodsIssue_due_date_From.start);
                    //}
                    //else if (!string.IsNullOrEmpty(model.planGoodsIssue_Due_Date_To))
                    //{
                    //    var planGoodsIssue_due_date_To = model.planGoodsIssue_Due_Date_To.toBetweenDate();
                    //    query = query.Where(c => c.PlanGoodsIssue_Due_Date <= planGoodsIssue_due_date_To.start);
                    //}

                    if (!string.IsNullOrEmpty(model.create_By))
                    {
                        query = query.Where(c => c.Create_By == (model.create_By));
                    }

                    if (!string.IsNullOrEmpty(model.billing_no) && model.billing_no != "-")
                    {
                        query = query.Where(c => c.DocumentRef_No5 == model.billing_no);
                    }
                    if (!string.IsNullOrEmpty(model.matdoc_no) && model.matdoc_no != "-")
                    {
                        query = query.Where(c => c.Matdoc == model.matdoc_no);
                    }
                    if (!string.IsNullOrEmpty(model.TruckLoad_No) && model.TruckLoad_No != "-")
                    {
                        query = query.Where(c => c.TruckLoad_No == model.TruckLoad_No);
                    }
                    if (!string.IsNullOrEmpty(model.GoodsIssue_No) && model.GoodsIssue_No != "-")
                    {
                        query = query.Where(c => c.GoodsIssue_No == model.GoodsIssue_No);
                    }
                }

                #endregion

                #region Basic
                else
                {
                    if (!string.IsNullOrEmpty(model.key))
                    {
                        query = query.Where(c => c.PlanGoodsIssue_No.Contains(model.key)
                                            //|| c.QTY.Equals(model.key)
                                            //|| c.Weight.Equals(model.key)
                                            || c.Owner_Name.Contains(model.key)
                                            || c.Create_By.Contains(model.key)
                                            || c.DocumentRef_No1.Contains(model.key)
                                            || c.DocumentType_Name.Contains(model.key));
                    }

                    if (!string.IsNullOrEmpty(model.planGoodsIssue_Date) && !string.IsNullOrEmpty(model.planGoodsIssue_Date_To))
                    {
                        var dateStart = model.planGoodsIssue_Date.toBetweenDate();
                        var dateEnd = model.planGoodsIssue_Date_To.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsIssue_Date >= dateStart.start && c.PlanGoodsIssue_Date <= dateEnd.end);
                    }
                    else if (!string.IsNullOrEmpty(model.planGoodsIssue_Date))
                    {
                        var planGoodsIssue_date_From = model.planGoodsIssue_Date.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsIssue_Date >= planGoodsIssue_date_From.start);
                    }
                    else if (!string.IsNullOrEmpty(model.planGoodsIssue_Date_To))
                    {
                        var planGoodsIssue_date_To = model.planGoodsIssue_Date_To.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsIssue_Due_Date <= planGoodsIssue_date_To.start);
                    }

                    var statusModels = new List<int?>();
                    var sortModels = new List<SortModel>();

                    if (model.status.Count > 0)
                    {
                        foreach (var item in model.status)
                        {

                            if (item.value == 0)
                            {
                                statusModels.Add(0);
                            }
                            if (item.value == 1)
                            {
                                statusModels.Add(1);
                            }
                            if (item.value == 2)
                            {
                                statusModels.Add(2);
                            }
                            if (item.value == 3)
                            {
                                statusModels.Add(3);
                            }
                            if (item.value == 4)
                            {
                                statusModels.Add(4);
                            }
                            if (item.value == -1)
                            {
                                statusModels.Add(-1);
                            }
                            if (item.value == 5)
                            {
                                statusModels.Add(5);
                            }
                        }

                        query = query.Where(c => statusModels.Contains(c.Document_Status));
                    }

                    if (model.sort.Count > 0)
                    {
                        foreach (var item in model.sort)
                        {

                            if (item.value == "PlanGoodsIssue_No")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "PlanGoodsIssue_No",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "PlanGoodsIssue_Date")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "PlanGoodsIssue_Date",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "DocumentType_Name")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "DocumentType_Name",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "Qty")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "Qty",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "Weight")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "Weight",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "ProcessStatus_Name")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "Document_Status",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "Create_By")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "Create_By",
                                    Sort = "desc"
                                });

                            }
                        }
                        query = query.KWOrderBy(sortModels);

                    }

                }

                #endregion

                var Item = new List<VIEW_PlanGoodsIssue>();
                var TotalRow = new List<VIEW_PlanGoodsIssue>();


                TotalRow = query.OrderBy(c => c.Order_seq).ToList();


                if (model.CurrentPage != 0 && model.PerPage != 0)
                {
                    query = query.OrderBy(c => c.Order_seq).Skip(((model.CurrentPage - 1) * model.PerPage));
                }

                if (model.PerPage != 0)
                {
                    query = query.Take(model.PerPage);

                }
                if (model.sort.Count > 0)
                {
                    Item = query.ToList();
                }
                else
                {
                    Item = query.ToList();
                }
                
                var ProcessStatus = new List<ProcessStatusViewModel>();

                var filterModel = new ProcessStatusViewModel();

                filterModel.process_Index = new Guid("80E8E627-6A2D-4075-9BA6-04B7178C1203");

                //GetConfig
                ProcessStatus = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), filterModel.sJson());


                String Statue = "";
                var result = new List<SearchDetailModel>();

                foreach (var item in Item)
                {
                    var resultItem = new SearchDetailModel();
                    resultItem.planGoodsIssue_Index = item.PlanGoodsIssue_Index;
                    resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;
                    resultItem.planGoodsIssue_Date = item.PlanGoodsIssue_Date.toString();
                    resultItem.planGoodsIssue_Due_Date = item.PlanGoodsIssue_Due_Date.toString();
                    resultItem.date_Budget = item.Date_Budget.toString();
                    resultItem.documentType_Index = item.DocumentType_Index;
                    resultItem.documentType_Name = item.DocumentType_Name;
                    resultItem.document_Status = item.Document_Status;
                    resultItem.owner_Index = item.Owner_Index;
                    resultItem.owner_Id = item.Owner_Id;
                    resultItem.owner_Name = item.Owner_Name;
                    resultItem.shipTo_Index = item.ShipTo_Index;
                    resultItem.shipTo_Id = item.ShipTo_Id;
                    resultItem.shipTo_Name = item.ShipTo_Name;
                    resultItem.round_Index = item.Round_Index;
                    resultItem.round_Id = item.Round_Id;
                    resultItem.round_Name = item.Round_Name;
                    resultItem.route_Index = item.Route_Index;
                    resultItem.route_Id = item.Route_Id;
                    resultItem.route_Name = item.Route_Name;
                    resultItem.subRoute_Index = item.SubRoute_Index;
                    resultItem.subRoute_Id = item.SubRoute_Id;
                    resultItem.subRoute_Name = item.SubRoute_Name;
                    resultItem.shippingMethod_Index = item.ShippingMethod_Index;
                    resultItem.shippingMethod_Id = item.ShippingMethod_Id;
                    resultItem.shippingMethod_Name = item.ShippingMethod_Name;
                    resultItem.billing_no = item.DocumentRef_No5;
                    resultItem.status_AMZ = item.DocumentRef_No7;
                    resultItem.reamrk = item.DocumentRef_No8;
                    if (item.DocumentType_Index != Guid.Parse("CBD5CF3A-ACEA-4035-AD25-6BE57C8F1485"))
                    {
                        resultItem.matdoc = item.Matdoc;
                    }
                    resultItem.TruckLoad_No = item.TruckLoad_No;
                    resultItem.GoodsIssue_No = item.GoodsIssue_No;
                    resultItem.RunWave_Status = item.RunWave_Status;
                    resultItem.first_Order = item.First_Order ?? 0;

                    Statue = item.Document_Status.ToString();
                    var ProcessStatusName = ProcessStatus.Where(c => c.processStatus_Id == Statue).FirstOrDefault();
                    resultItem.processStatus_Name = ProcessStatusName.processStatus_Name;

                    resultItem.document_Remark = item.Document_Remark;
                    //resultItem.qty = string.Format(String.Format("{0:N3}", item.QTY));
                    //resultItem.weight = string.Format(String.Format("{0:N3}", item.Weight));

                    resultItem.create_By = item.Create_By;
                    resultItem.update_By = item.Update_By;
                    resultItem.cancel_By = item.Cancel_By;
                    resultItem.TruckLoad_No = item.TruckLoad_No;
                    resultItem.Truckload_status = item.Truckload_status;
                    resultItem.documentRef_No3 = item.Return_status;
                    result.Add(resultItem);
                }
                var count = TotalRow.Count;

                var actionResult = new actionResultViewModel();
                actionResult.itemsPlanGI = result.OrderByDescending(o => o.create_Date).ThenByDescending(o => o.create_Date).ToList();
                actionResult.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage, };

                return actionResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public actionResultViewModel FilterInClause(SearchPlanGoodsIssueInClauseViewModel model)
        {
            try
            {
                var query = db.im_PlanGoodsIssue.AsQueryable();

                if (!(model is null))
                {
                    if (!(model.List_PlanGoodsIssue_Index is null) && model.List_PlanGoodsIssue_Index.Count > 0)
                    {
                        query = query.Where(w => model.List_PlanGoodsIssue_Index.Contains(w.PlanGoodsIssue_Index));
                    }

                    if (!(model.List_PlanGoodsIssue_No is null) && model.List_PlanGoodsIssue_No.Count > 0)
                    {
                        query = query.Where(w => model.List_PlanGoodsIssue_No.Contains(w.PlanGoodsIssue_No));
                    }
                }

                var Item = new List<im_PlanGoodsIssue>();
                var TotalRow = new List<im_PlanGoodsIssue>();

                TotalRow = query.OrderByDescending(o => o.Create_Date).ThenByDescending(o => o.Create_Date).ToList();


                if (model.CurrentPage != 0 && model.PerPage != 0)
                {
                    query = query.Skip(((model.CurrentPage - 1) * model.PerPage));
                }

                if (model.PerPage != 0)
                {
                    query = query.Take(model.PerPage);

                }

                Item = query.ToList();
                //var perpages = model.PerPage == 0 ? query.ToList() : query.OrderByDescending(o => o.Create_Date).ThenByDescending(o => o.Create_Date).Skip((model.CurrentPage - 1) * model.PerPage).Take(model.PerPage).ToList();

                var ProcessStatus = new List<ProcessStatusViewModel>();

                var filterModel = new ProcessStatusViewModel();

                filterModel.process_Index = new Guid("80E8E627-6A2D-4075-9BA6-04B7178C1203");

                //GetConfig
                ProcessStatus = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), filterModel.sJson());


                String Statue = "";
                var result = new List<SearchDetailModel>();

                foreach (var item in Item)
                {
                    var resultItem = new SearchDetailModel();
                    resultItem.planGoodsIssue_Index = item.PlanGoodsIssue_Index;
                    resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;
                    resultItem.planGoodsIssue_Date = item.PlanGoodsIssue_Date.toString();
                    resultItem.planGoodsIssue_Due_Date = item.PlanGoodsIssue_Due_Date.toString();
                    resultItem.date_Budget = item.Date_Budget.toString();
                    resultItem.documentType_Index = item.DocumentType_Index;
                    resultItem.documentType_Name = item.DocumentType_Name;
                    resultItem.document_Status = item.Document_Status;
                    resultItem.owner_Index = item.Owner_Index;
                    resultItem.owner_Id = item.Owner_Id;
                    resultItem.owner_Name = item.Owner_Name;
                    resultItem.shipTo_Index = item.ShipTo_Index;
                    resultItem.shipTo_Id = item.ShipTo_Id;
                    resultItem.shipTo_Name = item.ShipTo_Name;
                    resultItem.round_Index = item.Round_Index;
                    resultItem.round_Id = item.Round_Id;
                    resultItem.round_Name = item.Round_Name;
                    resultItem.route_Index = item.Route_Index;
                    resultItem.route_Id = item.Route_Id;
                    resultItem.route_Name = item.Route_Name;
                    resultItem.subRoute_Index = item.SubRoute_Index;
                    resultItem.subRoute_Id = item.SubRoute_Id;
                    resultItem.subRoute_Name = item.SubRoute_Name;
                    resultItem.shippingMethod_Index = item.ShippingMethod_Index;
                    resultItem.shippingMethod_Id = item.ShippingMethod_Id;
                    resultItem.shippingMethod_Name = item.ShippingMethod_Name;

                    Statue = item.Document_Status.ToString();
                    var ProcessStatusName = ProcessStatus.Where(c => c.processStatus_Id == Statue).FirstOrDefault();
                    resultItem.processStatus_Name = ProcessStatusName.processStatus_Name;

                    resultItem.document_Remark = item.Document_Remark;
                    //resultItem.qty = string.Format(String.Format("{0:N3}", item.QTY));
                    //resultItem.weight = string.Format(String.Format("{0:N3}", item.Weight));

                    resultItem.create_By = item.Create_By;
                    resultItem.update_By = item.Update_By;
                    resultItem.cancel_By = item.Cancel_By;
                    result.Add(resultItem);
                }
                var count = TotalRow.Count;

                var actionResult = new actionResultViewModel();
                actionResult.itemsPlanGI = result.OrderByDescending(o => o.create_Date).ThenByDescending(o => o.create_Date).ToList();
                actionResult.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage, };

                return actionResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        
        #region printOutDeliveryNote

        #region New
        public string printOutDeliveryNote(ReportPlanGoodsIssueViewModel data, string rootPath = "")
        {

            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                db.Database.SetCommandTimeout(360);
                var result = new List<DeliveryNote>();

                var XXX = data.model.Select(x => x.planGoodsIssue_Index).ToList();
                var rpt_data = db.View_RPT_Delivery_Note.Where(C => XXX.Contains(C.PlanGoodsIssue_Index)).OrderBy(c=> c.Order_seq).ThenBy(c=> c.Row_Runing_product).ToList();
                foreach (var item in rpt_data)
                {

                    if (item.DocumentType_Index == Guid.Parse("319C8EB2-2B10-4E1C-8ACA-E57FB8160583"))
                    {
                        var Splitdate = item.Appointment_Date.ToString().Split('/');
                        int Month = int.Parse(Splitdate[0]);
                        var year = (int.Parse((Splitdate[2].Split(' '))[0])) + 543;
                        var day = Splitdate[1];
                        var thaiMonth = "";
                        switch (Month)
                        {
                            case 1:
                                thaiMonth = "มกราคม";
                                break;
                            case 2:
                                thaiMonth = "กุมภาพันธ์";
                                break;
                            case 3:
                                thaiMonth = "มีนาคม";
                                break;
                            case 4:
                                thaiMonth = "เมษายน";
                                break;
                            case 5:
                                thaiMonth = "พฤษภาคม";
                                break;
                            case 6:
                                thaiMonth = "มิถุนายน";
                                break;
                            case 7:
                                thaiMonth = "กรกฎาคม";
                                break;
                            case 8:
                                thaiMonth = "สิงหาคม";
                                break;
                            case 9:
                                thaiMonth = "กันยายน";
                                break;
                            case 10:
                                thaiMonth = "ตุลาคม";
                                break;
                            case 11:
                                thaiMonth = "พฤศจิกายน";
                                break;
                            case 12:
                                thaiMonth = "ธันวาคม";
                                break;
                        }

                        var list = new DeliveryNote();
                        list.planGoods_Barcode = new NetBarcode.Barcode(item.PlanGoodsIssue_No, NetBarcode.Type.Code128B).GetBase64Image();
                        if (item.Billing != null)
                        {
                            list.Billing_Barcode = new NetBarcode.Barcode(item.Billing, NetBarcode.Type.Code128B).GetBase64Image();
                        }
                        list.copy_int = "";
                        list.for_report_int = "2";
                        list.for_report =  "สำหรับขนส่งและคลัง For DC";
                        list.TruckLoad_No = item.TruckLoad_No;
                        list.Appointment_Date = day + " " + thaiMonth + " " + year;
                        list.Appointment_Time = item.Appointment_Time;
                        list.branch = item.branch;
                        list.ShipTo_Name = item.ShipTo_Name;
                        list.ShipTo_Address = item.ShipTo_Address;
                        list.PlanGoodsIssue_No = item.PlanGoodsIssue_No;
                        list.Billing = item.Billing;
                        list.Product_Id = item.Product_Id;
                        list.Product_Name = item.Product_Name;
                        list.Product_Lot = item.Product_Lot;
                        list.ProductConversion_Name = item.ProductConversion_Name;
                        list.ShipTo_Id = item.ShipTo_Id;
                        list.qty = item.qty;
                        list.Row_Runing_product = item.Row_Runing_product;
                        list.pick = item.pick;
                        list.Shipping_Remark = item.Shipping_Remark;
                        list.Order_Remark = item.Order_Remark;


                        result.Add(list);
                    }
                    else {
                        for (int i = 0; i < 2; i++)
                        {
                            var Splitdate = item.Appointment_Date.ToString().Split('/');
                            int Month = int.Parse(Splitdate[0]);
                            var year = (int.Parse((Splitdate[2].Split(' '))[0])) + 543;
                            var day = Splitdate[1];
                            var thaiMonth = "";
                            switch (Month)
                            {
                                case 1:
                                    thaiMonth = "มกราคม";
                                    break;
                                case 2:
                                    thaiMonth = "กุมภาพันธ์";
                                    break;
                                case 3:
                                    thaiMonth = "มีนาคม";
                                    break;
                                case 4:
                                    thaiMonth = "เมษายน";
                                    break;
                                case 5:
                                    thaiMonth = "พฤษภาคม";
                                    break;
                                case 6:
                                    thaiMonth = "มิถุนายน";
                                    break;
                                case 7:
                                    thaiMonth = "กรกฎาคม";
                                    break;
                                case 8:
                                    thaiMonth = "สิงหาคม";
                                    break;
                                case 9:
                                    thaiMonth = "กันยายน";
                                    break;
                                case 10:
                                    thaiMonth = "ตุลาคม";
                                    break;
                                case 11:
                                    thaiMonth = "พฤศจิกายน";
                                    break;
                                case 12:
                                    thaiMonth = "ธันวาคม";
                                    break;
                            }

                            var list = new DeliveryNote();
                            list.planGoods_Barcode = new NetBarcode.Barcode(item.PlanGoodsIssue_No, NetBarcode.Type.Code128B).GetBase64Image();
                            if (item.Billing != null)
                            {
                                list.Billing_Barcode = new NetBarcode.Barcode(item.Billing, NetBarcode.Type.Code128B).GetBase64Image();
                            }
                            list.copy_int = i == 0 ? "ต้นฉบับ" : "สำเนา";
                            list.for_report_int = i == 0 ? "1" : "2";
                            list.for_report = i == 0 ? "สำหรับลูกค้า For Customer" : "สำหรับขนส่งและคลัง For DC";
                            list.TruckLoad_No = item.TruckLoad_No;
                            list.Appointment_Date = day + " " + thaiMonth + " " + year;
                            list.Appointment_Time = item.Appointment_Time;
                            list.branch = item.branch;
                            list.ShipTo_Name = item.ShipTo_Name;
                            list.ShipTo_Address = item.ShipTo_Address;
                            list.PlanGoodsIssue_No = item.PlanGoodsIssue_No;
                            list.Billing = item.Billing;
                            list.Product_Id = item.Product_Id;
                            list.Product_Name = item.Product_Name;
                            list.Product_Lot = item.Product_Lot;
                            list.ProductConversion_Name = item.ProductConversion_Name;
                            list.ShipTo_Id = item.ShipTo_Id;
                            list.qty = item.qty;
                            list.Row_Runing_product = item.Row_Runing_product;
                            list.pick = item.pick;
                            list.Shipping_Remark = item.Shipping_Remark;
                            list.Order_Remark = item.Order_Remark;


                            result.Add(list);
                        }
                    }
                }

                rootPath = rootPath.Replace("\\PlanGIAPI", "");
                var reportPath = rootPath + new AppSettingConfig().GetUrl("DeliveryNote");
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region emergency
        public string printOutDeliveryNote_emergency(ReportPlanGoodsIssueViewModel data, string rootPath = "")
        {

            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var result = new List<DeliveryNote>();

                var PGI = new SqlParameter("@planGoodsIssue_Index", data.planGoodsIssue_Index);
                var rpt_data = db.View_RPT_DeliveryNote_emergency.FromSql("sp_RPT_DeliveryNote_emergency_E @planGoodsIssue_Index", PGI).OrderBy(c => c.Order_seq).ThenBy(c => c.Row_Runing_product).ToList();

                foreach (var item in rpt_data)
                {

                    if (item.DocumentType_Index == Guid.Parse("319C8EB2-2B10-4E1C-8ACA-E57FB8160583"))
                    {
                        var Splitdate = item.Appointment_Date.ToString().Split('/');
                        int Month = int.Parse(Splitdate[0]);
                        var year = (int.Parse((Splitdate[2].Split(' '))[0])) + 543;
                        var day = Splitdate[1];
                        var thaiMonth = "";
                        switch (Month)
                        {
                            case 1:
                                thaiMonth = "มกราคม";
                                break;
                            case 2:
                                thaiMonth = "กุมภาพันธ์";
                                break;
                            case 3:
                                thaiMonth = "มีนาคม";
                                break;
                            case 4:
                                thaiMonth = "เมษายน";
                                break;
                            case 5:
                                thaiMonth = "พฤษภาคม";
                                break;
                            case 6:
                                thaiMonth = "มิถุนายน";
                                break;
                            case 7:
                                thaiMonth = "กรกฎาคม";
                                break;
                            case 8:
                                thaiMonth = "สิงหาคม";
                                break;
                            case 9:
                                thaiMonth = "กันยายน";
                                break;
                            case 10:
                                thaiMonth = "ตุลาคม";
                                break;
                            case 11:
                                thaiMonth = "พฤศจิกายน";
                                break;
                            case 12:
                                thaiMonth = "ธันวาคม";
                                break;
                        }

                        var list = new DeliveryNote();
                        list.planGoods_Barcode = new NetBarcode.Barcode(item.PlanGoodsIssue_No, NetBarcode.Type.Code128B).GetBase64Image();
                        if (item.Billing != null)
                        {
                            list.Billing_Barcode = new NetBarcode.Barcode(item.Billing, NetBarcode.Type.Code128B).GetBase64Image();
                        }
                        list.copy_int = "";
                        list.for_report_int = "2";
                        list.for_report = "สำหรับขนส่งและคลัง For DC";
                        list.TruckLoad_No = item.TruckLoad_No;
                        list.Appointment_Date = day + " " + thaiMonth + " " + year;
                        list.Appointment_Time = item.Appointment_Time;
                        list.branch = item.branch;
                        list.ShipTo_Name = item.ShipTo_Name;
                        list.ShipTo_Address = item.ShipTo_Address;
                        list.PlanGoodsIssue_No = item.PlanGoodsIssue_No;
                        list.Billing = item.Billing;
                        list.Product_Id = item.Product_Id;
                        list.Product_Name = item.Product_Name;
                        list.Product_Lot = item.Product_Lot;
                        list.ProductConversion_Name = item.ProductConversion_Name;
                        list.ShipTo_Id = item.ShipTo_Id;
                        list.qty = item.qty;
                        list.Row_Runing_product = item.Row_Runing_product;
                        list.pick = item.pick;
                        list.Shipping_Remark = item.Shipping_Remark;
                        list.Order_Remark = item.Order_Remark;


                        result.Add(list);
                    }
                    else
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            var Splitdate = item.Appointment_Date.ToString().Split('/');
                            int Month = int.Parse(Splitdate[0]);
                            var year = (int.Parse((Splitdate[2].Split(' '))[0])) + 543;
                            var day = Splitdate[1];
                            var thaiMonth = "";
                            switch (Month)
                            {
                                case 1:
                                    thaiMonth = "มกราคม";
                                    break;
                                case 2:
                                    thaiMonth = "กุมภาพันธ์";
                                    break;
                                case 3:
                                    thaiMonth = "มีนาคม";
                                    break;
                                case 4:
                                    thaiMonth = "เมษายน";
                                    break;
                                case 5:
                                    thaiMonth = "พฤษภาคม";
                                    break;
                                case 6:
                                    thaiMonth = "มิถุนายน";
                                    break;
                                case 7:
                                    thaiMonth = "กรกฎาคม";
                                    break;
                                case 8:
                                    thaiMonth = "สิงหาคม";
                                    break;
                                case 9:
                                    thaiMonth = "กันยายน";
                                    break;
                                case 10:
                                    thaiMonth = "ตุลาคม";
                                    break;
                                case 11:
                                    thaiMonth = "พฤศจิกายน";
                                    break;
                                case 12:
                                    thaiMonth = "ธันวาคม";
                                    break;
                            }

                            var list = new DeliveryNote();
                            list.planGoods_Barcode = new NetBarcode.Barcode(item.PlanGoodsIssue_No, NetBarcode.Type.Code128B).GetBase64Image();
                            if (item.Billing != null)
                            {
                                list.Billing_Barcode = new NetBarcode.Barcode(item.Billing, NetBarcode.Type.Code128B).GetBase64Image();
                            }
                            list.copy_int = i == 0 ? "ต้นฉบับ" : "สำเนา";
                            list.for_report_int = i == 0 ? "1" : "2";
                            list.for_report = i == 0 ? "สำหรับลูกค้า For Customer" : "สำหรับขนส่งและคลัง For DC";
                            list.TruckLoad_No = item.TruckLoad_No;
                            list.Appointment_Date = day + " " + thaiMonth + " " + year;
                            list.Appointment_Time = item.Appointment_Time;
                            list.branch = item.branch;
                            list.ShipTo_Name = item.ShipTo_Name;
                            list.ShipTo_Address = item.ShipTo_Address;
                            list.PlanGoodsIssue_No = item.PlanGoodsIssue_No;
                            list.Billing = item.Billing;
                            list.Product_Id = item.Product_Id;
                            list.Product_Name = item.Product_Name;
                            list.Product_Lot = item.Product_Lot;
                            list.ProductConversion_Name = item.ProductConversion_Name;
                            list.ShipTo_Id = item.ShipTo_Id;
                            list.qty = item.qty;
                            list.Row_Runing_product = item.Row_Runing_product;
                            list.pick = item.pick;
                            list.Shipping_Remark = item.Shipping_Remark;
                            list.Order_Remark = item.Order_Remark;


                            result.Add(list);
                        }
                    }
                }

                rootPath = rootPath.Replace("\\PlanGIAPI", "");
                var reportPath = rootPath + new AppSettingConfig().GetUrl("DeliveryNote_emergency");
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region OLD
        //public string printOutDeliveryNote(ReportPlanGoodsIssueViewModel data, string rootPath = "")
        //{

        //    var culture = new System.Globalization.CultureInfo("en-US");
        //    String State = "Start";
        //    String msglog = "";
        //    var olog = new logtxt();

        //    try
        //    {
        //        var result = new List<DeliveryNote>();
        //        var rpt_data = db.View_RPT_Delivery_Note.Where(C => C.PlanGoodsIssue_Index == data.planGoodsIssue_Index).OrderBy(c => c.Row_Runing_product).ToList();
        //        foreach (var item in rpt_data)
        //        {

        //            var Splitdate = item.Appointment_Date.ToString().Split('/');
        //            int Month = int.Parse(Splitdate[0]);
        //            var year = (int.Parse((Splitdate[2].Split(' '))[0])) + 543;
        //            var day = Splitdate[1];
        //            var thaiMonth = "";
        //            switch (Month)
        //            {
        //                case 1:
        //                    thaiMonth = "มกราคม";
        //                    break;
        //                case 2:
        //                    thaiMonth = "กุมภาพันธ์";
        //                    break;
        //                case 3:
        //                    thaiMonth = "มีนาคม";
        //                    break;
        //                case 4:
        //                    thaiMonth = "เมษายน";
        //                    break;
        //                case 5:
        //                    thaiMonth = "พฤษภาคม";
        //                    break;
        //                case 6:
        //                    thaiMonth = "มิถุนายน";
        //                    break;
        //                case 7:
        //                    thaiMonth = "กรกฎาคม";
        //                    break;
        //                case 8:
        //                    thaiMonth = "สิงหาคม";
        //                    break;
        //                case 9:
        //                    thaiMonth = "กันยายน";
        //                    break;
        //                case 10:
        //                    thaiMonth = "ตุลาคม";
        //                    break;
        //                case 11:
        //                    thaiMonth = "พฤศจิกายน";
        //                    break;
        //                case 12:
        //                    thaiMonth = "ธันวาคม";
        //                    break;
        //            }

        //            var list = new DeliveryNote();
        //            list.planGoods_Barcode = new NetBarcode.Barcode(item.PlanGoodsIssue_No, NetBarcode.Type.Code128B).GetBase64Image();
        //            if (item.Billing != null)
        //            {
        //                list.Billing_Barcode = new NetBarcode.Barcode(item.Billing, NetBarcode.Type.Code128B).GetBase64Image();
        //            }

        //            list.TruckLoad_No = item.TruckLoad_No;
        //            list.Appointment_Date = day + " " + thaiMonth + " " + year;
        //            list.Appointment_Time = item.Appointment_Time;
        //            list.branch = item.branch;
        //            list.ShipTo_Name = item.ShipTo_Name;
        //            list.ShipTo_Address = item.ShipTo_Address;
        //            list.PlanGoodsIssue_No = item.PlanGoodsIssue_No;
        //            list.Billing = item.Billing;
        //            list.Product_Id = item.Product_Id;
        //            list.Product_Name = item.Product_Name;
        //            list.Product_Lot = item.Product_Lot;
        //            list.ProductConversion_Name = item.ProductConversion_Name;
        //            list.ShipTo_Id = item.ShipTo_Id;
        //            list.qty = item.qty;
        //            list.Row_Runing_product = item.Row_Runing_product;
        //            list.pick = item.pick;
        //            list.Shipping_Remark = item.Shipping_Remark;
        //            list.Order_Remark = item.Order_Remark;


        //            result.Add(list);
        //        }

        //        rootPath = rootPath.Replace("\\PlanGIAPI", "");
        //        var reportPath = rootPath + new AppSettingConfig().GetUrl("DeliveryNote");
        //        LocalReport report = new LocalReport(reportPath);
        //        report.AddDataSource("DataSet1", result);

        //        System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        //        string fileName = "";
        //        string fullPath = "";
        //        fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

        //        var renderedBytes = report.Execute(RenderType.Pdf);

        //        Utils objReport = new Utils();
        //        fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
        //        var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
        //        return saveLocation;


        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        #endregion

        #endregion
            
    }
}