using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PlanGIBusiness.PlanGoodIssue
{
    public class PlanGoodIssueWaveViewModel
    {
        public Guid PlanGoodsIssueIndex { get; set; }

        public Guid OwnerIndex { get; set; }

        [StringLength(50)]
        public string OwnerId { get; set; }

        [StringLength(50)]
        public string OwnerName { get; set; }

        public Guid SoldToIndex { get; set; }

        [StringLength(50)]
        public string SoldToId { get; set; }

        [StringLength(200)]
        public string SoldToName { get; set; }

        [StringLength(200)]
        public string SoldToAddress { get; set; }

        public Guid ShipToIndex { get; set; }

        [StringLength(50)]
        public string ShipToId { get; set; }

        [StringLength(200)]
        public string ShipToName { get; set; }

        [StringLength(200)]
        public string ShipToAddress { get; set; }

        public Guid? DocumentTypeIndex { get; set; }

        [StringLength(50)]
        public string DocumentTypeId { get; set; }

        [StringLength(200)]
        public string DocumentTypeName { get; set; }

        [Required]
        [StringLength(50)]
        public string PlanGoodsIssueNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public string PlanGoodsIssueDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public string PlanGoodsIssueDueDate { get; set; }

        [StringLength(200)]
        public string DocumentRefNo1 { get; set; }

        [StringLength(200)]
        public string DocumentRefNo2 { get; set; }

        [StringLength(200)]
        public string DocumentRefNo3 { get; set; }

        [StringLength(200)]
        public string DocumentRefNo4 { get; set; }

        [StringLength(200)]
        public string DocumentRefNo5 { get; set; }

        public int? DocumentStatus { get; set; }

        [StringLength(200)]
        public string UDF1 { get; set; }

        [StringLength(200)]
        public string UDF2 { get; set; }

        [StringLength(200)]
        public string UDF3 { get; set; }

        [StringLength(200)]
        public string UDF4 { get; set; }

        [StringLength(200)]
        public string UDF5 { get; set; }

        public int? DocumentPriorityStatus { get; set; }

        public Guid? WarehouseIndex { get; set; }

        [StringLength(50)]
        public string WarehouseId { get; set; }

        [StringLength(200)]
        public string WarehouseName { get; set; }

        public Guid? WarehouseIndexTo { get; set; }

        [StringLength(50)]
        public string WarehouseIdTo { get; set; }

        [StringLength(200)]
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

        [StringLength(200)]
        public string CreateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public string CreateDate { get; set; }

        [StringLength(200)]
        public string UpdateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public string UpdateDate { get; set; }

        [StringLength(200)]
        public string CancelBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public string CancelDate { get; set; }

        public List<PlanGoodIssueViewModelItem> listPlanGoodIssueViewModelItem { get; set; }

    }
}
