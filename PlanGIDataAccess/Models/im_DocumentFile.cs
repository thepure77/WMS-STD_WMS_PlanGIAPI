using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIDataAccess.Models
{
    public partial class im_DocumentFile
    {
        [Key]
        public Guid DocumentFile_Index { get; set; }
        public string DocumentFile_Name { get; set; }
        public string DocumentFile_Path { get; set; }
        public string DocumentFile_Url { get; set; }
        public string DocumentFile_Type { get; set; }

        public int? DocumentFile_Status { get; set;}

        [StringLength(200)]
        public string Create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Create_Date { get; set; }

        [StringLength(200)]
        public string Update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Update_Date { get; set; }

        [StringLength(200)]
        public string Cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Cancel_Date { get; set; }
        public Guid? Ref_Index { get; set; }
        public string Ref_No { get; set; }
    }
}
