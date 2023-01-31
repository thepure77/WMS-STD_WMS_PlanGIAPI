using PlanGIBusiness;
using System;
using System.Collections.Generic;
using System.Text;

namespace planGIBusiness.PlanGoodsIssue
{
    public partial class PopupGIViewModel : Pagination
    {

        public Guid planGoodsIssue_Index { get; set; }
        public Guid owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public string planGoodsIssue_No { get; set; }
        public string planGoodsIssue_Date { get; set; }
        public string planGoodsIssue_Due_Date { get; set; }
        public Guid? warehouse_Index_To { get; set; }
        public string warehouse_Id_To { get; set; }
        public string warehouse_Name_To { get; set; }
        public List<plangoodsissueitemViewModel> itemDetail { get; set; }

        public class actionResultPopupGIViewModel 
        {
            public IList<PopupGIViewModel> items { get; set; }
            public Pagination pagination { get; set; }
            public string msg { get; set; }
            public bool isUse { get; set; }
        }

        public class plangoodsissueitemViewModel
        {
            public Guid planGoodsIssueItem_Index { get; set; }
            public Guid planGoodsIssue_Index { get; set; }
            public string lineNum { get; set; }
            public Guid? product_Index { get; set; }
            public string product_Id { get; set; }
            public string product_Name { get; set; }
            public string product_SecondName { get; set; }
            public string product_ThirdName { get; set; }
            public string product_Lot { get; set; }
            public Guid? itemStatus_Index { get; set; }
            public string itemStatus_Id { get; set; }
            public string itemStatus_Name { get; set; }
            public decimal? qty { get; set; }
            public decimal? ratio { get; set; }
            public decimal? totalQty { get; set; }
            public Guid? productConversion_Index { get; set; }
            public string productConversion_Id { get; set; }
            public string productConversion_Name { get; set; }
            public string productConversion_Base { get; set; }
            public DateTime? mfg_Date { get; set; }
            public DateTime? exp_Date { get; set; }
            public decimal? unitWeight { get; set; }
            public decimal? weight { get; set; }
            public decimal? unitWidth { get; set; }
            public decimal? unitLength { get; set; }
            public decimal? unitHeight { get; set; }
            public decimal? unitVolume { get; set; }
            public decimal? volume { get; set; }
            public decimal? unitPrice { get; set; }
            public decimal? price { get; set; }
            public string documentRef_No1 { get; set; }
            public string documentRef_No2 { get; set; }
            public string documentRef_No3 { get; set; }
            public string documentRef_No4 { get; set; }
            public string documentRef_No5 { get; set; }
            public string documentItem_Remark { get; set; }
            public int? document_Status { get; set; }
            public string udf_1 { get; set; }
            public string udf_2 { get; set; }
            public string udf_3 { get; set; }
            public string udf_4 { get; set; }
            public string udf_5 { get; set; }
            public string create_By { get; set; }
            public DateTime? create_Date { get; set; }
            public string update_By { get; set; }
            public DateTime? update_Date { get; set; }
            public string cancel_By { get; set; }
            public DateTime? cancel_Date { get; set; }
            public decimal? qtyBackOrder { get; set; }
            public int? backOrderStatus { get; set; }
            public string planGoodsIssue_Size { get; set; }
            public decimal? qtyQA { get; set; }
            public int? isQA { get; set; }
            public decimal? qty_Inner_Pack { get; set; }
            public decimal? qty_Sup_Pack { get; set; }
            public string imageUri { get; set; }
            public string zoneCode { get; set; }
            public string batch_Id { get; set; }
            public string qa_By { get; set; }
            public DateTime? qa_Date { get; set; }
            public int? runWave_Status { get; set; }
            public string planGoodsIssue_No { get; set; }
            public decimal? countQty { get; set; }
            public bool isDelete { get; set; }
            public string ref_DocumentItem_Index { get; set; }
            public string product_Id_RefNo2 { get; set; }
            public string erp_Location { get; set; }
        }
    }

    public class SearchPlanGoodsIssueInClauseViewModel : Pagination
    {
        public List<Guid> List_PlanGoodsIssue_Index { get; set; }

        public List<string> List_PlanGoodsIssue_No { get; set; }
    }

    public class planGoodsIssueAlreadyWave : Result
    {
        public planGoodsIssueAlreadyWave()
        {
            CheckWaveDipmodel = new List<CheckWaveDipmodel>();
            CheckWaveDipbyWavemodel = new List<CheckWaveDipbyWavemodel>();
            CheckWave_Roundmodel = new List<CheckWave_Round>();
            CheckStatusWavemodel = new List<CheckStatus_Wave>();
        }

        public string goodsIssue_No { get; set; }
        public string goodsIssue_Index { get; set; }
        public string planGoodsIssue_Due_Date { get; set; }
        public int? CountOrder { get; set; }
        public int? CountOrderLine { get; set; }
        public Guid? round_Index { get; set; }
        public string round_Id { get; set; }
        public string round_Name { get; set; }
        public bool ready { get; set; }

        public List<CheckWaveDipmodel> CheckWaveDipmodel { get; set; }
        public List<CheckWaveDipbyWavemodel> CheckWaveDipbyWavemodel { get; set; }
        public List<CheckWave_Round> CheckWave_Roundmodel { get; set; }
        public List<CheckStatus_Wave> CheckStatusWavemodel { get; set; }
    }

    public class Result
    {
        public bool resultIsUse { get; set; }

        public string resultMsg { get; set; }

    }

    public class CheckWave_Round
    {
        public long? RowIndex { get; set; }
        public string Round_Id { get; set; }
        public string Round_Name { get; set; }
        public DateTime? PlanGoodsIssue_Due_Date { get; set; }
        public int? countPlangi { get; set; }
        public int? CountPlanGIItem { get; set; }
        public int? CalMin { get; set; }
        public bool ready { get; set; }
    }

    public class CheckStatus_Wave
    {
        public long? RowIndex { get; set; }
        public Guid GoodsIssue_Index { get; set; }
        public string GoodsIssue_No { get; set; }
        public DateTime? GoodsIssue_Date { get; set; }
        public string Wave_Round { get; set; }
        public DateTime? minCreate_Date { get; set; }
        public DateTime? maxCreate_Date { get; set; }
        public int? MinUse { get; set; }
        public int? counRows { get; set; }
        public int? Document_Status { get; set; }
        public int? GI_status { get; set; }
        public int? TaskGI_status { get; set; }
        public int? TagOut_status { get; set; }
        public int? WCS_status { get; set; }
    }

    public class CheckWaveDipmodel
    {
        public string TruckLoad_No { get; set; }
        public string Appointment_Id { get; set; }
        public string Dock_Name { get; set; }
        public DateTime? Appointment_Date { get; set; }
        public string Appointment_Time { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public string ShipTo_Id { get; set; }
        public string ShipTo_Name { get; set; }
        public string BranchCode { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal? Order_Qty { get; set; }
        public string Order_Unit { get; set; }
    }

    public class CheckWaveDipbyWavemodel
    {
        public long? RowIndex { get; set; }
        public string TruckLoad_No { get; set; }
        public string Appointment_Id { get; set; }
        public DateTime? Appointment_Date { get; set; }
        public string Appointment_Time { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public string Order_Seq { get; set; }
        public string LineNum { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal? BU_Order_TotalQty { get; set; }
        public decimal? BU_GI_TotalQty { get; set; }
        public decimal? SU_Order_TotalQty { get; set; }
        public decimal? SU_GI_TotalQty { get; set; }
        public string SU_Unit { get; set; }
        public string ERP_Location { get; set; }
        public string Product_Lot { get; set; }
        public decimal? SU_Diff { get; set; }
        public string GoodsIssue_No { get; set; }
        public string Document_Remark { get; set; }
        public string DocumentRef_No3 { get; set; }

    }
}
