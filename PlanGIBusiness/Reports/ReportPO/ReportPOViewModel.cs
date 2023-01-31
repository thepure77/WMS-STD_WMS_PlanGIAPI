using System;
using System.Collections.Generic;
using System.Text;

namespace PlanGIBusiness.Reports
{
    public class ReportPOViewModel
    {
        public Guid planGoodsIssue_Index { get; set; }
        public string planGoodsIssue_No { get; set; }
        public string planGoodsIssue_Date { get; set; }
        public string planGoodsIssue_Due_Date { get; set; }
        public Guid? owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public string owner_Address { get; set; }


        public string owner_TaxID { get; set; }
        public string contact_Tel { get; set; }
        public string owner_Fax { get; set; }
        public int? countRow { get; set; }
        public string date_Print { get; set; }
        
        public Guid? shipTo_Index { get; set; }
        public string shipTo_Id { get; set; }
        public string shipTo_Name { get; set; }
        public string shipTo_Address { get; set; }

        public string shipTo_TaxNo { get; set; }
        public string shipTo_Telephone { get; set; }
        public string shipTo_Fax { get; set; }
        public string lineNum { get; set; }
        public Guid? product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public string productConversion_Name { get; set; }
        public decimal? qty { get; set; }
        public string warehouse_Id { get; set; }
        public string warehouse_Name { get; set; }
        public string planGoodsIssue_Barcode { get; set; }

        public bool checkQuery { get; set; }
    }


}
