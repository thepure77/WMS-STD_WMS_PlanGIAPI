using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace PlanGIDataAccess.Models
{

    public partial class CheckStatusWave
    {
        [Key]
        public long? RowIndex { get; set; }
        public Guid GoodsIssue_Index { get; set; }
        public string GoodsIssue_No { get; set; }
        public DateTime? GoodsIssue_Date { get; set; }
        public string Wave_Round { get; set; }
        public DateTime? minCreate_Date { get; set; }
        public DateTime? maxCreate_Date { get; set; }
        public int? MinUse { get; set; }
        public int? counRows { get; set; }
        public int? Document_Status { get; set; }
        public int? GI_status { get; set; }
        public int? TaskGI_status { get; set; }
        public int? TagOut_status { get; set; }
        public int? WCS_status { get; set; }
 

    }
}


 

 
 
  
 
 
 
 
  