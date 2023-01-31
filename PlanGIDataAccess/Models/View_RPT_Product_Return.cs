using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlanGIDataAccess.Models
{
    public class View_RPT_Product_Return
    {
        [Key]
        public long? RowIndex { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public Guid PlanGoodsIssue_Index { get; set; }
        public Guid PlanGoodsIssueItem_Index { get; set; }
        public string Branch { get; set; }
        public string ShipTo_Name { get; set; }
        public Guid ShipTo_Index { get; set; }
        public string Shipto_Id { get; set; }
        public string Shipto_address { get; set; }
        public string Round_Name { get; set; }
        public string Product_Return_No { get; set; }
        public string Claim_No { get; set; }
        public string Ref_Billing_No { get; set; }
        public DateTime? PlanGoodsIssue_Date { get; set; }
        public int? Row_Runing_product { get; set; }
        public Guid Product_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public Guid ProductConversion_Index { get; set; }
        public string ProductConversion_Name { get; set; }
        public decimal QtyBackOrder { get; set; }
        public string Product_Lot { get; set; }
        public string DocumentItem_Remark { get; set; }
        public string Return_remark { get; set; }
        public string Shipping_Remark { get; set; }
    }
}
