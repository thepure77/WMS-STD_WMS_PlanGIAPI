using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Comone.Utils;
using static planGIBusiness.PlanGoodsIssue.PopupGIViewModel;
using PlanGIDataAccess.Models;
using System.Data;
using Business.Library;
using MasterDataBusiness.ViewModels;

namespace PlanGIBusiness.PlanGoodIssue
{
    public class PlanGoodsIssueItemService
    {


        private PlanGIDbContext db;

        public PlanGoodsIssueItemService()
        {
            db = new PlanGIDbContext();
        }

        public PlanGoodsIssueItemService(PlanGIDbContext db)
        {
            this.db = db;
        }

        #region find
        public List<PlanGoodIssueDocViewModelItem> find(Guid id)
        {

            try
            {
                var result = new List<PlanGoodIssueDocViewModelItem>();

                var queryResult = db.im_PlanGoodsIssueItem.Where(c => c.PlanGoodsIssue_Index == id && c.Document_Status != -1).ToList();

                


                foreach (var item in queryResult.OrderBy(c => c.LineNum.sParse<int>()))
                {

                    //service cross service
                    var Findproduct = new List<ProductViewModel>();

                    var filterModel = new ProductViewModel();

                    filterModel.product_Index = item.Product_Index;

                    //GetConfig
                    Findproduct = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("Product"), filterModel.sJson());
                   

                    var resultItem = new PlanGoodIssueDocViewModelItem();

                    resultItem.planGoodsIssueItem_Index = item.PlanGoodsIssueItem_Index;
                    resultItem.planGoodsIssue_Index = item.PlanGoodsIssue_Index;
                    resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;
                    resultItem.lineNum = item.LineNum;
                    resultItem.product_Index = item.Product_Index;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.product_SecondName = item.Product_SecondName;
                    resultItem.product_ThirdName = item.Product_ThirdName;
                    resultItem.product_Lot = item.Product_Lot;
                    resultItem.itemStatus_Index = item.ItemStatus_Index;
                    resultItem.itemStatus_Id = item.ItemStatus_Id;
                    resultItem.itemStatus_Name = item.ItemStatus_Name;
                    resultItem.qty = string.Format(String.Format("{0:N0}", item.Qty));
                    resultItem.ratio = item.Ratio;
                    resultItem.totalQty = item.TotalQty;
                    resultItem.productConversion_Index = item.ProductConversion_Index;
                    resultItem.productConversion_Id = item.ProductConversion_Id;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.productConversion_Bu_Name = Findproduct.FirstOrDefault().productConversion_Name; // bu
                    resultItem.mfg_Date = item.MFG_Date.toString();
                    resultItem.exp_Date = item.EXP_Date.toString();
                    resultItem.unitWeight = item.UnitWeight;
                    resultItem.unitWeight_Index = item.UnitWeight_Index;
                    resultItem.unitWeight_Id = item.UnitWeight_Id;
                    resultItem.unitWeight_Name = item.UnitWeight_Name;
                    resultItem.unitWeightRatio = item.UnitWeightRatio;
                    resultItem.weight = item.Weight;
                    resultItem.weight_Index = item.Weight_Index;
                    resultItem.weight_Id = item.Weight_Id;
                    resultItem.weight_Name = item.Weight_Name;
                    resultItem.weightRatio = item.WeightRatio;
                    resultItem.unitNetWeight = item.UnitNetWeight;
                    resultItem.unitNetWeight_Index = item.UnitNetWeight_Index;
                    resultItem.unitNetWeight_Id = item.UnitNetWeight_Id;
                    resultItem.unitNetWeight_Name = item.UnitNetWeight_Name;
                    resultItem.unitNetWeightRatio = item.UnitNetWeightRatio;
                    resultItem.netWeight = item.NetWeight;
                    resultItem.netWeight_Index = item.NetWeight_Index;
                    resultItem.netWeight_Id = item.NetWeight_Id;
                    resultItem.netWeight_Name = item.NetWeight_Name;
                    resultItem.netWeightRatio = item.NetWeightRatio;
                    resultItem.unitGrsWeight = item.UnitGrsWeight;
                    resultItem.unitGrsWeight_Index = item.UnitGrsWeight_Index;
                    resultItem.unitGrsWeight_Id = item.UnitGrsWeight_Id;
                    resultItem.unitGrsWeight_Name = item.UnitGrsWeight_Name;
                    resultItem.unitGrsWeightRatio = item.UnitGrsWeightRatio;
                    resultItem.grsWeight = item.GrsWeight;
                    resultItem.grsWeight_Index = item.GrsWeight_Index;
                    resultItem.grsWeight_Id = item.GrsWeight_Id;
                    resultItem.grsWeight_Name = item.GrsWeight_Name;
                    resultItem.grsWeightRatio = item.GrsWeightRatio;
                    resultItem.unitWidth = item.UnitWidth;
                    resultItem.unitWidth_Index = item.UnitWidth_Index;
                    resultItem.unitWidth_Id = item.UnitWidth_Id;
                    resultItem.unitWidth_Name = item.UnitWidth_Name;
                    resultItem.unitWidthRatio = item.UnitWidthRatio;
                    resultItem.width = item.Width;
                    resultItem.width_Index = item.Width_Index;
                    resultItem.width_Id = item.Width_Id;
                    resultItem.width_Name = item.Width_Name;
                    resultItem.widthRatio = item.WidthRatio;
                    resultItem.unitLength = item.UnitLength;
                    resultItem.unitLength_Index = item.UnitLength_Index;
                    resultItem.unitLength_Id = item.UnitLength_Id;
                    resultItem.unitLength_Name = item.UnitLength_Name;
                    resultItem.unitLengthRatio = item.UnitLengthRatio;
                    resultItem.length = item.Length;
                    resultItem.length_Index = item.Length_Index;
                    resultItem.length_Id = item.Length_Id;
                    resultItem.length_Name = item.Length_Name;
                    resultItem.lengthRatio = item.LengthRatio;
                    resultItem.unitHeight = item.UnitHeight;
                    resultItem.unitHeight_Index = item.UnitHeight_Index;
                    resultItem.unitHeight_Id = item.UnitHeight_Id;
                    resultItem.unitHeight_Name = item.UnitHeight_Name;
                    resultItem.unitHeightRatio = item.UnitHeightRatio;
                    resultItem.height = item.Height;
                    resultItem.height_Index = item.Height_Index;
                    resultItem.height_Id = item.Height_Id;
                    resultItem.height_Name = item.Height_Name;
                    resultItem.heightRatio = item.HeightRatio;
                    resultItem.unitVolume = item.UnitVolume;
                    resultItem.volume = item.Volume;
                    resultItem.unitPrice = item.UnitPrice;
                    resultItem.unitPrice_Index = item.UnitPrice_Index;
                    resultItem.unitPrice_Id = item.UnitPrice_Id;
                    resultItem.unitPrice_Name = item.UnitPrice_Name;
                    resultItem.price = item.Price;
                    resultItem.price_Index = item.Price_Index;
                    resultItem.price_Id = item.Price_Id;
                    resultItem.price_Name = item.Price_Name;
                    resultItem.documentRef_No1 = item.DocumentRef_No1;
                    resultItem.documentRef_No2 = item.DocumentRef_No2;
                    resultItem.documentRef_No3 = item.DocumentRef_No3;
                    resultItem.documentRef_No4 = item.DocumentRef_No4;
                    resultItem.documentRef_No5 = item.DocumentRef_No5;
                    resultItem.document_Status = item.Document_Status;
                    resultItem.documentItem_Remark = item.DocumentItem_Remark;
                    resultItem.uDF_1 = item.UDF_1;
                    resultItem.uDF_2 = item.UDF_2;
                    resultItem.uDF_3 = item.UDF_3;
                    resultItem.uDF_4 = item.UDF_4;
                    resultItem.uDF_5 = item.UDF_5;
                    resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;
                    resultItem.erp_Location = item.ERP_Location;

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

        #region find_with_wave
        public List<PlanGoodIssueDocViewModelItem> find_with_wave(Guid id,string status)
        {

            try
            {
                var result = new List<PlanGoodIssueDocViewModelItem>();

                var queryResult = db.View_PlanGIWithWave.Where(c => c.PlanGoodsIssue_Index == id && c.Document_Status != -1).ToList();
                if (status == "3")
                {
                    queryResult = queryResult.Where(c => c.GI_TotalQty > 0).ToList();
                }
                else if (status == "2") {
                    queryResult = queryResult.Where(c => c.Remain_TotalQty > 0).ToList();
                }

                foreach (var item in queryResult.OrderBy(c => c.LineNum.sParse<int>()))
                {
                    var resultItem = new PlanGoodIssueDocViewModelItem();

                    resultItem.planGoodsIssueItem_Index = item.PlanGoodsIssueItem_Index;
                    resultItem.planGoodsIssue_Index = item.PlanGoodsIssue_Index;
                    resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;
                    resultItem.lineNum = item.LineNum;
                    resultItem.product_Index = item.Product_Index;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.product_SecondName = item.Product_SecondName;
                    resultItem.product_ThirdName = item.Product_ThirdName;
                    resultItem.product_Lot = item.Product_Lot;
                    resultItem.itemStatus_Index = item.ItemStatus_Index;
                    resultItem.itemStatus_Id = item.ItemStatus_Id;
                    resultItem.itemStatus_Name = item.ItemStatus_Name;
                    resultItem.qty = string.Format(String.Format("{0:N0}", item.Qty));
                    resultItem.ratio = item.Ratio;
                    resultItem.totalQty = item.TotalQty;
                    resultItem.productConversion_Index = item.ProductConversion_Index;
                    resultItem.productConversion_Id = item.ProductConversion_Id;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.mfg_Date = item.MFG_Date.toString();
                    resultItem.exp_Date = item.EXP_Date.toString();
                    resultItem.unitWeight = item.UnitWeight;
                    resultItem.unitWeight_Index = item.UnitWeight_Index;
                    resultItem.unitWeight_Id = item.UnitWeight_Id;
                    resultItem.unitWeight_Name = item.UnitWeight_Name;
                    resultItem.unitWeightRatio = item.UnitWeightRatio;
                    resultItem.weight = item.Weight;
                    resultItem.weight_Index = item.Weight_Index;
                    resultItem.weight_Id = item.Weight_Id;
                    resultItem.weight_Name = item.Weight_Name;
                    resultItem.weightRatio = item.WeightRatio;
                    resultItem.unitNetWeight = item.UnitNetWeight;
                    resultItem.unitNetWeight_Index = item.UnitNetWeight_Index;
                    resultItem.unitNetWeight_Id = item.UnitNetWeight_Id;
                    resultItem.unitNetWeight_Name = item.UnitNetWeight_Name;
                    resultItem.unitNetWeightRatio = item.UnitNetWeightRatio;
                    resultItem.netWeight = item.NetWeight;
                    resultItem.netWeight_Index = item.NetWeight_Index;
                    resultItem.netWeight_Id = item.NetWeight_Id;
                    resultItem.netWeight_Name = item.NetWeight_Name;
                    resultItem.netWeightRatio = item.NetWeightRatio;
                    resultItem.unitGrsWeight = item.UnitGrsWeight;
                    resultItem.unitGrsWeight_Index = item.UnitGrsWeight_Index;
                    resultItem.unitGrsWeight_Id = item.UnitGrsWeight_Id;
                    resultItem.unitGrsWeight_Name = item.UnitGrsWeight_Name;
                    resultItem.unitGrsWeightRatio = item.UnitGrsWeightRatio;
                    resultItem.grsWeight = item.GrsWeight;
                    resultItem.grsWeight_Index = item.GrsWeight_Index;
                    resultItem.grsWeight_Id = item.GrsWeight_Id;
                    resultItem.grsWeight_Name = item.GrsWeight_Name;
                    resultItem.grsWeightRatio = item.GrsWeightRatio;
                    resultItem.unitWidth = item.UnitWidth;
                    resultItem.unitWidth_Index = item.UnitWidth_Index;
                    resultItem.unitWidth_Id = item.UnitWidth_Id;
                    resultItem.unitWidth_Name = item.UnitWidth_Name;
                    resultItem.unitWidthRatio = item.UnitWidthRatio;
                    resultItem.width = item.Width;
                    resultItem.width_Index = item.Width_Index;
                    resultItem.width_Id = item.Width_Id;
                    resultItem.width_Name = item.Width_Name;
                    resultItem.widthRatio = item.WidthRatio;
                    resultItem.unitLength = item.UnitLength;
                    resultItem.unitLength_Index = item.UnitLength_Index;
                    resultItem.unitLength_Id = item.UnitLength_Id;
                    resultItem.unitLength_Name = item.UnitLength_Name;
                    resultItem.unitLengthRatio = item.UnitLengthRatio;
                    resultItem.length = item.Length;
                    resultItem.length_Index = item.Length_Index;
                    resultItem.length_Id = item.Length_Id;
                    resultItem.length_Name = item.Length_Name;
                    resultItem.lengthRatio = item.LengthRatio;
                    resultItem.unitHeight = item.UnitHeight;
                    resultItem.unitHeight_Index = item.UnitHeight_Index;
                    resultItem.unitHeight_Id = item.UnitHeight_Id;
                    resultItem.unitHeight_Name = item.UnitHeight_Name;
                    resultItem.unitHeightRatio = item.UnitHeightRatio;
                    resultItem.height = item.Height;
                    resultItem.height_Index = item.Height_Index;
                    resultItem.height_Id = item.Height_Id;
                    resultItem.height_Name = item.Height_Name;
                    resultItem.heightRatio = item.HeightRatio;
                    resultItem.unitVolume = item.UnitVolume;
                    resultItem.volume = item.Volume;
                    resultItem.unitPrice = item.UnitPrice;
                    resultItem.unitPrice_Index = item.UnitPrice_Index;
                    resultItem.unitPrice_Id = item.UnitPrice_Id;
                    resultItem.unitPrice_Name = item.UnitPrice_Name;
                    resultItem.price = item.Price;
                    resultItem.price_Index = item.Price_Index;
                    resultItem.price_Id = item.Price_Id;
                    resultItem.price_Name = item.Price_Name;
                    resultItem.documentRef_No1 = item.DocumentRef_No1;
                    resultItem.documentRef_No2 = item.DocumentRef_No2;
                    resultItem.documentRef_No3 = item.DocumentRef_No3;
                    resultItem.documentRef_No4 = item.DocumentRef_No4;
                    resultItem.documentRef_No5 = item.DocumentRef_No5;
                    resultItem.document_Status = item.Document_Status;
                    resultItem.documentItem_Remark = item.DocumentItem_Remark;
                    resultItem.uDF_1 = item.UDF_1;
                    resultItem.uDF_2 = item.UDF_2;
                    resultItem.uDF_3 = item.UDF_3;
                    resultItem.uDF_4 = item.UDF_4;
                    resultItem.uDF_5 = item.UDF_5;
                    resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;
                    resultItem.gI_Qty = item.GI_Qty;
                    resultItem.gI_TotalQty = item.GI_TotalQty;
                    resultItem.remain_TotalQty = item.Remain_TotalQty;

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

        #region UpdateStatusPGII
        public bool UpdateStatusPGII(DocumentViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                var result = new List<PlanGoodIssueDocViewModelItem>();

                var PGII = new List<im_PlanGoodsIssueItem>();
                if (model.listDocumentViewModel.FirstOrDefault().whereDocument_Status != null)
                {
                    //PGII = db.im_PlanGoodsIssueItem.Where(c => model.listDocumentViewModel.Select(s => s.documentItem_Index).Contains(c.PlanGoodsIssueItem_Index) && c.Document_Status == model.listDocumentViewModel.FirstOrDefault().whereDocument_Status).ToList();
                    PGII = db.im_PlanGoodsIssueItem.Where(c => c.PlanGoodsIssueItem_Index == model.listDocumentViewModel.FirstOrDefault().documentItem_Index && c.Document_Status == model.listDocumentViewModel.FirstOrDefault().whereDocument_Status).ToList();
                }
                else
                {
                    PGII = db.im_PlanGoodsIssueItem.Where(c => model.listDocumentViewModel.Select(s => s.documentItem_Index).Contains(c.PlanGoodsIssueItem_Index)).ToList();
                }

                foreach (var p in PGII)
                {
                    p.Document_Status = model.listDocumentViewModel.FirstOrDefault().document_Status;
                }

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("UpdateStatusPGII", msglog);
                    transaction.Rollback();
                    throw exy;
                }

                return true;
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("UpdateStatusPGII", msglog);
                return false;
            }
        }
        #endregion

        #region im_PlanGoodsIssueItem
        public List<PlanGoodIssueViewModelItem> im_PlanGoodsIssueItem(DocumentViewModel model)
        {
            try
            {

                var query = db.im_PlanGoodsIssueItem.AsQueryable();

                var result = new List<PlanGoodIssueViewModelItem>();

                if (model.listDocumentViewModel.FirstOrDefault().document_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Index).Contains(c.PlanGoodsIssue_Index));
                }
                if (model.listDocumentViewModel.FirstOrDefault().documentItem_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.documentItem_Index).Contains(c.PlanGoodsIssueItem_Index));
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
                    var resultItem = new PlanGoodIssueViewModelItem();

                    resultItem.planGoodsIssueItem_Index = item.PlanGoodsIssueItem_Index;
                    resultItem.planGoodsIssue_Index = item.PlanGoodsIssue_Index;
                    resultItem.lineNum = item.LineNum;
                    resultItem.product_Index = item.Product_Index;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.product_SecondName = item.Product_SecondName;
                    resultItem.product_ThirdName = item.Product_ThirdName;
                    resultItem.product_Lot = item.Product_Lot;
                    resultItem.itemStatus_Index = item.ItemStatus_Index;
                    resultItem.itemStatus_Id = item.ItemStatus_Id;
                    resultItem.itemStatus_Name = item.ItemStatus_Name;
                    resultItem.qty = item.Qty;
                    resultItem.ratio = item.Ratio;
                    resultItem.totalQty = item.TotalQty;
                    resultItem.productConversion_Index = item.ProductConversion_Index;
                    resultItem.productConversion_Id = item.ProductConversion_Id;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.mfg_Date = item.MFG_Date;
                    resultItem.exp_Date = item.EXP_Date;
                    resultItem.unitWeight = item.UnitWeight;
                    resultItem.weight = item.Weight;
                    resultItem.unitWidth = item.UnitWidth;
                    resultItem.unitLength = item.UnitLength;
                    resultItem.unitHeight = item.UnitHeight;
                    resultItem.unitVolume = item.UnitVolume;
                    resultItem.volume = item.Volume;
                    resultItem.unitPrice = item.UnitPrice;
                    resultItem.price = item.Price;
                    resultItem.documentRef_No1 = item.DocumentRef_No1;
                    resultItem.documentRef_No2 = item.DocumentRef_No2;
                    resultItem.documentRef_No3 = item.DocumentRef_No3;
                    resultItem.documentRef_No4 = item.DocumentRef_No4;
                    resultItem.documentRef_No5 = item.DocumentRef_No5;
                    resultItem.documentItem_Remark = item.DocumentItem_Remark;
                    resultItem.document_Status = item.Document_Status;
                    resultItem.udf_1 = item.UDF_1;
                    resultItem.udf_2 = item.UDF_2;
                    resultItem.udf_3 = item.UDF_3;
                    resultItem.udf_4 = item.UDF_4;
                    resultItem.udf_5 = item.UDF_5;
                    resultItem.create_By = item.Create_By;
                    resultItem.create_Date = item.Create_Date;
                    resultItem.update_By = item.Update_By;
                    resultItem.update_Date = item.Update_Date;
                    resultItem.cancel_By = item.Cancel_By;
                    resultItem.cancel_Date = item.Cancel_Date;
                    resultItem.qtyBackOrder = item.QtyBackOrder;
                    resultItem.backOrderStatus = item.BackOrderStatus;
                    resultItem.planGoodsIssue_Size = item.PlanGoodsIssue_Size;
                    resultItem.qtyQA = item.QtyQA;
                    resultItem.isQA = item.IsQA;
                    resultItem.qty_Inner_Pack = item.Qty_Inner_Pack;
                    resultItem.qty_Sup_Pack = item.Qty_Sup_Pack;
                    resultItem.imageUri = item.ImageUri;
                    resultItem.zoneCode = item.ZoneCode;
                    resultItem.batch_Id = item.Batch_Id;
                    resultItem.qa_By = item.QA_By;
                    resultItem.qa_Date = item.QA_Date;
                    resultItem.runWave_Status = item.RunWave_Status;
                    resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;
                    resultItem.countQty = item.CountQty;

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

        #region returnmatdoc
        public List<PlanGoodIssueDocViewModelItem> returnmatdoc(PlanGoodIssueDocViewModelItem data)
        {
            try
            {
                var result = new List<PlanGoodIssueDocViewModelItem>();

                var get_returnMatdoc = db.im_PlanGoodsIssue.FirstOrDefault(c => c.Matdoc == data.matdoc);
                if (get_returnMatdoc == null)
                {
                    return result;
                }
                else {
                    var queryResult = db.im_PlanGoodsIssueItem.Where(c => c.PlanGoodsIssue_Index == get_returnMatdoc.PlanGoodsIssue_Index && c.Document_Status != -1).ToList();
                    foreach (var item in queryResult.OrderBy(c => c.LineNum.sParse<int>()))
                    {
                        var resultItem = new PlanGoodIssueDocViewModelItem();

                        resultItem.planGoodsIssueItem_Index = item.PlanGoodsIssueItem_Index;
                        resultItem.planGoodsIssue_Index = item.PlanGoodsIssue_Index;
                        resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;
                        resultItem.lineNum = item.LineNum;
                        resultItem.product_Index = item.Product_Index;
                        resultItem.product_Id = item.Product_Id;
                        resultItem.product_Name = item.Product_Name;
                        resultItem.product_SecondName = item.Product_SecondName;
                        resultItem.product_ThirdName = item.Product_ThirdName;
                        resultItem.product_Lot = item.Product_Lot;
                        resultItem.itemStatus_Index = item.ItemStatus_Index;
                        resultItem.itemStatus_Id = item.ItemStatus_Id;
                        resultItem.itemStatus_Name = item.ItemStatus_Name;
                        resultItem.qty = string.Format(String.Format("{0:N0}", item.Qty));
                        resultItem.ratio = item.Ratio;
                        resultItem.totalQty = item.TotalQty;
                        resultItem.productConversion_Index = item.ProductConversion_Index;
                        resultItem.productConversion_Id = item.ProductConversion_Id;
                        resultItem.productConversion_Name = item.ProductConversion_Name;
                        resultItem.mfg_Date = item.MFG_Date.toString();
                        resultItem.exp_Date = item.EXP_Date.toString();
                        resultItem.unitWeight = item.UnitWeight;
                        resultItem.unitWeight_Index = item.UnitWeight_Index;
                        resultItem.unitWeight_Id = item.UnitWeight_Id;
                        resultItem.unitWeight_Name = item.UnitWeight_Name;
                        resultItem.unitWeightRatio = item.UnitWeightRatio;
                        resultItem.weight = item.Weight;
                        resultItem.weight_Index = item.Weight_Index;
                        resultItem.weight_Id = item.Weight_Id;
                        resultItem.weight_Name = item.Weight_Name;
                        resultItem.weightRatio = item.WeightRatio;
                        resultItem.unitNetWeight = item.UnitNetWeight;
                        resultItem.unitNetWeight_Index = item.UnitNetWeight_Index;
                        resultItem.unitNetWeight_Id = item.UnitNetWeight_Id;
                        resultItem.unitNetWeight_Name = item.UnitNetWeight_Name;
                        resultItem.unitNetWeightRatio = item.UnitNetWeightRatio;
                        resultItem.netWeight = item.NetWeight;
                        resultItem.netWeight_Index = item.NetWeight_Index;
                        resultItem.netWeight_Id = item.NetWeight_Id;
                        resultItem.netWeight_Name = item.NetWeight_Name;
                        resultItem.netWeightRatio = item.NetWeightRatio;
                        resultItem.unitGrsWeight = item.UnitGrsWeight;
                        resultItem.unitGrsWeight_Index = item.UnitGrsWeight_Index;
                        resultItem.unitGrsWeight_Id = item.UnitGrsWeight_Id;
                        resultItem.unitGrsWeight_Name = item.UnitGrsWeight_Name;
                        resultItem.unitGrsWeightRatio = item.UnitGrsWeightRatio;
                        resultItem.grsWeight = item.GrsWeight;
                        resultItem.grsWeight_Index = item.GrsWeight_Index;
                        resultItem.grsWeight_Id = item.GrsWeight_Id;
                        resultItem.grsWeight_Name = item.GrsWeight_Name;
                        resultItem.grsWeightRatio = item.GrsWeightRatio;
                        resultItem.unitWidth = item.UnitWidth;
                        resultItem.unitWidth_Index = item.UnitWidth_Index;
                        resultItem.unitWidth_Id = item.UnitWidth_Id;
                        resultItem.unitWidth_Name = item.UnitWidth_Name;
                        resultItem.unitWidthRatio = item.UnitWidthRatio;
                        resultItem.width = item.Width;
                        resultItem.width_Index = item.Width_Index;
                        resultItem.width_Id = item.Width_Id;
                        resultItem.width_Name = item.Width_Name;
                        resultItem.widthRatio = item.WidthRatio;
                        resultItem.unitLength = item.UnitLength;
                        resultItem.unitLength_Index = item.UnitLength_Index;
                        resultItem.unitLength_Id = item.UnitLength_Id;
                        resultItem.unitLength_Name = item.UnitLength_Name;
                        resultItem.unitLengthRatio = item.UnitLengthRatio;
                        resultItem.length = item.Length;
                        resultItem.length_Index = item.Length_Index;
                        resultItem.length_Id = item.Length_Id;
                        resultItem.length_Name = item.Length_Name;
                        resultItem.lengthRatio = item.LengthRatio;
                        resultItem.unitHeight = item.UnitHeight;
                        resultItem.unitHeight_Index = item.UnitHeight_Index;
                        resultItem.unitHeight_Id = item.UnitHeight_Id;
                        resultItem.unitHeight_Name = item.UnitHeight_Name;
                        resultItem.unitHeightRatio = item.UnitHeightRatio;
                        resultItem.height = item.Height;
                        resultItem.height_Index = item.Height_Index;
                        resultItem.height_Id = item.Height_Id;
                        resultItem.height_Name = item.Height_Name;
                        resultItem.heightRatio = item.HeightRatio;
                        resultItem.unitVolume = item.UnitVolume;
                        resultItem.volume = item.Volume;
                        resultItem.unitPrice = item.UnitPrice;
                        resultItem.unitPrice_Index = item.UnitPrice_Index;
                        resultItem.unitPrice_Id = item.UnitPrice_Id;
                        resultItem.unitPrice_Name = item.UnitPrice_Name;
                        resultItem.price = item.Price;
                        resultItem.price_Index = item.Price_Index;
                        resultItem.price_Id = item.Price_Id;
                        resultItem.price_Name = item.Price_Name;
                        resultItem.documentRef_No1 = item.DocumentRef_No1;
                        resultItem.documentRef_No2 = item.DocumentRef_No2;
                        resultItem.documentRef_No3 = item.DocumentRef_No3;
                        resultItem.documentRef_No4 = item.DocumentRef_No4;
                        resultItem.documentRef_No5 = item.DocumentRef_No5;
                        resultItem.document_Status = item.Document_Status;
                        resultItem.documentItem_Remark = item.DocumentItem_Remark;
                        resultItem.uDF_1 = item.UDF_1;
                        resultItem.uDF_2 = item.UDF_2;
                        resultItem.uDF_3 = item.UDF_3;
                        resultItem.uDF_4 = item.UDF_4;
                        resultItem.uDF_5 = item.UDF_5;
                        resultItem.planGoodsIssue_No = item.PlanGoodsIssue_No;

                        result.Add(resultItem);

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
    }
}
