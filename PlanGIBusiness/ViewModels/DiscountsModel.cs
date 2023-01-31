using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace PlanGIBusiness.ViewModels
{

    public class DiscountsModel
    {
        [Key]
        public Guid DiscountsIndex { get; set; }

        public Guid PlanGoodsIssueIndex { get; set; }

        public Guid PlanGoodsIssueItemIndex { get; set; }

        [StringLength(50)]
        public string DiscountsType { get; set; }

        [StringLength(50)]
        public string DiscountsBarcode { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DiscountsAmount { get; set; }
    }
}
