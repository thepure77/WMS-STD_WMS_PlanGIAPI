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
        public string receiveName { get; set; }
        public string receiveAddress { get; set; }
        public string receiveSubDistrict { get; set; }
        public string receiveDistrict { get; set; }
        public string receiveProvince { get; set; }
        public string receivePostCode { get; set; }
        public string receiveTel { get; set; }
        //public string planGoodsIssue_Date { get; set; }
        //public string document_Status { get; set; }

        public string senderName { get; set; }
        public string senderAddress { get; set; }
        public string senderSubDistrict { get; set; }
        public string senderDistrict   { get; set; }
        public string senderProvince { get; set; }
        public string senderPostCode { get; set; }
        public string senderTel { get; set; }


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
        public string document_No { get; set; }
        public int status { get; set; }
        public string message { get; set; }
    }
}
