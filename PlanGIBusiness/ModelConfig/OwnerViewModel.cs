﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MasterDataBusiness.ViewModels
{
    public class OwnerViewModel
    {
        public Guid owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public string owner_SecondName { get; set; }
        public string owner_Address { get; set; }

        public Guid? ownerType_Index { get; set; }
        public string ownerType_Id { get; set; }
        public string ownerType_Name { get; set; }

        public Guid? province_Index { get; set; }
        public string province_Id { get; set; }
        public string province_Name { get; set; }
        public Guid? country_Index { get; set; }
        public string country_Id { get; set; }
        public string country_Name { get; set; }
        public Guid? subDistrict_Index { get; set; }
        public string subDistrict_Id { get; set; }
        public string subDistrict_Name { get; set; }
        public Guid? district_Index { get; set; }
        public string district_Id { get; set; }
        public string district_Name { get; set; }
        public Guid? postcode_Index { get; set; }
        public string postcode_Id { get; set; }
        public string postcode_Name { get; set; }

        public string owner_TaxID { get; set; }
        public string owner_Email { get; set; }
        public string owner_Fax { get; set; }
        public string owner_Tel { get; set; }
        public string owner_Mobile { get; set; }
        public string owner_Barcode { get; set; }
        public string contact_Person { get; set; }
        public string contact_Person2 { get; set; }
        public string contact_Person3 { get; set; }
        public string contact_Tel { get; set; }
        public string contact_Tel2 { get; set; }
        public string contact_Tel3 { get; set; }
        public string contact_Email { get; set; }
        public string contact_Email2 { get; set; }
        public string contact_Email3 { get; set; }

        public string ref_No1 { get; set; }
        public string ref_No2 { get; set; }
        public string ref_No3 { get; set; }
        public string ref_No4 { get; set; }
        public string ref_No5 { get; set; }
        public string remark { get; set; }
        public string udf_1 { get; set; }
        public string udf_2 { get; set; }
        public string udf_3 { get; set; }
        public string udf_4 { get; set; }
        public string udf_5 { get; set; }

        public int? isActive { get; set; }

        public int? isDelete { get; set; }

        public int? isSystem { get; set; }

        public int? status_Id { get; set; }

        public string create_By { get; set; }

        public DateTime? create_Date { get; set; }

        public string update_By { get; set; }

        public DateTime? update_Date { get; set; }

        public string cancel_By { get; set; }

        public DateTime? cancel_Date { get; set; }

        public string key { get; set; }

        public string value1 { get; set; }

    }
}
