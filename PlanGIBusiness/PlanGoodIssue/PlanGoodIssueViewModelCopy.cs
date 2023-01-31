using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PlanGIBusiness.PlanGoodIssue
{
    public class PlanGoodIssueViewModelCopy
    {
        [Key]
        public Guid PlanGoodsIssue_Index { get; set; }

        public Guid Owner_Index { get; set; }

        [StringLength(50)]
        public string Owner_Id { get; set; }

        [StringLength(50)]
        public string Owner_Name { get; set; }

        public Guid SoldTo_Index { get; set; }

        [StringLength(50)]
        public string SoldTo_Id { get; set; }

        [StringLength(200)]
        public string SoldTo_Name { get; set; }

        [StringLength(200)]
        public string SoldTo_Address { get; set; }

        public Guid ShipTo_Index { get; set; }

        [StringLength(50)]
        public string ShipTo_Id { get; set; }

        [StringLength(200)]
        public string ShipTo_Name { get; set; }

        [StringLength(200)]
        public string ShipTo_Address { get; set; }

        public Guid? DocumentType_Index { get; set; }

        [StringLength(50)]
        public string DocumentType_Id { get; set; }

        [StringLength(200)]
        public string DocumentType_Name { get; set; }

        [Required]
        [StringLength(50)]
        public string PlanGoodsIssue_No { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime PlanGoodsIssue_Date { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? PlanGoodsIssue_Due_Date { get; set; }

        [StringLength(200)]
        public string DocumentRef_No1 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No2 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No3 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No4 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No5 { get; set; }

        [StringLength(200)]
        public string Document_Remark { get; set; }

        public int? Document_Status { get; set; }

        [StringLength(200)]
        public string UDF_1 { get; set; }

        [StringLength(200)]
        public string UDF_2 { get; set; }

        [StringLength(200)]
        public string UDF_3 { get; set; }

        [StringLength(200)]
        public string UDF_4 { get; set; }

        [StringLength(200)]
        public string UDF_5 { get; set; }

        public int? DocumentPriority_Status { get; set; }

        public Guid? Warehouse_Index { get; set; }

        [StringLength(50)]
        public string Warehouse_Id { get; set; }

        [StringLength(200)]
        public string Warehouse_Name { get; set; }

        public Guid? Warehouse_Index_To { get; set; }

        [StringLength(50)]
        public string Warehouse_Id_To { get; set; }

        [StringLength(200)]
        public string Warehouse_Name_To { get; set; }

        public Guid? SoldTo_SubDistrict_Index { get; set; }

        public Guid? SoldTo_District_Index { get; set; }

        public Guid? SoldTo_Province_Index { get; set; }

        public Guid? SoldTo_Country_Index { get; set; }

        public Guid? SoldTo_Postcode_Index { get; set; }

        public Guid? SubDistrict_Index { get; set; }

        public Guid? District_Index { get; set; }

        public Guid? Province_Index { get; set; }

        public Guid? Country_Index { get; set; }

        public Guid? Postcode_Index { get; set; }

        [StringLength(200)]
        public string Create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Create_Date { get; set; }

        [StringLength(200)]
        public string Update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Update_Date { get; set; }

        [StringLength(200)]
        public string Cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Cancel_Date { get; set; }

        public int? BackOrderStatus { get; set; }

        public Guid? Round_Index { get; set; }

        [StringLength(50)]
        public string Round_Id { get; set; }

        [StringLength(200)]
        public string Round_Name { get; set; }

        public Guid? Route_Index { get; set; }

        [StringLength(50)]
        public string Route_Id { get; set; }

        [StringLength(200)]
        public string Route_Name { get; set; }

        public int? PrintTaxInvoice { get; set; }

        [StringLength(50)]
        public string Payment_Type { get; set; }

        [StringLength(50)]
        public string Payment_Code { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Cal_GrandTotal { get; set; }

        public Guid? ReasonCode_Index { get; set; }

        [StringLength(50)]
        public string ReasonCode_Id { get; set; }

        [StringLength(200)]
        public string ReasonCode_Name { get; set; }

        public Guid? Ref_PlanGoodsIssue_Index { get; set; }

        [StringLength(50)]
        public string Ref_PlanGoodsIssue_No { get; set; }

        [StringLength(200)]
        public string Status_RMS { get; set; }

        [StringLength(200)]
        public string Status_Desc_RMS { get; set; }

        [StringLength(50)]
        public string Vendor_Id { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? STPlanGoodsIssue_Due_Date { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Create_Date_File { get; set; }

        [StringLength(50)]
        public string Status_EDI { get; set; }

        [StringLength(50)]
        public string Status_Reason { get; set; }

        [StringLength(50)]
        public string Round_Time { get; set; }

        [StringLength(200)]
        public string Warehouse_Phone { get; set; }

        [StringLength(50)]
        public string SoldTo_T1C { get; set; }

        [StringLength(50)]
        public string SoldTo_T1CPhone { get; set; }

        [StringLength(200)]
        public string SoldTo_Email { get; set; }

        [StringLength(200)]
        public string SoldTo_Phone { get; set; }

        [StringLength(200)]
        public string ShipTo_CompanyName { get; set; }

        [StringLength(200)]
        public string ShipTo_Remark { get; set; }

        [StringLength(200)]
        public string ShipTo_Telephone { get; set; }

        [StringLength(200)]
        public string ShipTo_TaxNo { get; set; }

        [StringLength(200)]
        public string Invoice_Name { get; set; }

        [StringLength(200)]
        public string Invoice_CompanyName { get; set; }

        [StringLength(200)]
        public string Invoice_Address { get; set; }

        [StringLength(200)]
        public string Invoice_Remark { get; set; }

        [StringLength(50)]
        public string Invoice_Telephone { get; set; }

        [StringLength(50)]
        public string Invoice_TaxNo { get; set; }

        [StringLength(50)]
        public string Payment_Issuer { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Cal_PromotionDiscount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Cal_Cpn2Discount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Cal_Cpn9Discount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Cal_EvoucherDiscount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Cal_TotalAfterDiscount { get; set; }

        [StringLength(200)]
        public string Document_Remark_Sub { get; set; }

    }
}
