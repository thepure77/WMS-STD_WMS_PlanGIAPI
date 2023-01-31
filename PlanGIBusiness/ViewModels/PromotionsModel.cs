using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace PlanGIBusiness.ViewModels
{

    public class PromotionsModel
    {
        [Key]
        public Guid PromotionsIndex { get; set; }

        public Guid? PlanGoodsIssueIndex { get; set; }

        public string PromotionsLink { get; set; }

        public string PromotionsType { get; set; }

        public string PromotionsBarcode { get; set; }

        public decimal? PromotionsAmount { get; set; }

        public string PromotionsApplied { get; set; }

        public decimal? PromotionsCount { get; set; }

        public string PromotionsStep { get; set; }


    }
}
