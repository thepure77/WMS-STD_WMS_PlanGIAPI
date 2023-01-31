using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace PlanGIDataAccess.Models
{

    public partial class CheckWaveDipbyWave
    {
        [Key]
        public long? RowIndex { get; set; }
        public string TruckLoad_No { get; set; }
        public string Appointment_Id { get; set; }
        public DateTime? Appointment_Date { get; set; }
        public string Appointment_Time { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public string Order_Seq { get; set; }
        public string LineNum { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal? BU_Order_TotalQty { get; set; }
        public decimal? BU_GI_TotalQty { get; set; }
        public decimal? SU_Order_TotalQty { get; set; }
        public decimal? SU_GI_TotalQty { get; set; }
        public string SU_Unit { get; set; }
        public string ERP_Location { get; set; }
        public string Product_Lot { get; set; }
        public decimal? SU_Diff { get; set; }
        public string GoodsIssue_No { get; set; }
        public string Document_Remark { get; set; }
        public string DocumentRef_No3 { get; set; }

    }
}


 

 
 
  
 
 
 
 
  