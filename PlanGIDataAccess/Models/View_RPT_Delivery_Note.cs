using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace PlanGIDataAccess.Models
{
    public partial class View_RPT_Delivery_Note
    {
        [Key]
        public long? RowIndex { get; set; }



        public Guid TruckLoad_Index { get; set; }
        public Guid PlanGoodsIssue_Index { get; set; }
        public Guid? DocumentType_Index { get; set; }


        public string TruckLoad_No { get; set; }


        public DateTime? Appointment_Date { get; set; }


        public string Appointment_Time { get; set; }


        public string branch { get; set; }

        
        public string ShipTo_Name { get; set; }

        
        public string ShipTo_Id { get; set; }

        
        public string ShipTo_Address { get; set; }

        public string Shipping_Remark { get; set; }
        public string Order_Remark { get; set; }


        public string ShipTo_Tel { get; set; }

        public int? Row_Runing_product { get; set; }

        
        public string PlanGoodsIssue_No { get; set; }

        
        public string Product_Id { get; set; }

        
        public string Product_Name { get; set; }

        
        public decimal? qty { get; set; }

        public int? count_tagout_plan_product { get; set; }
        public int Order_seq { get; set; }

        
        public string ProductConversion_Name { get; set; }

        
        public string Product_Lot { get; set; }
        
        public decimal? pick { get; set; }

        
        public string Billing { get; set; }
    }
}
