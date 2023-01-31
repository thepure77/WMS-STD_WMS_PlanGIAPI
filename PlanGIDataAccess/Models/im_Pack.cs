using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class im_Pack
    {
        [Key]
        public Guid Pack_Index { get; set; }


        public string Pack_No { get; set; }

        public DateTime Pack_Date { get; set; }


        public Guid Owner_Index { get; set; }


        public string Owner_Id { get; set; }


        public string Owner_Name { get; set; }


        public Guid SoldTo_Index { get; set; }


        public string SoldTo_Id { get; set; }


        public string SoldTo_Name { get; set; }


        public string SoldTo_Address { get; set; }


        public Guid ShipTo_Index { get; set; }


        public string ShipTo_Id { get; set; }


        public string ShipTo_Name { get; set; }


        public string ShipTo_Address { get; set; }

        public Guid? DocumentType_Index { get; set; }


        public string DocumentType_Id { get; set; }


        public string DocumentType_Name { get; set; }

        public Guid? Round_Index { get; set; }


        public string Round_Id { get; set; }


        public string Round_Name { get; set; }


        public string Round_Time { get; set; }

        public Guid? Route_Index { get; set; }


        public string Route_Id { get; set; }


        public string Route_Name { get; set; }


        public string DocumentRef_No1 { get; set; }


        public string DocumentRef_No2 { get; set; }


        public string DocumentRef_No3 { get; set; }


        public string DocumentRef_No4 { get; set; }


        public string DocumentRef_No5 { get; set; }


        public string Document_Remark { get; set; }

        public int? Document_Status { get; set; }


        public string UDF_1 { get; set; }


        public string UDF_2 { get; set; }


        public string UDF_3 { get; set; }


        public string UDF_4 { get; set; }


        public string UDF_5 { get; set; }

        public Guid? SoldTo_SubDistrict_Index { get; set; }

        public Guid? SoldTo_District_Index { get; set; }

        public Guid? SoldTo_Province_Index { get; set; }

        public Guid? SoldTo_Country_Index { get; set; }

        public Guid? SoldTo_Postcode_Index { get; set; }

        public Guid? SubDistrict_Index { get; set; }

        public Guid? District_Index { get; set; }

        public Guid? Province_Index { get; set; }

        public Guid? Country_Index { get; set; }

        public Guid? Postcode_Index { get; set; }

        public int? PrintTaxInvoice { get; set; }


        public string Payment_Type { get; set; }


        public string Payment_Code { get; set; }


        public string SoldTo_Email { get; set; }


        public string SoldTo_Phone { get; set; }


        public string ShipTo_phone { get; set; }


        public string ShipTo_Remark { get; set; }


        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }


        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }


        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }

    }
}
