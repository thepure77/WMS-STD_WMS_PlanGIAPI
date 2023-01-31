using System;
using System.Collections.Generic;

namespace planGIBusiness.PlanGoodsIssue
{
    public partial class BinbalanceViewModel
    {
        public string Product_Id { get; set; }
        public string Product_Lot { get; set; }
        public Guid? ItemStatus_Index { get; set; }
        public string erp_Location { get; set; }
        public decimal? binBalance_QtyBal { get; set; }
        public decimal? binBalance_QtyReserve { get; set; }
        public decimal? binBalance_QtyBegin { get; set; }


    }

}

