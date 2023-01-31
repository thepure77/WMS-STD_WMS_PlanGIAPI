using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PlanGIBusiness.Reports.DeliveryNote
{
    public class DeliveryNote
    {
        public long? RowIndex { get; set; }

        public string copy_int { get; set; }

        public string for_report_int { get; set; }
        public string for_report { get; set; }

        public string planGoods_Barcode { get; set; }


        public string Billing_Barcode { get; set; }

        public Guid TruckLoad_Index { get; set; }


        public string TruckLoad_No { get; set; }


        public string Appointment_Date { get; set; }


        public string Appointment_Time { get; set; }


        public string branch { get; set; }


        public string ShipTo_Name { get; set; }


        public string ShipTo_Id { get; set; }
        public string Shipping_Remark { get; set; }
        public string Order_Remark { get; set; }


        public string ShipTo_Address { get; set; }


        public string ShipTo_Tel { get; set; }

        public int? Row_Runing_product { get; set; }


        public string PlanGoodsIssue_No { get; set; }


        public string Product_Id { get; set; }


        public string Product_Name { get; set; }


        public decimal? qty { get; set; }

        public int? count_tagout_plan_product { get; set; }


        public string ProductConversion_Name { get; set; }


        public string Product_Lot { get; set; }

        public decimal? pick { get; set; }


        public string Billing { get; set; }
    }

}
