using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PlanGIBusiness.Reports
{
    public class ReportPlanGoodsIssueViewModel
    {
        public Guid planGoodsIssue_Index { get; set; }

        public Guid owner_Index { get; set; }

        public string owner_Id { get; set; }

        public string owner_Name { get; set; }

        public Guid soldTo_Index { get; set; }

        public string soldTo_Id { get; set; }

        public string soldTo_Name { get; set; }

        public string soldTo_Address { get; set; }

        public Guid shipTo_Index { get; set; }

        public string shipTo_Id { get; set; }

        public string shipTo_Name { get; set; }

        public string shipTo_Address { get; set; }

        public Guid? documentType_Index { get; set; }

        public string documentType_Id { get; set; }

        public string documentType_Name { get; set; }

        public string planGoodsIssue_No { get; set; }

        public string planGoodsIssue_Date { get; set; }

        public string planGoodsIssue_Due_Date { get; set; }

        public string documentRef_No1 { get; set; }

        public string documentRef_No2 { get; set; }

        public string documentRef_No3 { get; set; }

        public string documentRef_No4 { get; set; }

        public string documentRef_No5 { get; set; }

        public string document_Remark { get; set; }

        public int? document_Status { get; set; }

        public string uDF_1 { get; set; }

        public string uDF_2 { get; set; }

        public string uDF_3 { get; set; }

        public string uDF_4 { get; set; }

        public string uDF_5 { get; set; }

        public int? documentPriority_Status { get; set; }

        public Guid? warehouse_Index { get; set; }

        public string warehouse_Id { get; set; }

        public string warehouse_Name { get; set; }

        public Guid? warehouse_Index_To { get; set; }

        public string warehouse_Id_To { get; set; }

        public string warehouse_Name_To { get; set; }

        public Guid? soldTo_SubDistrict_Index { get; set; }

        public Guid? soldTo_District_Index { get; set; }

        public Guid? soldTo_Province_Index { get; set; }

        public Guid? soldTo_Country_Index { get; set; }

        public Guid? soldTo_Postcode_Index { get; set; }

        public Guid? subDistrict_Index { get; set; }

        public Guid? District_Index { get; set; }

        public Guid? province_Index { get; set; }

        public Guid? country_Index { get; set; }

        public Guid? postcode_Index { get; set; }

        public string create_By { get; set; }

        public string create_Date { get; set; }

        public string update_By { get; set; }

        public DateTime? update_Date { get; set; }

        public string cancel_By { get; set; }

        public DateTime? cancel_Date { get; set; }

        public int? backOrderStatus { get; set; }

        public Guid? round_Index { get; set; }

        public string round_Id { get; set; }

        public string round_Name { get; set; }

        public Guid? route_Index { get; set; }

        public string route_Id { get; set; }

        public string route_Name { get; set; }

        public int? printTaxInvoice { get; set; }

        public string payment_Type { get; set; }

        public string payment_Code { get; set; }

        public decimal? cal_GrandTotal { get; set; }

        public Guid? reasonCode_Index { get; set; }

        public string reasonCode_Id { get; set; }

        public string reasonCode_Name { get; set; }

        public Guid? ref_PlanGoodsIssue_Index { get; set; }

        public string ref_PlanGoodsIssue_No { get; set; }

        public string status_RMS { get; set; }

        public string status_Desc_RMS { get; set; }

        public string vendor_Id { get; set; }

        public DateTime? sTPlanGoodsIssue_Due_Date { get; set; }

        public DateTime? create_Date_File { get; set; }

        public string status_EDI { get; set; }

        public string status_Reason { get; set; }

        public string round_Time { get; set; }

        public string warehouse_Phone { get; set; }

        public string soldTo_T1C { get; set; }

        public string soldTo_T1CPhone { get; set; }

        public string soldTo_Email { get; set; }

        public string soldTo_Phone { get; set; }

        public string shipTo_CompanyName { get; set; }

        public string shipTo_Remark { get; set; }

        public string shipTo_Telephone { get; set; }

        public string shipTo_TaxNo { get; set; }

        public string invoice_Name { get; set; }

        public string invoice_CompanyName { get; set; }

        public string invoice_Address { get; set; }

        public string invoice_Remark { get; set; }

        public string invoice_Telephone { get; set; }

        public string invoice_TaxNo { get; set; }

        public string payment_Issuer { get; set; }

        public decimal? cal_PromotionDiscount { get; set; }

        public decimal? cal_Cpn2Discount { get; set; }

        public decimal? cal_Cpn9Discount { get; set; }

        public decimal? cal_EvoucherDiscount { get; set; }

        public decimal? cal_TotalAfterDiscount { get; set; }

        public string document_Remark_Sub { get; set; }

        public string userAssign { get; set; }

        public string soldTo_Name_Cus { get; set; }

        public string soldTo_Tel { get; set; }

        public string soldTo_Email_Cus { get; set; }

        public string soldTo_Phone_Cus { get; set; }

        public string soldTo_Address_Cus { get; set; }

        public bool? isPostPickConfirm { get; set; }

        public DateTime? isPostPickConfirm_Date { get; set; }

        public bool? isPostShippmentDispatch { get; set; }

        public DateTime? isPostShippmentDispatch_Date { get; set; }

        public string statusDropST { get; set; }

        public DateTime? statusDropST_Date { get; set; }

        public string shipTo_AddressName { get; set; }

        public string invoice_AddressName { get; set; }

        public Guid? ref_WavePick_index { get; set; }

        public string planGoodsIssue_Time { get; set; }



        public Guid planGoodsIssueItem_Index { get; set; }

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
        
        public DateTime? mFG_Date { get; set; }
        
        public string eXP_Date { get; set; }
        
        public decimal? unitWeight { get; set; }
        
        public decimal? weight { get; set; }
        
        public decimal? unitWidth { get; set; }
        
        public decimal? unitLength { get; set; }
        
        public decimal? unitHeight { get; set; }
        
        public decimal? unitVolume { get; set; }
        
        public decimal? volume { get; set; }
        
        public decimal? unitPrice { get; set; }
        
        public decimal? price { get; set; }
                
        public string documentItem_Remark { get; set; }
                
        public decimal? qtyBackOrder { get; set; }
                
        public string planGoodsIssue_Size { get; set; }
        
        public decimal? qtyQA { get; set; }

        public int? isQA { get; set; }
        
        public decimal? qty_Inner_Pack { get; set; }
        
        public decimal? qty_Sup_Pack { get; set; }
        
        public string imageUri { get; set; }
        
        public string zoneCode { get; set; }
        
        public string batch_Id { get; set; }
        
        public string qA_By { get; set; }
        
        public DateTime? qA_Date { get; set; }

        public int? runWave_Status { get; set; }
               
        public decimal? countQty { get; set; }

        public int? table_No { get; set; }

        public List<ReportPlanGoodsIssueViewModel> model { get; set; }

    }
}
