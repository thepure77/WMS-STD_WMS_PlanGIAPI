using System;
using System.Collections.Generic;
using System.Text;

namespace PlanGIBusiness.Demo
{
    public class DemoSORequestViewModel
    {
        public string wmsTrans_Id { get; set; }
        public string so_No { get; set; }
        public string so_Cha { get; set; }
        //public string do_Type { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string sub_district { get; set; }
        public string district { get; set; }
        public string province { get; set; }
        public string postCode { get; set; }
        //public string planGoodsIssue_Date { get; set; }
        //public string document_Status { get; set; }
        public string export_Case { get; set; }
        public string document_Remark { get; set; }
        //public string Creat_Date           { get; set; }
        public string creat_By { get; set; }
        //public string Update_Date          { get; set; }
        //public string update_By { get; set; }

        public List<DemoSOItem_RequestViewModel> items { get; set; }
    }

    public class DemoSOItem_RequestViewModel
    {
        public string line_Num { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public decimal? plan_QTY { get; set; }
        public string sale_Unit { get; set; }
    }

    public class DemoSOResponseViewModel
    {
        public string stauts { get; set; }
        public string message { get; set; }
    }
}
