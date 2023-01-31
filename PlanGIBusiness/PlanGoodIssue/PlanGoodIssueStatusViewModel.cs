using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlanGIBusiness.PlanGoodIssue
{
    public class PlanGoodIssueStatusViewModel
    {
        public Guid PlanGoodsIssueIndex { get; set; }

        [StringLength(200)]
        public string PlanGoodsIssueNo { get; set; }

        public int? DocumentStatus { get; set; }

        [StringLength(200)]
        public string CreateBy { get; set; }       
    }
}
