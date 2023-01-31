using System;
using System.Collections.Generic;
using System.Text;

namespace MasterDataBusiness.StorageLoc
{
    public class StorageLocViewModel
    {

        public Guid storageLoc_Index { get; set; }

        public string storageLoc_Id { get; set; }

        public string storageLoc_Name { get; set; }

        public Guid? warehouse_Index { get; set; }
        public Guid? warehouse_Index_To { get; set; }

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
