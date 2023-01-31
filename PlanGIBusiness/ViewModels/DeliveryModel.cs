using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace PlanGIBusiness.ViewModels
{

    public class DeliveryModel
    {

        [Key]
        public Guid DeliveryIndex { get; set; }

        public Guid? PlanGoodsIssueIndex { get; set; }

        [StringLength(50)]
        public string DeliveryBarcode { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DeliveryAmount { get; set; }
    }
}
