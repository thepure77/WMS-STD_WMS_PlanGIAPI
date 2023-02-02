using Business.Library;
using Comone.Utils;
using DataAccess;
using MasterDataBusiness.ViewModels;
using PlanGIDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanGIBusiness.Demo
{
    public class DemoService
    {
        private PlanGIDbContext db;

        public DemoService()
        {
            db = new PlanGIDbContext();
        }

        public DemoService(PlanGIDbContext db)
        {
            this.db = db;
        }

        public DemoSOResponseViewModel CreateSO(DemoSORequestViewModel param)
        {
            var result = new DemoSOResponseViewModel();
            string message = "";
            var productList = new List<ProductViewModel>();
            var conversionList = new List<ProductConversionViewModelDoc>();

            string state = "0";
            try
            {
                var chkreq = CheckReq_SO(param);
                if (chkreq != "")
                {
                    result.stauts = "-1";
                    result.message = chkreq;
                }
                else { }

                foreach (var i in param.items)
                {
                    i.product_Id = i.product_Id.TrimStart(new Char[] { '0' });
                    i.product_Id = i.product_Id.Trim(new Char[] { ' ', ' ' });

                    var ProductfilterModel = new ProductViewModel();
                    ProductfilterModel.product_Id = i.product_Id;
                    //GetConfig
                    var productMasterResult = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("product"), ProductfilterModel.sJson());
                    productList.Add(productMasterResult.FirstOrDefault());
                    if (productList.Count == 0)
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            message += " | ";
                        }
                        message += "Product Id " + i.product_Id + " Error : Product not found";
                        continue;
                    }

                    state = "1";

                    var conversionModel = new ProductConversionViewModelDoc();
                    conversionModel.productConversion_Name = i.sale_Unit;
                    conversionModel.product_Index = productMasterResult[0].product_Index ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
                    var conversionMasterResult = utils.SendDataApi<List<ProductConversionViewModelDoc>>(new AppSettingConfig().GetUrl("dropdownProductconversion"), conversionModel.sJson());
                    conversionList.Add(conversionMasterResult.FirstOrDefault());
                    if (conversionList.Count == 0)
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            message += " | ";
                        }
                        message += "Product Id " + i.product_Id + " Error : Product Conversion not found";
                        continue;
                    }
                }

                state = "2";
                if (message != "")
                {
                    result.stauts = "-1";
                    result.message = message;
                    return result;
                }
                else
                {
                    var plan = db.im_PlanGoodsIssue.Where(c => c.PlanGoodsIssue_No == param.so_No).FirstOrDefault();
                    if (plan != null)
                    {
                        result.stauts = "-1";
                        result.message = "Order Duplicate";
                        return result;
                    }

                    var genList = new List<GenDocumentTypeViewModel>();
                    var genmodel = new GenDocumentTypeViewModel();

                    var url = new AppSettingConfig().GetUrl("dropDownDocumentType");
                    var docrequest = new GenDocumentTypeViewModel();
                    docrequest.documentType_Index = Guid.Parse("7A0710B4-DAD8-47DD-8424-2AB50B8D37A8");
                    //docrequest.documentType_Id = "PK10";
                    docrequest.process_Index = Guid.Parse("80E8E627-6A2D-4075-9BA6-04B7178C1203");
                    var doctype = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), docrequest.sJson());

                    DateTime DocumentDate = Convert.ToDateTime(DateTime.Now);
                    im_PlanGoodsIssue head = new im_PlanGoodsIssue();
                    head.PlanGoodsIssue_Index = Guid.NewGuid();
                    head.PlanGoodsIssue_No = param.so_No;
                    head.DocumentType_Index = doctype[0].documentType_Index;
                    head.DocumentType_Id = doctype[0].documentType_Id;
                    head.DocumentType_Name = doctype[0].documentType_Name;
                    head.Owner_Index = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    head.SoldTo_Index = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    head.ShipTo_Index = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    head.ShipTo_Name = param.name;
                    head.ShipTo_Address = param.address;
                    head.SubDistrict_Name = param.sub_district;
                    head.District_Name = param.district;
                    head.Province_Name = param.province;
                    head.Postcode_Name = param.postCode;
                    head.PlanGoodsIssue_Date = DocumentDate;
                    //head.PlanGoodsIssue_Due_Date = DocumentDate;
                    head.Document_Status = 0;

                    head.Document_Remark = param.document_Remark;
                    head.Create_Date = DateTime.Now;
                    head.Create_By = param.create_By;
                    //head.Update_Date = DateTime.Now;
                    //head.Update_By = param.update_By;
                    head.Transaction_Id = param.wmsTrans_Id;
                    head.Shipping_Channel = param.so_Cha;
                    head.Export_Case = param.export_Case;

                    //head.ShippingMethod_Index = Guid.Parse("F6799861-0A44-42B3-8F9B-49CE89448B2A");
                    //head.ShippingMethod_Id = "DD";
                    //head.ShippingMethod_Name = "ส่งตามรอบ";
                    //head.ShippingTerms_Index = Guid.Parse("70021789-6AEA-42E1-8478-4862F552845B");
                    //head.ShippingTerms_Id = "O";
                    //head.ShippingTerms_Name = "คิดค่าขนส่งปกติ";

                    db.im_PlanGoodsIssue.Add(head);

                    state = "3";
                    int i = 0;
                    foreach (var item in param.items)
                    {
                        im_PlanGoodsIssueItem planItem = new im_PlanGoodsIssueItem();
                        planItem.PlanGoodsIssueItem_Index = Guid.NewGuid();
                        planItem.PlanGoodsIssue_Index = head.PlanGoodsIssue_Index;
                        planItem.LineNum = item.line_Num;
                        planItem.Product_Index = productList[i].product_Index;
                        planItem.Product_Id = productList[i].product_Id;
                        planItem.Product_Name = productList[i].product_Name;
                        planItem.Qty = Convert.ToDecimal(item.plan_QTY);
                        planItem.ProductConversion_Index = conversionList[i].productConversion_Index;
                        planItem.ProductConversion_Id = conversionList[i].productConversion_Id;
                        planItem.ProductConversion_Name = conversionList[i].productConversion_Name;

                        state = "4";

                        planItem.Ratio = Convert.ToDecimal(conversionList[i].productconversion_Ratio);
                        planItem.TotalQty = Convert.ToDecimal(item.plan_QTY) * conversionList[i].productconversion_Ratio;
                        planItem.UnitWeight = conversionList[i].productConversion_Weight;
                        planItem.Weight = conversionList[i].productConversion_Weight * Convert.ToDecimal(item.plan_QTY);
                        planItem.NetWeight = conversionList[i].productConversion_Weight * Convert.ToDecimal(item.plan_QTY);
                        planItem.UnitGrsWeight = conversionList[i].productConversion_GrsWeight;
                        planItem.GrsWeight = conversionList[i].productConversion_GrsWeight * Convert.ToDecimal(item.plan_QTY);
                        planItem.UnitWidth = conversionList[i].productConversion_Width;
                        planItem.Width = conversionList[i].productConversion_Width * Convert.ToDecimal(item.plan_QTY);
                        planItem.UnitLength = conversionList[i].productConversion_Length;
                        planItem.Length = conversionList[i].productConversion_Length * Convert.ToDecimal(item.plan_QTY);
                        planItem.UnitHeight = conversionList[i].productConversion_Height;
                        planItem.Height = conversionList[i].productConversion_Height * Convert.ToDecimal(item.plan_QTY);
                        planItem.ItemStatus_Index = Guid.Parse("525BCFF1-2AD9-4ACB-819D-0DEA4E84EA12");
                        planItem.ItemStatus_Id = "10";
                        planItem.ItemStatus_Name = "Goods-UR";
                        planItem.NetWeight_Index = Guid.Parse("080AEF7B-E9C5-4B84-969A-2D033F0C1E2A");
                        planItem.NetWeight_Id = "1";
                        planItem.NetWeight_Name = "KG";
                        planItem.NetWeightRatio = 1;
                        planItem.Weight_Index = Guid.Parse("080AEF7B-E9C5-4B84-969A-2D033F0C1E2A");
                        planItem.Weight_Id = "1";
                        planItem.Weight_Name = "KG";
                        planItem.WeightRatio = 1;
                        planItem.GrsWeight_Index = Guid.Parse("080AEF7B-E9C5-4B84-969A-2D033F0C1E2A");
                        planItem.GrsWeight_Id = "1";
                        planItem.GrsWeight_Name = "KG";
                        planItem.GrsWeightRatio = 1;
                        planItem.Width_Index = Guid.Parse("3778CD6E-45ED-499A-8ACC-9EB1F3AB1A6A");
                        planItem.Width_Id = "2";
                        planItem.Width_Name = "CM";
                        planItem.WidthRatio = 1;
                        planItem.Height_Index = Guid.Parse("3778CD6E-45ED-499A-8ACC-9EB1F3AB1A6A");
                        planItem.Height_Id = "2";
                        planItem.Height_Name = "CM";
                        planItem.HeightRatio = 1;
                        planItem.Length_Index = Guid.Parse("3778CD6E-45ED-499A-8ACC-9EB1F3AB1A6A");
                        planItem.Length_Id = "2";
                        planItem.Length_Name = "CM";
                        planItem.LengthRatio = 1;

                        planItem.UnitNetWeight_Index = Guid.Parse("080AEF7B-E9C5-4B84-969A-2D033F0C1E2A");
                        planItem.UnitNetWeight = conversionList[i].productConversion_Weight;
                        planItem.UnitNetWeight_Id = "1";
                        planItem.UnitNetWeight_Name = "KG";
                        planItem.UnitNetWeightRatio = 1;
                        planItem.UnitGrsWeight_Index = Guid.Parse("080AEF7B-E9C5-4B84-969A-2D033F0C1E2A");
                        planItem.UnitGrsWeight_Id = "1";
                        planItem.UnitGrsWeight_Name = "KG";
                        planItem.UnitGrsWeightRatio = 1;
                        planItem.UnitWidth_Index = Guid.Parse("3778CD6E-45ED-499A-8ACC-9EB1F3AB1A6A");
                        planItem.UnitWidth_Id = "2";
                        planItem.UnitWidth_Name = "CM";
                        planItem.UnitWidthRatio = 1;
                        planItem.UnitLength_Index = Guid.Parse("3778CD6E-45ED-499A-8ACC-9EB1F3AB1A6A");
                        planItem.UnitLength_Id = "2";
                        planItem.UnitLength_Name = "CM";
                        planItem.UnitLengthRatio = 1;
                        planItem.UnitHeight_Index = Guid.Parse("3778CD6E-45ED-499A-8ACC-9EB1F3AB1A6A");
                        planItem.UnitHeight_Id = "2";
                        planItem.UnitHeight_Name = "CM";
                        planItem.UnitHeightRatio = 1;
                        planItem.UnitWeight_Index = Guid.Parse("080AEF7B-E9C5-4B84-969A-2D033F0C1E2A");
                        planItem.UnitWeight_Id = "1";
                        planItem.UnitWeight_Name = "KG";
                        planItem.UnitWeightRatio = 1;

                        state = "4";

                        var width = (planItem.UnitWidth ?? 0);
                        var Length = (planItem.UnitLength ?? 0);
                        var Height = (planItem.UnitHeight ?? 0);
                        var unitVolume = (width * Length * Height);
                        planItem.UnitVolume = unitVolume;
                        planItem.Volume = ((planItem.TotalQty ?? 0) * (unitVolume / (planItem.UnitHeightRatio ?? 1)));
                        planItem.Create_By = param.create_By;
                        planItem.Create_Date = DateTime.Now;
                        planItem.Create_Date = DateTime.Now;
                        i++;
                        db.im_PlanGoodsIssueItem.Add(planItem);
                    }
                }

                state = "5";
                db.SaveChanges();

                result.stauts = "1";
                result.message = "SUCCESS";

                return result;
            }
            catch (Exception ex)
            {
                result.stauts = "0";
                result.message = state + " : " + ex.Message;
                return result;
            }

        }

        public string CheckReq_SO(DemoSORequestViewModel param)
        {
            var result = "";

            if (string.IsNullOrEmpty(param.wmsTrans_Id))
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += "|";
                }
                else
                {
                    result += " wmsTrans_Id is empty";
                    return result;
                }
            }
            else { }

            if (string.IsNullOrEmpty(param.so_No))
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += "|";
                }
                else
                {
                    result += " SO_No is empty";
                    return result;
                }
            }
            else { }

            if (string.IsNullOrEmpty(param.so_Cha))
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += "|";
                }
                else
                {
                    result += " SO_Cha is empty";
                    return result;
                }
            }
            else { }

            if (param.items == null)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += "|";
                }
                else
                {
                    result += " items is empty";
                    return result;
                }
            }
            else { }

            if (param.items.Count == 0)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += "|";
                }
                else
                {
                    result += " items is emptyd";
                    return result;
                }
            }
            else { }

            if (string.IsNullOrEmpty(param.create_By))
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += "|";
                }
                else
                {
                    result += " SO_Cha is empty";
                    return result;
                }
            }
            else { }


            foreach (var item in param.items)
            {
                if (string.IsNullOrEmpty(item.line_Num))
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        result += "|";
                    }
                    else
                    {
                        result += " Line_Num is empty";
                        return result;
                    }
                }
                else { }

                if (string.IsNullOrEmpty(item.product_Id))
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        result += "|";
                    }
                    else
                    {
                        result += " Product_Id is empty";
                        return result;
                    }
                }
                else { }

                if (string.IsNullOrEmpty(item.product_Name))
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        result += "|";
                    }
                    else
                    {
                        result += " Product_Name is empty";
                        return result;
                    }
                }
                else { }

                if (item.plan_QTY == null || item.plan_QTY == 0)
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        result += "|";
                    }
                    else
                    {
                        result += " Plan_QTY is empty";
                        return result;
                    }
                }
                else { }

                if (string.IsNullOrEmpty(item.sale_Unit))
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        result += "|";
                    }
                    else
                    {
                        result += " Sale_Unit is empty";
                        return result;
                    }
                }
                else { }
            }
            return result;
        }
    }
}
