using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlanGIDataAccess.Models
{
    public class im_TruckLoad
    {
        [Key]
        public Guid TruckLoad_Index { get; set; }

        public string TruckLoad_No { get; set; }

        public DateTime? TruckLoad_Date { get; set; }

        public string Vehicle_Registration { get; set; }

        public double? Weight_in { get; set; }

        public double? Weight_out { get; set; }

        public string Time_in { get; set; }

        public string Time_out { get; set; }

        public string Start_load { get; set; }

        public string End_load { get; set; }

        public Guid? DocumentType_Index { get; set; }

        public string DocumentType_Id { get; set; }

        public string DocumentType_Name { get; set; }

        public string DocumentRef_No1 { get; set; }

        public string DocumentRef_No2 { get; set; }

        public string DocumentRef_No3 { get; set; }

        public string DocumentRef_No4 { get; set; }

        public string DocumentRef_No5 { get; set; }

        public string DocumentRef_No6 { get; set; }

        public string DocumentRef_No7 { get; set; }

        public string DocumentRef_No8 { get; set; }

        public string DocumentRef_No9 { get; set; }

        public string DocumentRef_No10 { get; set; }

        public string Document_Remark { get; set; }

        public int? Document_Status { get; set; }

        public string UDF_1 { get; set; }

        public string UDF_2 { get; set; }

        public string UDF_3 { get; set; }

        public string UDF_4 { get; set; }

        public string UDF_5 { get; set; }

        public int? Is_Booking { get; set; }

        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }

        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }

        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }

        public string Approve_By { get; set; }

        public DateTime? Approve_Date { get; set; }
        public Guid? VehicleType_Index { get; set; }

        public string VehicleType_Id { get; set; }
        public string VehicleType_Name { get; set; }

        public Guid? VehicleCompany_Index { get; set; }

        public string VehicleCompany_Id { get; set; }
        public string VehicleCompany_Name { get; set; }

        public DateTime? Expect_Delivery_Date { get; set; }
        public DateTime? Expect_Pickup_Date { get; set; }
    }
}
