using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
  

    public partial class im_Signatory_log
    {
        [Key]
        public Guid Signatory_Index { get; set; }

        public string SignatoryType_Id { get; set; }

        public string SignatoryType_Name { get; set; }
        public Guid? User_Index { get; set; }
        
        public string User_Id { get; set; }

        public string User_Name { get; set; }

        public string First_Name { get; set; }


        public string Last_Name { get; set; }


        public string Position_Name { get; set; }


        public string Position_Code { get; set; }

        public Guid DocumentType_Index { get; set; }

        public string DocumentType_Id { get; set; }


        public string DocumentType_Name { get; set; }

        public Guid? Ref_Document_Index { get; set; }


        public string Ref_Document_No { get; set; }


        public string Remark { get; set; }

        public int? IsActive { get; set; }

        public int? IsDelete { get; set; }

        public int? IsSystem { get; set; }

        public int? Status_Id { get; set; }


        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }


        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }


        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }
    }
}
