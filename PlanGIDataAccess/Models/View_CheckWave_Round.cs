using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace PlanGIDataAccess.Models
{

    public partial class View_CheckWave_Round
    {
        [Key]
        public long? RowIndex { get; set; }

        public string Round_Id { get; set; }

        public string Round_Name { get; set; }

        public DateTime? PlanGoodsIssue_Due_Date { get; set; }

        public int? countPlangi { get; set; }

        public int? CountPlanGIItem { get; set; }

        public int? CalMin { get; set; }
    }
}
