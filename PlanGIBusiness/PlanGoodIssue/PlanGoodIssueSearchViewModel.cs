using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PlanGIBusiness.PlanGoodIssue
{
    public class PlanGoodIssueSearchViewModel : Pagination
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

        public int? document_status { get; set; }


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

        public Guid? soldTo_subdistrict_Index { get; set; }

        public Guid? soldTo_district_Index { get; set; }

        public Guid? soldTo_province_Index { get; set; }

        public Guid? soldTo_vountry_Index { get; set; }

        public Guid? soldTo_postcode_Index { get; set; }

        public Guid? subdistrict_Index { get; set; }

        public Guid? district_Index { get; set; }

        public Guid? province_Index { get; set; }

        public Guid? vountry_Index { get; set; }

        public Guid? postcode_Index { get; set; }


        public string create_By { get; set; }


        public DateTime? create_date { get; set; }


        public string update_By { get; set; }


        public DateTime? update_Date { get; set; }


        public string vancel_By { get; set; }


        public DateTime? vancel_date { get; set; }

        public int? backorderstatus { get; set; }

        public Guid? round_Index { get; set; }


        public string round_Id { get; set; }


        public string round_Name { get; set; }

        public Guid? route_Index { get; set; }


        public string route_Id { get; set; }


        public string route_Name { get; set; }
        public Guid? ref_planGoodsIssue_Index { get; set; }

        public string ref_planGoodsIssue_No { get; set; }
        public string payment_Type { get; set; }

        public int? printTaxInvoice { get; set; }


        public string payment_Code { get; set; }

        public decimal? cal_GrandTotal { get; set; }

        public string processStatus_Name { get; set; }
        public string userAssign { get; set; }
        public Guid? reasonCode_Index { get; set; }

        public string reasonCode_Id { get; set; }
        public string reasonCode_Name { get; set; }
        public string soldTo_Name_Cus { get; set; }
    }

    public class actionResultPlanGISearchViewModel
    {
        public IList<PlanGoodIssueSearchViewModel> items { get; set; }
        public IList<PlanGoodsIssueItemSearchViewModel> listitems { get; set; }

        public Pagination pagination { get; set; }
    }
}
