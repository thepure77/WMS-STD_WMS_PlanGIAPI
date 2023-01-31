using System;
using System.Collections.Generic;
using System.Text;

namespace MasterDataBusiness.CostCenter
{
    public class CostCenterViewModel
    {

        public Guid costCenter_Index { get; set; }


        public string costCenter_Id { get; set; }


        public string costCenter_Name { get; set; }


        public string costCenter_Description { get; set; }


        public int? isActive { get; set; }

        public int? isDelete { get; set; }


        public string create_By { get; set; }


        public DateTime? create_Date { get; set; }


        public string update_By { get; set; }


        public DateTime? update_Date { get; set; }


        public string cancel_By { get; set; }


        public DateTime? cancel_Date { get; set; }
    }
}
