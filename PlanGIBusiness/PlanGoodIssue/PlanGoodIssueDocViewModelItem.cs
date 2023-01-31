using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlanGIBusiness.PlanGoodIssue
{
    public partial class PlanGoodIssueDocViewModelItem
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


        public string qty { get; set; }


        public decimal? ratio { get; set; }


        public decimal? totalQty { get; set; }

        public Guid? productConversion_Index { get; set; }


        public string productConversion_Id { get; set; }


        public string productConversion_Name { get; set; }

        public string productConversion_Bu_Name { get; set; }


        public DateTime? mFG_Date { get; set; }


        public DateTime? eXP_Date { get; set; }
        public string mfg_Date { get; set; }
        public string exp_Date { get; set; }


        public decimal? unitWeight { get; set; }
        public Guid? unitWeight_Index { get; set; }
        public string unitWeight_Id { get; set; }
        public string unitWeight_Name { get; set; }
        public decimal? unitWeightRatio { get; set; }

        public decimal? weight { get; set; }
        public Guid? weight_Index { get; set; }
        public string weight_Id { get; set; }
        public string weight_Name { get; set; }
        public decimal? weightRatio { get; set; }

        public decimal? unitNetWeight { get; set; }
        public Guid? unitNetWeight_Index { get; set; }
        public string unitNetWeight_Id { get; set; }
        public string unitNetWeight_Name { get; set; }
        public decimal? unitNetWeightRatio { get; set; }

        public decimal? netWeight { get; set; }
        public Guid? netWeight_Index { get; set; }
        public string netWeight_Id { get; set; }
        public string netWeight_Name { get; set; }
        public decimal? netWeightRatio { get; set; }

        public decimal? unitGrsWeight { get; set; }
        public Guid? unitGrsWeight_Index { get; set; }
        public string unitGrsWeight_Id { get; set; }
        public string unitGrsWeight_Name { get; set; }
        public decimal? unitGrsWeightRatio { get; set; }

        public decimal? grsWeight { get; set; }
        public Guid? grsWeight_Index { get; set; }
        public string grsWeight_Id { get; set; }
        public string grsWeight_Name { get; set; }
        public decimal? grsWeightRatio { get; set; }

        public decimal? unitWidth { get; set; }
        public Guid? unitWidth_Index { get; set; }
        public string unitWidth_Id { get; set; }
        public string unitWidth_Name { get; set; }
        public decimal? unitWidthRatio { get; set; }

        public decimal? width { get; set; }
        public Guid? width_Index { get; set; }
        public string width_Id { get; set; }
        public string width_Name { get; set; }
        public decimal? widthRatio { get; set; }

        public decimal? unitLength { get; set; }
        public Guid? unitLength_Index { get; set; }
        public string unitLength_Id { get; set; }
        public string unitLength_Name { get; set; }
        public decimal? unitLengthRatio { get; set; }

        public decimal? length { get; set; }
        public Guid? length_Index { get; set; }
        public string length_Id { get; set; }
        public string length_Name { get; set; }
        public decimal? lengthRatio { get; set; }

        public decimal? unitHeight { get; set; }
        public Guid? unitHeight_Index { get; set; }
        public string unitHeight_Id { get; set; }
        public string unitHeight_Name { get; set; }
        public decimal? unitHeightRatio { get; set; }

        public decimal? height { get; set; }
        public Guid? height_Index { get; set; }
        public string height_Id { get; set; }
        public string height_Name { get; set; }
        public decimal? heightRatio { get; set; }

        public decimal? unitVolume { get; set; }
        public Guid? unitVolume_Index { get; set; }
        public string unitVolume_Id { get; set; }
        public string unitVolume_Name { get; set; }
        public decimal? unitVolumeRatio { get; set; }

        public decimal? volume { get; set; }
        public Guid? volume_Index { get; set; }
        public string volume_Id { get; set; }
        public string volume_Name { get; set; }
        public decimal? volumeRatio { get; set; }


        public decimal? unitPrice { get; set; }
        public Guid? unitPrice_Index { get; set; }
        public string unitPrice_Id { get; set; }
        public string unitPrice_Name { get; set; }

        public decimal? price { get; set; }
        public Guid? price_Index { get; set; }
        public string price_Id { get; set; }
        public string price_Name { get; set; }


        public string documentRef_No1 { get; set; }


        public string documentRef_No2 { get; set; }


        public string documentRef_No3 { get; set; }


        public string documentRef_No4 { get; set; }


        public string documentRef_No5 { get; set; }


        public string documentItem_Remark { get; set; }

        public int? document_Status { get; set; }


        public string uDF_1 { get; set; }


        public string uDF_2 { get; set; }


        public string uDF_3 { get; set; }


        public string uDF_4 { get; set; }


        public string uDF_5 { get; set; }


        public string create_By { get; set; }


        public DateTime? create_Date { get; set; }


        public string update_By { get; set; }


        public DateTime? update_Date { get; set; }


        public string cancel_By { get; set; }


        public DateTime? cancel_Date { get; set; }


        public decimal? qtyBackOrder { get; set; }

        public int? BackOrderStatus { get; set; }


        public string planGoodsIssue_Size { get; set; }


        public decimal? qtyQA { get; set; }

        public int? isQA { get; set; }


        public decimal? qty_Inner_Pack { get; set; }


        public decimal? qty_Sup_Pack { get; set; }


        public string imageUri { get; set; }


        public string ZoneCode { get; set; }


        public string Batch_Id { get; set; }


        public string qA_By { get; set; }


        public DateTime? qA_Date { get; set; }

        public int? runWave_Status { get; set; }


        public string planGoodsIssue_No { get; set; }


        public decimal? countQty { get; set; }
        public decimal? gI_Qty { get; set; }
        public decimal? gI_TotalQty { get; set; }
        public decimal? remain_TotalQty { get; set; }
        public string erp_Location { get; set; }
        public string matdoc { get; set; }
    }
}
