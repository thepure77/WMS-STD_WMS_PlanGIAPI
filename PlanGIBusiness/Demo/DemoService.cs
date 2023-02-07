using Business.Library;
using Comone.Utils;
using DataAccess;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using planGIBusiness.AutoNumber;
using PlanGIBusiness.ModelConfig;
using PlanGIDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
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

        public DemoCallbackViewModel CreateSO(DemoSORequestViewModel param)
        {
            var result = new DemoCallbackViewModel();
            string message = "";
            var productList = new List<ProductViewModel>();
            var conversionList = new List<ProductConversionViewModelDoc>();

            string state = "0";
            try
            {
                result.referenceNo = param.so_No;
                result.status = "100";
                result.statusAfter = "101";
                result.statusBefore = "000";
                result.statusDesc = "Order API";
                result.statusDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
                var chkreq = CheckReq_SO(param);

                result.status = "101";
                result.statusAfter = "102";
                result.statusBefore = "100";
                result.statusDesc = "รอยืนยัน";

                if (chkreq != "")
                {
                    
                    result.statusDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
                    result.statusDesc = chkreq;
                    Callback_OMS(result);
                    return result;
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
                        message += " Error : Product " + i.product_Id + " not found";
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
                        message += " Error : Product " + i.product_Id + " Conversion not found";
                        continue;
                    }
                }

                state = "2";
                if (message != "")
                {
                    
                    result.statusDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
                    result.statusDesc = message;
                    Callback_OMS(result);
                    return result;
                }
                else
                {
                    var plan = db.im_PlanGoodsIssue.Where(c => c.PlanGoodsIssue_No == param.so_No).FirstOrDefault();
                    if (plan != null)
                    {
                        
                        result.statusDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
                        result.statusDesc = "Order Duplicate";
                        Callback_OMS(result);
                        return result;
                    }

                    //var tran = db.im_PlanGoodsIssue.Where(c => c.Transaction_Id == param.wmsTrans_Id).FirstOrDefault();
                    //if (tran != null)
                    //{
                    //    result.stauts = "-1";
                    //    result.message = "wmsTrans_Id Duplicate";
                    //    return result;
                    //}

                    var url = new AppSettingConfig().GetUrl("dropDownDocumentType");
                    var docrequest = new GenDocumentTypeViewModel();
                    docrequest.documentType_Index = Guid.Parse("7A0710B4-DAD8-47DD-8424-2AB50B8D37A8");
                    //docrequest.documentType_Id = "PK10";
                    docrequest.process_Index = Guid.Parse("80E8E627-6A2D-4075-9BA6-04B7178C1203");
                    var doctype = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), docrequest.sJson());

                    var OwnerModel = new OwnerViewModel();
                    var urlOwner = new AppSettingConfig().GetUrl("dropdownOwner");
                    var resultOwner = utils.SendDataApi<List<OwnerViewModel>>(urlOwner, OwnerModel.sJson());
                    var DataOwner = resultOwner.Find(c => c.owner_Index == Guid.Parse("02B31868-9D3D-448E-B023-05C121A424F4"));

                    DateTime DocumentDate = Convert.ToDateTime(DateTime.Now);
                    im_PlanGoodsIssue head = new im_PlanGoodsIssue();
                    head.PlanGoodsIssue_Index = Guid.NewGuid();
                    head.PlanGoodsIssue_No = param.so_No;
                    head.DocumentType_Index = doctype[0].documentType_Index;
                    head.DocumentType_Id = doctype[0].documentType_Id;
                    head.DocumentType_Name = doctype[0].documentType_Name;
                    //head.Owner_Index = Guid.Parse("00000000-0000-0000-0000-000000000000");

                    head.Owner_Index = DataOwner.owner_Index;
                    head.Owner_Id = DataOwner.owner_Id;
                    head.Owner_Name = DataOwner.owner_Name;

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
                    head.Create_By = param.creat_By;
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
                        planItem.PlanGoodsIssue_No = head.PlanGoodsIssue_No;
                        planItem.LineNum = item.line_Num;
                        planItem.Product_Index = productList[i].product_Index;
                        planItem.Product_Id = productList[i].product_Id;
                        planItem.Product_Name = productList[i].product_Name;
                        planItem.Qty = Convert.ToDecimal(item.plan_QTY);
                        planItem.ProductConversion_Index = conversionList[i].productConversion_Index;
                        planItem.ProductConversion_Id = conversionList[i].productConversion_Id;
                        planItem.ProductConversion_Name = conversionList[i].productConversion_Name;
                        planItem.Document_Status = 0;
                        planItem.ERP_Location = "AB01";

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
                        planItem.Create_By = param.creat_By;
                        planItem.Create_Date = DateTime.Now;
                        i++;
                        db.im_PlanGoodsIssueItem.Add(planItem);
                    }
                }

                state = "5";
                db.SaveChanges();

                
                result.statusDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
                result.statusDesc = "รอยืนยัน";
                Callback_OMS(result);

                return result;
            }
            catch (Exception ex)
            {
                
                result.statusDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
                result.statusDesc = ex.Message;
                Callback_OMS(result);
                return result;
            }

        }

        public DemoShipmentResponseViewModel CreateShipment(DemoShipmentRequestViewModel param)
        {
            var result = new DemoShipmentResponseViewModel();
            result.message = "";
            result.reason_code = 0;
            Boolean IsNew = false;
            var truckLoadIndex = Guid.NewGuid();
            var p = param.sJson();

            try
            {
                var provider = new System.Globalization.CultureInfo("en-US");
                //var vehicleUrl = new AppSettingConfig().GetUrl("VehicleTypeUrl");
                var vehicleModel = new VehicleTypeViewModel();
                vehicleModel.key = param.vehicleType_Id;
                //var vehicleJson = SendDataUtil.SerializeObject(vehicleModel);
                //var vehicle = SendDataUtil.SendDataApi<actionResultVehicleTypeViewModel>(vehicleUrl, vehicleJson);
                var vehicle = utils.SendDataApi<actionResultVehicleTypeViewModel>(new AppSettingConfig().GetUrl("VehicleTypeUrl"), vehicleModel.sJson());

                if (vehicle == null || vehicle.itemsVehicleType.Count == 0)
                {
                    result.document_No = param.tm_no;
                    result.result = false;
                    result.message = "vehicleType_Id not found.";
                    return result;
                }
                else
                {
                    vehicle.itemsVehicleType = vehicle.itemsVehicleType.Where(c => c.vehicleType_Id == param.vehicleType_Id).ToList();
                    if (vehicle.itemsVehicleType.Count == 0)
                    {
                        result.document_No = param.tm_no;
                        result.result = false;
                        result.message = "vehicleType_Id not found.";
                        return result;
                    }
                }

                

                var checkdata = CheckReq_Shipment(param);
                if (checkdata != "")
                {
                    result.document_No = param.tm_no;
                    result.result = false;
                    result.message = checkdata;
                    return result;
                }
                else
                {
                    var truckLoad = db.im_TruckLoad.Where(c => c.TruckLoad_No == param.tm_no && c.Document_Status != -1).FirstOrDefault();
                    if (truckLoad != null)
                    {
                        result.document_No = param.tm_no;
                        result.result = false;
                        result.message = "tm_no is duplicate.";
                        return result;
                    }
                    else
                    {
                        foreach (var item in param.items)
                        {
                            var planGI = db.im_PlanGoodsIssue.Where(c => c.PlanGoodsIssue_No == item.planGoodsIssue_No && c.Document_Status != -1).FirstOrDefault();
                            if (planGI == null)
                            {
                                result.document_No = param.tm_no;
                                result.result = false;
                                result.message = "planGoodsIssue_No " + item.planGoodsIssue_No + " not found.";
                                return result;
                            }
                        }

                        IsNew = true;
                        var url = new AppSettingConfig().GetUrl("dropDownDocumentType");
                        var docrequest = new GenDocumentTypeViewModel();
                        docrequest.documentType_Index = new Guid("971682a9-2083-4bf8-85c3-85ab966ddb66");
                        docrequest.process_Index = new Guid("1150720E-EE32-426D-A98E-6CC659D9AAD5");
                        var doctype = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), docrequest.sJson());

                        //string tlDate = DateTime.Parse(param.tm_date).ToString("yyyyMMddHHmmss");
                        //string tlExDate = DateTime.Parse(param.expect_Pickup_Date).ToString("yyyyMMddHHmmss");

                        var genDoc = new AutoNumberService();
                        string DocNo = "";
                        //DateTime DocumentDate = (DateTime)data.truckLoad_Date.toDate();
                        //DateTime DocumentDate = Utils.ConvertStringToDate(DateTime.Now.ToString("yyyyMMdd"));
                        DateTime DocumentDate = DateTime.Now;
                        DocNo = genDoc.genAutoDocmentNumber(doctype, DocumentDate);

                        im_TruckLoad itemHeader = new im_TruckLoad();
                        var document_status = 0;
                        itemHeader.TruckLoad_Index = truckLoadIndex;
                        itemHeader.TruckLoad_No = param.tm_no;
                        itemHeader.TruckLoad_Date = Convert.ToDateTime(param.tm_date);
                        itemHeader.Vehicle_Registration = param.vehicle_No;
                        itemHeader.Expect_Pickup_Date = Convert.ToDateTime(param.expect_Pickup_Date);
                        itemHeader.DocumentType_Index = doctype[0].documentType_Index;
                        itemHeader.DocumentType_Id = doctype[0].documentType_Id;
                        itemHeader.DocumentType_Name = doctype[0].documentType_Name;
                        itemHeader.DocumentRef_No1 = param.driver_Name;
                        itemHeader.DocumentRef_No2 = param.route_Id;
                        itemHeader.DocumentRef_No3 = param.subRoute_Id;
                        //itemHeader.DocumentRef_No4 = param.tm_Index;
                        itemHeader.DocumentRef_No5 = param.expect_Pickup_Time;
                        itemHeader.Document_Status = document_status;
                        itemHeader.VehicleCompany_Id = param.vehicleCompany_Id;
                        itemHeader.VehicleCompany_Name = param.vehicleCompany_Name;
                        itemHeader.VehicleType_Index = vehicle.itemsVehicleType[0].vehicleType_Index;
                        itemHeader.VehicleType_Id = vehicle.itemsVehicleType[0].vehicleType_Id;
                        itemHeader.VehicleType_Name = param.vehicleType_Name;
                        itemHeader.DocumentRef_No6 = param.IsAirCon;
                        itemHeader.DocumentRef_No7 = param.FreightKind_Name;
                        if (param.flagNoBook == "X")
                        {
                            itemHeader.UDF_1 = "NB";
                        }
                        else
                        {
                        }

                        if (IsNew == true)
                        {
                            itemHeader.Create_By = "TMS Interface";
                            itemHeader.Create_Date = DateTime.Now;
                        }
                        db.im_TruckLoad.Add(itemHeader);
                        
                        if (param.items != null)
                        {
                            foreach (var item in param.items)
                            {
                                //string tlExPickDate = DateTime.Parse(item.expect_Delivery_Date).ToString("yyyyMMddHHmmss");
                                var planGI = db.im_PlanGoodsIssue.Where(c => c.PlanGoodsIssue_No == item.planGoodsIssue_No && c.Document_Status != -1).FirstOrDefault();
                                //var planGI = omsDB.im_PlanGoodsIssue.Where(c => c.PlanGoodsIssue_No == item.planGoodsIssue_No).FirstOrDefault();
                                if (planGI == null)
                                {
                                    result.document_No = param.tm_no;
                                    result.result = false;
                                    result.message = "planGoodsIssue_No " + item.planGoodsIssue_No + " not found.";
                                    
                                    return result;
                                }
                                else
                                {
                                    im_TruckLoadItem resultItem = new im_TruckLoadItem();
                                    resultItem.TruckLoadItem_Index = Guid.NewGuid();
                                    resultItem.TruckLoad_Index = truckLoadIndex;
                                    resultItem.DocumentRef_No1 = item.seq;
                                    resultItem.DocumentRef_No2 = item.item_seq;
                                    resultItem.DocumentRef_No3 = item.is_return;
                                    resultItem.DocumentRef_No5 = item.Drop_seq;
                                    resultItem.Document_Status = 0;
                                    resultItem.UDF_1 = item.shiptoid;
                                    resultItem.UDF_2 = item.shiptoname;
                                    resultItem.UDF_3 = item.shiptoaddress;
                                    resultItem.UDF_4 = item.tel;
                                    resultItem.PlanGoodsIssue_Index = planGI.PlanGoodsIssue_Index;
                                    resultItem.PlanGoodsIssue_No = planGI.PlanGoodsIssue_No;
                                    resultItem.Expect_Delivery_Date = Convert.ToDateTime(item.expect_Delivery_Date);
                                    if (IsNew == true)
                                    {
                                        resultItem.Create_By = "TMS Interface";
                                        resultItem.Create_Date = DateTime.Now;
                                    }
                                    db.im_TruckLoadItem.Add(resultItem);
                                }
                            }
                        }
                    }
                }

                var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    transactionx.Rollback();
                    throw exy;

                }
                result.document_No = param.tm_no;
                result.result = true;
                return result;
            }
            catch(Exception ex)
            {
                result.document_No = param.tm_no;
                result.result = false;
                result.message = "vehicleType_Id not found.";
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
                    result += " items is empty";
                    return result;
                }
            }
            else { }

            if (string.IsNullOrEmpty(param.creat_By))
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += "|";
                }
                else
                {
                    result += " create_By is empty";
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

        public string CheckReq_Shipment(DemoShipmentRequestViewModel param)
        {
            string message = "";
            //if (string.IsNullOrEmpty(param.tm_Index))
            //{
            //    if (!string.IsNullOrEmpty(message))
            //    {
            //        message += " | ";
            //    }
            //    message += "tm_Index is empty";
            //}
            if (string.IsNullOrEmpty(param.tm_no))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += " | ";
                }
                message += "tm_no is empty";
            }
            if (string.IsNullOrEmpty(param.tm_date))
            {
                message += "tm_date is empty";
            }
            if (string.IsNullOrEmpty(param.vehicleType_Id))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += " | ";
                }
                message += "vehicleType_Id is empty";
            }
            if (string.IsNullOrEmpty(param.vehicleType_Name))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += " | ";
                }
                message += "vehicleType_Name is empty";
            }
            if (string.IsNullOrEmpty(param.vehicle_No))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += " | ";
                }
                message += "vehicle_No is empty";
            }
            if (string.IsNullOrEmpty(param.driver_Name))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += " | ";
                }
                message += "driver_Name is empty";
            }

            if (param.items == null)
            {
                message += "items is null";
            }
            else
            {
                foreach (var item in param.items)
                {
                    if (string.IsNullOrEmpty(item.planGoodsIssue_No))
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            message += " | ";
                        }
                        message += "planGoodsIssue_No is empty";
                        return message;
                    }

                    if (string.IsNullOrEmpty(item.seq))
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            message += " | ";
                        }
                        message += "seq is empty";
                        return message;
                    }

                    if (item.seq == "0")
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            message += " | ";
                        }
                        message += "seq is 0";
                        return message;
                    }

                    if (string.IsNullOrEmpty(item.Drop_seq))
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            message += " | ";
                        }
                        message += "Drop_seq is empty";
                        return message;
                    }

                    if (item.Drop_seq == "0")
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            message += " | ";
                        }
                        message += "Drop_seq is 0";
                    }

                    if (string.IsNullOrEmpty(item.item_seq))
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            message += " | ";
                        }
                        message += "Item_seq is empty";
                        return message;
                    }

                    if (item.item_seq == "0")
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            message += " | ";
                        }
                        message += "Item_seq is 0";
                        return message;
                    }
                }
            }
            return message;
        }

        public string Callback_OMS(DemoCallbackViewModel param)
        {
            try
            {
                var conversionMasterResult = utils.SendDataApi<List<ProductConversionViewModelDoc>>(new AppSettingConfig().GetUrl("callback_OMS"), param.sJson());
                return "Success";
            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }
    }
}
