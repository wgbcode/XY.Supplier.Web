using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XY.Boss;
using XY.DBUtil;
using XY.Entity.xy_boss;
using XY.Enums.boss;
using XY.Model.Boss.Transport;
using XY.Supplier.Web.Models.Purchase;
using XY.Supplier.Web.Models.Transport;

namespace XY.Supplier.Web.Controllers
{
    public class TransportController : ControllerAuthorized
    {
        public TransportController(IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment) { }

        public IActionResult Order()
        {
            return View();
        }


        public IActionResult TransportDoc_BaseInfo()
        {
            JObject result = new JObject();
            var writeBackStatus = XY.Enums.boss.Tools.write_back_status_list();
            var transportStatus = XY.Enums.boss.Tools.transport_status_list();
            result.Add("writeBackErpStatus", JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(writeBackStatus)));
            result.Add("transportStatus", JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(transportStatus)));
            return Json(JsonConvert.SerializeObject(result));
        }

        [HttpGet]
        public IActionResult TransportDoc_List(string packingNumber, string transportDocOrder,string fillInStatus="",string orderStatus="", int offset = 0, int limit = 10)
        {
            transportDocOrder = transportDocOrder.TrimNull();
            packingNumber = packingNumber.TrimNull();
            var index = (offset / limit) + 1;
            var supplierID = this.PassportInfo.SupplierID;

            var supplierInfo = Business.Boss.Instance.SupplierInfo(supplierID);
            var supplierCode = supplierInfo.account;
            if (PassportInfo.Name == "管理员")
                supplierCode = "";

            if (!string.IsNullOrEmpty(transportDocOrder) && !transportDocOrder.Contains("PO-"))
            {
                transportDocOrder = "PO-" + transportDocOrder;
            }

            JObject data = XY.Business.TransportDoc.Instance.TransportDocList(index, limit, null, null, packingNumber, 
                transportDocOrder, supplierCode,"","", fillInStatus,"", orderStatus);

            return Json(JsonConvert.SerializeObject(data));
        }

        [HttpGet]
        public IActionResult TransportDoc_Detail(int transportDocID, string backurl = "/Transport/Order")
        {
            if (transportDocID <= 0)
                return RedirectPage("请求参数有误！", backurl);

            TransportDoc_View_Model model = new TransportDoc_View_Model();
            var transportDocInfo = Business.TransportDoc.Instance.TransportDocInfo(transportDocID);
            if (transportDocInfo == null)
                return RedirectPage("获取单据明细信息失败！", backurl);

            model.Backurl = backurl;
            int docentry = int.Parse(transportDocInfo.transport_doc_order.Replace("PO-", ""));

            //支付信息
            DataTable dt = Business.TransportDoc.Instance.TransportDocDetailPayInfo(docentry);
            DataRow dr = null;
            if (dt.Rows.Count > 0)
                dr = dt.Rows[0];

            model.write_back_status = transportDocInfo.write_back_status;
            //填写状态
            model.TransportStatus = transportDocInfo.transport_status;

            #region 发货主体

            var shipCompany = Business.TransportDoc.Instance.TransportDocShipComapny(transportDocInfo.consignor_company_code)?.FirstOrDefault();
            model.OidcCode = shipCompany.name ?? "";
            //发货地址
            model.ConsignorAddress = transportDocInfo.consignor_address;
            //物流单号
            model.TransportDocOrder = transportDocInfo.transport_doc_order;
            //订单状态
            model.Status = Business.TransportDoc.Instance.GetOrderStatus(transportDocInfo.status + transportDocInfo.printed);
            //过账日期
            model.DocDate = transportDocInfo.doc_date;
            //交货日期
            model.DocDueDate = transportDocInfo.doc_due_date;
            //采购员/销售员
            model.SlpName = transportDocInfo.slp_name;
            //备注
            model.Comments = transportDocInfo.comments;

            //发货人
            model.Consignor = transportDocInfo.consignor;
            //发货单位
            model.ConsignorCompany = shipCompany.name;
            //发货地址
            model.ConsignorAddress = transportDocInfo.consignor_address;
            //发货人联系电话
            model.ConsignorTel = transportDocInfo.consignor_tel;

            #endregion

            #region 供应商信息

            //物流公司
            model.CardNname = transportDocInfo.card_name;
            //物流公司业务代码
            model.CardCode = transportDocInfo.card_code;
            //物流公司地址
            model.Address = dr == null ? "" : dr["Address"].ToString();
            //联系人
            model.CntctCode = XY.Business.Boss.Instance.ContactInfo(transportDocInfo.cntct_code)?.name ?? "";
            //付款条件
            model.PymntGroup = dr == null ? "" : dr["PymntGroup"].ToString();
            //付款方式
            model.PaymentMethod = "";
            //发票类型
            model.UFPLB = dr == null ? "" : dr["U_FPLB"].ToString();
            //税率
            model.U_SL = dr == null ? "" : dr["U_SL"].ToString();
            //快递单号
            model.CourierNumber = dr == null ? "" : dr["LicTradNum"].ToString();
            //预付款日期
            model.PrepaDay = dr == null ? "" : dr["PrepaDay"].ToString();
            //收款账号
            model.UACCT = dr == null ? "" : dr["U_ACCT"].ToString();
            //开户行
            model.UBANK = dr == null ? "" : dr["U_BANK"].ToString();

            #endregion

            #region 收件公司信息
            //收件公司
            model.ConsigneeCompany = dr == null ? "" : dr["U_OC_CN"].ToString();  //transportDocInfo.consignee_company;
            //收件地址
            model.ConsigneeAddress = transportDocInfo.consignee_address;
            //收件人
            model.Consignee = transportDocInfo.consignee;
            //收件人联系电话
            model.ConsigneeTel = transportDocInfo.consignee_tel;

            #endregion

            #region 物流单详情 

            //托运日期
            model.DateOfConsignment = DateTime.Parse(transportDocInfo.doc_due_date.ToString()).ToString("yyyy.MM.dd");
            //采购单号

            //运费单物料明细
            var transportDocItemList = Business.TransportDoc.Instance.TransportDocItem(transportDocInfo.transport_doc_id).ToList();
            List<TransportDocDetail> listItem = new List<TransportDocDetail>();
            for (int i = 0; i < transportDocItemList.Count; i++)
            {
                TransportDocDetail transportDocDetail = new TransportDocDetail()
                {
                    num = (i + 1).ToString(),
                    goods = transportDocItemList[i].item_code,
                    item_number = transportDocItemList[i].item_number,
                    packingType = transportDocItemList[i].packing_type,
                    pieces = transportDocItemList[i].packages,
                    volume = transportDocItemList[i].volume,
                    weight = transportDocItemList[i].weight,
                    packingNumber = transportDocItemList[i].packing_number,
                    itemID = transportDocItemList[i].item_id
                };

                listItem.Add(transportDocDetail);
            }

            model.transportDocDetailList = listItem;

            //费用信息
            model.FreightFee = transportDocInfo.freight_fee;//运费
            model.DeliveryFee = transportDocInfo.delivery_fee;//送货费
            model.PackagingFee = transportDocInfo.packaging_fee;//包装费
            model.ForkliftFee = transportDocInfo.forklift_fee;//叉车费
            model.PickupFee = transportDocInfo.pickup_fee;//提货费
            model.Insurance = transportDocInfo.insurance;//保险费

            //其它费用
            model.DischarGecargoFee = transportDocInfo.dischar_gecargo_fee;
            model.WaitFee = transportDocInfo.wait_fee;
            model.WarehousingFee = transportDocInfo.warehousing_fee;
            model.CabinetInstallationFee = transportDocInfo.cabinet_installation_fee;
            model.ReturnWithGoodsFee = transportDocInfo.return_with_goods_fee;
            model.UpstairsFee = transportDocInfo.upstairs_fee;
            model.OvernightChargeFee = transportDocInfo.overnight_charge_fee;

            //合计（大写）
            Dictionary<int, string> dic = ConvertToChinese(decimal.Parse(transportDocInfo.doc_total.ToString("#0.00")));
            model.DocTotal_w = dic[0];
            model.DocTotal_q = dic[1];
            model.DocTotal_b = dic[2];
            model.DocTotal_s = dic[3];
            model.DocTotal_y = dic[4];
            model.DocTotal_j = dic[5];
            model.DocTotal_f = dic[6];

            //运费合计
            //model.DocTotal = transportDocInfo.doc_total.ToString("#0.00");
            //合计
            model.DocTotal = (transportDocInfo.freight_fee + transportDocInfo.delivery_fee + transportDocInfo.packaging_fee + transportDocInfo.forklift_fee + 
                transportDocInfo.pickup_fee + transportDocInfo.insurance+ model.DischarGecargoFee+ model.WaitFee+ model.WarehousingFee+ model.CabinetInstallationFee+
                model.ReturnWithGoodsFee+ model.UpstairsFee+ model.OvernightChargeFee).ToString("#0.00");


            //单据在ERP创建时间
            model.CreateDate = transportDocInfo.create_date;
            #endregion

            return View(model);
        }

        [HttpPost]
        public IActionResult TransportDoc_Detail_Update(TransportDoc_View_Model transportDoc)
        {
            var transportDocID = transportDoc.TransportDocID;
            //var analysisReason = transportDoc.AnalysisReason;
            //var materialSource = transportDoc.MaterialSource;
            //var packingNumber = transportDoc.PackingNumber;
            //var consignmentDescription = transportDoc.ConsignmentDescription;
            //var consignmentQuantity = transportDoc.ConsignmentQuantity;
            //var licensePlateNumber = transportDoc.LicensePlateNumber;
            var deliveryFee = transportDoc.DeliveryFee;
            var forkliftFee = transportDoc.ForkliftFee;
            var packagingFee = transportDoc.PackagingFee;
            var pickupFee = transportDoc.PickupFee;
            var insurance = transportDoc.Insurance;
            var freightFee = transportDoc.FreightFee;

            //其它费用
            var discharGecargoFee = transportDoc.DischarGecargoFee;
            var waitFee = transportDoc.WaitFee;
            var warehousingFee = transportDoc.WarehousingFee;
            var cabinetInstallationFee = transportDoc.CabinetInstallationFee;
            var returnWithGoodsFee = transportDoc.ReturnWithGoodsFee;
            var upstairsFee = transportDoc.UpstairsFee;
            var overnightChargeFee = transportDoc.OvernightChargeFee;

            //运输方式
            string modeOfTransport = transportDoc.ModeOfTransport;
            //车辆类型
            string vehicleType = "";
            //车辆数
            int vehicleNum = 0;
            //是否往返
            int roundTrip = 0;

            JObject result = new JObject();
            result.Add("code", 0);
            result.Add("message", "");

            if (transportDoc.TransportDocID <= 0)
            {
                result["message"] = "请求参数有误！";
                return Json(Newtonsoft.Json.JsonConvert.SerializeObject(result));
            }
            var res = Business.TransportDoc.Instance.TransportFeeUpdate(transportDocID, freightFee, deliveryFee, packagingFee, 
                insurance, pickupFee, forkliftFee, XY.Enums.boss.transport_status.filled_finish,
                discharGecargoFee, waitFee, warehousingFee, cabinetInstallationFee, returnWithGoodsFee, upstairsFee, overnightChargeFee,
                modeOfTransport,
                //,vehicleType,vehicleNum, roundTrip,
                this.PassportInfo.SupplierID);

            if (res < 0)
            {
                result["message"] = "物流单编辑异常！";
                return Json(JsonConvert.SerializeObject(result));
            }

            //var upRes = Business.TransportDoc.Instance.TransportStatusUpdate(transportDocID, transport_status.filled_finish);
            //if (upRes < 0)
            //{
            //    result["message"] = "物流单编辑异常！";
            //    return Json(JsonConvert.SerializeObject(result));
            //}

            //保存填写的物料重量，体积信息
            List<TransportDocDetail> tdiLst = transportDoc.transportDocDetailList;
            if (tdiLst.Count > 0)
            {
                foreach (var item in tdiLst)
                {
                    if (string.IsNullOrEmpty(item.goods))
                        continue;

                    transport_doc_item tdi = new transport_doc_item();
                    tdi.transport_doc_id = transportDocID;
                    tdi.item_code = item.goods;
                    tdi.item_number = item.item_number.TrimNull();
                    tdi.packing_type = item.packingType.TrimNull();
                    tdi.volume = item.volume.TrimNull();
                    tdi.weight = item.weight.TrimNull();
                    tdi.updater = this.PassportInfo.SupplierID;
                    tdi.packages = item.pieces;
                    Business.TransportDoc.Instance.TransportDocItemInfoUpdate(tdi);
                }
            }

            return Json(JsonConvert.SerializeObject(result));
        }
        
        /// <summary>
        /// 锁定
        /// </summary>
        /// <param name="transportDoc"></param>
        /// <returns></returns>
        [HttpPost]
        //public IActionResult TransportDoc_Lock(TransportDoc_View_Model transportDoc)
        //{
        //    //锁定前先保存
        //    var transportDocID = transportDoc.TransportDocID;
        //    //var analysisReason = transportDoc.AnalysisReason;
        //    //var materialSource = transportDoc.MaterialSource;
        //    //var packingNumber = transportDoc.PackingNumber;
        //    //var consignmentDescription = transportDoc.ConsignmentDescription;
        //    //var consignmentQuantity = transportDoc.ConsignmentQuantity;
        //    //var licensePlateNumber = transportDoc.LicensePlateNumber;
        //    var deliveryFee = transportDoc.DeliveryFee;
        //    var forkliftFee = transportDoc.ForkliftFee;
        //    var packagingFee = transportDoc.PackagingFee;
        //    var pickupFee = transportDoc.PickupFee;
        //    var insurance = transportDoc.Insurance;
        //    var freightFee = transportDoc.FreightFee;

        //    JObject result = new JObject();
        //    result.Add("code", 0);
        //    result.Add("message", "");

        //    if (transportDoc.TransportDocID <= 0)
        //    {
        //        result["message"] = "请求参数有误！";
        //        return Json(Newtonsoft.Json.JsonConvert.SerializeObject(result));
        //    }

        //    var transportDocInfo = Business.TransportDoc.Instance.TransportDocInfo(transportDoc.TransportDocID);

        //    var res = Business.TransportDoc.Instance.TransportFeeUpdate(transportDocID, freightFee, deliveryFee, 
        //        packagingFee, insurance, pickupFee, forkliftFee, XY.Enums.boss.transport_status.filled_finish,1);

        //    if (res < 0)
        //    {
        //        result["message"] = "物流单编辑异常！";
        //        return Json(JsonConvert.SerializeObject(result));
        //    }

        //    //var upRes = Business.TransportDoc.Instance.TransportStatusUpdate(transportDocID, transport_status.filled_finish);
        //    //if (upRes < 0)
        //    //{
        //    //    result["message"] = "物流单编辑异常！";
        //    //    return Json(JsonConvert.SerializeObject(result));
        //    //}

        //    //锁定后自动将费用等数据回写ERP3.0

        //    UpdateTransPurchaseOrder updateOrder = new UpdateTransPurchaseOrder();
        //    updateOrder.u_BZF = packagingFee.ToString();//包装费
        //    updateOrder.u_CCF = forkliftFee.ToString();//叉车费
        //    updateOrder.u_KDF = deliveryFee.ToString();//送货费
        //    updateOrder.u_YF = freightFee.ToString();//运费
        //    updateOrder.u_YCF = insurance.ToString();//保险费
        //    updateOrder.u_THF = pickupFee.ToString();//提货费
        //    updateOrder.docTotal = (packagingFee + forkliftFee + deliveryFee + freightFee + insurance + pickupFee).ToString();
        //    updateOrder.docNum = transportDocInfo.transport_doc_order.Replace("PO-", "");
        //    List<PurchaseAcctCode> purchaseAcctCodeList = new List<PurchaseAcctCode>();
        //    PurchaseAcctCode purchaseAcctCode = new PurchaseAcctCode();
        //    purchaseAcctCode.u_TYSL = transportDocInfo.consignment_quantity.ToString();
        //    purchaseAcctCode.price = updateOrder.docTotal;//总金额
        //    purchaseAcctCode.lineTotal = updateOrder.docTotal; //总金额
        //    purchaseAcctCode.u_TYWP = transportDocInfo.consignment_description?.ToString();//托运物品
        //    purchaseAcctCode.u_WLLY = transportDocInfo.material_source?.ToString();//物料来源
        //    purchaseAcctCode.u_YYFX = "";//原因分析
        //    purchaseAcctCode.u_ZXDH = transportDocInfo.packing_number?.ToString();//装箱单号
        //    purchaseAcctCode.u_CPH = "";//车牌号
        //    purchaseAcctCodeList.Add(purchaseAcctCode);
        //    updateOrder.purchaseAcctCodeList = purchaseAcctCodeList;

        //    var wbRes = Business.TransportDoc.Instance.WritebackTransportFeeInfoERP3(updateOrder, transportDocID,this.PassportInfo.SupplierID);

        //    if (wbRes["code"] == "0")
        //    {
        //        result["message"] = wbRes["message"];
        //        return Json(JsonConvert.SerializeObject(result));
        //    }

        //    return Json(JsonConvert.SerializeObject(result));
        //}

        [HttpPost]
        public IActionResult TransportDoc_UnLock(TransportDoc_View_Model transportDoc)
        {
            var transportDocID = transportDoc.TransportDocID;

            JObject result = new JObject();
            result.Add("code", 0);
            result.Add("message", "");

            if (transportDoc.TransportDocID <= 0)
            {
                result["message"] = "请求参数有误！";
                return Json(Newtonsoft.Json.JsonConvert.SerializeObject(result));
            }

            var res = Business.TransportDoc.Instance.TransportStatusUpdate(transportDocID, transport_status.wait_filled, this.PassportInfo.SupplierID);

            if (res < 0)
            {
                result["message"] = "物流单编辑异常！";
                return Json(JsonConvert.SerializeObject(result));
            }

            return Json(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 使用正则表达式将数字转换为大写
        /// </summary>
        /// <param name="number"></param>
        /// <returns>返回大写形式</returns>
        public Dictionary<int, string> ConvertToChinese(decimal number)
        {
            var s = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());

            Dictionary<int, string> dic = new Dictionary<int, string>();
            string str = "";
            string w = "";
            string q = "";
            string b = "";
            string ss = "";
            string y = "";
            string j = "";
            string f = "";

            if (r.LastIndexOf('万') > 0)
            {
                w = r.Substring(0, r.LastIndexOf('万'));
                r = r.Substring(r.LastIndexOf('万') + 1);
            }
            if (r.LastIndexOf('仟') > 0)
            {
                q = r.Substring(0, r.LastIndexOf('仟'));
                r = r.Substring(r.LastIndexOf('仟') + 1);
            }
            if (r.LastIndexOf('佰') > 0)
            {
                b = r.Substring(0, r.LastIndexOf('佰'));
                r = r.Substring(r.LastIndexOf('佰') + 1);
            }
            if (r.LastIndexOf('拾') > 0)
            {
                ss = r.Substring(0, r.LastIndexOf('拾'));
                r = r.Substring(r.LastIndexOf('拾') + 1);
            }
            if (r.LastIndexOf('元') > 0)
            {
                y = r.Substring(0, r.LastIndexOf('元'));
                r = r.Substring(r.LastIndexOf('元') + 1);
            }
            if (r.LastIndexOf('角') > 0)
            {
                j = r.Substring(0, r.LastIndexOf('角'));
                r = r.Substring(r.LastIndexOf('角') + 1);
            }
            if (r.LastIndexOf('分') > 0)
            {
                f = r.Substring(0, r.LastIndexOf('分'));
                r = r.Substring(r.LastIndexOf('分') + 1);
            }

            dic.Add(0, w);
            dic.Add(1, q);
            dic.Add(2, b);
            dic.Add(3, ss);
            dic.Add(4, y);
            dic.Add(5, j);
            dic.Add(6, f);
            //str= w + "," + q + "," + b + "," + ss + "," + y;
            //str = w + "    " + q + "      " + b + "         " + ss + "         " + y;
            return dic;
        }

    }
}
