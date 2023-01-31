using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlanGIBusiness.PlanGoodIssue
{
    public class PackViewModel
    {
        public Guid pack_Index { get; set; }


        public string pack_No { get; set; }

        public string pack_Date { get; set; }


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

        public Guid? round_Index { get; set; }


        public string round_Id { get; set; }


        public string round_Name { get; set; }


        public string round_Time { get; set; }

        public Guid? route_Index { get; set; }


        public string route_Id { get; set; }


        public string route_Name { get; set; }


        public string documentref_No1 { get; set; }


        public string documentref_No2 { get; set; }


        public string documentref_No3 { get; set; }


        public string documentref_No4 { get; set; }


        public string documentref_No5 { get; set; }


        public string document_Remark { get; set; }

        public int? document_Status { get; set; }


        public string uDF_1 { get; set; }


        public string uDF_2 { get; set; }


        public string uDF_3 { get; set; }


        public string uDF_4 { get; set; }


        public string UDF_5 { get; set; }

        public Guid? soldTo_subDistrict_Index { get; set; }

        public Guid? soldTo_District_Index { get; set; }

        public Guid? soldTo_Province_Index { get; set; }

        public Guid? soldTo_Country_Index { get; set; }

        public Guid? soldTo_Postcode_Index { get; set; }

        public Guid? subDistrict_Index { get; set; }

        public Guid? district_Index { get; set; }

        public Guid? province_Index { get; set; }

        public Guid? country_Index { get; set; }

        public Guid? postcode_Index { get; set; }

        public int? printTaxInvoice { get; set; }


        public string payment_Type { get; set; }


        public string payment_Code { get; set; }


        public string soldTo_Email { get; set; }


        public string soldTo_Phone { get; set; }


        public string shipTo_phone { get; set; }


        public string shipTo_remark { get; set; }


        public string create_By { get; set; }

        public DateTime? create_Date { get; set; }


        public string update_By { get; set; }

        public DateTime? update_Date { get; set; }


        public string cancel_By { get; set; }

        public DateTime? cancel_Date { get; set; }
        public List<PackItemViewModel> listPackItemViewModel { get; set; }

    }
}
