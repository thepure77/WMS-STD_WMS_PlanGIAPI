using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PlanGIBusiness.Reports.Pack
{
    public class printPackModel
    {
        [DataMember]
        public Guid packItem_Index { get; set; }

        [DataMember]
        public Guid pack_Index { get; set; }

        [DataMember]
        public string pack_No { get; set; }

        [DataMember]
        public string packNo_Barcode { get; set; }

        [DataMember]
        public string lineNum { get; set; }

        [DataMember]
        public Guid? product_Index { get; set; }

        [DataMember]
        public string product_Id { get; set; }

        [DataMember]
        public string product_Name { get; set; }

        [DataMember]
        public string product_SecondName { get; set; }

        [DataMember]
        public string product_ThirdName { get; set; }

        [DataMember]
        public string product_Lot { get; set; }

        [DataMember]
        public Guid? itemStatus_Index { get; set; }

        [DataMember]
        public string itemStatus_Id { get; set; }

        [DataMember]
        public string itemStatus_Name { get; set; }

        [DataMember]
        public decimal? qty { get; set; }

        [DataMember]
        public decimal? ratio { get; set; }

        [DataMember]
        public decimal? totalQty { get; set; }

        [DataMember]
        public Guid? productConversion_Index { get; set; }

        [DataMember]
        public string productConversion_Id { get; set; }

        [DataMember]
        public string productConversion_Name { get; set; }

        [DataMember]
        public DateTime? mFG_Date { get; set; }

        [DataMember]
        public DateTime? eXP_Date { get; set; }

        [DataMember]
        public decimal? unitWeight { get; set; }

        [DataMember]
        public decimal? weight { get; set; }

        [DataMember]
        public decimal? unitWidth { get; set; }

        [DataMember]
        public decimal? unitLength { get; set; }

        [DataMember]
        public decimal? unitHeight { get; set; }

        [DataMember]
        public decimal? unitVolume { get; set; }

        [DataMember]
        public decimal? volume { get; set; }

        [DataMember]
        public decimal? unitPrice { get; set; }

        [DataMember]
        public decimal? price { get; set; }

        [DataMember]
        public string documentRef_No1 { get; set; }

        [DataMember]
        public string documentRef_No2 { get; set; }

        [DataMember]
        public string documentRef_No3 { get; set; }

        [DataMember]
        public string documentRef_No4 { get; set; }

        [DataMember]
        public string documentRef_No5 { get; set; }

        [DataMember]
        public string document_Remark { get; set; }

        [DataMember]
        public int? document_Status { get; set; }

        [DataMember]
        public string uDF_1 { get; set; }

        [DataMember]
        public string uDF_2 { get; set; }

        [DataMember]
        public string uDF_3 { get; set; }

        [DataMember]
        public string uDF_4 { get; set; }

        [DataMember]
        public string uDF_5 { get; set; }

        [DataMember]
        public Guid? ref_Process_Index { get; set; }

        [DataMember]
        public string ref_Document_No { get; set; }

        [DataMember]
        public string ref_Document_LineNum { get; set; }

        [DataMember]
        public Guid? ref_Document_Index { get; set; }

        [DataMember]
        public Guid? ref_DocumentItem_Index { get; set; }

        [DataMember]
        public string create_By { get; set; }

        [DataMember]
        public DateTime? create_Date { get; set; }

        [DataMember]
        public string update_By { get; set; }

        [DataMember]
        public DateTime? update_Date { get; set; }

        [DataMember]
        public string cancel_By { get; set; }

        [DataMember]
        public DateTime? cancel_Date { get; set; }

        [DataMember]
        public decimal? packQty { get; set; }

        [DataMember]
        public decimal? packRatio { get; set; }

        [DataMember]
        public decimal? packTotalQty { get; set; }

        [DataMember]
        public Guid? packProductConversion_Index { get; set; }

        [DataMember]
        public string packProductConversion_Id { get; set; }

        [DataMember]
        public string packProductConversion_Name { get; set; }
    }

}
