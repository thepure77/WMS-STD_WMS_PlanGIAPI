using System;
using System.Collections.Generic;
using System.Text;

namespace PlanGIBusiness.PlanGoodIssue
{
    public class PlanGoodIssueUpdateReasonModel
    {
        public Guid PlanGoodsIssueIndex { get; set; }

        public Guid ReasonCodeIndex { get; set; }
        public string ReasonCodeId { get; set; }
        public string ReasonCodeName { get; set; }
        public string UserName { get; set; }

    }
}
