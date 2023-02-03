using System;
using System.Collections.Generic;
using System.Text;

namespace PlanGIBusiness.Demo
{
    public class DemoShipmentRequestViewModel
    {
        //public string tm_Index { get; set; }
        public string tm_no { get; set; }
        public string tm_date { get; set; }
        public string vehicleType_Id { get; set; }
        public string vehicleType_Name { get; set; }
        public string vehicle_No { get; set; }
        public string driver_Name { get; set; }
        public string route_Id { get; set; }
        public string subRoute_Id { get; set; }
        public string vehicleCompany_Id { get; set; }
        public string vehicleCompany_Name { get; set; }
        public string expect_Pickup_Date { get; set; }
        public string expect_Pickup_Time { get; set; }
        public string flagCancel { get; set; }
        public string flagUpdate { get; set; }
        public string flagNoBook { get; set; }
        //public string flagColdRoom { get; set; }
        public string IsAirCon { get; set; }

        public string FreightKind_Name { get; set; }

        public List<DemoShipmentItemViewModel> items { get; set; }
    }

    public class DemoShipmentItemViewModel
    {
        public string planGoodsIssue_No { get; set; }
        public string seq { get; set; }
        public string Drop_seq { get; set; }
        public string item_seq { get; set; }
        public string is_return { get; set; }
        public string shiptoid { get; set; }
        public string shiptoname { get; set; }
        public string shiptoaddress { get; set; }
        public string tel { get; set; }
        public string expect_Delivery_Date { get; set; }
    }

    public class DemoShipmentResponseViewModel
    {
        public string document_No { get; set; }
        public bool result { get; set; }
        public int? reason_code { get; set; }
        public string message { get; set; }
    }
}
