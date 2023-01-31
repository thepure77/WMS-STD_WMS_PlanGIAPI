using System;
using System.Collections.Generic;
using System.Text;

namespace PlanGIBusiness.PlanGoodIssue
{
    public class PlanGoodIssuePopupViewmodel : Pagination
    {

        public decimal? PlanGiBackOrder { get; set; }


        public decimal? PlanGiTotalQTY { get; set; }

        public Guid? PlanGoodsIssueIndex { get; set; }


        public string PlanGoodsIssueNo { get; set; }

        public Guid? RoundIndex { get; set; }


        public string RoundId { get; set; }

      
        public string RoundName { get; set; }

        public Guid? RouteIndex { get; set; }


        public string RouteId { get; set; }

        public string RouteName { get; set; }


    
        public Guid? OwnerIndex { get; set; }

    
        public string OwnerName { get; set; }


        public string OwnerId { get; set; }

        public Guid? WarehouseIndex { get; set; }

      
        public string WarehouseId { get; set; }

      
        public string WarehouseName { get; set; }

        public int? DocumentStatus { get; set; }

        public Guid SoldToIndex { get; set; }

     
        public string SoldToId { get; set; }

    
        public string SoldToName { get; set; }

        
        public string SoldToAddress { get; set; }

        public Guid ShipToIndex { get; set; }

    
        public string ShipToId { get; set; }
        
        public string ShipToName { get; set; }

        
        public string ShipToAddress { get; set; }

        public Guid? DocumentTypeIndex { get; set; }

    
        public string DocumentTypeId { get; set; }

      
        public string DocumentTypeName { get; set; }


        public string PlanGoodsIssueDate { get; set; }


        public string PlanGoodsIssueDueDate { get; set; }


        public string DocumentRefNo1 { get; set; }

        
        public string DocumentRefNo2 { get; set; }

    
        public string DocumentRefNo3 { get; set; }

    
        public string DocumentRefNo4 { get; set; }


        public string DocumentRefNo5 { get; set; }


        public string UDF1 { get; set; }


        public string UDF2 { get; set; }


        public string UDF3 { get; set; }


        public string UDF4 { get; set; }


        public string UDF5 { get; set; }

        public int? DocumentPriorityStatus { get; set; }

        public Guid? WarehouseIndexTo { get; set; }


        public string WarehouseIdTo { get; set; }


        public string WarehouseNameTo { get; set; }

        public Guid? SoldToSubDistrictIndex { get; set; }

        public Guid? SoldToDistrictIndex { get; set; }

        public Guid? SoldToProvinceIndex { get; set; }

        public Guid? SoldToCountryIndex { get; set; }

        public Guid? SoldToPostcodeIndex { get; set; }

        public Guid? SubDistrictIndex { get; set; }

        public Guid? DistrictIndex { get; set; }

        public Guid? ProvinceIndex { get; set; }

        public Guid? CountryIndex { get; set; }

        public Guid? PostcodeIndex { get; set; }


        public string CreateBy { get; set; }

        
        public string CreateDate { get; set; }

    
        public string UpdateBy { get; set; }

   
        public string UpdateDate { get; set; }


        public string CancelBy { get; set; }

        public string CancelDate { get; set; }

        public int? BackOrderStatus { get; set; }

        public string chk { get; set; }

        public Guid MarshallIndex { get; set; }


        public string MarshallNo { get; set; }
       

        
        public decimal? MSQTY { get; set; }

        public decimal? MSTotalQTY { get; set; }


        public decimal? PQTY { get; set; }

        
        public decimal? PTotaolQty { get; set; }

        public int count { get; set; }

        public class actionResultPlanGIPopupViewModel
        {
            public IList<PlanGoodIssuePopupViewmodel> itemsPlanGI { get; set; }
            public Pagination pagination { get; set; }
        }

    }
}
