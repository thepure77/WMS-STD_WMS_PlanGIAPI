using System;
using System.Collections.Generic;
using System.Text;

namespace PlanGIBusiness.PlanGoodIssue
{
    public class PlanGoodIssueSearchDetailModel : Pagination
    {

        public Guid PlanGoodsIssueIndex { get; set; }

        public Guid OwnerIndex { get; set; }
        
        public string OwnerId { get; set; }
        
        public string OwnerName { get; set; }

        public string PlanGoodsIssueNo { get; set; }

        public string PlanGoodsIssueDate { get; set; }

        public string PlanGoodsIssueDateTo { get; set; }
        public string PlanGoodsIssueDueDate { get; set; }
        public string PlanGoodsIssueDueDateTo { get; set; }

        public Guid SoldToIndex { get; set; }

        public string SoldToId { get; set; }

        public string SoldToName { get; set; }

        public string SoldToAddress { get; set; }

        public Guid ShipToIndex { get; set; }

        public string ShipToId { get; set; }

        public string ShipToName { get; set; }

        public string ShipToAddress { get; set; }

        public Guid? WarehouseIndex { get; set; }

        public string WarehouseId { get; set; }

        public string WarehouseName { get; set; }

        public Guid? WarehouseIndexTo { get; set; }

        public string WarehouseIdTo { get; set; }

        public string WarehouseNameTo { get; set; }

        public Guid? RouteIndex { get; set; }

        public Guid? SubRouteIndex { get; set; }

        public string RouteId { get; set; }

        public string RouteName { get; set; }

        public Guid? RoundIndex { get; set; }

        public string RoundId { get; set; }

        public string RoundName { get; set; }

        public Guid? DocumentTypeIndex { get; set; }
 
        public string DocumentTypeId { get; set; }

        public string DocumentTypeName { get; set; }

        public string DocumentRefNo1 { get; set; }

        public string DocumentRefNo2 { get; set; }

        public string DocumentRefNo3 { get; set; }

        public string UDF1 { get; set; }

        public string UDF2 { get; set; }

        public string UDF3 { get; set; }

        public int? DocumentStatus { get; set; }
        public string DocumentRemark { get; set; }

        public string ProcessStatusName { get; set; }

        public string ColumnName { get; set; }

        public string Orderby { get; set; }
        public string CreateDate { get; set; }
        public string SoldToNameCus { get; set; }

    }

    public class actionResultPlanGIViewModel 
    {
        public IList<PlanGoodIssueSearchDetailModel> items { get; set; }
        public IList<PlanGoodIssueViewModelItem> listitems { get; set; }

        public Pagination pagination { get; set; }
    }


   
}
