using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlanGIDataAccess.Models
{
    public class im_TruckLoadItem
    {
        [Key]
        public Guid TruckLoadItem_Index { get; set; }

        public Guid TruckLoad_Index { get; set; }

        public Guid? GoodsIssue_Index { get; set; }

        public string GoodsIssue_No { get; set; }

        public string DocumentRef_No1 { get; set; }

        public string DocumentRef_No2 { get; set; }

        public string DocumentRef_No3 { get; set; }

        public string DocumentRef_No4 { get; set; }

        public string DocumentRef_No5 { get; set; }

        public string Document_Remark { get; set; }

        public int? Document_Status { get; set; }

        public string UDF_1 { get; set; }

        public string UDF_2 { get; set; }

        public string UDF_3 { get; set; }

        public string UDF_4 { get; set; }

        public string UDF_5 { get; set; }

        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }

        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }

        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }
        public Guid? PlanGoodsIssue_Index { get; set; }

        public string PlanGoodsIssue_No { get; set; }
        public int? Is_AMZ { get; set; }
        public DateTime? Expect_Delivery_Date { get; set; }
    }
}
