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
using PlanGIBusiness.Reports.Pack;
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
using static planGIBusiness.PlanGoodsIssue.PopupGIViewModel;
using static planGIBusiness.PlanGoodsIssue.CheckStockModel;
using static PlanGIBusiness.PlanGoodIssue.PlanGoodDocIssueViewModel;

namespace PlanGIBusiness.PlanGoodIssue
{
    public class CheckStockService
    {

        private PlanGIDbContext db;

        public CheckStockService()
        {
            db = new PlanGIDbContext();
        }

        public CheckStockService(PlanGIDbContext db)
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


        public actionResultViewModel filter(CheckStockModel model)
        {
            try
            {
                var listDataLocation = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("locationFilter"), new { }.sJson());

                List<Guid> location_type = new List<Guid>
                {
                    Guid.Parse("14C5F85D-137D-470E-8C70-C1E535005DC3"),
                    Guid.Parse("2E9338D3-0931-4E36-B240-782BF9508641"),
                    Guid.Parse("65A2D25D-5520-47D3-8776-AE064D909285"),
                    Guid.Parse("94D86CEA-3D04-4304-9E97-28E954F03C35"),
                    Guid.Parse("64341969-E596-4B8B-8836-395061777490"),
                    Guid.Parse("6A1FB140-CC78-4C2B-BEC8-42B2D0AE62E9"),
                    Guid.Parse("F9EDDAEC-A893-4F63-A700-526C69CC0774"),
                    Guid.Parse("A1F7BFA0-1429-4010-863D-6A0EB01DB61D"),
                    Guid.Parse("472E5117-3A7A-4C23-B8C2-7FEA55B3E69C"),
                    Guid.Parse("7D30298A-8BA0-47ED-8342-E3F953E11D8C"),
                    Guid.Parse("A706D789-F5C9-41A6-BEC7-E57034DFC166"),
                    Guid.Parse("E4310B71-D6A7-4FF6-B4A8-EACBDFADAFFC"),
                    Guid.Parse("D4DFC92C-C5DC-4397-BF87-FEEEB579C0AF"),
                    Guid.Parse("3a7d807a-9f2c-4215-8703-f51846bcc4bd"),
                    Guid.Parse("8A545442-77A3-43A4-939A-6B9102DFE8C6"),
                    Guid.Parse("1D2DF268-F004-4820-831F-B823FF9C7564")
                    //Guid.Parse("7F3E1BC2-F18B-4B16-80A9-2394EB8BBE63"),
                    //Guid.Parse("02F5CBFC-769A-411B-9146-1D27F92AE82D")
                };

                listDataLocation = listDataLocation.Where(c => !location_type.Contains(c.locationType_Index.Value)).ToList();

                IQueryable<im_PlanGoodsIssue> planGI = db.im_PlanGoodsIssue.Where(c => c.Document_Status == 0);

                if (!string.IsNullOrEmpty(model.planGoodsIssue_Date) && !string.IsNullOrEmpty(model.planGoodsIssue_Date_To))
                {
                    betweenDate dateStart = model.planGoodsIssue_Date.toBetweenDate();
                    betweenDate dateEnd = model.planGoodsIssue_Date_To.toBetweenDate();
                    planGI = planGI.Where(c => c.PlanGoodsIssue_Due_Date >= dateStart.start && c.PlanGoodsIssue_Due_Date <= dateEnd.end);
                }
                else if (!string.IsNullOrEmpty(model.planGoodsIssue_Date))
                {
                    betweenDate planGoodsIssue_date_From = model.planGoodsIssue_Date.toBetweenDate();
                    planGI = planGI.Where(c => c.PlanGoodsIssue_Date >= planGoodsIssue_date_From.start);
                }
                else if (!string.IsNullOrEmpty(model.planGoodsIssue_Date_To))
                {
                    betweenDate planGoodsIssue_date_To = model.planGoodsIssue_Date_To.toBetweenDate();
                    planGI = planGI.Where(c => c.PlanGoodsIssue_Due_Date <= planGoodsIssue_date_To.start);
                }
                if (!string.IsNullOrEmpty(model.round_Id))
                {
                    planGI = planGI.Where(c => c.Round_Id == model.round_Id);
                }

                List<Guid> planGI_index = planGI.GroupBy(c => c.PlanGoodsIssue_Index).Select(s => s.Key).ToList();

                var planGIitem = db.im_PlanGoodsIssueItem.Where(c => planGI_index.Contains(c.PlanGoodsIssue_Index))
                    .GroupBy(s => new { s.Product_Id, s.Product_Name, s.ItemStatus_Index, Product_Lot = s.Product_Lot == null ? "" : s.Product_Lot, s.ERP_Location })
                    .Select(c => new { c.Key.Product_Id, c.Key.Product_Name, c.Key.ItemStatus_Index, c.Key.Product_Lot, c.Key.ERP_Location, Plan_QTY = c.Sum(k => k.TotalQty) }).ToList();

                var ListBin = new List<CheckStockModel>();
                var ListPlanGI = new List<CheckStockModel>();
                foreach (var item in planGIitem)
                {
                    CheckStockModel checkmodel = new CheckStockModel
                    {
                        Product_Id = item.Product_Id,
                        Product_Name = item.Product_Name.ToUpper(),
                        ItemStatus_Index = item.ItemStatus_Index,
                        //Product_Lot = item.Product_Lot,
                        erp_Location = item.ERP_Location,
                        Plan_QTY = item.Plan_QTY
                    };

                    var BinbalanceViewModel = new CheckStockModel();
                    var planGi_model = item.sJson();
                    BinbalanceViewModel = utils.SendDataApi<CheckStockModel>(new AppSettingConfig().GetUrl("CheckStock"), planGi_model);
                    if (listDataLocation.Select(c=> c.location_Index).Contains(BinbalanceViewModel.location_Index))
                    {
                        ListBin.Add(BinbalanceViewModel);
                    }
                    ListPlanGI.Add(checkmodel);
                }

                var a = ListPlanGI.Select(
                     c => new
                     {
                         c.Product_Id,
                         c.Product_Name,
                         c.ItemStatus_Index,
                         //c.Product_Lot,
                         c.erp_Location,
                         Plan_QTY = Convert.ToDecimal(c.Plan_QTY),
                         binBalance_QtyBal = Convert.ToDecimal(0),
                         binBalance_QtyReserve = Convert.ToDecimal(0),
                         binBalance_QtyBegin = Convert.ToDecimal(0),
                         remark = ""
                     }
                    ).Distinct();

                var b = ListBin.Select(
                         c => new
                         {
                             c.Product_Id,
                             c.Product_Name,
                             c.ItemStatus_Index,
                             //c.Product_Lot,
                             c.erp_Location,
                             Plan_QTY = Convert.ToDecimal(0),
                             binBalance_QtyBal = Convert.ToDecimal(c.binBalance_QtyBal),
                             binBalance_QtyReserve = Convert.ToDecimal(c.binBalance_QtyReserve),
                             binBalance_QtyBegin = Convert.ToDecimal(c.binBalance_QtyBegin),
                             c.remark
                         }
                        ).Distinct();


                var cccc = a.Union(b).ToList();

                var gg = cccc.GroupBy(c => new
                {
                    c.Product_Id,
                    c.Product_Name,
                    c.ItemStatus_Index,
                    //c.Product_Lot,
                    c.erp_Location
                }).Select(c => new
                {
                    c.Key.Product_Id,
                    c.Key.Product_Name,
                    c.Key.ItemStatus_Index,
                    //c.Key.Product_Lot,
                    c.Key.erp_Location,
                    Plan_QTY = c.Sum(s => s.Plan_QTY),
                    binBalance_QtyBal = c.Sum(s => s.binBalance_QtyBal),
                    binBalance_QtyReserve = c.Sum(s => s.binBalance_QtyReserve),
                    binBalance_QtyBegin = c.Sum(s => s.binBalance_QtyBegin),
                    remark = string.Join("", c.Select(i => i.remark))

                }).ToList();

            //    _context.Log.GroupBy(l => new { l.UserId, l.dates.Date, l.Deptname })
            //.Select(g => new { g.Key.UserId, g.Key.Date, g.Key.Deptname, Log = string.Join(",", g.Select(i => i.times)) });

                var listmodel = ListPlanGI.Union(ListBin);

                List<int?> statusModels = new List<int?>();

                List<im_PlanGoodsIssue> Item = new List<im_PlanGoodsIssue>();
                List<im_PlanGoodsIssue> TotalRow = new List<im_PlanGoodsIssue>();

                if (model.CurrentPage != 0 && model.PerPage != 0)
                {
                    listmodel = listmodel.Skip(((model.CurrentPage - 1) * model.PerPage));
                }

                if (model.PerPage != 0)
                {
                    listmodel = listmodel.Take(model.PerPage);
                }

                var result = new List<CheckStockModel>();

                foreach (var item in gg)
                {
                    if (item.Product_Id == null && item.Product_Name == null) { continue; }
                    var resultItem = new CheckStockModel
                    {
                        Product_Id = item.Product_Id,
                        Product_Name = item.Product_Name,
                        //Product_Lot = item.Product_Lot,
                        erp_Location = item.erp_Location,
                        Plan_QTY = item.Plan_QTY,
                        binBalance_QtyBal = item.binBalance_QtyBal,
                        binBalance_QtyReserve = item.binBalance_QtyReserve,
                        binBalance_QtyBegin = item.binBalance_QtyBegin,
                        Qty_Ava = item.binBalance_QtyBegin - item.Plan_QTY,
                        remark = item.remark
                    };

                    result.Add(resultItem);
                }
                var count = TotalRow.Count;

                var actionResult = new actionResultViewModel();
                actionResult.CheckStockModel = result;
                actionResult.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage, };

                return actionResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CheckStockModel> getSctock_filter(CheckStockModel model) {
            try
            {
                IQueryable<im_PlanGoodsIssue> planGI = db.im_PlanGoodsIssue.Where(c => c.Document_Status == 0);

                if (!string.IsNullOrEmpty(model.planGoodsIssue_Date) && !string.IsNullOrEmpty(model.planGoodsIssue_Date_To))
                {
                    betweenDate dateStart = model.planGoodsIssue_Date.toBetweenDate();
                    betweenDate dateEnd = model.planGoodsIssue_Date_To.toBetweenDate();
                    planGI = planGI.Where(c => c.PlanGoodsIssue_Due_Date >= dateStart.start && c.PlanGoodsIssue_Due_Date <= dateEnd.end);
                }
                else if (!string.IsNullOrEmpty(model.planGoodsIssue_Date))
                {
                    betweenDate planGoodsIssue_date_From = model.planGoodsIssue_Date.toBetweenDate();
                    planGI = planGI.Where(c => c.PlanGoodsIssue_Date >= planGoodsIssue_date_From.start);
                }
                else if (!string.IsNullOrEmpty(model.planGoodsIssue_Date_To))
                {
                    betweenDate planGoodsIssue_date_To = model.planGoodsIssue_Date_To.toBetweenDate();
                    planGI = planGI.Where(c => c.PlanGoodsIssue_Due_Date <= planGoodsIssue_date_To.start);
                }
                if (!string.IsNullOrEmpty(model.round_Id))
                {
                    planGI = planGI.Where(c => c.Round_Id == model.round_Id);
                }
                List<Guid> planGI_index = planGI.GroupBy(c => c.PlanGoodsIssue_Index).Select(s => s.Key).ToList();

                var planGIitem = db.im_PlanGoodsIssueItem.Where(c => planGI_index.Contains(c.PlanGoodsIssue_Index)).ToList();
                planGIitem = planGIitem.Where(c => c.Product_Id == model.Product_Id).ToList();
                planGIitem = planGIitem.Where(c => c.ERP_Location == model.erp_Location).ToList();
                //planGIitem = planGIitem.Where(c => (c.Product_Lot ?? "") == model.Product_Lot).ToList();

                var planGI_list = (from plangi in planGI
                                   join planGiitem in planGIitem on plangi.PlanGoodsIssue_Index equals planGiitem.PlanGoodsIssue_Index
                                   select new CheckStockModel
                                   {
                                       planGoodsIssue_Index = plangi.PlanGoodsIssue_Index,
                                       planGoodsIssue_No = plangi.PlanGoodsIssue_No,
                                       round_Id = plangi.Round_Id,
                                       Plan_QTY = planGiitem.TotalQty,
                                       DocumentPriority_Status = plangi.DocumentPriority_Status
                                   }).GroupBy(c => new
                                   {
                                       c.planGoodsIssue_Index,
                                       c.planGoodsIssue_No,
                                       c.round_Id,
                                       c.Plan_QTY,
                                       c.DocumentPriority_Status

                                   }).Select(c => new
                                   {
                                       c.Key.planGoodsIssue_Index,
                                       c.Key.planGoodsIssue_No,
                                       c.Key.round_Id,
                                       c.Key.Plan_QTY,
                                       c.Key.DocumentPriority_Status
                                   }).OrderBy(c=> c.DocumentPriority_Status).ThenBy(c=> c.round_Id).ToList();

                var allqty = model.binBalance_QtyBegin;
                var result = new List<CheckStockModel>();
                foreach (var item in planGI_list)
                {
                    bool isdiff = false;
                    decimal? qtygi = 0;
                    decimal? qtybal = 0;
                    decimal? qty_diff = 0;
                    qtygi = item.Plan_QTY - allqty;
                    allqty = allqty - item.Plan_QTY;
                    if (allqty == 0)
                    {
                        qtygi = 0;

                    }
                    if (qtygi <= 0) {
                        qtybal = item.Plan_QTY;
                        qty_diff = 0;
                    }
                    else {
                        qtybal = 0;
                        qty_diff = qtygi;
                        isdiff = true;
                    }
                    if (allqty < 0 )
                    {
                        allqty = 0;
                    }

                    var resultitem = new CheckStockModel
                    {
                        planGoodsIssue_Index = item.planGoodsIssue_Index,
                        planGoodsIssue_No = item.planGoodsIssue_No,
                        round_Id = item.round_Id,
                        Plan_QTY = item.Plan_QTY,
                        binBalance_QtyBal = qtybal,
                        Qty_Ava = qty_diff,
                        Isdiff = isdiff

                    };
                    result.Add(resultitem);
                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
    }
}