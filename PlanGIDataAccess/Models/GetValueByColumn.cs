using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GIDataAccess.Models
{
    public partial class GetValueByColumn
    {
        [Key]
        [StringLength(2000)]
        public string dataincolumn1 { get; set; }
        [StringLength(2000)]
        public string dataincolumn2 { get; set; }
        [StringLength(2000)]
        public string dataincolumn3 { get; set; }
        [StringLength(2000)]
        public string dataincolumn4 { get; set; }
        [StringLength(2000)]
        public string dataincolumn5 { get; set; }

    }
}
