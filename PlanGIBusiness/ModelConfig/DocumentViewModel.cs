using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PlanGIBusiness.PlanGoodIssue
{
    public class DocumentViewModel
    {
        [Key]
        public Guid? documentItem_Index { get; set; }

        public Guid? document_Index { get; set; }

        public int? document_Status { get; set; }

        public int? whereDocument_Status { get; set; }
        public string document_No { get; set; }

        public List<DocumentViewModel> listDocumentViewModel { get; set; }

    }
}
