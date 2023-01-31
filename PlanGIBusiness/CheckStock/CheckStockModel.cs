using PlanGIBusiness;
using System;
using System.Collections.Generic;
using System.Text;

namespace planGIBusiness.PlanGoodsIssue
{
    public partial class CheckStockModel : Pagination
    {
        public Guid planGoodsIssue_Index { get; set; }
        public string planGoodsIssue_No { get; set; }
        public string planGoodsIssue_Date { get; set; }
        public string planGoodsIssue_Date_To { get; set; }
        public Guid? round_Index { get; set; }
        public string round_Id { get; set; }
        public string round_Name { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_Lot { get; set; }
        public int? DocumentPriority_Status { get; set; }
        public Guid? ItemStatus_Index { get; set; }
        public Guid? location_Index { get; set; }
        public string erp_Location { get; set; }
        public decimal? binBalance_QtyBal { get; set; }
        public decimal? binBalance_QtyReserve { get; set; }
        public decimal? binBalance_QtyBegin { get; set; }
        public decimal? Plan_QTY { get; set; }
        public decimal? Qty_Ava { get; set; }
        public bool Isdiff { get; set; }
        public string remark { get; set; }


        public class actionResultViewModel
        {
            public IList<CheckStockModel> CheckStockModel { get; set; }
            public Pagination pagination { get; set; }
        }

    }
}
