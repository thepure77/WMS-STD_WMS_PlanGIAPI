using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlanGIBusiness.PlanGoodIssue
{
    public class FindPlanGoodIssueItem
    {
        [Key]
        public Guid PlanGoodsIssueItemIndex { get; set; }

        public Guid PlanGoodsIssueIndex { get; set; }

        public Guid? ProductIndex { get; set; }        

        public int? IsDelete { get; set; }
    }
}
