
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace planGoodsIssueBusiness.GoodsReceive
{

    public partial class im_Signatory_logViewModel
    {

        [Key]
        public Guid signatory_Index { get; set; }

        public string signatoryType_Id { get; set; }

        public string signatoryType_Name { get; set; }
        public Guid? user_Index { get; set; }

        public string user_Id { get; set; }

        public string user_Name { get; set; }

        public string first_Name { get; set; }


        public string last_Name { get; set; }


        public string position_Name { get; set; }


        public string position_Code { get; set; }

        public Guid documentType_Index { get; set; }

        public string documentType_Id { get; set; }


        public string documentType_Name { get; set; }

        public Guid? ref_Document_Index { get; set; }


        public string ref_Document_No { get; set; }


        public string remark { get; set; }

        public int? isActive { get; set; }

        public int? isDelete { get; set; }

        public int? isSystem { get; set; }

        public int? status_Id { get; set; }


        //public string create_By { get; set; }

        //public DateTime? create_Date { get; set; }


        //public string update_By { get; set; }

        //public DateTime? update_Date { get; set; }


        //public string cancel_By { get; set; }

        //public DateTime? cancel_Date { get; set; }
        public string planGoodsIssue_No { get; set; }

    }
}
