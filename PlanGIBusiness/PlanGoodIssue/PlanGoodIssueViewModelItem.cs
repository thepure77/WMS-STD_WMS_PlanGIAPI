using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PlanGIBusiness.PlanGoodIssue
{
   public class PlanGoodIssueViewModelItem
    {
        [Key]
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

    }
}
