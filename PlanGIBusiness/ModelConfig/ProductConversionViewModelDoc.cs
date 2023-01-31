using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PlanGIBusiness;

namespace MasterDataBusiness.ViewModels
{


    public  class ProductConversionViewModelDoc : Pagination
    {
        public Guid productConversion_Index { get; set; }

        public Guid product_Index { get; set; }


        public string productConversion_Id { get; set; }


        public string productConversion_Name { get; set; }


        public decimal? productconversion_Ratio { get; set; }


        public decimal? productConversion_Weight { get; set; }

        public Guid? productConversion_Weight_Index { get; set; }


        public string productConversion_Weight_Id { get; set; }


        public string productConversion_Weight_Name { get; set; }


        public decimal? productConversion_WeightRatio { get; set; }


        public decimal? productConversion_GrsWeight { get; set; }

        public Guid? productConversion_GrsWeight_Index { get; set; }


        public string productConversion_GrsWeight_Id { get; set; }


        public string productConversion_GrsWeight_Name { get; set; }


        public decimal? productConversion_GrsWeightRatio { get; set; }


        public decimal? productConversion_Width { get; set; }

        public Guid? productConversion_Width_Index { get; set; }


        public string productConversion_Width_Id { get; set; }


        public string productConversion_Width_Name { get; set; }


        public decimal? productConversion_WidthRatio { get; set; }


        public decimal? productConversion_Length { get; set; }

        public Guid? productConversion_Length_Index { get; set; }


        public string productConversion_Length_Id { get; set; }


        public string productConversion_Length_Name { get; set; }


        public decimal? productConversion_LengthRatio { get; set; }


        public decimal? productConversion_Height { get; set; }

        public Guid? productConversion_Height_Index { get; set; }


        public string productConversion_Height_Id { get; set; }


        public string productConversion_Height_Name { get; set; }


        public decimal? productConversion_HeightRatio { get; set; }


        public decimal? productConversion_Volume { get; set; }

        public Guid? productConversion_Volume_Index { get; set; }


        public string productConversion_Volume_Id { get; set; }


        public string productConversion_Volume_Name { get; set; }


        public decimal productConversion_VolumeRatio { get; set; }


        public string ref_No1 { get; set; }


        public string ref_No2 { get; set; }


        public string ref_No3 { get; set; }


        public string ref_No4 { get; set; }


        public string ref_No5 { get; set; }


        public string remark { get; set; }


        public string uDF_1 { get; set; }


        public string uDF_2 { get; set; }


        public string uDF_3 { get; set; }


        public string uDF_4 { get; set; }


        public string uDF_5 { get; set; }

        public int isActive { get; set; }

        public int isDelete { get; set; }

        public int issystem { get; set; }

        public int status_id { get; set; }


        public string create_By { get; set; }


        public DateTime? create_Date { get; set; }


        public string update_By { get; set; }


        public DateTime? update_Date { get; set; }


        public string cancel_By { get; set; }


        public DateTime? cancel_Date { get; set; }
    }

    public class actionResultProductConversionViewModel
    {
        public IList<ProductConversionViewModelDoc> items { get; set; }
        public Pagination pagination { get; set; }
    }
}
