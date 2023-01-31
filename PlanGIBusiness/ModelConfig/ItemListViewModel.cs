using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MasterBusiness.PlanGoodsIssue
{

    public partial class ItemListViewModel
    {
        public Guid? index { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string key2 { get; set; }
        public string key3 { get; set; }
        public string key4 { get; set; }
        public string key5 { get; set; }
        public string address { get; set; }
        public int chk { get; set; }

        public string value1 { get; set; }
        public string value2 { get; set; }
        public string value3 { get; set; }
        public string value4 { get; set; }
        public Guid? value5 { get; set; }
        public string value6 { get; set; }
        public string value7 { get; set; }
    }
}
