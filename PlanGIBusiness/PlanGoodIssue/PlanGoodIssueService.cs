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
    public class PlanGoodIssueService
    {

        private PlanGIDbContext db;

        public PlanGoodIssueService()
        {
            db = new PlanGIDbContext();
        }

        public PlanGoodIssueService(PlanGIDbContext db)
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
                    resultItem.Pre_Order = item.Pre_Order ?? 0 /*( == 0 ? "No" : "Yes")*/;

                    Statue = item.Document_Status.ToString();
                    var ProcessStatusName = ProcessStatus.Where(c => c.processStatus_Id == Statue).FirstOrDefault();
                    resultItem.processStatus_Name = ProcessStatusName.processStatus_Name;

                    resultItem.document_Remark = item.Document_Remark;
                    //resultItem.qty = string.Format(String.Format("{0:N3}", item.QTY));
                    //resultItem.weight = string.Format(String.Format("{0:N3}", item.Weight));

                    resultItem.create_By = item.Create_By;
                    resultItem.create_Date = item.Create_Date;
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


        #region CreateOrUpdate
        public actionResult CreateOrUpdate(PlanGoodDocIssueViewModel data)
        {
            Guid PlanGoodsIssueIndex = new Guid();
            String PlanGoodsIssueNo = "";

            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            Boolean IsNew = false;

            var actionResult = new actionResult();

            try
            {
                var WeightViewModel = utils.SendDataApi<List<WeightViewModel>>(new AppSettingConfig().GetUrl("dropdownWeight"), new {  }.sJson()).FirstOrDefault(c => c.weight_Index == Guid.Parse("080AEF7B-E9C5-4B84-969A-2D033F0C1E2A"));
                //var VolumeViewModel = utils.SendDataApi<List<VolumeViewModel>>(new AppSettingConfig().GetUrl("dropdownVolume"), new {  }.sJson()).FirstOrDefault(c => c.volume_Index == Guid.Parse("3778CD6E-45ED-499A-8ACC-9EB1F3AB1A6A"));
                var itemDetail = new List<im_PlanGoodsIssueItem>();

                var PlanGoodsIssueOld = db.im_PlanGoodsIssue.Find(data.planGoodsIssue_Index);


                if (PlanGoodsIssueOld == null)
                {
                    IsNew = true;
                    PlanGoodsIssueIndex = Guid.NewGuid();

                    var result = new List<GenDocumentTypeViewModel>();

                    var filterModel = new GenDocumentTypeViewModel();



                    var resultWH = new List<warehouseDocViewModel>();
                    var filterModelWH = new warehouseDocViewModel();
                    filterModelWH.warehouse_Index = (data.warehouse_Index ?? Guid.Empty);
                    State = "GetdropdownWarehouse";

                    resultWH = utils.SendDataApi<List<warehouseDocViewModel>>(new AppSettingConfig().GetUrl("dropdownWarehouse"), filterModelWH.sJson());

                    filterModel.process_Index = new Guid("80E8E627-6A2D-4075-9BA6-04B7178C1203");
                    filterModel.documentType_Index = data.documentType_Index;

                    State = "GetdropDownDocumentType";

                    //GetConfig
                    result = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), filterModel.sJson());

                    var genDoc = new AutoNumberService();
                    string DocNo = "";

                    if (string.IsNullOrEmpty(data?.planGoodsIssue_No?.Trim() ?? string.Empty))
                    {
                        DateTime DocumentDate = (DateTime)data.planGoodsIssue_Date.toDate();
                        DocNo = genDoc.genAutoDocmentNumber(result, DocumentDate);
                    }
                    else
                    {
                        DocNo = data.planGoodsIssue_No.Trim();
                    }

                    im_PlanGoodsIssue itemHeader = new im_PlanGoodsIssue();
                    var document_status = 0;

                    PlanGoodsIssueNo = DocNo;
                    itemHeader.PlanGoodsIssue_Index = PlanGoodsIssueIndex;
                    itemHeader.PlanGoodsIssue_No = DocNo;
                    itemHeader.Owner_Index = data.owner_Index;
                    itemHeader.Owner_Id = data.owner_Id;
                    itemHeader.Owner_Name = data.owner_Name;
                    itemHeader.Vendor_Id = data.vendor_Id;
                    itemHeader.DocumentType_Index = data.documentType_Index;
                    itemHeader.DocumentType_Id = data.documentType_Id;
                    itemHeader.DocumentType_Name = data.documentType_Name;
                    itemHeader.PlanGoodsIssue_Date = data.planGoodsIssue_Date.toDate();
                    itemHeader.PlanGoodsIssue_Time = data.planGoodsIssue_Time;
                    itemHeader.PlanGoodsIssue_Due_Date = data.planGoodsIssue_Due_Date.toDate();
                    itemHeader.DocumentRef_No1 = data.documentRef_No1;
                    itemHeader.DocumentRef_No2 = data.documentRef_No2;
                    itemHeader.DocumentRef_No3 = data.documentRef_No3;
                    itemHeader.DocumentRef_No4 = data.documentRef_No4;
                    itemHeader.DocumentRef_No5 = data.documentRef_No5;
                    itemHeader.Document_Status = document_status;
                    itemHeader.ShipTo_Index = data.shipTo_Index;
                    itemHeader.ShipTo_Id = data.shipTo_Id;
                    itemHeader.ShipTo_Name = data.shipTo_Name;
                    itemHeader.ShipTo_Address = data.shipTo_Address;
                    itemHeader.ShipTo_Contact_Person = data.shipTo_Contact_Person;
                    itemHeader.SoldTo_Index = data.soldTo_Index;
                    itemHeader.SoldTo_Id = data.soldTo_Id;
                    itemHeader.SoldTo_Name = data.soldTo_Name;
                    itemHeader.SoldTo_Address = data.soldTo_Address;
                    itemHeader.SoldTo_Contact_Person = data.soldTo_Contact_Person;
                    itemHeader.UDF_1 = data.uDF_1;
                    itemHeader.UDF_2 = data.uDF_2;
                    itemHeader.UDF_3 = data.uDF_3;
                    itemHeader.UDF_4 = data.uDF_4;
                    itemHeader.UDF_5 = data.uDF_5;
                    itemHeader.DocumentPriority_Status = data.documentPriority_Status;
                    itemHeader.Document_Remark = data.document_Remark;
                    itemHeader.Warehouse_Index = resultWH.FirstOrDefault().warehouse_Index;
                    itemHeader.Warehouse_Id = resultWH.FirstOrDefault().warehouse_Id;
                    itemHeader.Warehouse_Name = resultWH.FirstOrDefault().warehouse_Name;
                    itemHeader.Warehouse_Index_To = data.warehouse_Index_To;
                    itemHeader.Warehouse_Id_To = data.warehouse_Id_To;
                    itemHeader.Warehouse_Name_To = data.warehouse_Name_To;
                    itemHeader.Round_Index = data.round_Index;
                    itemHeader.Round_Id = data.round_Id;
                    itemHeader.Round_Name = data.round_Name;
                    itemHeader.Route_Index = data.route_Index;
                    itemHeader.Route_Id = data.route_Id;
                    itemHeader.Route_Name = data.route_Name;
                    itemHeader.SubRoute_Index = data.subRoute_Index;
                    itemHeader.SubRoute_Id = data.subRoute_Id;
                    itemHeader.SubRoute_Name = data.subRoute_Name;
                    itemHeader.CostCenter_Index = data.costCenter_Index;
                    itemHeader.CostCenter_Id = data.costCenter_Id;
                    itemHeader.CostCenter_Name = data.costCenter_Name;
                    itemHeader.Sloc_Index = data.storageLoc_Index;
                    itemHeader.Sloc_Id = data.storageLoc_Id;
                    itemHeader.Sloc_Name = data.storageLoc_Name;
                    itemHeader.MovementType_Index = data.movementType_Index;
                    itemHeader.MovementType_Id = data.movementType_Id;
                    itemHeader.MovementType_Name = data.movementType_Name;

                    itemHeader.ShippingTerms_Index = data.shippingTerms_Index;
                    itemHeader.ShippingTerms_Id = data.shippingTerms_Id;
                    itemHeader.ShippingTerms_Name = data.shippingTerms_Name;
                    itemHeader.ShippingMethod_Index = data.shippingMethod_Index;
                    itemHeader.ShippingMethod_Id = data.shippingMethod_Id;
                    itemHeader.ShippingMethod_Name = data.shippingMethod_Name;
                    itemHeader.Shipping_Remark = data.shipping_Remark;
                    itemHeader.PaymentType_Index = data.paymentType_Index;
                    itemHeader.PaymentType_Id = data.paymentType_Id;
                    itemHeader.PaymentType_Name = data.paymentType_Name;
                    itemHeader.PaymentType_Due_Date = data.paymentType_Due_Date.toDate();
                    itemHeader.Credit_Term = data.credit_Term;
                    itemHeader.Forwarder_Index = data.forwarder_Index;
                    itemHeader.Forwarder_Id = data.forwarder_Id;
                    itemHeader.Forwarder_Name = data.forwarder_Name;
                    itemHeader.Sales_Person = data.sales_Person;
                    itemHeader.Promotion_Code = data.promotion_Code;
                    itemHeader.Import_Index = data.import_Index;
                    itemHeader.First_Order = data.first_Order;
                    itemHeader.Pre_Order = data.Pre_Order;

                    if (IsNew == true)
                    {
                        itemHeader.Create_By = data.create_By;
                        itemHeader.Create_Date = DateTime.Now;
                    }
                    db.im_PlanGoodsIssue.Add(itemHeader);
                    int addNumber = 0;

                    if (data.documents != null)
                    {
                        foreach (var d in data.documents.Where(c => !c.isDelete))
                        {
                            im_DocumentFile documents = new im_DocumentFile();

                            documents.DocumentFile_Index = Guid.NewGuid(); ;
                            documents.DocumentFile_Name = d.filename;
                            documents.DocumentFile_Path = d.path;
                            documents.DocumentFile_Url = d.urlAttachFile;
                            documents.DocumentFile_Type = d.type;
                            documents.DocumentFile_Status = 0;
                            documents.Create_By = data.create_By;
                            documents.Create_Date = DateTime.Now;
                            documents.Ref_Index = itemHeader.PlanGoodsIssue_Index;
                            documents.Ref_No = itemHeader.PlanGoodsIssue_No;
                            db.im_DocumentFile.Add(documents);
                        }
                    }

                    foreach (var item in data.listplanGoodsIssueItemViewModel)
                    {

                        var Productresult = new List<ProductViewModel>();

                        var ProductfilterModel = new ProductViewModel();
                        ProductfilterModel.product_Index = item.product_Index;

                        State = "GetProductMaster";

                        //GetConfig
                        Productresult = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("product"), ProductfilterModel.sJson());



                        im_PlanGoodsIssueItem resultItem = new im_PlanGoodsIssueItem();

                        addNumber++;
                        // Gen Index for line item

                        item.planGoodsIssueItem_Index = Guid.NewGuid();
                        resultItem.PlanGoodsIssue_Index = PlanGoodsIssueIndex;
                        resultItem.LineNum = addNumber.ToString();
                        resultItem.ItemStatus_Index = item.itemStatus_Index;

                        resultItem.ItemStatus_Id = item.itemStatus_Id;

                        resultItem.ItemStatus_Name = item.itemStatus_Name;
                        resultItem.Product_Index = item.product_Index;
                        resultItem.Product_Id = item.product_Id;
                        resultItem.Product_Name = item.product_Name;
                        if (Productresult.Count > 0)
                        {
                            resultItem.Product_SecondName = Productresult.FirstOrDefault().product_SecondName;
                            resultItem.Product_ThirdName = Productresult.FirstOrDefault().product_ThirdName;
                        }

                        if (item.product_Lot != null)
                        {
                            resultItem.Product_Lot = item.product_Lot;
                        }
                        else
                        {
                            resultItem.Product_Lot = "";
                        }

                        resultItem.Qty = Convert.ToDecimal(item.qty);
                        resultItem.Ratio = item.ratio;
                        if (item.ratio != 0)
                        {
                            var totalqty = Convert.ToDecimal(item.qty) * item.ratio;
                            item.totalQty = totalqty;
                        }
                        resultItem.TotalQty = item.totalQty;
                        resultItem.ProductConversion_Index = item.productConversion_Index;
                        resultItem.ProductConversion_Id = item.productConversion_Id;
                        resultItem.ProductConversion_Name = item.productConversion_Name;
                        resultItem.MFG_Date = item.mfg_Date.toDate();
                        resultItem.EXP_Date = item.exp_Date.toDate();

                        var weight = (item.unitWeight ?? 0) * (item.unitWeightRatio ?? 0);
                        resultItem.UnitWeight = weight;
                        resultItem.UnitWeight_Index = WeightViewModel.weight_Index;
                        resultItem.UnitWeight_Id = WeightViewModel.weight_Id;
                        resultItem.UnitWeight_Name = WeightViewModel.weight_Name;
                        resultItem.UnitWeightRatio = WeightViewModel.weight_Ratio;

                        resultItem.Weight = (item.totalQty ?? 0) * weight;
                        resultItem.Weight_Index = WeightViewModel.weight_Index;
                        resultItem.Weight_Id = WeightViewModel.weight_Id;
                        resultItem.Weight_Name = WeightViewModel.weight_Name;
                        resultItem.WeightRatio = WeightViewModel.weight_Ratio;
                   
                        var Netweight = (item.unitNetWeight ?? 0) * (item.unitNetWeightRatio ?? 0);
                        resultItem.UnitNetWeight = Netweight;
                        resultItem.UnitNetWeight_Index = WeightViewModel.weight_Index;
                        resultItem.UnitNetWeight_Id = WeightViewModel.weight_Id;
                        resultItem.UnitNetWeight_Name = WeightViewModel.weight_Name;
                        resultItem.UnitNetWeightRatio = WeightViewModel.weight_Ratio;

                        resultItem.NetWeight = (item.totalQty ?? 0) * Netweight;
                        resultItem.NetWeight_Index = WeightViewModel.weight_Index;
                        resultItem.NetWeight_Id = WeightViewModel.weight_Id;
                        resultItem.NetWeight_Name = WeightViewModel.weight_Name;
                        resultItem.NetWeightRatio = WeightViewModel.weight_Ratio;
                     
                        var Grsweight = (item.unitGrsWeight ?? 0) * (item.unitGrsWeightRatio ?? 0);
                        resultItem.UnitGrsWeight = Grsweight;
                        resultItem.UnitGrsWeight_Index = WeightViewModel.weight_Index;
                        resultItem.UnitGrsWeight_Id = WeightViewModel.weight_Id;
                        resultItem.UnitGrsWeight_Name = WeightViewModel.weight_Name;
                        resultItem.UnitGrsWeightRatio = WeightViewModel.weight_Ratio;

                        resultItem.GrsWeight = (item.totalQty ?? 0) * Grsweight;
                        resultItem.GrsWeight_Index = WeightViewModel.weight_Index;
                        resultItem.GrsWeight_Id = WeightViewModel.weight_Id;
                        resultItem.GrsWeight_Name = WeightViewModel.weight_Name;
                        resultItem.GrsWeightRatio = WeightViewModel.weight_Ratio;

                        //var width = (item.unitWidth ?? 0) / (item.unitWidthRatio ?? 0);
                        var width = (item.unitWidth ?? 0);
                        resultItem.UnitWidth = width;
                        resultItem.UnitWidth_Index = item.unitWidth_Index;
                        resultItem.UnitWidth_Id = item.unitWidth_Id;
                        resultItem.UnitWidth_Name = item.unitWidth_Name;
                        resultItem.UnitWidthRatio = item.unitWidthRatio;

                        resultItem.Width = (item.totalQty ?? 0) * width;
                        resultItem.Width_Index = item.unitWidth_Index;
                        resultItem.Width_Id = item.unitWidth_Id;
                        resultItem.Width_Name = item.unitWidth_Name;
                        resultItem.WidthRatio = item.unitWidthRatio;

                        //var Length = (item.unitLength ?? 0) / (item.unitLengthRatio ?? 0);
                        var Length = (item.unitLength ?? 0);
                        resultItem.UnitLength = Length;
                        resultItem.UnitLength_Index = item.unitLength_Index;
                        resultItem.UnitLength_Id = item.unitLength_Id;
                        resultItem.UnitLength_Name = item.unitLength_Name;
                        resultItem.UnitLengthRatio = item.unitLengthRatio;

                        resultItem.Length = (item.totalQty ?? 0) * Length;
                        resultItem.Length_Index = item.unitLength_Index;
                        resultItem.Length_Id = item.unitLength_Id;
                        resultItem.Length_Name = item.unitLength_Name;
                        resultItem.LengthRatio = item.unitLengthRatio;

                        //var Height = (item.unitHeight ?? 0) / (item.unitHeightRatio ?? 0);
                        var Height = (item.unitHeight ?? 0);
                        resultItem.UnitHeight = Height;
                        resultItem.UnitHeight_Index = item.unitHeight_Index;
                        resultItem.UnitHeight_Id = item.unitHeight_Id;
                        resultItem.UnitHeight_Name = item.unitHeight_Name;
                        resultItem.UnitHeightRatio = item.unitHeightRatio;

                        resultItem.Height = (item.totalQty ?? 0) * Height;
                        resultItem.Height_Index = item.unitHeight_Index;
                        resultItem.Height_Id = item.unitHeight_Id;
                        resultItem.Height_Name = item.unitHeight_Name;
                        resultItem.HeightRatio = item.unitHeightRatio;

                        var unitVolume = (width * Length * Height);
                        //var unitVolume = (item.unitWidth * item.unitLength * item.unitHeight);
                        resultItem.UnitVolume = unitVolume;
                        resultItem.Volume = ((item.totalQty ?? 0) * (unitVolume / (item.unitHeightRatio ?? 1)));

                        resultItem.UnitPrice = item.unitPrice;
                        resultItem.UnitPrice_Index = item.unitPrice_Index;
                        resultItem.UnitPrice_Id = item.unitPrice_Id;
                        resultItem.UnitPrice_Name = item.unitPrice_Name;

                        resultItem.Price = item.price;
                        resultItem.Price_Index = item.price_Index;
                        resultItem.Price_Id = item.price_Id;
                        resultItem.Price_Name = item.price_Name;

                        resultItem.DocumentRef_No1 = item.documentRef_No1;
                        resultItem.DocumentRef_No2 = item.documentRef_No2;
                        resultItem.DocumentRef_No3 = item.documentRef_No3;
                        resultItem.DocumentRef_No4 = item.documentRef_No4;
                        resultItem.DocumentRef_No5 = item.documentRef_No5;
                        resultItem.Document_Status = 0;
                        resultItem.DocumentItem_Remark = item.documentItem_Remark;
                        resultItem.UDF_1 = item.uDF_1;
                        resultItem.UDF_2 = item.uDF_2;
                        resultItem.UDF_3 = item.uDF_3;
                        resultItem.UDF_4 = item.uDF_4;
                        resultItem.UDF_5 = item.uDF_5;
                        resultItem.PlanGoodsIssue_No = itemHeader.PlanGoodsIssue_No;
                        resultItem.ERP_Location = item.erp_Location;

                        if (IsNew == true)
                        {
                            resultItem.Create_By = data.create_By;
                            resultItem.Create_Date = DateTime.Now;
                        }
                        db.im_PlanGoodsIssueItem.Add(resultItem);

                    }
                }
                else
                {
                    var resultWH = new List<warehouseDocViewModel>();
                    var filterModelWH = new warehouseDocViewModel();
                    filterModelWH.warehouse_Index = (data.warehouse_Index ?? Guid.Empty);
                    State = "GetdropdownWarehouse";

                    resultWH = utils.SendDataApi<List<warehouseDocViewModel>>(new AppSettingConfig().GetUrl("dropdownWarehouse"), filterModelWH.sJson());

                    PlanGoodsIssueOld.PlanGoodsIssue_Index = data.planGoodsIssue_Index;
                    PlanGoodsIssueOld.PlanGoodsIssue_No = data.planGoodsIssue_No;
                    PlanGoodsIssueOld.Owner_Index = data.owner_Index;
                    PlanGoodsIssueOld.Owner_Id = data.owner_Id;
                    PlanGoodsIssueOld.Owner_Name = data.owner_Name;
                    PlanGoodsIssueOld.Vendor_Id = data.vendor_Id;
                    PlanGoodsIssueOld.DocumentType_Index = data.documentType_Index;
                    PlanGoodsIssueOld.DocumentType_Id = data.documentType_Id;
                    PlanGoodsIssueOld.DocumentType_Name = data.documentType_Name;
                    PlanGoodsIssueOld.PlanGoodsIssue_Date = data.planGoodsIssue_Date.toDate();
                    PlanGoodsIssueOld.PlanGoodsIssue_Due_Date = data.planGoodsIssue_Due_Date.toDate();
                    PlanGoodsIssueOld.PlanGoodsIssue_Time = data.planGoodsIssue_Time;
                    PlanGoodsIssueOld.DocumentRef_No1 = data.documentRef_No1;
                    PlanGoodsIssueOld.DocumentRef_No2 = data.documentRef_No2;
                    PlanGoodsIssueOld.DocumentRef_No3 = data.documentRef_No3;
                    PlanGoodsIssueOld.DocumentRef_No4 = data.documentRef_No4;
                    PlanGoodsIssueOld.DocumentRef_No5 = data.documentRef_No5;
                    PlanGoodsIssueOld.Document_Status = data.document_Status;
                    PlanGoodsIssueOld.ShipTo_Index = data.shipTo_Index;
                    PlanGoodsIssueOld.ShipTo_Id = data.shipTo_Id;
                    PlanGoodsIssueOld.ShipTo_Name = data.shipTo_Name;
                    PlanGoodsIssueOld.ShipTo_Address = data.shipTo_Address;
                    PlanGoodsIssueOld.ShipTo_Contact_Person = data.shipTo_Contact_Person;
                    PlanGoodsIssueOld.SoldTo_Index = data.soldTo_Index;
                    PlanGoodsIssueOld.SoldTo_Id = data.soldTo_Id;
                    PlanGoodsIssueOld.SoldTo_Name = data.soldTo_Name;
                    PlanGoodsIssueOld.SoldTo_Address = data.soldTo_Address;
                    PlanGoodsIssueOld.SoldTo_Contact_Person = data.soldTo_Contact_Person;
                    PlanGoodsIssueOld.UDF_1 = data.uDF_1;
                    PlanGoodsIssueOld.UDF_2 = data.uDF_2;
                    //PlanGoodsIssueOld.UDF_3 = data.uDF_3;
                    PlanGoodsIssueOld.UDF_4 = data.uDF_4;
                    PlanGoodsIssueOld.UDF_5 = data.uDF_5;
                    PlanGoodsIssueOld.DocumentPriority_Status = data.documentPriority_Status;
                    PlanGoodsIssueOld.Document_Remark = data.document_Remark;
                    PlanGoodsIssueOld.Warehouse_Index = resultWH.FirstOrDefault().warehouse_Index;
                    PlanGoodsIssueOld.Warehouse_Id = resultWH.FirstOrDefault().warehouse_Id;
                    PlanGoodsIssueOld.Warehouse_Name = resultWH.FirstOrDefault().warehouse_Name;
                    PlanGoodsIssueOld.Warehouse_Index_To = data.warehouse_Index_To;
                    PlanGoodsIssueOld.Warehouse_Id_To = data.warehouse_Id_To;
                    PlanGoodsIssueOld.Warehouse_Name_To = data.warehouse_Name_To;
                    PlanGoodsIssueOld.Round_Index = data.round_Index;
                    PlanGoodsIssueOld.Round_Id = data.round_Id;
                    PlanGoodsIssueOld.Round_Name = data.round_Name;
                    PlanGoodsIssueOld.Route_Index = data.route_Index;
                    PlanGoodsIssueOld.Route_Id = data.route_Id;
                    PlanGoodsIssueOld.Route_Name = data.route_Name;
                    PlanGoodsIssueOld.SubRoute_Index = data.subRoute_Index;
                    PlanGoodsIssueOld.SubRoute_Id = data.subRoute_Id;
                    PlanGoodsIssueOld.SubRoute_Name = data.subRoute_Name;
                    PlanGoodsIssueOld.UserAssign = "";
                    //PlanGoodsIssueOld.CostCenter_Index = data.costCenter_Index;
                    //PlanGoodsIssueOld.CostCenter_Id = data.costCenter_Id;
                    //PlanGoodsIssueOld.CostCenter_Name = data.costCenter_Name;
                    PlanGoodsIssueOld.Sloc_Index = data.storageLoc_Index;
                    PlanGoodsIssueOld.Sloc_Id = data.storageLoc_Id;
                    PlanGoodsIssueOld.Sloc_Name = data.storageLoc_Name;
                    PlanGoodsIssueOld.MovementType_Index = data.movementType_Index;
                    PlanGoodsIssueOld.MovementType_Id = data.movementType_Id;
                    PlanGoodsIssueOld.MovementType_Name = data.movementType_Name;

                    PlanGoodsIssueOld.ShippingTerms_Index = data.shippingTerms_Index;
                    PlanGoodsIssueOld.ShippingTerms_Id = data.shippingTerms_Id;
                    PlanGoodsIssueOld.ShippingTerms_Name = data.shippingTerms_Name;
                    PlanGoodsIssueOld.ShippingMethod_Index = data.shippingMethod_Index;
                    PlanGoodsIssueOld.ShippingMethod_Id = data.shippingMethod_Id;
                    PlanGoodsIssueOld.ShippingMethod_Name = data.shippingMethod_Name;
                    PlanGoodsIssueOld.Shipping_Remark = data.shipping_Remark;
                    PlanGoodsIssueOld.PaymentType_Index = data.paymentType_Index;
                    PlanGoodsIssueOld.PaymentType_Id = data.paymentType_Id;
                    PlanGoodsIssueOld.PaymentType_Name = data.paymentType_Name;
                    PlanGoodsIssueOld.PaymentType_Due_Date = data.paymentType_Due_Date.toDate();
                    PlanGoodsIssueOld.Credit_Term = data.credit_Term;
                    PlanGoodsIssueOld.Forwarder_Index = data.forwarder_Index;
                    PlanGoodsIssueOld.Forwarder_Id = data.forwarder_Id;
                    PlanGoodsIssueOld.Forwarder_Name = data.forwarder_Name;
                    PlanGoodsIssueOld.Sales_Person = data.sales_Person;
                    PlanGoodsIssueOld.Promotion_Code = data.promotion_Code;
                    PlanGoodsIssueOld.First_Order = data.first_Order;

                    if (IsNew != true)
                    {
                        PlanGoodsIssueOld.Update_By = data.create_By;
                        PlanGoodsIssueOld.Update_Date = DateTime.Now;
                    }

                    if (data.documents != null)
                    {
                        foreach (var d in data.documents)
                        {
                            if (d.index == null || d.index == Guid.Empty)
                            {
                                im_DocumentFile documents = new im_DocumentFile();

                                documents.DocumentFile_Index = Guid.NewGuid(); ;
                                documents.DocumentFile_Name = d.filename;
                                documents.DocumentFile_Path = d.path;
                                documents.DocumentFile_Url = d.urlAttachFile;
                                documents.DocumentFile_Type = d.type;
                                documents.DocumentFile_Status = 0;
                                documents.Create_By = data.create_By;
                                documents.Create_Date = DateTime.Now;
                                documents.Ref_Index = PlanGoodsIssueOld.PlanGoodsIssue_Index;
                                documents.Ref_No = PlanGoodsIssueOld.PlanGoodsIssue_No;
                                db.im_DocumentFile.Add(documents);
                            }
                            else if ((d.index != null || d.index != Guid.Empty) && d.isDelete)
                            {
                                var Documents = db.im_DocumentFile.FirstOrDefault(c => c.DocumentFile_Index == d.index && c.Ref_Index == PlanGoodsIssueOld.PlanGoodsIssue_Index && c.DocumentFile_Status == 0);
                                Documents.DocumentFile_Status = -1;
                                Documents.Update_By = data.create_By;
                                Documents.Update_Date = DateTime.Now;
                            }
                        }
                    }

                    foreach (var item in data.listplanGoodsIssueItemViewModel)
                    {



                        var PlanGoodsIssueItemOld = db.im_PlanGoodsIssueItem.Find(item.planGoodsIssueItem_Index);

                        if (PlanGoodsIssueItemOld != null)
                        {

                            var Productresult = new List<ProductViewModel>();

                            var ProductfilterModel = new ProductViewModel();
                            ProductfilterModel.product_Index = item.product_Index;

                            State = "GetProductMaster";
                            //GetConfig
                            Productresult = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("product"), ProductfilterModel.sJson());


                            int addNumber = 0;

                            im_PlanGoodsIssueItem resultItem = new im_PlanGoodsIssueItem();

                            //Get ItemStatus

                            addNumber++;

                            PlanGoodsIssueItemOld.PlanGoodsIssueItem_Index = item.planGoodsIssueItem_Index;
                            PlanGoodsIssueItemOld.PlanGoodsIssue_Index = item.planGoodsIssue_Index;

                            if (item.lineNum == null)
                            {
                                PlanGoodsIssueItemOld.LineNum = addNumber.ToString();
                            }
                            else
                            {
                                PlanGoodsIssueItemOld.LineNum = item.lineNum;
                            }

                            PlanGoodsIssueItemOld.Product_Index = item.product_Index;
                            PlanGoodsIssueItemOld.Product_Id = item.product_Id;
                            PlanGoodsIssueItemOld.Product_Name = item.product_Name;
                            if (Productresult.Count > 0)
                            {
                                PlanGoodsIssueItemOld.Product_SecondName = Productresult.FirstOrDefault().product_SecondName;
                                PlanGoodsIssueItemOld.Product_ThirdName = Productresult.FirstOrDefault().product_ThirdName;
                            }

                            if (item.product_Lot != null)
                            {
                                PlanGoodsIssueItemOld.Product_Lot = item.product_Lot;
                            }
                            else
                            {
                                PlanGoodsIssueItemOld.Product_Lot = "";
                            }
                            PlanGoodsIssueItemOld.ItemStatus_Index = item.itemStatus_Index;
                            PlanGoodsIssueItemOld.ItemStatus_Id = item.itemStatus_Id;
                            PlanGoodsIssueItemOld.ItemStatus_Name = item.itemStatus_Name;

                            PlanGoodsIssueItemOld.Qty = Convert.ToDecimal(item.qty);
                            PlanGoodsIssueItemOld.Ratio = item.ratio;
                            if (item.ratio != 0)
                            {
                                var totalqty = Convert.ToDecimal(item.qty) * item.ratio;
                                item.totalQty = totalqty;
                            }
                            PlanGoodsIssueItemOld.TotalQty = item.totalQty;
                            PlanGoodsIssueItemOld.ProductConversion_Index = item.productConversion_Index;
                            PlanGoodsIssueItemOld.ProductConversion_Id = item.productConversion_Id;
                            PlanGoodsIssueItemOld.ProductConversion_Name = item.productConversion_Name;
                            PlanGoodsIssueItemOld.MFG_Date = item.mfg_Date.toDate();
                            PlanGoodsIssueItemOld.EXP_Date = item.exp_Date.toDate();


                            var weight = (item.unitWeight ?? 0) * (item.unitWeightRatio ?? 0);
                            PlanGoodsIssueItemOld.UnitWeight = weight;
                            PlanGoodsIssueItemOld.UnitWeight_Index = WeightViewModel.weight_Index;
                            PlanGoodsIssueItemOld.UnitWeight_Id = WeightViewModel.weight_Id;
                            PlanGoodsIssueItemOld.UnitWeight_Name = WeightViewModel.weight_Name;
                            PlanGoodsIssueItemOld.UnitWeightRatio = WeightViewModel.weight_Ratio;

                            PlanGoodsIssueItemOld.Weight = (item.totalQty ?? 0) * weight;
                            PlanGoodsIssueItemOld.Weight_Index = WeightViewModel.weight_Index;
                            PlanGoodsIssueItemOld.Weight_Id = WeightViewModel.weight_Id;
                            PlanGoodsIssueItemOld.Weight_Name = WeightViewModel.weight_Name;
                            PlanGoodsIssueItemOld.WeightRatio = WeightViewModel.weight_Ratio;

                            var Netweight = (item.unitNetWeight ?? 0) * (item.unitNetWeightRatio ?? 0);
                            PlanGoodsIssueItemOld.UnitNetWeight = Netweight;
                            PlanGoodsIssueItemOld.UnitNetWeight_Index = WeightViewModel.weight_Index;
                            PlanGoodsIssueItemOld.UnitNetWeight_Id = WeightViewModel.weight_Id;
                            PlanGoodsIssueItemOld.UnitNetWeight_Name = WeightViewModel.weight_Name;
                            PlanGoodsIssueItemOld.UnitNetWeightRatio = WeightViewModel.weight_Ratio;

                            PlanGoodsIssueItemOld.NetWeight = (item.totalQty ?? 0) * Netweight;
                            PlanGoodsIssueItemOld.NetWeight_Index = WeightViewModel.weight_Index;
                            PlanGoodsIssueItemOld.NetWeight_Id = WeightViewModel.weight_Id;
                            PlanGoodsIssueItemOld.NetWeight_Name = WeightViewModel.weight_Name;
                            PlanGoodsIssueItemOld.NetWeightRatio = WeightViewModel.weight_Ratio;

                            var Grsweight = (item.unitGrsWeight ?? 0) * (item.unitGrsWeightRatio ?? 0);
                            PlanGoodsIssueItemOld.UnitGrsWeight = Grsweight;
                            PlanGoodsIssueItemOld.UnitGrsWeight_Index = WeightViewModel.weight_Index;
                            PlanGoodsIssueItemOld.UnitGrsWeight_Id = WeightViewModel.weight_Id;
                            PlanGoodsIssueItemOld.UnitGrsWeight_Name = WeightViewModel.weight_Name;
                            PlanGoodsIssueItemOld.UnitGrsWeightRatio = WeightViewModel.weight_Ratio;

                            PlanGoodsIssueItemOld.GrsWeight = (item.totalQty ?? 0) * Grsweight;
                            PlanGoodsIssueItemOld.GrsWeight_Index = WeightViewModel.weight_Index;
                            PlanGoodsIssueItemOld.GrsWeight_Id = WeightViewModel.weight_Id;
                            PlanGoodsIssueItemOld.GrsWeight_Name = WeightViewModel.weight_Name;
                            PlanGoodsIssueItemOld.GrsWeightRatio = WeightViewModel.weight_Ratio;

                            //var width = (item.unitWidth ?? 0) / (item.unitWidthRatio ?? 0);
                            var width = (item.unitWidth ?? 0);
                            PlanGoodsIssueItemOld.UnitWidth = width;
                            PlanGoodsIssueItemOld.UnitWidth_Index = item.unitWidth_Index;
                            PlanGoodsIssueItemOld.UnitWidth_Id = item.unitWidth_Id;
                            PlanGoodsIssueItemOld.UnitWidth_Name = item.unitWidth_Name;
                            PlanGoodsIssueItemOld.UnitWidthRatio = item.unitWidthRatio;

                            PlanGoodsIssueItemOld.Width = (item.totalQty ?? 0) * width;
                            PlanGoodsIssueItemOld.Width_Index = item.unitWidth_Index;
                            PlanGoodsIssueItemOld.Width_Id = item.unitWidth_Id;
                            PlanGoodsIssueItemOld.Width_Name = item.unitWidth_Name;
                            PlanGoodsIssueItemOld.WidthRatio = item.unitWidthRatio;

                            //var Length = (item.unitLength ?? 0) / (item.unitLengthRatio ?? 0);
                            var Length = (item.unitLength ?? 0);
                            PlanGoodsIssueItemOld.UnitLength = Length;
                            PlanGoodsIssueItemOld.UnitLength_Index = item.unitLength_Index;
                            PlanGoodsIssueItemOld.UnitLength_Id = item.unitLength_Id;
                            PlanGoodsIssueItemOld.UnitLength_Name = item.unitLength_Name;
                            PlanGoodsIssueItemOld.UnitLengthRatio = item.unitLengthRatio;

                            PlanGoodsIssueItemOld.Length = (item.totalQty ?? 0) * Length;
                            PlanGoodsIssueItemOld.Length_Index = item.unitLength_Index;
                            PlanGoodsIssueItemOld.Length_Id = item.unitLength_Id;
                            PlanGoodsIssueItemOld.Length_Name = item.unitLength_Name;
                            PlanGoodsIssueItemOld.LengthRatio = item.unitLengthRatio;

                            //var Height = (item.unitHeight ?? 0) / (item.unitHeightRatio ?? 0);
                            var Height = (item.unitHeight ?? 0);
                            PlanGoodsIssueItemOld.UnitHeight = Height;
                            PlanGoodsIssueItemOld.UnitHeight_Index = item.unitHeight_Index;
                            PlanGoodsIssueItemOld.UnitHeight_Id = item.unitHeight_Id;
                            PlanGoodsIssueItemOld.UnitHeight_Name = item.unitHeight_Name;
                            PlanGoodsIssueItemOld.UnitHeightRatio = item.unitHeightRatio;

                            PlanGoodsIssueItemOld.Height = (item.totalQty ?? 0) * Height;
                            PlanGoodsIssueItemOld.Height_Index = item.unitHeight_Index;
                            PlanGoodsIssueItemOld.Height_Id = item.unitHeight_Id;
                            PlanGoodsIssueItemOld.Height_Name = item.unitHeight_Name;
                            PlanGoodsIssueItemOld.HeightRatio = item.unitHeightRatio;

                            var unitVolume = (width * Length * Height);
                            //var unitVolume = (item.unitWidth * item.unitLength * item.unitHeight);
                            PlanGoodsIssueItemOld.UnitVolume = unitVolume;
                            PlanGoodsIssueItemOld.Volume = ((item.totalQty ?? 0) * (unitVolume / (item.unitHeightRatio ?? 0)));

                            PlanGoodsIssueItemOld.UnitPrice = item.unitPrice;
                            PlanGoodsIssueItemOld.UnitPrice_Index = item.unitPrice_Index;
                            PlanGoodsIssueItemOld.UnitPrice_Id = item.unitPrice_Id;
                            PlanGoodsIssueItemOld.UnitPrice_Name = item.unitPrice_Name;

                            PlanGoodsIssueItemOld.Price = item.price;
                            PlanGoodsIssueItemOld.Price_Index = item.price_Index;
                            PlanGoodsIssueItemOld.Price_Id = item.price_Id;
                            PlanGoodsIssueItemOld.Price_Name = item.price_Name;

                            PlanGoodsIssueItemOld.DocumentRef_No1 = item.documentRef_No1;
                            PlanGoodsIssueItemOld.DocumentRef_No2 = item.documentRef_No2;
                            PlanGoodsIssueItemOld.DocumentRef_No3 = item.documentRef_No3;
                            PlanGoodsIssueItemOld.DocumentRef_No4 = item.documentRef_No4;
                            PlanGoodsIssueItemOld.DocumentRef_No5 = item.documentRef_No5;
                            PlanGoodsIssueItemOld.Document_Status = 0;
                            PlanGoodsIssueItemOld.DocumentItem_Remark = item.documentItem_Remark;
                            PlanGoodsIssueItemOld.UDF_1 = item.uDF_1;
                            PlanGoodsIssueItemOld.UDF_2 = item.uDF_2;
                            PlanGoodsIssueItemOld.UDF_3 = item.uDF_3;
                            PlanGoodsIssueItemOld.UDF_4 = item.uDF_4;
                            PlanGoodsIssueItemOld.UDF_5 = item.uDF_5;
                            PlanGoodsIssueItemOld.PlanGoodsIssue_No = PlanGoodsIssueOld.PlanGoodsIssue_No;
                            PlanGoodsIssueItemOld.ERP_Location = item.erp_Location;

                            if (IsNew != true)
                            {
                                PlanGoodsIssueItemOld.Update_By = data.update_By;
                                PlanGoodsIssueItemOld.Update_Date = DateTime.Now;
                            }
                        }
                        else
                        {
                            int addNumber = 0;

                            im_PlanGoodsIssueItem resultItem = new im_PlanGoodsIssueItem();

                            var Productresult = new List<ProductViewModel>();

                            var ProductfilterModel = new ProductViewModel();
                            ProductfilterModel.product_Index = item.product_Index;

                            State = "GetProductMaster";

                            //GetConfig
                            Productresult = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("product"), ProductfilterModel.sJson());


                            addNumber++;
                            // Gen Index for line item
                            resultItem.PlanGoodsIssueItem_Index = Guid.NewGuid();


                            // Index From Header
                            resultItem.PlanGoodsIssue_Index = data.planGoodsIssue_Index;

                            if (item.lineNum == null)
                            {
                                resultItem.LineNum = addNumber.ToString();
                            }
                            else
                            {
                                resultItem.LineNum = item.lineNum;
                            }
                            resultItem.ItemStatus_Index = item.itemStatus_Index;

                            resultItem.ItemStatus_Id = item.itemStatus_Id;

                            resultItem.ItemStatus_Name = item.itemStatus_Name;
                            resultItem.Product_Index = item.product_Index;
                            resultItem.Product_Id = item.product_Id;
                            resultItem.Product_Name = item.product_Name;
                            if (Productresult.Count > 0)
                            {
                                resultItem.Product_SecondName = Productresult.FirstOrDefault().product_SecondName;
                                resultItem.Product_ThirdName = Productresult.FirstOrDefault().product_ThirdName;
                            }
                            if (item.product_Lot != null)
                            {
                                resultItem.Product_Lot = item.product_Lot;
                            }
                            else
                            {
                                resultItem.Product_Lot = "";
                            }
                            resultItem.Qty = Convert.ToDecimal(item.qty);
                            resultItem.Ratio = item.ratio;

                            resultItem.TotalQty = item.totalQty;
                            resultItem.ProductConversion_Index = item.productConversion_Index;
                            resultItem.ProductConversion_Id = item.productConversion_Id;
                            resultItem.ProductConversion_Name = item.productConversion_Name;
                            resultItem.MFG_Date = item.mfg_Date.toDate();
                            resultItem.EXP_Date = item.exp_Date.toDate();

                            var weight = (item.unitWeight ?? 0) * (item.unitWeightRatio ?? 0);
                            resultItem.UnitWeight = weight;
                            resultItem.UnitWeight_Index = WeightViewModel.weight_Index;
                            resultItem.UnitWeight_Id = WeightViewModel.weight_Id;
                            resultItem.UnitWeight_Name = WeightViewModel.weight_Name;
                            resultItem.UnitWeightRatio = WeightViewModel.weight_Ratio;

                            resultItem.Weight = (item.totalQty ?? 0) * weight;
                            resultItem.Weight_Index = WeightViewModel.weight_Index;
                            resultItem.Weight_Id = WeightViewModel.weight_Id;
                            resultItem.Weight_Name = WeightViewModel.weight_Name;
                            resultItem.WeightRatio = WeightViewModel.weight_Ratio;

                            var Netweight = (item.unitNetWeight ?? 0) * (item.unitNetWeightRatio ?? 0);
                            resultItem.UnitNetWeight = Netweight;
                            resultItem.UnitNetWeight_Index = WeightViewModel.weight_Index;
                            resultItem.UnitNetWeight_Id = WeightViewModel.weight_Id;
                            resultItem.UnitNetWeight_Name = WeightViewModel.weight_Name;
                            resultItem.UnitNetWeightRatio = WeightViewModel.weight_Ratio;

                            resultItem.NetWeight = (item.totalQty ?? 0) * Netweight;
                            resultItem.NetWeight_Index = WeightViewModel.weight_Index;
                            resultItem.NetWeight_Id = WeightViewModel.weight_Id;
                            resultItem.NetWeight_Name = WeightViewModel.weight_Name;
                            resultItem.NetWeightRatio = WeightViewModel.weight_Ratio;

                            var Grsweight = (item.unitGrsWeight ?? 0) * (item.unitGrsWeightRatio ?? 0);
                            resultItem.UnitGrsWeight = Grsweight;
                            resultItem.UnitGrsWeight_Index = WeightViewModel.weight_Index;
                            resultItem.UnitGrsWeight_Id = WeightViewModel.weight_Id;
                            resultItem.UnitGrsWeight_Name = WeightViewModel.weight_Name;
                            resultItem.UnitGrsWeightRatio = WeightViewModel.weight_Ratio;

                            resultItem.GrsWeight = (item.totalQty ?? 0) * Grsweight;
                            resultItem.GrsWeight_Index = WeightViewModel.weight_Index;
                            resultItem.GrsWeight_Id = WeightViewModel.weight_Id;
                            resultItem.GrsWeight_Name = WeightViewModel.weight_Name;
                            resultItem.GrsWeightRatio = WeightViewModel.weight_Ratio;


                            //var width = (item.unitWidth ?? 0) / (item.unitWidthRatio ?? 0);
                            var width = (item.unitWidth ?? 0);
                            resultItem.UnitWidth = width;
                            resultItem.UnitWidth_Index = item.unitWidth_Index;
                            resultItem.UnitWidth_Id = item.unitWidth_Id;
                            resultItem.UnitWidth_Name = item.unitWidth_Name;
                            resultItem.UnitWidthRatio = item.unitWidthRatio;

                            resultItem.Width = (item.totalQty ?? 0) * width;
                            resultItem.Width_Index = item.unitWidth_Index;
                            resultItem.Width_Id = item.unitWidth_Id;
                            resultItem.Width_Name = item.unitWidth_Name;
                            resultItem.WidthRatio = item.unitWidthRatio;

                            //var Length = (item.unitLength ?? 0) / (item.unitLengthRatio ?? 0);
                            var Length = (item.unitLength ?? 0);
                            resultItem.UnitLength = Length;
                            resultItem.UnitLength_Index = item.unitLength_Index;
                            resultItem.UnitLength_Id = item.unitLength_Id;
                            resultItem.UnitLength_Name = item.unitLength_Name;
                            resultItem.UnitLengthRatio = item.unitLengthRatio;

                            resultItem.Length = (item.totalQty ?? 0) * Length;
                            resultItem.Length_Index = item.unitLength_Index;
                            resultItem.Length_Id = item.unitLength_Id;
                            resultItem.Length_Name = item.unitLength_Name;
                            resultItem.LengthRatio = item.unitLengthRatio;

                            //var Height = (item.unitHeight ?? 0) / (item.unitHeightRatio ?? 0);
                            var Height = (item.unitHeight ?? 0);
                            resultItem.UnitHeight = Height;
                            resultItem.UnitHeight_Index = item.unitHeight_Index;
                            resultItem.UnitHeight_Id = item.unitHeight_Id;
                            resultItem.UnitHeight_Name = item.unitHeight_Name;
                            resultItem.UnitHeightRatio = item.unitHeightRatio;

                            resultItem.Height = (item.totalQty ?? 0) * Height;
                            resultItem.Height_Index = item.unitHeight_Index;
                            resultItem.Height_Id = item.unitHeight_Id;
                            resultItem.Height_Name = item.unitHeight_Name;
                            resultItem.HeightRatio = item.unitHeightRatio;

                            var unitVolume = (width * Length * Height);
                            //var unitVolume = (item.unitWidth * item.unitLength * item.unitHeight);
                            resultItem.UnitVolume = unitVolume;
                            resultItem.Volume = ((item.totalQty ?? 0) * (unitVolume / (item.unitHeightRatio ?? 0)));

                            resultItem.UnitPrice = item.unitPrice;
                            resultItem.UnitPrice_Index = item.unitPrice_Index;
                            resultItem.UnitPrice_Id = item.unitPrice_Id;
                            resultItem.UnitPrice_Name = item.unitPrice_Name;

                            resultItem.Price = item.price;
                            resultItem.Price_Index = item.price_Index;
                            resultItem.Price_Id = item.price_Id;
                            resultItem.Price_Name = item.price_Name;

                            resultItem.DocumentRef_No1 = item.documentRef_No1;
                            resultItem.DocumentRef_No2 = item.documentRef_No2;
                            resultItem.DocumentRef_No3 = item.documentRef_No3;
                            resultItem.DocumentRef_No4 = item.documentRef_No4;
                            resultItem.DocumentRef_No5 = item.documentRef_No5;
                            resultItem.Document_Status = 0;
                            resultItem.DocumentItem_Remark = item.documentItem_Remark;
                            resultItem.UDF_1 = item.uDF_1;
                            resultItem.UDF_2 = item.uDF_2;
                            resultItem.UDF_3 = item.uDF_3;
                            resultItem.UDF_4 = item.uDF_4;
                            resultItem.UDF_5 = item.uDF_5;
                            resultItem.PlanGoodsIssue_No = PlanGoodsIssueOld.PlanGoodsIssue_No;
                            resultItem.ERP_Location = item.erp_Location;

                            if (IsNew != true)
                            {
                                resultItem.Update_By = data.update_By;
                                resultItem.Update_Date = DateTime.Now;
                            }
                            db.im_PlanGoodsIssueItem.Add(resultItem);
                        }


                    }

                    var deleteItem = db.im_PlanGoodsIssueItem.Where(c => !data.listplanGoodsIssueItemViewModel.Select(s => s.planGoodsIssueItem_Index).Contains(c.PlanGoodsIssueItem_Index)
                                        && c.PlanGoodsIssue_Index == PlanGoodsIssueOld.PlanGoodsIssue_Index).ToList();

                    foreach (var c in deleteItem)
                    {
                        var deletePlanGoodsIssueItem = db.im_PlanGoodsIssueItem.Find(c.PlanGoodsIssueItem_Index);

                        deletePlanGoodsIssueItem.Document_Status = -1;
                        deletePlanGoodsIssueItem.Update_By = data.update_By;
                        deletePlanGoodsIssueItem.Update_Date = DateTime.Now;

                    }
                }

                State = "transactionSave";

                var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SavePlanGI", msglog);
                    transactionx.Rollback();

                    throw exy;

                }

                actionResult.document_No = PlanGoodsIssueNo;
                actionResult.Message = true;

                return actionResult;
            }
            catch (Exception ex)
            {
                msglog = State + " Error " + ex.Message.ToString();
                olog.logging("ErrorSavePlanGI", msglog);
                throw ex;
            }

        }

        #endregion

        #region find
        public PlanGoodDocIssueViewModel find(Guid id)
        {

            try
            {
                var queryResult = db.im_PlanGoodsIssue.Where(c => c.PlanGoodsIssue_Index == id).FirstOrDefault();

                var resultItem = new PlanGoodDocIssueViewModel();

                var StatusModel = new ProcessStatusViewModel();

                var StatusName = new List<ProcessStatusViewModel>();

                StatusModel.process_Index = new Guid("80E8E627-6A2D-4075-9BA6-04B7178C1203");

                StatusModel.processStatus_Id = queryResult.Document_Status.ToString();

                //GetConfig
                StatusName = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), StatusModel.sJson());


                resultItem.planGoodsIssue_Index = queryResult.PlanGoodsIssue_Index;
                resultItem.planGoodsIssue_No = queryResult.PlanGoodsIssue_No;
                resultItem.documentType_Index = queryResult.DocumentType_Index;
                resultItem.documentType_Name = queryResult.DocumentType_Name;
                resultItem.documentType_Id = queryResult.DocumentType_Id;
                resultItem.vendor_Id = queryResult.Vendor_Id;
                resultItem.owner_Index = queryResult.Owner_Index;
                resultItem.owner_Id = queryResult.Owner_Id;
                resultItem.owner_Name = queryResult.Owner_Name;
                resultItem.shipTo_Index = queryResult.ShipTo_Index;
                resultItem.shipTo_Id = queryResult.ShipTo_Id;
                resultItem.shipTo_Name = queryResult.ShipTo_Name;
                resultItem.shipTo_Address = queryResult.ShipTo_Address;
                resultItem.shipTo_Contact_Person = queryResult.ShipTo_Contact_Person;
                resultItem.soldTo_Index = queryResult.SoldTo_Index;
                resultItem.soldTo_Id = queryResult.SoldTo_Id;
                resultItem.soldTo_Name = queryResult.SoldTo_Name;
                resultItem.soldTo_Address = queryResult.SoldTo_Address;
                resultItem.soldTo_Contact_Person = queryResult.SoldTo_Contact_Person;
                resultItem.uDF_1 = queryResult.UDF_1;
                resultItem.documentRef_No1 = queryResult.DocumentRef_No1;
                resultItem.documentRef_No2 = queryResult.DocumentRef_No2;
                resultItem.documentRef_No3 = queryResult.DocumentRef_No3;
                resultItem.documentRef_No4 = queryResult.DocumentRef_No4;
                resultItem.documentRef_No5 = queryResult.DocumentRef_No5;
                resultItem.document_Status = queryResult.Document_Status;
                resultItem.warehouse_Index = queryResult.Warehouse_Index;
                resultItem.warehouse_Index_To = queryResult.Warehouse_Index_To;
                resultItem.warehouse_Id = queryResult.Warehouse_Id;
                resultItem.warehouse_Id_To = queryResult.Warehouse_Id_To;
                resultItem.warehouse_Name = queryResult.Warehouse_Name;
                resultItem.warehouse_Name_To = queryResult.Warehouse_Name_To;
                resultItem.document_Remark = queryResult.Document_Remark;
                resultItem.documentPriority_Status = queryResult.DocumentPriority_Status;
                resultItem.planGoodsIssue_Date = queryResult.PlanGoodsIssue_Date.toString();
                resultItem.planGoodsIssue_Due_Date = queryResult.PlanGoodsIssue_Due_Date.toString();
                resultItem.date_Budget = queryResult.Date_Budget.toString();
                resultItem.userAssign = queryResult.UserAssign;
                resultItem.processStatus_Name = StatusName.FirstOrDefault().processStatus_Name;
                resultItem.planGoodsIssue_Time = queryResult.PlanGoodsIssue_Time;
                resultItem.round_Index = queryResult.Round_Index;
                resultItem.round_Id = queryResult.Round_Id;
                resultItem.round_Name = queryResult.Round_Name;
                resultItem.route_Index = queryResult.Route_Index;
                resultItem.route_Id = queryResult.Route_Id;
                resultItem.route_Name = queryResult.Route_Name;
                resultItem.subRoute_Index = queryResult.SubRoute_Index;
                resultItem.subRoute_Id = queryResult.SubRoute_Id;
                resultItem.subRoute_Name = queryResult.SubRoute_Name;
                resultItem.costCenter_Index = queryResult.CostCenter_Index;
                resultItem.costCenter_Id = queryResult.CostCenter_Id;
                resultItem.costCenter_Name = queryResult.CostCenter_Name;
                resultItem.storageLoc_Index = queryResult.Sloc_Index;
                resultItem.storageLoc_Id = queryResult.Sloc_Id;
                resultItem.storageLoc_Name = queryResult.Sloc_Name;
                resultItem.movementType_Index = queryResult.MovementType_Index;
                resultItem.movementType_Id = queryResult.MovementType_Id;
                resultItem.movementType_Name = queryResult.MovementType_Name;

                resultItem.shippingTerms_Index = queryResult.ShippingTerms_Index;
                resultItem.shippingTerms_Id = queryResult.ShippingTerms_Id;
                resultItem.shippingTerms_Name = queryResult.ShippingTerms_Name;
                resultItem.shippingMethod_Index = queryResult.ShippingMethod_Index;
                resultItem.shippingMethod_Id = queryResult.ShippingMethod_Id;
                resultItem.shippingMethod_Name = queryResult.ShippingMethod_Name;
                resultItem.shipping_Remark = queryResult.Shipping_Remark;
                resultItem.paymentType_Index = queryResult.PaymentType_Index;
                resultItem.paymentType_Id = queryResult.PaymentType_Id;
                resultItem.paymentType_Name = queryResult.PaymentType_Name;
                resultItem.paymentType_Due_Date = queryResult.PaymentType_Due_Date.toString();
                resultItem.credit_Term = queryResult.Credit_Term;
                resultItem.forwarder_Index = queryResult.Forwarder_Index;
                resultItem.forwarder_Id = queryResult.Forwarder_Id;
                resultItem.forwarder_Name = queryResult.Forwarder_Name;
                resultItem.sales_Person = queryResult.Sales_Person;
                resultItem.promotion_Code = queryResult.Promotion_Code;
                resultItem.first_Order = queryResult.First_Order ?? 0;
                resultItem.Pre_Order = queryResult.Pre_Order ?? 0;

                var Listdocuments = new List<document>();
                var DocumentFile = db.im_DocumentFile.Where(c => c.Ref_Index == queryResult.PlanGoodsIssue_Index && c.DocumentFile_Status == 0).ToList();
                foreach (var d in DocumentFile)
                {
                    var documents = new document();
                    documents.index = d.DocumentFile_Index;
                    documents.filename = d.DocumentFile_Name;
                    documents.path = d.DocumentFile_Path;
                    documents.urlAttachFile = d.DocumentFile_Url;
                    Listdocuments.Add(documents);
                }
                resultItem.documents = Listdocuments;

                return resultItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Delete
        public Boolean Delete(PlanGoodDocIssueViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                bool result = false;

                var filterModel = new goodsIssueItemLocationViewModel();

                filterModel.ref_Document_Index = data.planGoodsIssue_Index;

                bool hasGI = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("checkPGIHasGI"), filterModel.sJson());

                if (!hasGI)
                {
                    var PlanGi = db.im_PlanGoodsIssue.Find(data.planGoodsIssue_Index);

                    if (PlanGi != null)
                    {
                        PlanGi.Document_Status = -1;
                        PlanGi.Cancel_By = data.cancel_By;
                        PlanGi.Cancel_Date = DateTime.Now;


                        var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                            result = true;
                        }

                        catch (Exception exy)
                        {
                            msglog = State + " ex Rollback " + exy.Message.ToString();
                            olog.logging("DeletePlanGI", msglog);
                            transaction.Rollback();
                            throw exy;
                        }
                    }
                }

                return result;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region confirmStatus
        public Boolean confirmStatus(PlanGoodDocIssueViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {


                var PlanGoodsReceive = db.im_PlanGoodsIssue.Find(data.planGoodsIssue_Index);

                if (PlanGoodsReceive != null)
                {
                    PlanGoodsReceive.Document_Status = 1;
                    PlanGoodsReceive.Update_By = data.update_By;
                    PlanGoodsReceive.Update_Date = DateTime.Now;

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("confirmStatusPlanGI", msglog);
                        transaction.Rollback();
                        throw exy;
                    }

                }

                return false;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region comfirmPostDate
        public String comfirmPostDate(PlanGoodDocIssueViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var result = "";

                var PlanGoodsReceive = db.im_PlanGoodsIssue.Find(data.planGoodsIssue_Index);

                if (PlanGoodsReceive != null)
                {
                    PlanGoodsReceive.Confirm_Date = data.confirmpsotdate.toDate();
                    var datetime = PlanGoodsReceive.Confirm_Date.ToString().Split(' ');
                    result = "ทำการยืนยันวันที่เอกสาร วันที่ "+datetime[0]; 

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("confirmStatusPlanGI", msglog);
                        transaction.Rollback();
                        result = "ไม่สามารถอัปเดตวันที่ยืนยันได้";
                        throw exy;
                    }

                }

                return result;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region closeDocument
        public String closeDocument(PlanGoodDocIssueViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {


                var PlanGoodsIssue = db.im_PlanGoodsIssue.Find(data.planGoodsIssue_Index);

                if (PlanGoodsIssue != null)
                {

                    var result = new List<goodsIssueItemLocationViewModel>();

                    var filterModel = new goodsIssueItemLocationViewModel();

                    filterModel.ref_Document_Index = PlanGoodsIssue.PlanGoodsIssue_Index;

                    result = utils.SendDataApi<List<goodsIssueItemLocationViewModel>>(new AppSettingConfig().GetUrl("CheckPickGI"), filterModel.sJson());

                    if (PlanGoodsIssue.Document_Status == 4)
                    {
                        return "Sales Order มีการปิดเอกสารไปแล้ว";
                    }

                    else if (result.FirstOrDefault().message == "fail")
                    {
                        return "Sales Order นี้ยัง Pick ไม่เสร็จ";
                    }

                    else if (PlanGoodsIssue.Document_Status == 3 && result.FirstOrDefault().message == "true")
                    {
                        PlanGoodsIssue.Document_Status = 4;
                    }

                    else if (PlanGoodsIssue.Document_Status != 3)
                    {
                        PlanGoodsIssue.Document_Status = 4;
                    }

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                        return "success";
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("closeDocumentPlanGI", msglog);
                        transaction.Rollback();
                        throw exy;
                    }

                }

                return "ไม่ สามารถ ปิดเอกสารได้!";
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region confirmFirstOrder
        public Boolean confirmFirstOrder(PlanGoodDocIssueViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {


                var PlanGoodsReceive = db.im_PlanGoodsIssue.Find(data.planGoodsIssue_Index);

                if (PlanGoodsReceive != null)
                {
                    PlanGoodsReceive.First_Order = 1;

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("confirmStatusPlanGI", msglog);
                        transaction.Rollback();
                        throw exy;
                    }

                }

                return false;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region updateUserAssign
        public String updateUserAssign(PlanGoodDocIssueViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {

                var PlanGoodsReceive = db.im_PlanGoodsIssue.Find(data.planGoodsIssue_Index);

                if (PlanGoodsReceive != null)
                {
                    PlanGoodsReceive.UserAssign = data.userAssign;

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("UpdateUserAssign", msglog);
                        transaction.Rollback();
                        throw exy;
                    }
                }

                var FindUser = db.im_PlanGoodsIssue.Find(data.planGoodsIssue_Index);

                return FindUser.UserAssign.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region deleteUserAssign
        public String deleteUserAssign(PlanGoodDocIssueViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                if (!string.IsNullOrEmpty(data.planGoodsIssue_Index.ToString().Replace("00000000-0000-0000-0000-000000000000", "")))
                {
                    var PlanGoodsReceive = db.im_PlanGoodsIssue.Find(data.planGoodsIssue_Index);

                    if (PlanGoodsReceive != null)
                    {
                        PlanGoodsReceive.UserAssign = "";

                        var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        }

                        catch (Exception exy)
                        {
                            msglog = State + " ex Rollback " + exy.Message.ToString();
                            olog.logging("deleteUserAssign", msglog);
                            transaction.Rollback();
                            throw exy;
                        }
                    }

                    var FindUser = db.im_PlanGoodsIssue.Find(data.planGoodsIssue_Index);

                    return FindUser.UserAssign.ToString();
                }
                else
                {
                    return "";
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutobasicSuggestion
        public List<ItemListViewModel> autobasicSuggestion(ItemListViewModel data)
        {
            var items = new List<ItemListViewModel>();
            try
            {
                if (!string.IsNullOrEmpty(data.key))
                {
                    var query1 = db.View_PlanGI.Where(c => c.PlanGoodsIssue_No.Contains(data.key)).Select(s => new ItemListViewModel
                    {
                        name = s.PlanGoodsIssue_No,
                        key = s.PlanGoodsIssue_No
                    }).Distinct();

                    var query2 = db.View_PlanGI.Where(c => c.Owner_Name.Contains(data.key)).Select(s => new ItemListViewModel
                    {
                        name = s.Owner_Name,
                        key = s.Owner_Name
                    }).Distinct();

                    var query3 = db.View_PlanGI.Where(c => c.Create_By.Contains(data.key)).Select(s => new ItemListViewModel
                    {
                        name = s.Create_By,
                        key = s.Create_By

                    }).Distinct();
                    var query4 = db.View_PlanGI.Where(c => c.DocumentRef_No1.Contains(data.key)).Select(s => new ItemListViewModel
                    {
                        name = s.DocumentRef_No1,
                        key = s.DocumentRef_No1

                    }).Distinct();

                    var query = query1.Union(query2).Union(query2).Union(query3).Union(query4);

                    items = query.OrderBy(c => c.name).Take(10).ToList();
                }

            }
            catch (Exception ex)
            {

            }

            return items;
        }

        #endregion

        #region autoPlanGoodIssueNo
        public List<ItemListViewModel> autoPlanGoodIssueNo(ItemListViewModel data)
        {
            try
            {
                var query = db.View_PlanGI.AsQueryable();

                if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.PlanGoodsIssue_No.Contains(data.key));

                }

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.PlanGoodsIssue_Index, c.PlanGoodsIssue_No }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        index = item.PlanGoodsIssue_Index,
                        name = item.PlanGoodsIssue_No
                    };
                    items.Add(resultItem);

                }



                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoBilling_No
        public List<ItemListViewModel> AutoBilling_No(ItemListViewModel data)
        {
            try
            {
                var query = db.im_PlanGoodsIssue.Where(c=> c.DocumentRef_No5 != null && c.DocumentRef_No5 != "");

                if (!string.IsNullOrEmpty(data.key) && data.key != "-")
                {
                    query = query.Where(c => c.DocumentRef_No5.Contains(data.key));

                }

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.DocumentRef_No5 }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        name = item.DocumentRef_No5,
                        id = item.DocumentRef_No5
                    };
                    items.Add(resultItem);

                }



                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoMatdoc_No
        public List<ItemListViewModel> AutoMatdoc_No(ItemListViewModel data)
        {
            
            try
            {
                var query = db.im_PlanGoodsIssue.Where(c => c.Matdoc != null && c.Matdoc != "");

                if (!string.IsNullOrEmpty(data.key) && data.key != "-")
                {
                    query = query.Where(c => c.Matdoc.Contains(data.key));

                }

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.Matdoc }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        name = item.Matdoc,
                        id = item.Matdoc
                    };
                    items.Add(resultItem);

                }



                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoOwnerfilter
        public List<ItemListViewModel> autoOwnerfilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();
                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoOwnerFilter"), filterModel.sJson());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoUser
        public List<ItemListViewModel> autoUser(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();


                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }


                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoUserfilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoSku
        public List<ItemListViewModel> autoSkufilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();


                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }
                if (!string.IsNullOrEmpty(data.key2))
                {
                    filterModel.key2 = data.key2;
                }
                else
                {
                    filterModel.key2 = "00000000-0000-0000-0000-000000000000";
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoSkufilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoProduct
        public List<ItemListViewModel> autoProductfilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();


                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key3 = data.key;
                }
                if (!string.IsNullOrEmpty(data.key2))
                {
                    filterModel.key2 = data.key2;
                }
                else
                {
                    filterModel.key2 = "00000000-0000-0000-0000-000000000000";
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoSkufilter"), filterModel.sJson());
                //result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoProductfilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region DropdownDocumentType
        public List<DocumentTypeViewModel> DropdownDocumentType(DocumentTypeViewModel data)
        {
            try
            {
                var result = new List<DocumentTypeViewModel>();

                var filterModel = new DocumentTypeViewModel();


                filterModel.process_Index = new Guid("80E8E627-6A2D-4075-9BA6-04B7178C1203");

                //GetConfig
                result = utils.SendDataApi<List<DocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DropdownStatus
        public List<ProcessStatusViewModel> dropdownStatus(ProcessStatusViewModel data)
        {
            try
            {
                var result = new List<ProcessStatusViewModel>();

                var filterModel = new ProcessStatusViewModel();


                filterModel.process_Index = new Guid("80E8E627-6A2D-4075-9BA6-04B7178C1203");

                //GetConfig
                result = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("dropdownStatus"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DropdownWarehouse
        public List<warehouseDocViewModel> dropdownWarehouse(warehouseDocViewModel data)
        {
            try
            {
                var result = new List<warehouseDocViewModel>();

                var filterModel = new warehouseDocViewModel();

                //GetConfig
                result = utils.SendDataApi<List<warehouseDocViewModel>>(new AppSettingConfig().GetUrl("dropdownWarehouse"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DropdownRound
        public List<roundDocViewModel> dropdownRound(roundDocViewModel data)
        {
            try
            {
                var result = new List<roundDocViewModel>();

                var filterModel = new roundDocViewModel();

                //GetConfig
                result = utils.SendDataApi<List<roundDocViewModel>>(new AppSettingConfig().GetUrl("dropdownRound"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DropdownProductconversion
        public List<ProductConversionViewModelDoc> dropdownProductconversion(ProductConversionViewModelDoc data)
        {
            try
            {
                var result = new List<ProductConversionViewModelDoc>();

                var filterModel = new ProductConversionViewModelDoc();

                if (!string.IsNullOrEmpty(data.product_Index.ToString()))
                {
                    filterModel.product_Index = data.product_Index;
                }
                //GetConfig
                result = utils.SendDataApi<List<ProductConversionViewModelDoc>>(new AppSettingConfig().GetUrl("dropdownProductconversion"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownItemStatus
        public List<ItemStatusDocViewModel> dropdownItemStatus(ItemStatusDocViewModel data)
        {
            try
            {
                var result = new List<ItemStatusDocViewModel>();

                var filterModel = new ItemStatusDocViewModel();

                //GetConfig
                result = utils.SendDataApi<List<ItemStatusDocViewModel>>(new AppSettingConfig().GetUrl("dropdownItemStatus"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownTypeCar
        public List<TypeCarViewModel> dropdownTypeCar(TypeCarViewModel data)
        {
            try
            {
                var result = new List<TypeCarViewModel>();

                var filterModel = new TypeCarViewModel();

                //GetConfig
                result = utils.SendDataApi<List<TypeCarViewModel>>(new AppSettingConfig().GetUrl("dropdownTypeCar"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownTransport
        public List<TransportViewModel> dropdownTransport(TransportViewModel data)
        {
            try
            {
                var result = new List<TransportViewModel>();

                var filterModel = new TransportViewModel();

                //GetConfig
                result = utils.SendDataApi<List<TransportViewModel>>(new AppSettingConfig().GetUrl("dropdownTransport"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion



        #region fillterWave
        public planGoodsIssueAlreadyWave fillterWave(planGoodsIssueAlreadyWave model)
        {
            planGoodsIssueAlreadyWave result = new planGoodsIssueAlreadyWave();
            try
            {
                if (string.IsNullOrEmpty(model.planGoodsIssue_Due_Date))
                {
                    result.resultIsUse = false;
                    result.resultMsg = "กรณาระบุค่าให้ครบก่อนค้นหา";
                }
                else {
                    DateTime dateStart = (model.planGoodsIssue_Due_Date.toBetweenDate().start);
                    var PlanGoodsIssue_Due_Date = new SqlParameter("@PlanGoodsIssue_Due_Date", dateStart);
                    List<View_CheckWave_Round> checkWave_Rounds = db.View_CheckWave_Round.Where(c => c.PlanGoodsIssue_Due_Date == dateStart).OrderBy(c=> c.Round_Id).ToList();
                    if (model.round_Id != null)
                    {
                        checkWave_Rounds = checkWave_Rounds.Where(c => c.Round_Id == model.round_Id).ToList();
                    }
                    foreach (var item in checkWave_Rounds.OrderBy(c => c.Round_Id))
                    {
                        CheckWave_Round round = new CheckWave_Round();

                        round.Round_Id = item.Round_Id;
                        round.Round_Name = item.Round_Name;
                        round.PlanGoodsIssue_Due_Date = item.PlanGoodsIssue_Due_Date;
                        round.countPlangi = item.countPlangi;
                        round.CountPlanGIItem = item.CountPlanGIItem;
                        round.CalMin = item.CalMin;
                        round.ready = true;
                        
                        result.CheckWave_Roundmodel.Add(round);
                    }
                    
                    var CheckStatusWave_result = db.CheckStatusWave.FromSql("sp_Checkstepwave  @PlanGoodsIssue_Due_Date", PlanGoodsIssue_Due_Date).ToList();
                    foreach (var item in CheckStatusWave_result)
                    {
                        CheckStatus_Wave checkStatusWavemodel = new CheckStatus_Wave();
                            checkStatusWavemodel.GoodsIssue_Index  = item.GoodsIssue_Index;
                            checkStatusWavemodel.GoodsIssue_No     = item.GoodsIssue_No;
                            checkStatusWavemodel.GoodsIssue_Date   = item.GoodsIssue_Date;
                            checkStatusWavemodel.Wave_Round        = item.Wave_Round;
                            checkStatusWavemodel.minCreate_Date    = item.minCreate_Date;
                            checkStatusWavemodel.maxCreate_Date    = item.maxCreate_Date;
                            checkStatusWavemodel.MinUse            = item.MinUse;
                            checkStatusWavemodel.counRows          = item.counRows;
                            checkStatusWavemodel.Document_Status   = item.Document_Status;
                            checkStatusWavemodel.GI_status         = item.GI_status;
                            checkStatusWavemodel.TaskGI_status     = item.TaskGI_status;
                            checkStatusWavemodel.TagOut_status     = item.TagOut_status;
                            checkStatusWavemodel.WCS_status        = item.WCS_status;


                        result.CheckStatusWavemodel.Add(checkStatusWavemodel);
                    }



                    //im_GoodsIssue goodsIssue = db.im_GoodsIssue.FirstOrDefault(c => (c.Document_Status != -1 || c.Document_Status != -3) && c.GoodsIssue_Date == dateStart && c.DocumentRef_No1 == model.round_Id);
                    //if (goodsIssue != null)
                    //{
                    //    var Round_Id = new SqlParameter("@Round_Id", model.round_Id);
                    //    var goodsIssue_No = new SqlParameter("@GoodsIssue_No", goodsIssue.GoodsIssue_No);
                    //    var CheckWaveDip_Result = db.CheckWaveDip.FromSql("sp_CheckWaveDip  @GoodsIssue_No ,@Round_Id ,@PlanGoodsIssue_Due_Date", goodsIssue_No, Round_Id, PlanGoodsIssue_Due_Date).ToList();
                    //    var CheckWaveDipbyWave_Result = db.CheckWaveDipbyWave.FromSql("sp_CheckWaveDipbyWave  @GoodsIssue_No ,@Round_Id ,@PlanGoodsIssue_Due_Date", goodsIssue_No, Round_Id, PlanGoodsIssue_Due_Date).ToList();

                    //    foreach (var item in CheckWaveDip_Result)
                    //    {
                    //        CheckWaveDipmodel dipmodel = new CheckWaveDipmodel();
                    //        dipmodel.TruckLoad_No = item.TruckLoad_No;
                    //        dipmodel.Appointment_Id = item.Appointment_Id;
                    //        dipmodel.Dock_Name = item.Dock_Name;
                    //        dipmodel.Appointment_Date = item.Appointment_Date;
                    //        dipmodel.Appointment_Time = item.Appointment_Time;
                    //        dipmodel.PlanGoodsIssue_No = item.PlanGoodsIssue_No;
                    //        dipmodel.ShipTo_Id = item.ShipTo_Id;
                    //        dipmodel.ShipTo_Name = item.ShipTo_Name;
                    //        dipmodel.BranchCode = item.BranchCode;
                    //        dipmodel.Product_Id = item.Product_Id;
                    //        dipmodel.Product_Name = item.Product_Name;
                    //        dipmodel.Order_Qty = item.Order_Qty;
                    //        dipmodel.Order_Unit = item.Order_Unit;

                    //        result.CheckWaveDipmodel.Add(dipmodel);
                    //    }

                    //    foreach (var item in CheckWaveDipbyWave_Result)
                    //    {
                    //        CheckWaveDipbyWavemodel dipbyWavemodel = new CheckWaveDipbyWavemodel();
                    //        dipbyWavemodel.Appointment_Id = item.Appointment_Id;
                    //        dipbyWavemodel.Appointment_Date = item.Appointment_Date;
                    //        dipbyWavemodel.Appointment_Time = item.Appointment_Time;
                    //        dipbyWavemodel.TruckLoad_No = item.TruckLoad_No;
                    //        dipbyWavemodel.Order_Seq = item.Order_Seq;
                    //        dipbyWavemodel.PlanGoodsIssue_No = item.PlanGoodsIssue_No;
                    //        dipbyWavemodel.LineNum = item.LineNum;
                    //        dipbyWavemodel.Product_Id = item.Product_Id;
                    //        dipbyWavemodel.Product_Name = item.Product_Name;
                    //        dipbyWavemodel.BU_Order_TotalQty = item.BU_Order_TotalQty;
                    //        dipbyWavemodel.BU_GI_TotalQty = item.BU_GI_TotalQty;
                    //        dipbyWavemodel.SU_Order_TotalQty = item.SU_Order_TotalQty;
                    //        dipbyWavemodel.SU_GI_TotalQty = item.SU_GI_TotalQty;
                    //        dipbyWavemodel.SU_Unit = item.SU_Unit;
                    //        dipbyWavemodel.ERP_Location = item.ERP_Location;
                    //        dipbyWavemodel.Product_Lot = item.Product_Lot;
                    //        dipbyWavemodel.SU_Diff = item.SU_Diff;
                    //        dipbyWavemodel.GoodsIssue_No = item.GoodsIssue_No;
                    //        dipbyWavemodel.Document_Remark = item.Document_Remark;
                    //        dipbyWavemodel.DocumentRef_No3 = item.DocumentRef_No3;

                    //        result.CheckWaveDipbyWavemodel.Add(dipbyWavemodel);
                    //    }
                    //    result.goodsIssue_No = goodsIssue.GoodsIssue_No;
                    //    result.goodsIssue_Index = goodsIssue.GoodsIssue_Index.ToString();
                    //}
                    
                    result.resultIsUse = true;

                }
                
                return result;
            }
            catch (Exception ex)
            {
                result.resultIsUse = false;
                result.resultMsg = ex.Message;
                return result;
            }
        }

        #endregion

        #region PopupGIRunWave
        public actionResultPopupGIViewModel PopupGIRunWave(SearchDetailModel model)
        {
            try
            {
                var result = new List<PopupGIViewModel>();

                var query = db.im_PlanGoodsIssue.Where(c => c.Document_Status == 1);

                if (!string.IsNullOrEmpty(model.planGoodsIssue_No))
                {
                    query = query.Where(c => c.PlanGoodsIssue_No.Contains(model.planGoodsIssue_No));
                }

                if (model.owner_Index != Guid.Empty)
                {
                    query = query.Where(c => c.Owner_Index == model.owner_Index);
                }
                else if (!string.IsNullOrEmpty(model.owner_Name))
                {
                    query = query.Where(c => c.Owner_Name.Contains(model.owner_Name) || c.Owner_Id.Contains(model.owner_Name));
                }

                if (!string.IsNullOrEmpty(model.planGoodsIssue_Due_Date))
                {
                    var dateStart = model.planGoodsIssue_Due_Date.toBetweenDate();
                    var dateEnd = model.planGoodsIssue_Due_Date.toBetweenDate();
                    query = query.Where(c => c.PlanGoodsIssue_Due_Date >= dateStart.start && c.PlanGoodsIssue_Due_Date <= dateEnd.end);
                }

                var count = query.ToList().Count;

                query = query.OrderBy(o => o.PlanGoodsIssue_No);

                if (model.CurrentPage != 0 && model.PerPage != 0)
                {
                    query = query.Skip(((model.CurrentPage - 1) * model.PerPage));
                }

                if (model.PerPage != 0)
                {
                    query = query.Take(model.PerPage);
                }



                foreach (var i in query)
                {
                    var data = new PopupGIViewModel();
                    data.planGoodsIssue_Index = i.PlanGoodsIssue_Index;
                    data.planGoodsIssue_No = i.PlanGoodsIssue_No;
                    data.planGoodsIssue_Date = i.PlanGoodsIssue_Date.toString();
                    data.planGoodsIssue_Due_Date = i.PlanGoodsIssue_Due_Date.toString();
                    data.owner_Index = i.Owner_Index;
                    data.owner_Id = i.Owner_Id;
                    data.owner_Name = i.Owner_Name;
                    data.warehouse_Index_To = i.Warehouse_Index_To;
                    data.warehouse_Id_To = i.Warehouse_Id_To;
                    data.warehouse_Name_To = i.Warehouse_Name_To;

                    var dataItemList = new List<plangoodsissueitemViewModel>();
                    var queryitem = db.im_PlanGoodsIssueItem.Where(c => c.PlanGoodsIssue_Index == i.PlanGoodsIssue_Index && c.Document_Status == 0);
                    foreach (var q in queryitem)
                    {
                        var productRefNo2 = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("getProductMaster"), new { product_Index = q.Product_Index }.sJson());
                        var dataItem = new plangoodsissueitemViewModel
                        {
                            planGoodsIssueItem_Index = q.PlanGoodsIssueItem_Index,
                            planGoodsIssue_Index = q.PlanGoodsIssue_Index,
                            lineNum = q.LineNum,
                            product_Index = q.Product_Index,
                            product_Id = q.Product_Id,
                            product_Name = q.Product_Name,
                            product_SecondName = q.Product_SecondName,
                            product_ThirdName = q.Product_ThirdName,
                            product_Lot = q.Product_Lot,
                            itemStatus_Index = q.ItemStatus_Index,
                            itemStatus_Id = q.ItemStatus_Id,
                            itemStatus_Name = q.ItemStatus_Name,
                            qty = q.Qty,
                            ratio = q.Ratio,
                            totalQty = q.TotalQty,
                            productConversion_Index = q.ProductConversion_Index,
                            productConversion_Id = q.ProductConversion_Id,
                            productConversion_Name = q.ProductConversion_Name,
                            productConversion_Base = productRefNo2[0].productConversion_Name,
                            mfg_Date = q.MFG_Date,
                            exp_Date = q.EXP_Date,
                            unitWeight = q.UnitWeight,
                            weight = q.Weight,
                            unitWidth = q.UnitWidth,
                            unitLength = q.UnitLength,
                            unitHeight = q.UnitHeight,
                            unitVolume = q.UnitVolume,
                            volume = q.Volume,
                            unitPrice = q.UnitPrice,
                            price = q.Price,
                            documentRef_No1 = q.DocumentRef_No1,
                            documentRef_No2 = q.DocumentRef_No2,
                            documentRef_No3 = q.DocumentRef_No3,
                            documentRef_No4 = q.DocumentRef_No4,
                            documentRef_No5 = q.DocumentRef_No5,
                            documentItem_Remark = q.DocumentItem_Remark,
                            document_Status = q.Document_Status,
                            udf_1 = q.UDF_1,
                            udf_2 = q.UDF_2,
                            udf_3 = q.UDF_3,
                            udf_4 = q.UDF_4,
                            udf_5 = q.UDF_5,
                            create_By = q.Create_By,
                            create_Date = q.Create_Date,
                            update_By = q.Update_By,
                            update_Date = q.Update_Date,
                            cancel_By = q.Cancel_By,
                            cancel_Date = q.Cancel_Date,
                            qtyBackOrder = q.QtyBackOrder,
                            backOrderStatus = q.BackOrderStatus,
                            planGoodsIssue_Size = q.PlanGoodsIssue_Size,
                            qtyQA = q.QtyQA,
                            isQA = q.IsQA,
                            qty_Inner_Pack = q.Qty_Inner_Pack,
                            qty_Sup_Pack = q.Qty_Sup_Pack,
                            imageUri = q.ImageUri,
                            zoneCode = q.ZoneCode,
                            batch_Id = q.Batch_Id,
                            qa_By = q.QA_By,
                            qa_Date = q.QA_Date,
                            runWave_Status = q.RunWave_Status,
                            planGoodsIssue_No = q.PlanGoodsIssue_No,
                            countQty = q.CountQty,
                            isDelete = true,
                            ref_DocumentItem_Index = q.PlanGoodsIssueItem_Index.ToString(),
                            product_Id_RefNo2 = productRefNo2?.FirstOrDefault()?.Ref_No2,
                            erp_Location = q.ERP_Location
                        };

                        dataItemList.Add(dataItem);
                    }

                    data.itemDetail = dataItemList;
                    result.Add(data);
                }

                var actionResult = new actionResultPopupGIViewModel();
                actionResult.items = result.ToList();
                actionResult.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage, };

                return actionResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region updateStatusPlanGIRunWave
        public bool updateStatusPlanGIRunWave(plangoodsissueitemViewModel model)
        {
            String State = "Start " + model.sJson();
            String msglog = "";
            var olog = new logtxt();
            try
            {
                //var itemReserve = db.wm_BinCardReserve.Where(c=> c.BinCardReserve_Index ==(Guid.Parse(model.binCardReserve_Index)) && c.BinCardReserve_Status != -1).FirstOrDefault();
                var PGII = db.im_PlanGoodsIssueItem.Where(c => c.PlanGoodsIssueItem_Index == model.planGoodsIssueItem_Index && c.PlanGoodsIssue_Index == model.planGoodsIssue_Index).ToList();

                var PGI = db.im_PlanGoodsIssue.Find(model.planGoodsIssue_Index).Document_Status = 1;
                foreach (var p in PGII)
                {
                    p.Document_Status = 0;
                }

                {
                    var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transactionx.Commit();
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("deletepickProductBinbalance", msglog);
                        transactionx.Rollback();

                        throw exy;

                    }
                }

                //var PGIIstatus = db.im_PlanGoodsIssueItem.Where(c => c.PlanGoodsIssue_Index == model.planGoodsIssue_Index && c.Document_Status == 1).Count();

                //if (PGIIstatus == 0)
                //{
                //var PGI = db.im_PlanGoodsIssue.Where(c => c.PlanGoodsIssue_Index == model.planGoodsIssue_Index ).ToList();

                //foreach (var item in PGI)
                //{
                //    item.Document_Status = 1;
                //}

                //var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                //try
                //{
                //    db.SaveChanges();
                //    transactionx.Commit();
                //}

                //catch (Exception exy)
                //{
                //    msglog = State + " ex Rollback " + exy.Message.ToString();
                //    olog.logging("deletepickProductBinbalance", msglog);
                //    transactionx.Rollback();

                //    throw exy;

                //}
                //}



                return true;
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("pickProductBinbalance", msglog);
                return false;
            }
        }
        #endregion

        #region im_PlanGoodsIssue
        public List<PlanGoodIssueViewModel> im_PlanGoodsIssue(DocumentViewModel model)
        {
            try
            {

                var query = db.im_PlanGoodsIssue.AsQueryable();

                var result = new List<PlanGoodIssueViewModel>();

                if (model.listDocumentViewModel.FirstOrDefault().document_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Index).Contains(c.PlanGoodsIssue_Index));
                }

                if (model.listDocumentViewModel.FirstOrDefault().document_No != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_No).Contains(c.PlanGoodsIssue_No));
                }

                if (model.listDocumentViewModel.FirstOrDefault().document_Status != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Status).Contains(c.Document_Status));
                }


                var queryresult = query.ToList();


                foreach (var item in queryresult)
                {
                    var resultItem = new PlanGoodIssueViewModel();
                    resultItem.planGoodsIssue_Index = item.PlanGoodsIssue_Index;
                    resultItem.owner_Index = item.Owner_Index;
                    resultItem.owner_Id = item.Owner_Id;
                    resultItem.owner_Name = item.Owner_Name;
                    resultItem.soldTo_Index = item.SoldTo_Index;
                    resultItem.soldTo_Id = item.SoldTo_Id;
                    resultItem.soldTo_Name = item.SoldTo_Name;
                    resultItem.soldTo_Address = item.SoldTo_Address;
                    resultItem.documentType_Index = item.DocumentType_Index;
                    resultItem.documentType_Id = item.DocumentType_Id;
                    resultItem.documentType_Name = item.DocumentType_Name;
                    resultItem.shipTo_Index = item.ShipTo_Index;
                    resultItem.shipTo_Id = item.ShipTo_Id;
                    resultItem.shipTo_Name = item.ShipTo_Name;
                    resultItem.shipTo_Address = item.ShipTo_Address;
                    resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;
                    resultItem.planGoodsIssue_Date = item.PlanGoodsIssue_Date;
                    resultItem.planGoodsIssue_Due_Date = item.PlanGoodsIssue_Due_Date;
                    resultItem.documentRef_No1 = item.DocumentRef_No1;
                    resultItem.documentRef_No2 = item.DocumentRef_No2;
                    resultItem.documentRef_No3 = item.DocumentRef_No3;
                    resultItem.documentRef_No4 = item.DocumentRef_No4;
                    resultItem.documentRef_No5 = item.DocumentRef_No5;
                    resultItem.uDF_1 = item.UDF_1;
                    resultItem.uDF_2 = item.UDF_2;
                    resultItem.uDF_3 = item.UDF_3;
                    resultItem.uDF_4 = item.UDF_4;
                    resultItem.uDF_5 = item.UDF_5;
                    resultItem.documentPriority_Status = item.DocumentPriority_Status;
                    resultItem.warehouse_Index = item.Warehouse_Index;
                    resultItem.warehouse_Id = item.Warehouse_Id;
                    resultItem.warehouse_Name = item.Warehouse_Name;
                    resultItem.warehouse_Index_To = item.Warehouse_Index_To;
                    resultItem.warehouse_Id_To = item.Warehouse_Id_To;
                    resultItem.warehouse_Name_To = item.Warehouse_Name_To;
                    resultItem.soldTo_SubDistrict_Index = item.SoldTo_SubDistrict_Index;
                    resultItem.soldTo_District_Index = item.SoldTo_District_Index;
                    resultItem.soldTo_Province_Index = item.SoldTo_Province_Index;
                    resultItem.soldTo_Country_Index = item.SoldTo_Country_Index;
                    resultItem.soldTo_Postcode_Index = item.SoldTo_Postcode_Index;
                    resultItem.subDistrict_Index = item.SubDistrict_Index;
                    resultItem.District_Index = item.District_Index;
                    resultItem.province_Index = item.Province_Index;
                    resultItem.country_Index = item.Country_Index;
                    resultItem.postcode_Index = item.Postcode_Index;
                    resultItem.create_By = item.Create_By;
                    resultItem.create_Date = item.Create_Date;
                    resultItem.update_By = item.Update_By;
                    resultItem.update_Date = item.Update_Date;
                    resultItem.cancel_By = item.Cancel_By;
                    resultItem.cancel_Date = item.Cancel_Date;
                    resultItem.backOrderStatus = item.BackOrderStatus;
                    resultItem.round_Index = item.Round_Index;
                    resultItem.round_Id = item.Round_Id;
                    resultItem.round_Name = item.Round_Name;
                    resultItem.route_Index = item.Route_Index;
                    resultItem.route_Id = item.Route_Id;
                    resultItem.route_Name = item.Route_Name;
                    resultItem.printTaxInvoice = item.PrintTaxInvoice;
                    resultItem.payment_Type = item.Payment_Type;
                    resultItem.payment_Code = item.Payment_Code;
                    resultItem.cal_GrandTotal = item.Cal_GrandTotal;
                    resultItem.reasonCode_Index = item.ReasonCode_Index;
                    resultItem.reasonCode_Id = item.ReasonCode_Id;
                    resultItem.reasonCode_Name = item.ReasonCode_Name;
                    resultItem.ref_PlanGoodsIssue_Index = item.Ref_PlanGoodsIssue_Index;
                    resultItem.ref_PlanGoodsIssue_No = item.Ref_PlanGoodsIssue_No;
                    resultItem.status_RMS = item.Status_RMS;
                    resultItem.status_Desc_RMS = item.Status_Desc_RMS;
                    resultItem.Vendor_Id = item.Vendor_Id;
                    resultItem.sTPlanGoodsIssue_Due_Date = item.STPlanGoodsIssue_Due_Date;
                    resultItem.create_Date_File = item.Create_Date_File;
                    resultItem.status_EDI = item.Status_EDI;
                    resultItem.status_Reason = item.Status_Reason;
                    resultItem.round_Time = item.Round_Time;
                    resultItem.warehouse_Phone = item.Warehouse_Phone;
                    resultItem.soldTo_T1C = item.SoldTo_T1C;
                    resultItem.soldTo_T1CPhone = item.SoldTo_T1CPhone;
                    resultItem.soldTo_Email = item.SoldTo_Email;
                    resultItem.soldTo_Phone = item.SoldTo_Phone;
                    resultItem.shipTo_CompanyName = item.ShipTo_CompanyName;
                    resultItem.shipTo_Remark = item.ShipTo_Remark;
                    resultItem.shipTo_Telephone = item.ShipTo_Telephone;
                    resultItem.shipTo_TaxNo = item.ShipTo_TaxNo;
                    resultItem.invoice_Name = item.Invoice_Name;
                    resultItem.invoice_CompanyName = item.Invoice_CompanyName;
                    resultItem.invoice_Address = item.Invoice_Address;
                    resultItem.invoice_Remark = item.Invoice_Remark;
                    resultItem.invoice_Telephone = item.Invoice_Telephone;
                    resultItem.invoice_TaxNo = item.Invoice_TaxNo;
                    resultItem.payment_Issuer = item.Payment_Issuer;
                    resultItem.cal_PromotionDiscount = item.Cal_PromotionDiscount;
                    resultItem.cal_Cpn2Discount = item.Cal_Cpn2Discount;
                    resultItem.cal_Cpn9Discount = item.Cal_Cpn9Discount;
                    resultItem.cal_EvoucherDiscount = item.Cal_EvoucherDiscount;
                    resultItem.cal_TotalAfterDiscount = item.Cal_TotalAfterDiscount;
                    resultItem.document_Remark_Sub = item.Document_Remark;
                    resultItem.userAssign = item.UserAssign;
                    resultItem.soldTo_Name_Cus = item.SoldTo_Name_Cus;
                    resultItem.soldTo_Tel = item.SoldTo_Tel;
                    resultItem.soldTo_Email_Cus = item.SoldTo_Email_Cus;
                    resultItem.soldTo_Phone_Cus = item.SoldTo_Phone_Cus;
                    resultItem.soldTo_Address_Cus = item.SoldTo_Address_Cus;
                    resultItem.isPostPickConfirm = item.IsPostPickConfirm;
                    resultItem.isPostPickConfirm_Date = item.IsPostPickConfirm_Date;
                    resultItem.isPostShippmentDispatch = item.IsPostShippmentDispatch;
                    resultItem.isPostShippmentDispatch_Date = item.IsPostShippmentDispatch_Date;
                    resultItem.statusDropST = item.StatusDropST;
                    resultItem.statusDropST_Date = item.StatusDropST_Date;
                    resultItem.shipTo_AddressName = item.ShipTo_AddressName;
                    resultItem.invoice_AddressName = item.Invoice_AddressName;
                    resultItem.ref_WavePick_index = item.Ref_WavePick_index;
                    resultItem.planGoodsIssue_Time = item.PlanGoodsIssue_Time;

                    result.Add(resultItem);
                }

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoSoldTo
        public List<ItemListViewModel> autoSoldTofilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();


                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }
                if (!string.IsNullOrEmpty(data.key2))
                {
                    filterModel.key = data.key2;
                }


                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoSoldTofilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoShipTo
        public List<ItemListViewModel> autoShipTofilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();


                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoShipTofilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region autoMovementTypefilter
        public List<ItemListViewModel> autoMovementTypefilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();


                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoMovementTypefilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PrintPlanGoodsIssue
        public string PrintPlanGoodsIssue(ReportPlanGoodsIssueViewModel data, string rootPath = "")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            Guid? planGIItemIndex = new Guid();

            try
            {
                var queryHead = db.im_PlanGoodsIssue.FirstOrDefault(c => c.PlanGoodsIssue_Index == data.planGoodsIssue_Index);
                var query = db.im_PlanGoodsIssueItem.Where(c => c.PlanGoodsIssue_Index == data.planGoodsIssue_Index);
                var result = new List<ReportPlanGoodsIssueViewModel>();

                string createDate = queryHead.Create_Date.toString();
                string planGI_Create = DateTime.ParseExact(createDate.Substring(0, 8), "yyyyMMdd",
                System.Globalization.CultureInfo.InvariantCulture).ToString("dd.MM.yyyy", culture);



                int i = 1;
                foreach (var item in query)
                {
                    var resultItem = new ReportPlanGoodsIssueViewModel();
                    resultItem.table_No = i;
                    resultItem.planGoodsIssue_No = queryHead.PlanGoodsIssue_No;
                    resultItem.documentType_Name = queryHead.DocumentType_Name;
                    resultItem.documentRef_No1 = queryHead.DocumentRef_No1;
                    resultItem.create_Date = planGI_Create;
                    resultItem.soldTo_Id = queryHead.SoldTo_Id;
                    resultItem.soldTo_Name = queryHead.SoldTo_Name;
                    resultItem.shipTo_Address = queryHead.ShipTo_Address;
                    resultItem.product_Index = item.Product_Index;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.qty = Convert.ToInt32(item.Qty);
                    resultItem.productConversion_Name = item.ProductConversion_Name;

                    string expDate = item.EXP_Date.toString();
                    if (!String.IsNullOrEmpty(expDate))
                    {
                        resultItem.eXP_Date = DateTime.ParseExact(expDate.Substring(0, 8), "yyyyMMdd",
                        System.Globalization.CultureInfo.InvariantCulture).ToString("dd.MM.yyyy", culture);
                    }

                    result.Add(resultItem);
                    i++;
                }
                result.ToList();

                rootPath = rootPath.Replace("\\PlanGIAPI", "");
                var reportPath = rootPath + "\\PlanGIBusiness\\Reports\\PlanGI\\ReportPlanGoodsIssue.rdlc";
                //var reportPath = rootPath + "\\Report\\Pack\\StockMovementReport.rdlc";
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

                var check_status = db.View_im_PlanGoodsIssue_status_SAP.Where(c => XXX.Contains(c.PlanGoodsIssue_Index)).ToList();
                if (check_status.Count > 0)
                {
                    return "";
                }
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
                var rpt_data = db.View_RPT_DeliveryNote_emergency.FromSql("sp_RPT_DeliveryNote_emergency @planGoodsIssue_Index", PGI).OrderBy(c => c.Order_seq).ThenBy(c => c.Row_Runing_product).ToList();

                //var rpt_data = db.View_RPT_DeliveryNote_emergency.Where(C => C.PlanGoodsIssue_Index == data.planGoodsIssue_Index).OrderBy(c => c.Order_seq).ThenBy(c => c.Row_Runing_product).ToList();
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

        #region dropdownCostCenter
        public List<CostCenterViewModel> dropdownCostCenter(CostCenterViewModel data)
        {
            try
            {
                var result = new List<CostCenterViewModel>();

                var filterModel = new CostCenterViewModel();

                //GetConfig
                result = utils.SendDataApi<List<CostCenterViewModel>>(new AppSettingConfig().GetUrl("dropdownCostCenter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region dropdownStorageLoc
        public List<StorageLocViewModel> dropdownStorageLoc(StorageLocViewModel data)
        {
            try
            {
                var result = new List<StorageLocViewModel>();

                var filterModel = new StorageLocViewModel();

                if (data.warehouse_Index_To != new Guid("00000000-0000-0000-0000-000000000000".ToString()) && data.warehouse_Index_To != null)
                {
                    filterModel.warehouse_Index_To = data.warehouse_Index_To;
                }


                //GetConfig
                result = utils.SendDataApi<List<StorageLocViewModel>>(new AppSettingConfig().GetUrl("dropdownStorageLoc"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PrintOutPGI
        public string PrintOutPGI(PrintOutPGIViewModel model, string rootPath = "")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            Guid? planGIItemIndex = new Guid();

            try
            {

                var result = new List<PrintOutPGIViewModel>();

                var log = db.im_Signatory_log.Where(c => c.Ref_Document_Index == model.PlanGoodsIssue_Index).ToList();

                if (log.Count <= 0)
                {
                    #region InsertRepresentative
                    if (model.isRepresentative == true)
                    {
                        im_Signatory_log RepresentativeLogNew = new im_Signatory_log();

                        RepresentativeLogNew.Signatory_Index = Guid.NewGuid();
                        RepresentativeLogNew.SignatoryType_Name = "representative";
                        RepresentativeLogNew.User_Index = new Guid(model.representative_user_Index);
                        RepresentativeLogNew.User_Id = model.representative_user_Id;
                        RepresentativeLogNew.User_Name = model.representative_user_Name;
                        RepresentativeLogNew.First_Name = model.representative_first_Name;
                        RepresentativeLogNew.Last_Name = model.representative_last_Name;
                        RepresentativeLogNew.DocumentType_Index = model.documentType_Index.GetValueOrDefault();
                        RepresentativeLogNew.DocumentType_Id = model.documentType_Id;
                        RepresentativeLogNew.DocumentType_Name = model.documentType_Name;
                        RepresentativeLogNew.Ref_Document_Index = model.PlanGoodsIssue_Index;
                        RepresentativeLogNew.Ref_Document_No = model.PlanGoodsIssue_No;
                        RepresentativeLogNew.Position_Code = model.representative_position_Code;
                        RepresentativeLogNew.IsActive = 1;
                        RepresentativeLogNew.IsDelete = 0;
                        RepresentativeLogNew.IsSystem = 0;
                        RepresentativeLogNew.Status_Id = 0;
                        RepresentativeLogNew.Create_By = model.user;
                        RepresentativeLogNew.Create_Date = DateTime.Now;
                        db.im_Signatory_log.Add(RepresentativeLogNew);
                    }

                    #endregion

                    #region InsertEndorser
                    if (model.isEndorser == true)
                    {
                        im_Signatory_log EndorserLogNew = new im_Signatory_log();

                        EndorserLogNew.Signatory_Index = Guid.NewGuid();
                        EndorserLogNew.SignatoryType_Name = "endorser";
                        EndorserLogNew.User_Index = new Guid(model.endorser_user_Index);
                        EndorserLogNew.User_Id = model.endorser_user_Id;
                        EndorserLogNew.User_Name = model.endorser_user_Name;
                        EndorserLogNew.First_Name = model.endorser_first_Name;
                        EndorserLogNew.Last_Name = model.endorser_last_Name;
                        EndorserLogNew.DocumentType_Index = model.documentType_Index.GetValueOrDefault();
                        EndorserLogNew.DocumentType_Id = model.documentType_Id;
                        EndorserLogNew.DocumentType_Name = model.documentType_Name;
                        EndorserLogNew.Ref_Document_Index = model.PlanGoodsIssue_Index;
                        EndorserLogNew.Ref_Document_No = model.PlanGoodsIssue_No;
                        EndorserLogNew.Position_Code = model.endorser_position_Code;
                        EndorserLogNew.Position_Name = model.endorser_pos_Name;
                        EndorserLogNew.IsActive = 1;
                        EndorserLogNew.IsDelete = 0;
                        EndorserLogNew.IsSystem = 0;
                        EndorserLogNew.Status_Id = 0;
                        EndorserLogNew.Create_By = model.user;
                        EndorserLogNew.Create_Date = DateTime.Now;
                        db.im_Signatory_log.Add(EndorserLogNew);

                    }

                    #endregion

                    #region InsertTransferor
                    if (model.isTransferor == true)
                    {
                        im_Signatory_log TransferorLogNew = new im_Signatory_log();

                        TransferorLogNew.Signatory_Index = Guid.NewGuid();
                        TransferorLogNew.SignatoryType_Name = "transferor";
                        TransferorLogNew.User_Index = new Guid(model.transferor_user_Index);
                        TransferorLogNew.User_Id = model.transferor_user_Id;
                        TransferorLogNew.User_Name = model.transferor_user_Name;
                        TransferorLogNew.First_Name = model.transferor_first_Name;
                        TransferorLogNew.Last_Name = model.transferor_last_Name;
                        TransferorLogNew.DocumentType_Index = model.documentType_Index.GetValueOrDefault();
                        TransferorLogNew.DocumentType_Id = model.documentType_Id;
                        TransferorLogNew.DocumentType_Name = model.documentType_Name;
                        TransferorLogNew.Ref_Document_Index = model.PlanGoodsIssue_Index;
                        TransferorLogNew.Ref_Document_No = model.PlanGoodsIssue_No;
                        TransferorLogNew.Position_Code = model.transferor_position_Code;
                        TransferorLogNew.Position_Name = model.transferor_pos_Name;
                        TransferorLogNew.IsActive = 1;
                        TransferorLogNew.IsDelete = 0;
                        TransferorLogNew.IsSystem = 0;
                        TransferorLogNew.Status_Id = 0;
                        TransferorLogNew.Create_By = model.user;
                        TransferorLogNew.Create_Date = DateTime.Now;
                        db.im_Signatory_log.Add(TransferorLogNew);

                    }

                    #endregion

                    #region InsertTransfer_Approver
                    if (model.isTransfer_Approver == true)
                    {
                        im_Signatory_log Transfer_ApproverLogNew = new im_Signatory_log();

                        Transfer_ApproverLogNew.Signatory_Index = Guid.NewGuid();
                        Transfer_ApproverLogNew.SignatoryType_Name = "transfer_Approver";
                        Transfer_ApproverLogNew.User_Index = new Guid(model.transfer_Approver_user_Index);
                        Transfer_ApproverLogNew.User_Id = model.transfer_Approver_user_Id;
                        Transfer_ApproverLogNew.User_Name = model.transfer_Approver_user_Name;
                        Transfer_ApproverLogNew.First_Name = model.transfer_Approver_first_Name;
                        Transfer_ApproverLogNew.Last_Name = model.transfer_Approver_last_Name;
                        Transfer_ApproverLogNew.DocumentType_Index = model.documentType_Index.GetValueOrDefault();
                        Transfer_ApproverLogNew.DocumentType_Id = model.documentType_Id;
                        Transfer_ApproverLogNew.DocumentType_Name = model.documentType_Name;
                        Transfer_ApproverLogNew.Ref_Document_Index = model.PlanGoodsIssue_Index;
                        Transfer_ApproverLogNew.Ref_Document_No = model.PlanGoodsIssue_No;
                        Transfer_ApproverLogNew.Position_Code = model.transfer_Approver_position_Code;
                        Transfer_ApproverLogNew.Position_Name = model.transfer_Approver_pos_Name;
                        Transfer_ApproverLogNew.IsActive = 1;
                        Transfer_ApproverLogNew.IsDelete = 0;
                        Transfer_ApproverLogNew.IsSystem = 0;
                        Transfer_ApproverLogNew.Status_Id = 0;
                        Transfer_ApproverLogNew.Create_By = model.user;
                        Transfer_ApproverLogNew.Create_Date = DateTime.Now;
                        db.im_Signatory_log.Add(Transfer_ApproverLogNew);

                    }

                    #endregion

                    #region InsertRecorder
                    if (model.isRecorder == true)
                    {
                        im_Signatory_log RecorderLogNew = new im_Signatory_log();

                        RecorderLogNew.Signatory_Index = Guid.NewGuid();
                        RecorderLogNew.SignatoryType_Name = "recorder";
                        RecorderLogNew.User_Index = new Guid(model.recorder_user_Index);
                        RecorderLogNew.User_Id = model.recorder_user_Id;
                        RecorderLogNew.User_Name = model.recorder_user_Name;
                        RecorderLogNew.First_Name = model.recorder_first_Name;
                        RecorderLogNew.Last_Name = model.recorder_last_Name;
                        RecorderLogNew.DocumentType_Index = model.documentType_Index.GetValueOrDefault();
                        RecorderLogNew.DocumentType_Id = model.documentType_Id;
                        RecorderLogNew.DocumentType_Name = model.documentType_Name;
                        RecorderLogNew.Ref_Document_Index = model.PlanGoodsIssue_Index;
                        RecorderLogNew.Ref_Document_No = model.PlanGoodsIssue_No;
                        RecorderLogNew.Position_Code = model.recorder_position_Code;
                        RecorderLogNew.Position_Name = model.recorder_pos_Name;
                        RecorderLogNew.IsActive = 1;
                        RecorderLogNew.IsDelete = 0;
                        RecorderLogNew.IsSystem = 0;
                        RecorderLogNew.Status_Id = 0;
                        RecorderLogNew.Create_By = model.user;
                        RecorderLogNew.Create_Date = DateTime.Now;
                        db.im_Signatory_log.Add(RecorderLogNew);

                    }

                    #endregion

                }

                else
                {
                    foreach (var item in log)
                    {
                        var logOld = db.im_Signatory_log.Find(item.Signatory_Index);



                        logOld.Update_Date = DateTime.Now;
                        logOld.Update_By = model.user;

                        #region representative

                        var findRepresentative = db.im_Signatory_log.Where(c => c.Ref_Document_Index == item.Ref_Document_Index && c.SignatoryType_Name == "representative").FirstOrDefault();

                        if (findRepresentative == null)
                        {
                            #region InsertRepresentative
                            if (model.isRepresentative == true)
                            {
                                im_Signatory_log RepresentativeLogNew = new im_Signatory_log();

                                RepresentativeLogNew.Signatory_Index = Guid.NewGuid();
                                RepresentativeLogNew.SignatoryType_Name = "representative";
                                RepresentativeLogNew.User_Index = new Guid(model.representative_user_Index);
                                RepresentativeLogNew.User_Id = model.representative_user_Id;
                                RepresentativeLogNew.User_Name = model.representative_user_Name;
                                RepresentativeLogNew.First_Name = model.representative_first_Name;
                                RepresentativeLogNew.Last_Name = model.representative_last_Name;
                                RepresentativeLogNew.DocumentType_Index = model.documentType_Index.GetValueOrDefault();
                                RepresentativeLogNew.DocumentType_Id = model.documentType_Id;
                                RepresentativeLogNew.DocumentType_Name = model.documentType_Name;
                                RepresentativeLogNew.Ref_Document_Index = model.PlanGoodsIssue_Index;
                                RepresentativeLogNew.Ref_Document_No = model.PlanGoodsIssue_No;
                                RepresentativeLogNew.Position_Code = model.representative_position_Code;
                                RepresentativeLogNew.IsActive = 1;
                                RepresentativeLogNew.IsDelete = 0;
                                RepresentativeLogNew.IsSystem = 0;
                                RepresentativeLogNew.Status_Id = 0;
                                RepresentativeLogNew.Create_By = model.user;
                                RepresentativeLogNew.Create_Date = DateTime.Now;
                                db.im_Signatory_log.Add(RepresentativeLogNew);
                            }

                            #endregion

                        }

                        else
                        {
                            if (model.isRepresentative == true && logOld.SignatoryType_Name == "representative")
                            {
                                logOld.User_Index = new Guid(model.representative_user_Index);
                                logOld.User_Id = model.representative_user_Id;
                                logOld.User_Name = model.representative_user_Name;
                                logOld.First_Name = model.representative_first_Name;
                                logOld.Last_Name = model.representative_last_Name;
                                logOld.Position_Code = model.representative_position_Code;
                                logOld.IsActive = 1;
                                logOld.IsDelete = 0;
                            }
                            if (model.isRepresentative == false && logOld.SignatoryType_Name == "representative")
                            {
                                logOld.IsActive = 0;
                                logOld.IsDelete = 1;
                            }
                        }



                        #endregion

                        #region endorser
                        var findEndorser = db.im_Signatory_log.Where(c => c.Ref_Document_Index == item.Ref_Document_Index && c.SignatoryType_Name == "endorser").FirstOrDefault();

                        if (findEndorser == null)
                        {
                            #region InsertEndorser
                            if (model.isEndorser == true)
                            {
                                im_Signatory_log EndorserLogNew = new im_Signatory_log();

                                EndorserLogNew.Signatory_Index = Guid.NewGuid();
                                EndorserLogNew.SignatoryType_Name = "endorser";
                                EndorserLogNew.User_Index = new Guid(model.endorser_user_Index);
                                EndorserLogNew.User_Id = model.endorser_user_Id;
                                EndorserLogNew.User_Name = model.endorser_user_Name;
                                EndorserLogNew.First_Name = model.endorser_first_Name;
                                EndorserLogNew.Last_Name = model.endorser_last_Name;
                                EndorserLogNew.DocumentType_Index = model.documentType_Index.GetValueOrDefault();
                                EndorserLogNew.DocumentType_Id = model.documentType_Id;
                                EndorserLogNew.DocumentType_Name = model.documentType_Name;
                                EndorserLogNew.Ref_Document_Index = model.PlanGoodsIssue_Index;
                                EndorserLogNew.Ref_Document_No = model.PlanGoodsIssue_No;
                                EndorserLogNew.Position_Code = model.endorser_position_Code;
                                EndorserLogNew.Position_Name = model.endorser_pos_Name;
                                EndorserLogNew.IsActive = 1;
                                EndorserLogNew.IsDelete = 0;
                                EndorserLogNew.IsSystem = 0;
                                EndorserLogNew.Status_Id = 0;
                                EndorserLogNew.Create_By = model.user;
                                EndorserLogNew.Create_Date = DateTime.Now;
                                db.im_Signatory_log.Add(EndorserLogNew);

                            }

                            #endregion

                        }

                        else
                        {
                            if (model.isEndorser == true && logOld.SignatoryType_Name == "endorser")
                            {
                                logOld.User_Index = new Guid(model.endorser_user_Index);
                                logOld.User_Id = model.endorser_user_Id;
                                logOld.User_Name = model.endorser_user_Name;
                                logOld.First_Name = model.endorser_first_Name;
                                logOld.Position_Code = model.endorser_position_Code;
                                logOld.Position_Name = model.endorser_pos_Name;
                                logOld.IsActive = 1;
                                logOld.IsDelete = 0;
                            }
                            if (model.isEndorser == false && logOld.SignatoryType_Name == "endorser")
                            {
                                logOld.IsActive = 0;
                                logOld.IsDelete = 1;
                            }
                        }



                        #endregion

                        #region transferor

                        var findTransferor = db.im_Signatory_log.Where(c => c.Ref_Document_Index == item.Ref_Document_Index && c.SignatoryType_Name == "transferor").FirstOrDefault();

                        if (findTransferor == null)
                        {
                            #region InsertTransferor
                            if (model.isTransferor == true)
                            {
                                im_Signatory_log TransferorLogNew = new im_Signatory_log();

                                TransferorLogNew.Signatory_Index = Guid.NewGuid();
                                TransferorLogNew.SignatoryType_Name = "transferor";
                                TransferorLogNew.User_Index = new Guid(model.transferor_user_Index);
                                TransferorLogNew.User_Id = model.transferor_user_Id;
                                TransferorLogNew.User_Name = model.transferor_user_Name;
                                TransferorLogNew.First_Name = model.transferor_first_Name;
                                TransferorLogNew.Last_Name = model.transferor_last_Name;
                                TransferorLogNew.DocumentType_Index = model.documentType_Index.GetValueOrDefault();
                                TransferorLogNew.DocumentType_Id = model.documentType_Id;
                                TransferorLogNew.DocumentType_Name = model.documentType_Name;
                                TransferorLogNew.Ref_Document_Index = model.PlanGoodsIssue_Index;
                                TransferorLogNew.Ref_Document_No = model.PlanGoodsIssue_No;
                                TransferorLogNew.Position_Code = model.transferor_position_Code;
                                TransferorLogNew.Position_Name = model.transferor_pos_Name;
                                TransferorLogNew.IsActive = 1;
                                TransferorLogNew.IsDelete = 0;
                                TransferorLogNew.IsSystem = 0;
                                TransferorLogNew.Status_Id = 0;
                                TransferorLogNew.Create_By = model.user;
                                TransferorLogNew.Create_Date = DateTime.Now;
                                db.im_Signatory_log.Add(TransferorLogNew);

                            }

                            #endregion

                        }

                        else
                        {
                            if (model.isTransferor == true && logOld.SignatoryType_Name == "transferor")
                            {
                                logOld.User_Index = new Guid(model.transferor_user_Index);
                                logOld.User_Id = model.transferor_user_Id;
                                logOld.User_Name = model.transferor_user_Name;
                                logOld.First_Name = model.transferor_first_Name;
                                logOld.Last_Name = model.transferor_last_Name;
                                logOld.Position_Code = model.transferor_position_Code;
                                logOld.Position_Name = model.transferor_pos_Name;
                                logOld.IsActive = 1;
                                logOld.IsDelete = 0;
                            }
                            if (model.isTransferor == false && logOld.SignatoryType_Name == "transferor")
                            {
                                logOld.IsActive = 0;
                                logOld.IsDelete = 1;
                            }
                        }



                        #endregion

                        #region transfer_Approver

                        var findTransfer_Approver = db.im_Signatory_log.Where(c => c.Ref_Document_Index == item.Ref_Document_Index && c.SignatoryType_Name == "transfer_Approver").FirstOrDefault();

                        if (findTransfer_Approver == null)
                        {
                            #region InsertTransfer_Approver
                            if (model.isTransfer_Approver == true)
                            {
                                im_Signatory_log Transfer_ApproverLogNew = new im_Signatory_log();

                                Transfer_ApproverLogNew.Signatory_Index = Guid.NewGuid();
                                Transfer_ApproverLogNew.SignatoryType_Name = "transfer_Approver";
                                Transfer_ApproverLogNew.User_Index = new Guid(model.transfer_Approver_user_Index);
                                Transfer_ApproverLogNew.User_Id = model.transfer_Approver_user_Id;
                                Transfer_ApproverLogNew.User_Name = model.transfer_Approver_user_Name;
                                Transfer_ApproverLogNew.First_Name = model.transfer_Approver_first_Name;
                                Transfer_ApproverLogNew.Last_Name = model.transfer_Approver_last_Name;
                                Transfer_ApproverLogNew.DocumentType_Index = model.documentType_Index.GetValueOrDefault();
                                Transfer_ApproverLogNew.DocumentType_Id = model.documentType_Id;
                                Transfer_ApproverLogNew.DocumentType_Name = model.documentType_Name;
                                Transfer_ApproverLogNew.Ref_Document_Index = model.PlanGoodsIssue_Index;
                                Transfer_ApproverLogNew.Ref_Document_No = model.PlanGoodsIssue_No;
                                Transfer_ApproverLogNew.Position_Code = model.transfer_Approver_position_Code;
                                Transfer_ApproverLogNew.Position_Name = model.transfer_Approver_pos_Name;
                                Transfer_ApproverLogNew.IsActive = 1;
                                Transfer_ApproverLogNew.IsDelete = 0;
                                Transfer_ApproverLogNew.IsSystem = 0;
                                Transfer_ApproverLogNew.Status_Id = 0;
                                Transfer_ApproverLogNew.Create_By = model.user;
                                Transfer_ApproverLogNew.Create_Date = DateTime.Now;
                                db.im_Signatory_log.Add(Transfer_ApproverLogNew);

                            }

                            #endregion
                        }
                        else
                        {
                            if (model.isTransfer_Approver == true && logOld.SignatoryType_Name == "transfer_Approver")
                            {
                                logOld.User_Index = new Guid(model.transfer_Approver_user_Index);
                                logOld.User_Id = model.transfer_Approver_user_Id;
                                logOld.User_Name = model.transfer_Approver_user_Name;
                                logOld.First_Name = model.transfer_Approver_first_Name;
                                logOld.Last_Name = model.transfer_Approver_last_Name;
                                logOld.Position_Code = model.transfer_Approver_position_Code;
                                logOld.Position_Name = model.transfer_Approver_pos_Name;
                                logOld.IsActive = 1;
                                logOld.IsDelete = 0;
                            }
                            if (model.isTransfer_Approver == false && logOld.SignatoryType_Name == "transfer_Approver")
                            {
                                logOld.IsActive = 0;
                                logOld.IsDelete = 1;
                            }
                        }


                        #endregion

                        #region recorder

                        var findRecorder = db.im_Signatory_log.Where(c => c.Ref_Document_Index == item.Ref_Document_Index && c.SignatoryType_Name == "recorder").FirstOrDefault();

                        if (findRecorder == null)
                        {
                            #region InsertRecorder
                            if (model.isRecorder == true)
                            {
                                im_Signatory_log RecorderLogNew = new im_Signatory_log();

                                RecorderLogNew.Signatory_Index = Guid.NewGuid();
                                RecorderLogNew.SignatoryType_Name = "recorder";
                                RecorderLogNew.User_Index = new Guid(model.recorder_user_Index);
                                RecorderLogNew.User_Id = model.recorder_user_Id;
                                RecorderLogNew.User_Name = model.recorder_user_Name;
                                RecorderLogNew.First_Name = model.recorder_first_Name;
                                RecorderLogNew.Last_Name = model.recorder_last_Name;
                                RecorderLogNew.DocumentType_Index = model.documentType_Index.GetValueOrDefault();
                                RecorderLogNew.DocumentType_Id = model.documentType_Id;
                                RecorderLogNew.DocumentType_Name = model.documentType_Name;
                                RecorderLogNew.Ref_Document_Index = model.PlanGoodsIssue_Index;
                                RecorderLogNew.Ref_Document_No = model.PlanGoodsIssue_No;
                                RecorderLogNew.Position_Code = model.recorder_position_Code;
                                RecorderLogNew.Position_Name = model.recorder_pos_Name;
                                RecorderLogNew.IsActive = 1;
                                RecorderLogNew.IsDelete = 0;
                                RecorderLogNew.IsSystem = 0;
                                RecorderLogNew.Status_Id = 0;
                                RecorderLogNew.Create_By = model.user;
                                RecorderLogNew.Create_Date = DateTime.Now;
                                db.im_Signatory_log.Add(RecorderLogNew);

                            }

                            #endregion

                        }
                        else
                        {

                            if (model.isRecorder == true && logOld.SignatoryType_Name == "recorder")
                            {
                                logOld.User_Index = new Guid(model.recorder_user_Index);
                                logOld.User_Id = model.recorder_user_Id;
                                logOld.User_Name = model.recorder_user_Name;
                                logOld.First_Name = model.recorder_first_Name;
                                logOld.Last_Name = model.recorder_last_Name;
                                logOld.Position_Code = model.recorder_position_Code;
                                logOld.Position_Name = model.recorder_pos_Name;
                                logOld.IsActive = 1;
                                logOld.IsDelete = 0;
                            }
                            if (model.isRecorder == false && logOld.SignatoryType_Name == "recorder")
                            {
                                logOld.IsActive = 0;
                                logOld.IsDelete = 1;
                            }
                        }


                        #endregion

                    }
                }
                var data = (from pgii in db.im_PlanGoodsIssueItem
                            join pgi in db.im_PlanGoodsIssue on pgii.PlanGoodsIssue_Index equals pgi.PlanGoodsIssue_Index
                            where pgi.PlanGoodsIssue_Index == model.PlanGoodsIssue_Index
                            select new PrintOutPGIViewModel
                            {
                                PlanGoodsIssue_No = pgi.PlanGoodsIssue_No,
                                DocumentItem_Remark = string.Join(",", db.im_PlanGoodsIssueItem
                                .Where(c => c.PlanGoodsIssue_Index == pgi.PlanGoodsIssue_Index
                                && !(c.DocumentItem_Remark == null || c.DocumentItem_Remark == string.Empty)
                                && c.Document_Status != -1)
                                .GroupBy(g => g.DocumentItem_Remark)
                                .Select(s => s.Key)),
                                PlanGoodsIssue_Date = pgi.PlanGoodsIssue_Date.sParse<DateTime>().ToString("dd/MM/yyyy"),
                                Warehouse_Name = "3000",
                                SoldTo_Name = pgi.SoldTo_Id,
                                Sloc_Name = "3100",
                                Sloc_Name_To = pgi.Sloc_Id,
                                MovementType_Id = pgi.MovementType_Id,

                                Product_Id = pgii.Product_Id,
                                Product_Name = pgii.Product_Name,
                                Qty = pgii.Qty,
                                ProductConversion_Name = pgii.ProductConversion_Name,

                                isRepresentative = model.isRepresentative,
                                representative_Name = model.representative_Name,

                                isEndorser = model.isEndorser,
                                endorser_Name = model.endorser_Name,
                                endorser_pos_Name = model.endorser_pos_Name,

                                isTransferor = model.isTransferor,
                                transferor_Name = model.transferor_Name,
                                transferor_pos_Name = model.transferor_pos_Name,

                                isTransfer_Approver = model.isTransfer_Approver,
                                transfer_Approver_Name = model.transfer_Approver_Name,
                                transfer_Approver_pos_Name = model.transfer_Approver_pos_Name,

                                isRecorder = model.isRecorder,
                                recorder_Name = model.recorder_Name,
                                recorder_pos_Name = model.recorder_pos_Name,
                            }
                    ).ToList();


                rootPath = rootPath.Replace("\\PlanGIAPI", "");
                //var reportPath = rootPath + "\\PlanGIBusiness\\Reports\\PrintOutPGI\\PrintOutPGI.rdlc";
                var reportPath = rootPath + "\\Reports\\PrintOutPGI\\PrintOutPGI.rdlc";
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", data);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);

                State = "transactionSave";

                var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SaveLogPlanGI", msglog);
                    transactionx.Rollback();

                    throw exy;

                }
                return saveLocation;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        public List<im_Signatory_logViewModel> findUser(im_Signatory_logViewModel data)
        {
            try
            {

                var items = new List<im_Signatory_logViewModel>();



                var query = db.im_Signatory_log.AsQueryable();



                if (!string.IsNullOrEmpty(data.planGoodsIssue_No))
                {
                    query = query.Where(c => c.Ref_Document_No == data.planGoodsIssue_No);
                }


                var result = query.Take(100).OrderByDescending(o => o.Create_Date).ToList();

                foreach (var item in result)
                {
                    var resultItem = new im_Signatory_logViewModel();

                    resultItem.signatory_Index = item.Signatory_Index;
                    resultItem.signatoryType_Id = item.SignatoryType_Id;
                    resultItem.signatoryType_Name = item.SignatoryType_Name;
                    resultItem.first_Name = item.First_Name;
                    resultItem.last_Name = item.Last_Name;
                    resultItem.position_Name = item.Position_Name;

                    items.Add(resultItem);
                }


                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region dropdownWeight
        public List<WeightViewModel> dropdownWeight(WeightViewModel data)
        {
            try
            {
                //GetConfig
                var result = utils.SendDataApi<List<WeightViewModel>>(new AppSettingConfig().GetUrl("dropdownWeight"), new { }.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownVolume
        public List<VolumeViewModel> dropdownVolume(VolumeViewModel data)
        {
            try
            {
                //GetConfig
                var result = utils.SendDataApi<List<VolumeViewModel>>(new AppSettingConfig().GetUrl("dropdownVolume"), new { }.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownCurrency
        public List<CurrencyViewModel> dropdownCurrency(CurrencyViewModel data)
        {
            try
            {
                //GetConfig
                var result = utils.SendDataApi<List<CurrencyViewModel>>(new AppSettingConfig().GetUrl("dropdownCurrency"), new { }.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownRoute
        public List<RouteViewModel> dropdownRoute(RouteViewModel data)
        {
            try
            {
                //GetConfig
                var result = utils.SendDataApi<List<RouteViewModel>>(new AppSettingConfig().GetUrl("dropdownRoute"), new { }.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownSubRoute
        public List<SubRouteViewModel> dropdownSubRoute(SubRouteViewModel data)
        {
            try
            {
                //GetConfig
                var result = utils.SendDataApi<List<SubRouteViewModel>>(new AppSettingConfig().GetUrl("dropdownSubRoute"), new { }.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region shippingMethoddropdown
        public List<ShippingMethodViewModel> shippingMethoddropdown(ShippingMethodViewModel data)
        {
            try
            {
                //GetConfig
                var result = utils.SendDataApi<List<ShippingMethodViewModel>>(new AppSettingConfig().GetUrl("dropdownShippingMethod"), new { }.sJson());

                return result; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region shippingTermsdropdown
        public List<ShippingTermsViewModel> shippingTermsdropdown(ShippingTermsViewModel data)
        {
            try
            {
                //GetConfig
                var result = utils.SendDataApi<List<ShippingTermsViewModel>>(new AppSettingConfig().GetUrl("dropdownShippingTerms"), new { }.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region paymentTypedropdown
        public List<PaymentTypeViewModel> paymentTypedropdown(PaymentTypeViewModel data)
        {
            try
            {
                //GetConfig
                var result = utils.SendDataApi<List<PaymentTypeViewModel>>(new AppSettingConfig().GetUrl("dropdownpaymentType"), new { }.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ReportPO
        public string printReportPO(ReportPOViewModel data, string rootPath = "")
        {

            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var queryPlanGI = db.View_PrintOutPlanGI.AsQueryable();
                if(data.planGoodsIssue_Index != null)
                {
                    queryPlanGI = queryPlanGI.Where(c => c.PlanGoodsIssue_Index == data.planGoodsIssue_Index);
                }
                
                var result = new List<ReportPOViewModel>();

                var OwnerModel = new OwnerViewModel();
                var resultOwner = new List<OwnerViewModel>();

                var ownerAddress = "";
                var ownerTaxID = "";
                var ownerContactTel = "";
                var ownerFax = "";
                var shipToAddress = "";
                var shipToFax = "";
                var rowItem = 1;
                resultOwner = utils.SendDataApi<List<OwnerViewModel>>(new AppSettingConfig().GetUrl("dropdownOwner"), OwnerModel.sJson());
                if (resultOwner.Count > 0 && resultOwner != null)
                {
                    var DataOwner = resultOwner.Find(c => c.owner_Index == data.owner_Index);
                    ownerAddress = (queryPlanGI.FirstOrDefault().ShipTo_Id.ToUpper() == "D2" ? DataOwner.ref_No2 : DataOwner.owner_Id) + " " + DataOwner.owner_Name + " " + DataOwner.owner_Address;
                    ownerTaxID = DataOwner.owner_TaxID;
                    ownerContactTel = DataOwner.contact_Tel;
                    ownerFax = DataOwner.owner_Fax;
                }

                var ShipToModel = new ShipToViewModel();
                var resultShipTo = new List<ShipToViewModel>();
                resultShipTo = utils.SendDataApi<List<ShipToViewModel>>(new AppSettingConfig().GetUrl("dropdownShipTo"), ShipToModel.sJson());
                if (resultShipTo.Count > 0 && resultShipTo != null)
                {
                    var DataShipTo = resultShipTo.Find(c => c.shipTo_Index == data.shipTo_Index);
                    shipToAddress = DataShipTo.shipTo_Id + " " + DataShipTo.shipTo_Name;
                    shipToFax = DataShipTo.shipTo_Fax;

                }


                long DateTicks = DateTime.Now.Ticks;
                int day = new DateTime(DateTicks).Day;
                int month = new DateTime(DateTicks).Month;
                int year = new DateTime(DateTicks).Year;
                var time = DateTime.Now.ToString("HH:mm");


                var thaiMonth = "";
                switch (month)
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


                var query = queryPlanGI.OrderBy(o => o.LineNum.sParse<int>()).ThenBy(t => t.Product_Id).ToList();


                if (query.Count == 0)
                {
                    var resultItem = new ReportPOViewModel();
                                
                    resultItem.checkQuery = true;


                    result.Add(resultItem);
                }
                else
                {
                    foreach (var item in query)
                    {
                        var resultItem = new ReportPOViewModel();

                        string Pdate = item.PlanGoodsIssue_Date.toString();
                        string PlanGIDate = DateTime.ParseExact(Pdate.Substring(0, 8), "yyyyMMdd",
                        System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                        string PDuedate = item.PlanGoodsIssue_Due_Date.toString();
                        string PlanGIDueDate = DateTime.ParseExact(PDuedate.Substring(0, 8), "yyyyMMdd",
                        System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                        resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;
                        resultItem.planGoodsIssue_Date = PlanGIDate;
                        resultItem.planGoodsIssue_Due_Date = PlanGIDueDate;
                        resultItem.owner_Index = item.Owner_Index;
                        resultItem.owner_Id = item.Owner_Id;
                        resultItem.owner_Name = item.Owner_Name;
                        resultItem.owner_Address = ownerAddress;

                        resultItem.owner_TaxID = ownerTaxID;
                        resultItem.contact_Tel = ownerContactTel;
                        resultItem.owner_Fax = ownerFax;
                        resultItem.countRow = rowItem;
                        //resultItem.date_Print = "ข้อมูล ณ วันที่ " + day.ToString() + " " + thaiMonth + " " + year.ToString() + " เวลา " + time + " น.";
                        resultItem.date_Print = DateTime.Now.ToString("dd/MM/yyyy", culture); 
                        resultItem.shipTo_Index = item.ShipTo_Index;
                        resultItem.shipTo_Id = item.ShipTo_Id;
                        resultItem.shipTo_Name = item.ShipTo_Name;
                        resultItem.shipTo_Address = shipToAddress;

                        resultItem.shipTo_TaxNo = item.ShipTo_TaxNo;
                        resultItem.shipTo_Telephone = item.ShipTo_Telephone;
                        resultItem.shipTo_Fax = shipToFax;
                        resultItem.lineNum = item.LineNum;
                        resultItem.product_Index = item.Product_Index;
                        resultItem.product_Id = queryPlanGI.FirstOrDefault().ShipTo_Id.ToUpper() == "D2" ? utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("getProductMaster"), new { product_Index = item.Product_Index }.sJson()).FirstOrDefault().Ref_No2 : item.Product_Id;
                        resultItem.product_Name = item.Product_Name;
                        resultItem.qty = item.Qty;
                        resultItem.productConversion_Name = item.ProductConversion_Name;
                        resultItem.warehouse_Id = item.Warehouse_Id;
                        resultItem.warehouse_Name = item.Warehouse_Name;
                        resultItem.planGoodsIssue_Barcode = new NetBarcode.Barcode(item.PlanGoodsIssue_No, NetBarcode.Type.Code128B).GetBase64Image();

                        rowItem = rowItem + 1;
                        result.Add(resultItem);
                    }
                    result.ToList();
                }
                rootPath = rootPath.Replace("\\PlanGIAPI", "");
                //var reportPath = rootPath + "\\PlanGIBusiness\\Reports\\ReportPO\\ReportPO.rdlc";
                //var reportPath = rootPath + "\\Reports\\ReportPO\\ReportPO.rdlc";
                var reportPath = rootPath + new AppSettingConfig().GetUrl("ReportPO");
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

        #region ReportPO
        public string printReportBilling(ReportPOViewModel data, string rootPath = "")
        {
            try
            {
                List<im_PlanGoodsIssue> partbilling = db.im_PlanGoodsIssue.Where(c => c.PlanGoodsIssue_Index == data.planGoodsIssue_Index).ToList();
                Utils objReport = new Utils();
                return partbilling[0].DocumentRef_No6.Replace('"',' ').Trim();
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
        #endregion

        #region autoCostCenterFullfilter
        public List<ItemListViewModel> autoCostCenterFullfilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();


                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoCostCenterFullfilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region printProductReturnService
        public string printProductReturnService(ReportPlanGoodsIssueViewModel data, string rootPath = "")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                var result = new List<ProductReturnModel>();
                var PlanGoodsIssue_Index = new SqlParameter("@PlanGoodsIssue_Index", data.planGoodsIssue_Index);
                var rpt_data = db.View_RPT_Product_Return.FromSql("sp_RPT_Product_Return @PlanGoodsIssue_Index", PlanGoodsIssue_Index).ToList();
                foreach (var item in rpt_data)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var list = new ProductReturnModel();
                        var copy_int = "";
                        var for_report = "";
                        var for_report_int = "";
                        switch (i)
                        {

                            case 0:
                                copy_int = "ต้นฉบับ";
                                for_report_int = "1";
                                for_report = "สำหรับลูกค้า For Customer";
                                break;
                            case 1:
                                copy_int = "สำเนา";
                                for_report_int = "2";
                                for_report = "สำหรับขนส่งและคลัง For DC";
                                break;
                        }

                        string roundend = (!string.IsNullOrEmpty(item.Round_Name)) ? DateTime.Parse(item.Round_Name.Replace(".", ":")).AddHours(2).ToString("HH:mm") : "";
                        list.planGoods_Barcode = new NetBarcode.Barcode(item.PlanGoodsIssue_No, NetBarcode.Type.Code128B).GetBase64Image();
                        list.copy_int = copy_int;//i == 0 ? "ต้นฉบับ" : "สำเนา";
                        list.for_report_int = for_report_int; // i == 0 ? "1" : "2";
                        list.for_report = for_report; //i == 0 ? "สำหรับลูกค้า For Customer" : "สำหรับขนส่งและคลัง For DC";
                        list.PlanGoodsIssue_No = item.PlanGoodsIssue_No;
                        list.Branch = item.Branch;
                        list.ShipTo_Name = item.ShipTo_Name;
                        list.Shipto_Id = item.Shipto_Id;
                        list.Shipto_address = item.Shipto_address;
                        list.Round_Name = (!string.IsNullOrEmpty(item.Round_Name)) ? item.Round_Name + " - " + roundend + " น." : "";
                        list.Product_Return_No = item.Product_Return_No;
                        list.Claim_No = item.Claim_No;
                        list.Ref_Billing_No = item.Ref_Billing_No;
                        list.PlanGoodsIssue_Date = item.PlanGoodsIssue_Date.Value.ToString("dd/MM/yyy");
                        list.Row_Runing_product = item.Row_Runing_product;
                        list.Product_Id = item.Product_Id;
                        list.Product_Name = item.Product_Name;
                        list.ProductConversion_Name = item.ProductConversion_Name;
                        list.QtyBackOrder = item.QtyBackOrder;
                        list.Product_Lot = item.Product_Lot;
                        list.DocumentItem_Remark = item.DocumentItem_Remark;
                        list.Return_remark = item.Return_remark;
                        list.Shipping_Remark = item.Shipping_Remark;
                        result.Add(list);

                        
                    }
                }
                if (rpt_data.Count < 10)
                {
                    int countAdd = 10 - rpt_data.Count;
                    for (int j = 0; j < countAdd; j++)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            var listblank = new ProductReturnModel();
                            var copy_int = "";
                            var for_report = "";
                            var for_report_int = "";
                            switch (k)
                            {
                                case 0:
                                    copy_int = "ต้นฉบับ";
                                    for_report_int = "1";
                                    for_report = "สำหรับลูกค้า For Customer";
                                    break;
                                case 1:
                                    copy_int = "สำเนา";
                                    for_report_int = "2";
                                    for_report = "สำหรับขนส่งและคลัง For DC";
                                    break;
                            }
                            listblank.copy_int = copy_int;//i == 0 ? "ต้นฉบับ" : "สำเนา";
                            listblank.for_report_int = for_report_int; // i == 0 ? "1" : "2";
                            listblank.for_report = for_report; //i == 0 ? "สำหรับลูกค้า For Customer" : "สำหรับขนส่งและคลัง For DC";
                            listblank.planGoods_Barcode = "";
                            listblank.PlanGoodsIssue_No = rpt_data.FirstOrDefault().PlanGoodsIssue_No; // "";
                            listblank.Branch = "";
                            listblank.ShipTo_Name = "";
                            listblank.Shipto_Id = "";
                            listblank.Shipto_address = "";
                            listblank.Round_Name = "";
                            listblank.Product_Return_No = "";
                            listblank.Claim_No = "";
                            listblank.Ref_Billing_No = "";
                            listblank.PlanGoodsIssue_Date = "";
                            listblank.Row_Runing_product = null;
                            listblank.Product_Id = "";
                            listblank.Product_Name = "";
                            listblank.ProductConversion_Name = "";
                            listblank.QtyBackOrder = null;
                            listblank.Product_Lot = "";
                            listblank.DocumentItem_Remark = "";
                            listblank.Shipping_Remark = "";
                            result.Add(listblank);
                        }
                    }
                }



                rootPath = rootPath.Replace("\\PlanGIAPI", "");
                var reportPath = rootPath + new AppSettingConfig().GetUrl("ProductReturn");
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
    }
}