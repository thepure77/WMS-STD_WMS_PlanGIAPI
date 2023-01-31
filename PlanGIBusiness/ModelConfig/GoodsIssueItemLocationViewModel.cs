using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIBusiness.GoodIssue
{

    public partial class goodsIssueItemLocationViewModel
    {
        [Key]
        public Guid goodsIssueItemLocation_Index { get; set; }

        public Guid goodsIssueItem_Index { get; set; }

        public Guid goodsIssue_Index { get; set; }


        public string lineNum { get; set; }

        public Guid? tagItem_Index { get; set; }

        public Guid? tag_Index { get; set; }


        public string tag_No { get; set; }

        public Guid product_Index { get; set; }



        public string product_Id { get; set; }



        public string product_Name { get; set; }


        public string product_SecondName { get; set; }


        public string product_ThirdName { get; set; }


        public string product_Lot { get; set; }

        public Guid? itemStatus_Index { get; set; }



        public string itemStatus_Id { get; set; }



        public string itemStatus_Name { get; set; }

        public Guid? location_Index { get; set; }


        public string location_Id { get; set; }


        public string location_Name { get; set; }


        public decimal qty { get; set; }


        public decimal ratio { get; set; }


        public decimal totalQty { get; set; }

        public Guid productConversion_Index { get; set; }



        public string productConversion_Id { get; set; }



        public string productConversion_Name { get; set; }


        public DateTime? mFG_Date { get; set; }


        public DateTime? eXP_Date { get; set; }


        public decimal? unitWeight { get; set; }


        public decimal weight { get; set; }


        public decimal unitWidth { get; set; }


        public decimal unitLength { get; set; }


        public decimal unitHeight { get; set; }


        public decimal unitVolume { get; set; }


        public decimal volume { get; set; }


        public decimal unitPrice { get; set; }


        public decimal price { get; set; }


        public string documentRef_No1 { get; set; }


        public string documentRef_No2 { get; set; }


        public string documentRef_No3 { get; set; }


        public string documentRef_No4 { get; set; }


        public string documentRef_No5 { get; set; }

        public int? document_Status { get; set; }


        public string uDF_1 { get; set; }


        public string uDF_2 { get; set; }


        public string uDF_3 { get; set; }


        public string uDF_4 { get; set; }


        public string uDF_5 { get; set; }

        public Guid? ref_Process_Index { get; set; }


        public string ref_Document_No { get; set; }


        public string ref_Document_LineNum { get; set; }

        public Guid? ref_Document_Index { get; set; }

        public Guid? ref_DocumentItem_Index { get; set; }

        public Guid goodsReceiveItem_Index { get; set; }


        public string create_By { get; set; }


        public DateTime? create_Date { get; set; }


        public string update_By { get; set; }


        public DateTime? update_Date { get; set; }


        public string cancel_By { get; set; }


        public DateTime? cancel_Date { get; set; }

        public int? picking_Status { get; set; }


        public string picking_By { get; set; }


        public DateTime? picking_Date { get; set; }


        public string picking_Ref1 { get; set; }


        public string picking_Ref2 { get; set; }


        public decimal? picking_Qty { get; set; }


        public decimal? picking_Ratio { get; set; }


        public decimal? picking_TotalQty { get; set; }

        public Guid? picking_ProductConversion_Index { get; set; }

        public int? mashall_Status { get; set; }


        public decimal? mashall_Qty { get; set; }

        public int? cancel_Status { get; set; }


        public string goodsIssue_No { get; set; }
        public string message { get; set; }


    }
}
