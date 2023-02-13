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
using System.Threading.Tasks;
using XY.Boss;
using XY.DBUtil;
using XY.Entity.xy_boss;
using XY.Enums.boss;
using XY.Supplier.Web.Models.Purchase;

namespace XY.Supplier.Web.Controllers
{
    public class PurchaseController : ControllerAuthorized
    {
        public PurchaseController(IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment) { }

        #region 订单管理

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <remarks>林思源</remarks>
        /// <param name="isDelayStatus">是否延期</param>
        /// <param name="orderStatus">状态</param>
        /// <param name="orderKey">订单关键字</param>
        /// <param name="begin">下单开始时间</param>
        /// <param name="end">下单结束时间</param>
        /// <param name="page">页索引</param>
        /// <param name="pageSize">页尺寸</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Order(int isDelayStatus = 0, int orderStatus = 0, string orderKey = "", string begin = "", string end = "", int page = 1, int pageSize = 30)
        {
            orderKey = orderKey.TrimNull();
            begin = begin.TrimNull();
            end = end.TrimNull();

            if (begin.Length == 0)
                begin = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd 00:00:00");
            if (end.Length == 0)
                end = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            var model = new PurchaseOrder_Model();
            model.OrderKey = orderKey;
            model.OrderStatus = orderStatus;
            model.IsDelayStatus = isDelayStatus;
            model.Begin = begin;
            model.End = end;
            //参数验证
            if (!DateTime.TryParse(begin, out DateTime beginTime) || !DateTime.TryParse(end, out DateTime endTime))
            {
                ViewBag.Javascript = JS.AlertFocus("时间格式错误！", "Code");
                return View(model);
            }

            //获取供应商信息
            var supplierInfo = Business.Boss.Instance.SupplierInfo(PassportInfo.SupplierID);

            int supplierID = PassportInfo.SupplierID;
            if (PassportInfo.Name=="管理员")
                supplierID = 0;

            //获取采购订单状态列表
            model.OrderStatusList = XY.Enums.boss.Tools.purchase_status_list(true, 0, "全部").Select(x => new SelectListItem { Text = x.Value, Value = x.Key.ToString() });

            if (orderKey.Length > 0)
                model.OrderList = Business.Purchase.Instance.PurchaseList(supplierID, orderKey, page, pageSize);
            else
                model.OrderList = Business.Purchase.Instance.PurchaseList(supplierID, isDelayStatus, orderStatus, beginTime, endTime, page, pageSize);

            return View(model);
        }

        /// <summary>
        /// 采购订单详情页面
        /// </summary>
        /// <remarks>李华健创建，林思源修改</remarks>
        /// <param name="purchaseID">采购订单ID</param>
        /// <param name="backurl">返回URL</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Order_Detail(int purchaseID, string backurl = "/purchase/order")
        {
            //参数验证
            if (purchaseID <= 0)
                return RedirectPage("请求参数有误！", backurl);

            var model = new PurchaseOrder_View_Model();
            model.Backurl = backurl;

            //获取采购订单信息
            var purchInfo = Business.Purchase.Instance.PurchaseInfo(purchaseID);
            if (purchInfo == null)
                return RedirectPage("获取采购订单信息失败！", backurl);

            //获取员工信息
            var userInfo = Business.Boss.Instance.UserInfo(purchInfo.creator);

            //获取供应商信息
            var supplierInfo = Business.Boss.Instance.SupplierInfo(purchInfo.supplier_id);

            //获取审核流程信息
            var wfInfo = Business.WorkFlow.WorkFlow.Instance.WfInfo(purchInfo.wf_id);

            //获取地区信息
            var zoneInfo = Business.Boss.Instance.ZoneInfo(purchInfo.ship_zone_code);

            //获取采购订单包含物料
            model.OrderItemList = XY.Business.Purchase.Instance.PurchaseItemManageList(purchaseID);

            //获取采购订单其他费用
            model.OrderFeeList = Business.Purchase.Instance.PurchaseFeeList(purchaseID);

            //获取采购订单附件
            //var purchaseFileList = Business.Purchase.Instance.PurchaseFileList(purchInfo.purchase_id).OrderByDescending(x => x.file_id);
            //if(purchaseFileList != null && purchaseFileList.Count() > 0)
            //{
            //    var fileList = new List<Purchase_File_Model>();
            //    foreach(var file in purchaseFileList)
            //    {
            //        //获取附件信息
            //        var fileInfo = Business.Boss.Instance.FileInfo(file.file_id);
            //        if (fileInfo != null)
            //            fileList.Add(new Purchase_File_Model()
            //            {
            //                FileName = Path.GetFileName(fileInfo.name),
            //                FileUrl = Business.Tools.FileUrl(XY.Boss.Configs.FileSiteURL, fileInfo.file_id, Path.GetExtension(fileInfo.name).Replace(".", ""))
            //            });
            //    }
            //    model.OrderFileList = fileList;
            //}

            model.PurchaseID = purchaseID;
            model.PurchaseOrder = purchInfo.purchase_order;
            model.Status = purchInfo.status;
            model.SupplierName = supplierInfo?.name ?? string.Empty;
            model.Creator = userInfo?.name??string.Empty;
            model.DeliveryType = purchInfo.delivery_type;
            model.DeliveryTime = purchInfo.delivery_time;
            model.IsTax = purchInfo.is_tax;
            model.PayPlanID = purchInfo.pay_plan_id;
            model.PriceTotal = purchInfo.price_total;
            model.PriceSpecial = purchInfo.price_special;
            model.WfID = purchInfo.wf_id;
            model.WfName = wfInfo?.name ?? string.Empty;
            model.ShipAddr = purchInfo.ship_addr;
            model.ShipZoneCode = zoneInfo?.name ?? string.Empty;
            model.ShipName = purchInfo.ship_name;
            model.ShipPhone = purchInfo.ship_phone;
            model.Remark = purchInfo.remark;

            return View(model);
        }

        /// <summary>
        /// 采购单物料导出
        /// </summary>
        /// <remarks>林思源</remarks>
        /// <param name="purchaseID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Order_Export(string purchaseID)
        {
            //参数处理
            int[] idArray = null;
            try { idArray = XY.Serializable.JsonStringToObject<int[]>(purchaseID.TrimNull()); } catch (Exception) { }

            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, System.Text.Encoding.UTF8);
            sw.WriteLine("订单号,物料编号,数量");
            if(idArray != null && idArray.Length > 0)
            {
                using (IDataReader dr = XY.Business.Purchase.Instance.PurchaseExport(idArray))
                {
                    while (dr.Read())
                    {
                        var order = dr["purchase_order"].ToString();
                        var code = dr["code"].ToString();
                        var item_qty = dr["item_qty"].ToString();
                        code = string.Format(code.Contains(",") ? "\"{0}\"" : "\t{0}", code);
                        sw.WriteLine($"{order},{code},{item_qty}");
                    }
                }
            }
            sw.Flush();
            ms.Position = 0;
            return File(ms, XY.Enums.Tools.HttpContentType(Enums.HttpContentType.Csv), $"采购订单物料明细{DateTime.Now:yyyyMMddHHmmssfff}.csv");
        }

        /// <summary>
        /// 投屏页面
        /// </summary>
        /// <param name="isDelayStatus">是否延期</param>
        /// <param name="orderStatus">状态</param>
        /// <param name="purchaseOrderKey">订单关键字</param>
        /// <param name="begin">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IActionResult Order_Shot(int isDelayStatus = 0, int orderStatus = (int)XY.Enums.boss.purchase_status.wait_receipt, string purchaseOrderKey = "", string begin = "", string end = "", int page = 1, int pageSize = 10)
        {
            purchaseOrderKey = purchaseOrderKey.TrimNull();
            begin = begin.TrimNull();
            end = end.TrimNull();

            if (begin.Length == 0)
                begin = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd 00:00:00");
            if (end.Length == 0)
                end = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            var model = new PurchaseOrder_Shot_Model();
            model.IsDelayStatus = isDelayStatus;
            model.OrderStatus = orderStatus;
            model.PurchaseOrderKey = purchaseOrderKey ?? "";
            model.Begin = begin;
            model.End = end;
            //参数验证
            if (!DateTime.TryParse(begin, out DateTime beginTime) || !DateTime.TryParse(end, out DateTime endTime))
            {
                ViewBag.Javascript = JS.AlertFocus("时间格式错误！", "Code");
                return View(model);
            }
            model.BeginDay = beginTime.ToString("yyyy.MM.dd");
            model.EndDay = endTime.ToString("yyyy.MM.dd");
            //model.OrderList = Business.Purchase.Instance.PurchaseList(PassportInfo.SupplierID, beginTime, endTime, page, pageSize, "issue_time", true);
            model.DeliveryList = Business.Purchase.Instance.PurchaseList(PassportInfo.SupplierID, beginTime, endTime, page, 1000, "delivery_time", true);
            model.PageSize = 12;
            return View(model);
        }

        /// <summary>
        /// 采购订单关联物料条码打印详情页面
        /// </summary>
        /// <remarks>李思源</remarks>
        /// <param name="purchaseID"></param>
        /// <param name="backurl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Order_Print_Detail(int purchaseID, string backurl = "/purchase/order")
        {
            //参数验证
            if (purchaseID <= 0)
                return RedirectPage("请求参数有误！", backurl);

            var model = new PurchaseOrder_Print_Detail();
            model.Backurl = backurl;
            model.PurchaseID = purchaseID;

            //获取采购订单物料
            model.OrderItemList = Business.Purchase.Instance.PurchaseItemManageList(purchaseID);

            return View(model);
        }
        /// <summary>
        /// 采购订单生成二维码（生成图像）
        /// </summary>
        /// <remarks>林思源创建，郭小虎修改</remarks>
        /// <param name="purchaseID">采购订单ID</param>
        /// <param name="itemStr">需打印的物料集合字符串</param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<string> Order_Generate_QRCode(int purchaseID, string itemStr, int width1, int height1,bool isPrintOutPackage=false)
        {
            itemStr = itemStr.TrimNull();

            //PurchaseOrder_QRCode_Print_Model pqpm = new PurchaseOrder_QRCode_Print_Model();
            //List<Item_Print_Detail_Model> ItemPrintParamDetail = new List<Item_Print_Detail_Model>();
            //ItemPrintParamDetail.Add(new Item_Print_Detail_Model() { LabelQty=20,Copies=2,remark="备注1"});
            //ItemPrintParamDetail.Add(new Item_Print_Detail_Model() { LabelQty = 10, Copies = 2, remark = "备注2" });
            //ItemPrintParamDetail.Add(new Item_Print_Detail_Model() { LabelQty = 5, Copies = 1, remark = "备注3" });
            //pqpm.purchaseID = 0;
            //pqpm.width = 90;
            //pqpm.height = 40;
            //pqpm.isPrintOutPackage = true;
            //pqpm.ItemID = 4791;
            //pqpm.ItemPrintDetail = ItemPrintParamDetail;
            //string strJson= XY.Serializable.ObjectToJsonString(pqpm);

            //参数验证
            if (purchaseID <= 0 || itemStr.Length <= 0)
                return null;

            //物料参数集合处理
            List<PurchaseOrder_Item_QRCode_Para_Model> itemList = null;
            try { itemList = XY.Serializable.JsonStringToObject<List<PurchaseOrder_Item_QRCode_Para_Model>>(itemStr.TrimNull()); } catch (Exception) { }
            if (itemList == null || itemList.Count <= 0)
                return null;

            //获取采购订单信息
            var purchaseInfo = XY.Business.Purchase.Instance.PurchaseInfo(purchaseID);
            if (purchaseInfo == null)
                return null;

            #region 打印参数

            var urlPath = $"{XY.Boss.Configs.BossSiteURL}/temp";
            //生成文件保存目录
            var tempSaveDir = Path.Combine(WebHostEnvironment.WebRootPath, "temp");
            //生成的二维码PDF文件URL路径集合
            var urlList = new List<string>();
            decimal dpi = XY.Boss.Configs.PrintDPI;
            decimal baseWidth = width1;   //XY.Boss.Configs.PrintWidth;
            decimal baseHeight = height1; //XY.Boss.Configs.PrintHeight;
            int height = System.Convert.ToInt32(baseHeight * dpi / 25.4m);
            int width = System.Convert.ToInt32(baseWidth * dpi / 25.4m);
            int xOffset = System.Convert.ToInt32(0 * dpi / 25.4m);
            int yOffset = System.Convert.ToInt32(0 * dpi / 25.4m);
            //int height = (int)System.Math.Ceiling(baseHeight * dpi / 25.4m);
            //int width = (int)System.Math.Ceiling(baseWidth * dpi / 25.4m);
            //int xOffset = (int)System.Math.Ceiling(0 * dpi / 25.4m);

            //二维码左边距
            var qrcodeLeft = 20;
            //二维码上边距
            var qrcodeTop = 20;
            //二维码大小
            var qrcodeWidth = height - qrcodeTop * 2;

            var fontEmSize = qrcodeWidth / 8;
            //文本左边距
            var textLeft = 0;
            //文本上边距
            var textTop = qrcodeTop + fontEmSize / 3;
            //文本行间距
            float textSpace = (float)fontEmSize * 2;
            if (qrcodeWidth > width / 3)
            {
                qrcodeWidth = width / 3;
                fontEmSize = qrcodeWidth / 8;
                //文本左边距
                textLeft = 0;
                //文本上边距
                textTop = qrcodeTop + fontEmSize / 3;
                //文本行间距
                textSpace = (float)(fontEmSize * 1.4);
            }
            else
            {
                fontEmSize = qrcodeWidth / 8;
                //文本左边距
                textLeft = 0;
                //文本上边距
                textTop = qrcodeTop + fontEmSize / 3;
                //文本行间距
                textSpace = (float)(fontEmSize * 1.3);
            }

            //单行文本宽度限制：图像宽度-二维码左右边距-最右边距-二维码宽度-文本左边距-最大超长20-初始位置
            float textLimitLength = width - qrcodeLeft * 4 - qrcodeWidth - textLeft - 20 - xOffset * 2;
            //整张图片大小
            var imageSize = new System.Drawing.Size(width, height);
            var fontFamily = new System.Drawing.FontFamily("黑体");
            //二维码大小
            var qrcodeSize = new System.Drawing.Size(qrcodeWidth, qrcodeWidth);

            #endregion

            foreach (var item in itemList)
            {
                //获取物料数据
                var itemInfo = XY.Business.Item.Instance.ItemInfo(item.ItemID);
                if (itemInfo == null)
                    return null;

                //获取单位数据
                var unitInfo = XY.Business.Item.Instance.UnitInfo(itemInfo.unit_id);

                //获取生产批号
                string batchNumber = Business.Purchase.Instance.BatchNumberOrder();
                if (string.IsNullOrEmpty(batchNumber))
                    break;

                //备注信息
                string remarks = "";
                if (!string.IsNullOrEmpty(item.remark))
                    remarks = item.remark;

                ////供应商SN码
                //string supplierSN = "";
                //if (!string.IsNullOrEmpty(item.SupplierSN))
                //    supplierSN = item.SupplierSN;

                //打印的数量
                for (int i = 0; i < item.Copies; i++)
                {
                    //获取二维码内容
                    var itemQRInfo = XY.Business.Item.Instance.ItemQrAdd(item.ItemID, item.LabelQty, Enums.OrderType.PO, purchaseID, remarks, PassportInfo?.SupplierID ?? 0, batchNumber);
                    if (itemQRInfo == null)
                        return null;

                    if (!string.IsNullOrEmpty(item.remark))
                        itemQRInfo += $",{item.remark}";

                    var list = new List<string>();
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemInfo.code, "", fontFamily, fontEmSize, imageSize));
                    list.Add($"{item.LabelQty}/{unitInfo?.name}");
                    list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, purchaseInfo.purchase_order.ToString(), "", fontFamily, fontEmSize, imageSize)[0]);
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemQRInfo?.Split(',')[7] ?? "", "", fontFamily, fontEmSize, imageSize));
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, batchNumber ?? "", "", fontFamily, fontEmSize, imageSize));
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, remarks, "", fontFamily, fontEmSize, imageSize));
                    var image = QRCoderHelper.CreateItemLabel(imageSize, itemQRInfo, qrcodeSize, qrcodeLeft, qrcodeTop, fontFamily, fontEmSize, list, textLeft, textTop, textSpace);
                    var fileName = $"{Guid.NewGuid()}.png";
                    image.Save(Path.Combine(tempSaveDir, fileName));
                    urlList.Add($"{urlPath}/{fileName}");
                }

                //必须放在打印单张标签后面,对应相应货号
                if (isPrintOutPackage)
                {
                    DapperHelper dapper = DapperHelper.CreateDapperHelper(XY.Entity.Configs.XyBoss);
                    IDbConnection conn = dapper.GetConn();
                    //打开连接
                    dapper.OpenConn(conn);
                    IDbTransaction tran = dapper.BeginTransaction(conn);

                    //生成箱号
                    var packageNo = Business.package.Instance.packageNo(purchaseInfo.purchase_order.ToString());
                    package packageModel = new package();
                    packageModel.package_no = packageNo;
                    packageModel.creator = this.PassportInfo.SupplierID;
                    int packageID = Business.package.Instance.PackageInfoInsert(packageModel, conn);
                    if (packageID <= 0)
                        return null;

                    //添加包装箱号关联信息
                    int packageRelationID = Business.package.Instance.PackageRelationInfoInsert(new package_relation()
                    {
                        package_id = packageID,
                        relation_order_id = purchaseInfo.purchase_id,
                        relation_order = purchaseInfo.purchase_order.ToString(),
                        relation_order_type = XY.Enums.OrderType.PO
                    }, conn);

                    if (packageRelationID <= 0)
                        return null;

                    //获取二维码内容
                    var itemOutQRInfo = XY.Business.Item.Instance.ItemOutQrAdd(item.ItemID, item.Copies, Enums.OrderType.PO, purchaseID, "",
                        PassportInfo?.SupplierID ?? 0, batchNumber, packageID, Enums.BarcodeType.QRCode, item.LabelQty);
                    if (itemOutQRInfo == null)
                        return null;

                    //string info = itemOutQRInfo?.Split(',')[7];
                    var list = new List<string>();
                    list.Add("外包装");
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemInfo.code, "", fontFamily, fontEmSize, imageSize));
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, packageNo, "", fontFamily, fontEmSize, imageSize));
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, batchNumber ?? "", "", fontFamily, fontEmSize, imageSize));
                    list.Add($"{item.Copies * item.LabelQty}/{unitInfo?.name}");
                    //list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, purchaseInfo.purchase_order.ToString(), "", fontFamily, fontEmSize, imageSize));
                    list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, itemOutQRInfo?.Split(',')[1] ?? "", "", fontFamily, fontEmSize, imageSize)[0]);
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, remarks, "", fontFamily, fontEmSize, imageSize));
                    var image = QRCoderHelper.CreateItemLabel(imageSize, itemOutQRInfo, qrcodeSize, qrcodeLeft, qrcodeTop, fontFamily, fontEmSize, list, textLeft, textTop, textSpace);
                    var fileName = $"{Guid.NewGuid()}.png";
                    image.Save(Path.Combine(tempSaveDir, fileName));
                    urlList.Add($"{urlPath}/{fileName}");
                }

                //if (itemInfo.rule == 1)
                //{
                //    //获取二维码内容
                //    var itemOutQRInfo = XY.Business.Item.Instance.ItemOutQrAdd(item.ItemID, item.Copies, Enums.OrderType.PO, purchaseID, "", PassportInfo?.SupplierID ?? 0, batchNumber);
                //    if (itemOutQRInfo == null)
                //        return null;

                //    string info = itemOutQRInfo?.Split(',')[7];
                //    var list = new List<string>();
                //    //list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, "部门:"+item.Dept.ToString(), "", fontFamily, fontEmSize, imageSize)[0]);
                //    list.Add("外包装");
                //    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemInfo.code, "", fontFamily, fontEmSize, imageSize));
                //    list.Add($"{item.Copies}/{unitInfo?.name}");
                //    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, purchaseInfo.purchase_order.ToString(), "", fontFamily, fontEmSize, imageSize));
                //    list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, itemOutQRInfo?.Split(',')[1] ?? "", "", fontFamily, fontEmSize, imageSize)[0]);
                //    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, batchNumber ?? "", "", fontFamily, fontEmSize, imageSize));
                //    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, remarks, "", fontFamily, fontEmSize, imageSize));
                //    var image = QRCoderHelper.CreateItemLabel(imageSize, itemOutQRInfo.Replace(info, "") + info.Split('-')[0], qrcodeSize, qrcodeLeft, qrcodeTop, fontFamily, fontEmSize, list, textLeft, textTop, textSpace);
                //    var fileName = $"{Guid.NewGuid()}.png";
                //    image.Save(Path.Combine(tempSaveDir, fileName));
                //    urlList.Add($"{urlPath}/{fileName}");

                //}
            }
            return urlList;
        }

        private static readonly object printLock = new Object();

        [HttpPost]
        public IActionResult Order_Item_Print(string data)
        {
            //data = "{    \"purchaseID\":88332,    \"ItemID\":4791,    \"width\":90,    \"height\":40,    \"isPrintOutPackage\":true,    \"ItemPrintDetail\":[        {            \"LabelQty\":20,            \"Copies\":2,            \"remark\":\"备注1\"        },        {            \"LabelQty\":5,            \"Copies\":1,            \"remark\":\"备注2\"        },        {            \"LabelQty\":3,            \"Copies\":1,            \"remark\":\"备注3\"        }    ]}";
            Model.Result<List<string>> result = new Model.Result<List<string>>();
            //生成的二维码PDF文件URL路径集合
            List<string> urlList = new List<string>();

            try
            {
                PurchaseOrder_QRCode_Print_Model para = XY.Serializable.JsonStringToObject<PurchaseOrder_QRCode_Print_Model>(data);
                if (para == null)
                {
                    result.Code = Model.ResultCode.Failure;
                    result.Message = "请求数据为空";
                    result.Data = urlList;
                    return Json(JsonConvert.SerializeObject(result));
                }

                //获取采购订单信息
                var purchaseInfo = XY.Business.Purchase.Instance.PurchaseInfo(para.purchaseID);
                if (purchaseInfo == null)
                {
                    result.Code = Model.ResultCode.Failure;
                    result.Message = "获取采购订单信息为空";
                    result.Data = urlList;
                    return Json(JsonConvert.SerializeObject(result));
                }

                //List<PurchaseOrder_QRCode_Print_Model> printParam = XY.Serializable.JsonStringToObject<List<PurchaseOrder_QRCode_Print_Model>>(itemPrintParam);
                if (para.ItemPrintDetail.Count==0)
                {
                    result.Code = Model.ResultCode.Failure;
                    result.Message = "打印参数不能为空";
                    result.Data = urlList;
                    return Json(JsonConvert.SerializeObject(result));
                }

                int QrCodeCount = para.ItemPrintDetail.Sum(a => a.LabelQty * a.Copies);
                int QrCodeCopyCount = para.ItemPrintDetail.Sum(a => a.Copies);
                //int QrCodeLableQtyCount = para.ItemPrintDetail.Sum(a => a.LabelQty);

                //判断打印物料数量是否大于采购物料数量
                var purchaseItem = Business.Purchase.Instance.PurchaseItemInfo(para.purchaseID, para.ItemID);
                if (purchaseItem.item_qty < QrCodeCount)
                {
                    result.Code = Model.ResultCode.Failure;
                    result.Message = "标签数量不能大于采购订单物料总数量";
                    result.Data = urlList;
                    return Json(JsonConvert.SerializeObject(result));
                }

                #region 打印参数
                if (para.width<=10)
                {
                    result.Code = Model.ResultCode.Failure;
                    result.Message = "标签纸宽度不能小于10";
                    result.Data = urlList;
                    return Json(JsonConvert.SerializeObject(result));
                }
                if (para.height <= 10)
                {
                    result.Code = Model.ResultCode.Failure;
                    result.Message = "标签纸高度不能小于10";
                    result.Data = urlList;
                    return Json(JsonConvert.SerializeObject(result));
                }

                var urlPath = $"{XY.Boss.Configs.BossSiteURL}/temp";
                //生成文件保存目录
                var tempSaveDir = Path.Combine(WebHostEnvironment.WebRootPath, "temp");
                decimal dpi = XY.Boss.Configs.PrintDPI;
                decimal baseWidth = para.width;   //XY.Boss.Configs.PrintWidth;
                decimal baseHeight = para.height; //XY.Boss.Configs.PrintHeight;
                int picHeight = System.Convert.ToInt32(baseHeight * dpi / 25.4m);
                int picwidth = System.Convert.ToInt32(baseWidth * dpi / 25.4m);
                int xOffset = System.Convert.ToInt32(0 * dpi / 25.4m);
                int yOffset = System.Convert.ToInt32(0 * dpi / 25.4m);
                //int height = (int)System.Math.Ceiling(baseHeight * dpi / 25.4m);
                //int width = (int)System.Math.Ceiling(baseWidth * dpi / 25.4m);
                //int xOffset = (int)System.Math.Ceiling(0 * dpi / 25.4m);

                //二维码左边距
                var qrcodeLeft = 20;
                //二维码上边距
                var qrcodeTop = 20;
                //二维码大小
                var qrcodeWidth = picHeight - qrcodeTop * 2;

                var fontEmSize = qrcodeWidth / 8;
                //文本左边距
                var textLeft = 0;
                //文本上边距
                var textTop = qrcodeTop + fontEmSize / 3;
                //文本行间距
                float textSpace = (float)fontEmSize * 2;
                if (qrcodeWidth > picwidth / 3)
                {
                    qrcodeWidth = picwidth / 3;
                    fontEmSize = qrcodeWidth / 8;
                    //文本左边距
                    textLeft = 0;
                    //文本上边距
                    textTop = qrcodeTop + fontEmSize / 3;
                    //文本行间距
                    textSpace = (float)(fontEmSize * 1.4);
                }
                else
                {
                    fontEmSize = qrcodeWidth / 8;
                    //文本左边距
                    textLeft = 0;
                    //文本上边距
                    textTop = qrcodeTop + fontEmSize / 3;
                    //文本行间距
                    textSpace = (float)(fontEmSize * 1.3);
                }

                //单行文本宽度限制：图像宽度-二维码左右边距-最右边距-二维码宽度-文本左边距-最大超长20-初始位置
                float textLimitLength = picwidth - qrcodeLeft * 4 - qrcodeWidth - textLeft - 20 - xOffset * 2;
                //整张图片大小
                var imageSize = new System.Drawing.Size(picwidth, picHeight);
                var fontFamily = new System.Drawing.FontFamily("黑体");
                //二维码大小
                var qrcodeSize = new System.Drawing.Size(qrcodeWidth, qrcodeWidth);

                #endregion

                //获取物料数据
                var itemInfo = XY.Business.Item.Instance.ItemInfo(para.ItemID);
                if (itemInfo == null)
                    return null;

                //获取单位数据
                var unitInfo = XY.Business.Item.Instance.UnitInfo(itemInfo.unit_id);

                //获取生产批号
                string batchNumber = Business.Purchase.Instance.BatchNumberOrder();

                lock (printLock)
                {
                    //遍历物料
                    foreach (var itemPara in para.ItemPrintDetail)
                    {
                        //打印的数量
                        for (int i = 0; i < itemPara.Copies; i++)
                        {
                            //获取二维码内容
                            var itemQRInfo = XY.Business.Item.Instance.ItemQrAdd(para.ItemID, itemPara.LabelQty, Enums.OrderType.PO, para.purchaseID, itemPara.remark, PassportInfo?.SupplierID ?? 0, batchNumber);
                            if (itemQRInfo == null) break;

                            if (!string.IsNullOrEmpty(itemPara.remark))
                                itemQRInfo += $",{itemPara.remark}";

                            var list = new List<string>();
                            list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemInfo.code, "", fontFamily, fontEmSize, imageSize));
                            list.Add($"{itemPara.LabelQty}/{unitInfo?.name}");
                            list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, purchaseInfo.purchase_order.ToString(), "", fontFamily, fontEmSize, imageSize)[0]);
                            list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemQRInfo?.Split(',')[7] ?? "", "", fontFamily, fontEmSize, imageSize));
                            list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, batchNumber ?? "", "", fontFamily, fontEmSize, imageSize));
                            list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemPara.remark, "", fontFamily, fontEmSize, imageSize));
                            var image = QRCoderHelper.CreateItemLabel(imageSize, itemQRInfo, qrcodeSize, qrcodeLeft, qrcodeTop, fontFamily, fontEmSize, list, textLeft, textTop, textSpace);
                            var fileName = $"{Guid.NewGuid()}.png";
                            image.Save(Path.Combine(tempSaveDir, fileName));
                            urlList.Add($"{urlPath}/{fileName}");
                        }
                    }

                    //必须放在打印单张标签后面,对应相应货号
                    if (para.isPrintOutPackage)
                    {
                        DapperHelper dapper = DapperHelper.CreateDapperHelper(XY.Entity.Configs.XyBoss);
                        IDbConnection conn = dapper.GetConn();
                        //打开连接
                        dapper.OpenConn(conn);
                        IDbTransaction tran = dapper.BeginTransaction(conn);

                        //生成箱号
                        var packageNo = Business.package.Instance.packageNo(purchaseInfo.purchase_order.ToString());
                        package packageModel = new package();
                        packageModel.package_no = packageNo;
                        packageModel.creator = this.PassportInfo.SupplierID;
                        int packageID = Business.package.Instance.PackageInfoInsert(packageModel, conn);
                        if (packageID <= 0)
                            return null;

                        //添加包装箱号关联信息
                        int packageRelationID = Business.package.Instance.PackageRelationInfoInsert(new package_relation()
                        {
                            package_id = packageID,
                            relation_order_id = purchaseInfo.purchase_id,
                            relation_order = purchaseInfo.purchase_order.ToString(),
                            relation_order_type = XY.Enums.OrderType.PO
                        }, conn);

                        if (packageRelationID <= 0)
                            return null;

                        //获取二维码内容
                        var itemOutQRInfo = XY.Business.Item.Instance.ItemOutQrAdd(para.ItemID, QrCodeCopyCount, Enums.OrderType.PO, para.purchaseID, "",
                            PassportInfo?.SupplierID ?? 0, batchNumber, packageID, Enums.BarcodeType.QRCode, QrCodeCount);
                        if (itemOutQRInfo == null)
                            return null;

                        var list = new List<string>();
                        list.Add("外包装");
                        list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemInfo.code, "", fontFamily, fontEmSize, imageSize));
                        list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, packageNo, "", fontFamily, fontEmSize, imageSize));
                        list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, batchNumber ?? "", "", fontFamily, fontEmSize, imageSize));
                        list.Add($"{QrCodeCount}/{unitInfo?.name}");
                        list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, itemOutQRInfo?.Split(',')[1] ?? "", "", fontFamily, fontEmSize, imageSize)[0]);
                        var image = QRCoderHelper.CreateItemLabel(imageSize, itemOutQRInfo, qrcodeSize, qrcodeLeft, qrcodeTop, fontFamily, fontEmSize, list, textLeft, textTop, textSpace);
                        var fileName = $"{Guid.NewGuid()}.png";
                        image.Save(Path.Combine(tempSaveDir, fileName));
                        urlList.Add($"{urlPath}/{fileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                result.Code = Model.ResultCode.Failure;
                result.Message = "系统出错，请联系开发人员";
                result.Data = urlList;
                return Json(JsonConvert.SerializeObject(result));
            }

            result.Code = Model.ResultCode.Success;
            result.Message = "打印成功";
            result.Data = urlList;

            //string str = JsonConvert.SerializeObject(result);
            return Json(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 采购订单生成二维码（生成图像）
        /// </summary>
        /// <remarks>郭小虎创建</remarks>
        /// <param name="purchaseID">采购订单ID</param>
        /// <param name="itemStr">需打印的物料集合字符串</param>
        /// /// <param name="width1">标签宽度（mm）</param>
        /// <param name="height1">标签高度（mm）</param>
        /// <returns></returns>
        [HttpPost]
        public List<String> Order_Generate_QRCode1(int purchaseID, string itemStr, int width1, int height1)
        {
            itemStr = itemStr.TrimNull();

            //参数验证
            if (purchaseID <= 0 || itemStr.Length <= 0)
                return null;

            //物料参数集合处理
            List<PurchaseOrder_Item_QRCode_Para_Model> itemList = null;
            var itemInfoList = new List<String>();
            int imageCount = 0;
            try { itemList = XY.Serializable.JsonStringToObject<List<PurchaseOrder_Item_QRCode_Para_Model>>(itemStr.TrimNull()); } catch (Exception) { }
            if (itemList == null || itemList.Count <= 0)
                return null;

            //获取采购订单信息
            var purchaseInfo = XY.Business.Purchase.Instance.PurchaseInfo(purchaseID);
            if (purchaseInfo == null)
                return null;

            #region 打印参数

            var urlPath = $"{XY.Boss.Configs.BossSiteURL}/temp";
            //生成文件保存目录
            var tempSaveDir = Path.Combine(WebHostEnvironment.WebRootPath, "temp");
            //生成的二维码PDF文件URL路径集合
            var urlList = new List<string>();
            decimal dpi = XY.Boss.Configs.PrintDPI;
            decimal baseWidth = width1;
            decimal baseHeight = height1;
            //int height = System.Convert.ToInt32(baseHeight * dpi / 25.4m);
            //int width = System.Convert.ToInt32(baseWidth * dpi / 25.4m);
            //int xOffset = System.Convert.ToInt32(0 * dpi / 25.4m);
            int height = (int)System.Math.Ceiling(baseHeight * dpi / 25.4m);
            int width = (int)System.Math.Ceiling(baseWidth * dpi / 25.4m);
            int xOffset = (int)System.Math.Ceiling(0 * dpi / 25.4m);
            //int yOffset = System.Convert.ToInt32(0 * dpi / 25.4m);

            //二维码左边距
            var qrcodeLeft = 20;
            //二维码上边距
            var qrcodeTop = 20;
            //二维码大小
            var qrcodeWidth = height - qrcodeTop * 2;

            var fontEmSize = qrcodeWidth / 8;
            //文本左边距
            var textLeft = 0;
            //文本上边距
            var textTop = qrcodeTop + fontEmSize / 3;
            //文本行间距
            float textSpace = (float)fontEmSize * 2;
            if (qrcodeWidth > width / 3)
            {
                qrcodeWidth = width / 3;
                fontEmSize = qrcodeWidth / 8;
                //文本左边距
                textLeft = 0;
                //文本上边距
                textTop = qrcodeTop + fontEmSize / 3;
                //文本行间距
                textSpace = (float)(fontEmSize * 2.0);
            }
            else
            {
                fontEmSize = qrcodeWidth / 8;
                //文本左边距
                textLeft = 0;
                //文本上边距
                textTop = qrcodeTop + fontEmSize / 3;
                //文本行间距
                textSpace = (float)(fontEmSize * 1.3);
            }

            //单行文本宽度限制：图像宽度-二维码左右边距-最右边距-二维码宽度-文本左边距-最大超长20-初始位置
            float textLimitLength = width - qrcodeLeft * 4 - qrcodeWidth - textLeft - 20 - xOffset * 2;
            //整张图片大小
            var imageSize = new System.Drawing.Size(width, height);
            var fontFamily = new System.Drawing.FontFamily("黑体");
            //二维码大小
            var qrcodeSize = new System.Drawing.Size(qrcodeWidth, qrcodeWidth);

            #endregion

            foreach (var item in itemList)
            {
                //获取物料数据
                var itemInfo = XY.Business.Item.Instance.ItemInfo(item.ItemID);
                if (itemInfo == null)
                    return null;

                //获取单位数据
                var unitInfo = XY.Business.Item.Instance.UnitInfo(itemInfo.unit_id);

                //获取生产批号
                string batchNumber = Business.Purchase.Instance.BatchNumberOrder();
                if (string.IsNullOrEmpty(batchNumber))
                    return null;
                //打印的数量
                for (int i = 0; i < item.Copies; i++)
                {
                    //获取二维码内容
                    var itemQRInfo = XY.Business.Item.Instance.ItemQrAdd(item.ItemID, item.LabelQty, Enums.OrderType.PO, purchaseID, "", PassportInfo?.SupplierID ?? 0, batchNumber);
                    if (itemQRInfo == null)
                        return null;

                    var list = new List<string>();
                    //list.Add($"{QRCoderHelper.SplitStringByLabel(textLimitLength, 1, itemInfo.code, "", fontFamily, fontEmSize, imageSize)[0]}");
                    if (itemInfo.rule == 0)
                    {
                        //list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, "部门:" + item.Dept.ToString(), "", fontFamily, fontEmSize, imageSize)[0]);
                    }
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemInfo.code, "", fontFamily, fontEmSize, imageSize));
                    list.Add($"{item.LabelQty}/{unitInfo?.name}");
                    list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, purchaseInfo.purchase_order.ToString(), "", fontFamily, fontEmSize, imageSize)[0]);
                    //list.Add($"{itemQRInfo?.Split(',')[7] ?? ""}");
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemQRInfo?.Split(',')[7] ?? "", "", fontFamily, fontEmSize, imageSize));
                    var image = QRCoderHelper.CreateItemLabel(imageSize, itemQRInfo, qrcodeSize, qrcodeLeft, qrcodeTop, fontFamily, fontEmSize, list, textLeft, textTop, textSpace);
                    var fileName = $"{Guid.NewGuid()}.png";
                    image.Save(Path.Combine(tempSaveDir, fileName));
                    int totalbytes = 9600;
                    int rowbytes = 40;
                    string hex = ZebraUnity.BitmapToHex(image, out totalbytes, out rowbytes);//将图片转成ASCii码
                    string mubanstring = "~DGR:ZLOGO" + imageCount + ".GRF," + totalbytes.ToString() + "," + rowbytes.ToString() + "," + hex;//将图片生成模板指令
                    //传输文件名变化
                    imageCount++;
                    //将图片的ASCii码存储在List表中
                    itemInfoList.Add(mubanstring);
                    urlList.Add($"{urlPath}/{fileName}");
                }
                if (itemInfo.rule == 1)
                {
                    //获取二维码内容
                    var itemOutQRInfo = XY.Business.Item.Instance.ItemOutQrAdd(item.ItemID, item.Copies, Enums.OrderType.PO, purchaseID, "", PassportInfo?.SupplierID ?? 0);
                    if (itemOutQRInfo == null)
                        return null;

                    string info = itemOutQRInfo?.Split(',')[7];

                    var list = new List<string>();
                    //list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, "部门:" + item.Dept.ToString(), "", fontFamily, fontEmSize, imageSize)[0]);
                    list.Add("外包装");
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemInfo.code, "", fontFamily, fontEmSize, imageSize));
                    list.Add($"{item.Copies}/{unitInfo?.name}");
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, purchaseInfo.purchase_order.ToString(), "", fontFamily, fontEmSize, imageSize));
                    list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, itemOutQRInfo?.Split(',')[1] ?? "", "", fontFamily, fontEmSize, imageSize)[0]);
                    var image = QRCoderHelper.CreateItemLabel(imageSize, itemOutQRInfo.Replace(info, "") + info.Split('-')[0], qrcodeSize, qrcodeLeft, qrcodeTop, fontFamily, fontEmSize, list, textLeft, textTop, textSpace);
                    var fileName = $"{Guid.NewGuid()}.png";
                    image.Save(Path.Combine(tempSaveDir, fileName));
                    int totalbytes = 9600;
                    int rowbytes = 40;
                    string hex = ZebraUnity.BitmapToHex(image, out totalbytes, out rowbytes);//将图片转成ASCii码
                    string mubanstring = "~DGR:ZLOGO" + imageCount + ".GRF," + totalbytes.ToString() + "," + rowbytes.ToString() + "," + hex;//将图片生成模板指令
                    imageCount++;
                    itemInfoList.Add(mubanstring);
                    urlList.Add($"{urlPath}/{fileName}");

                }
            }
            //imageInfo.Count = imageCount;
            //将存储的图片ASCii码返回前端供打印方法调用
            return itemInfoList;
        }

        /// <summary>
        /// 获取生成图片后的打印文件
        /// </summary>
        /// <param name="purchaseID"></param>
        /// <param name="itemStr"></param>
        /// <param name="width1"></param>
        /// <param name="height1"></param>
        [HttpPost]
        public List<String> GetPrintFile(int purchaseID, string itemStr, int width1, int height1)
        {
            itemStr = itemStr.TrimNull();

            //参数验证
            if (purchaseID <= 0 || itemStr.Length <= 0)
                return null;

            //物料参数集合处理
            List<PurchaseOrder_Item_QRCode_Para_Model> itemList = null;
            var itemInfoList = new List<String>();
            int imageCount = 0;
            try { itemList = XY.Serializable.JsonStringToObject<List<PurchaseOrder_Item_QRCode_Para_Model>>(itemStr.TrimNull()); } catch (Exception) { }
            if (itemList == null || itemList.Count <= 0)
                return null;

            //获取采购订单信息
            var purchaseInfo = XY.Business.Purchase.Instance.PurchaseInfo(purchaseID);
            if (purchaseInfo == null)
                return null;

            #region 打印参数

            var urlPath = $"{XY.Boss.Configs.BossSiteURL}/temp";
            //生成文件保存目录
            var tempSaveDir = Path.Combine(WebHostEnvironment.WebRootPath, "temp");
            //生成的二维码PDF文件URL路径集合
            var urlList = new List<string>();
            decimal dpi = XY.Boss.Configs.PrintDPI;
            decimal baseWidth = width1;
            decimal baseHeight = height1;
            //int height = System.Convert.ToInt32(baseHeight * dpi / 25.4m);
            //int width = System.Convert.ToInt32(baseWidth * dpi / 25.4m);
            //int xOffset = System.Convert.ToInt32(0 * dpi / 25.4m);
            int height = (int)System.Math.Ceiling(baseHeight * dpi / 25.4m);
            int width = (int)System.Math.Ceiling(baseWidth * dpi / 25.4m);
            int xOffset = (int)System.Math.Ceiling(0 * dpi / 25.4m);
            //int yOffset = System.Convert.ToInt32(0 * dpi / 25.4m);

            //二维码左边距
            var qrcodeLeft = 20;
            //二维码上边距
            var qrcodeTop = 20;
            //二维码大小
            var qrcodeWidth = height - qrcodeTop * 2;

            var fontEmSize = qrcodeWidth / 8;
            //文本左边距
            var textLeft = 0;
            //文本上边距
            var textTop = qrcodeTop + fontEmSize / 3;
            //文本行间距
            float textSpace = (float)fontEmSize * 2;
            if (qrcodeWidth > width / 3)
            {
                qrcodeWidth = width / 3;
                fontEmSize = qrcodeWidth / 8;
                //文本左边距
                textLeft = 0;
                //文本上边距
                textTop = qrcodeTop + fontEmSize / 3;
                //文本行间距
                textSpace = (float)(fontEmSize * 2.0);
            }
            else
            {
                fontEmSize = qrcodeWidth / 8;
                //文本左边距
                textLeft = 0;
                //文本上边距
                textTop = qrcodeTop + fontEmSize / 3;
                //文本行间距
                textSpace = (float)(fontEmSize * 1.3);
            }

            //单行文本宽度限制：图像宽度-二维码左右边距-最右边距-二维码宽度-文本左边距-最大超长20-初始位置
            float textLimitLength = width - qrcodeLeft * 4 - qrcodeWidth - textLeft - 20 - xOffset * 2;
            //整张图片大小
            var imageSize = new System.Drawing.Size(width, height);
            var fontFamily = new System.Drawing.FontFamily("黑体");
            //二维码大小
            var qrcodeSize = new System.Drawing.Size(qrcodeWidth, qrcodeWidth);

            #endregion

            foreach (var item in itemList)
            {
                //获取物料数据
                var itemInfo = XY.Business.Item.Instance.ItemInfo(item.ItemID);
                if (itemInfo == null)
                    return null;

                //获取单位数据
                var unitInfo = XY.Business.Item.Instance.UnitInfo(itemInfo.unit_id);

                string batchNumber = Business.Purchase.Instance.BatchNumberOrder();
                if (string.IsNullOrEmpty(batchNumber))
                    return null;
                //打印的数量
                for (int i = 0; i < item.Copies; i++)
                {
                    //获取二维码内容
                    var itemQRInfo = XY.Business.Item.Instance.ItemQrAdd(item.ItemID, item.LabelQty, Enums.OrderType.PO, purchaseID, "", PassportInfo?.SupplierID ?? 0, batchNumber);
                    if (itemQRInfo == null)
                        return null;

                    var list = new List<string>();
                    //list.Add($"{QRCoderHelper.SplitStringByLabel(textLimitLength, 1, itemInfo.code, "", fontFamily, fontEmSize, imageSize)[0]}");
                    if (itemInfo.rule == 0)
                    {
                        //list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, "部门:" + item.Dept.ToString(), "", fontFamily, fontEmSize, imageSize)[0]);
                    }
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemInfo.code, "", fontFamily, fontEmSize, imageSize));
                    list.Add($"{item.LabelQty}/{unitInfo?.name}");
                    list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, purchaseInfo.purchase_order.ToString(), "", fontFamily, fontEmSize, imageSize)[0]);
                    //list.Add($"{itemQRInfo?.Split(',')[7] ?? ""}");
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemQRInfo?.Split(',')[7] ?? "", "", fontFamily, fontEmSize, imageSize));
                    var image = QRCoderHelper.CreateItemLabel(imageSize, itemQRInfo, qrcodeSize, qrcodeLeft, qrcodeTop, fontFamily, fontEmSize, list, textLeft, textTop, textSpace);
                    var fileName = $"{Guid.NewGuid()}.png";
                    image.Save(Path.Combine(tempSaveDir, fileName));
                    int totalbytes = 9600;
                    int rowbytes = 40;
                    string hex = ZebraUnity.BitmapToHex(image, out totalbytes, out rowbytes);//将图片转成ASCii码
                    string mubanstring = "~DGR:ZLOGO" + imageCount + ".GRF," + totalbytes.ToString() + "," + rowbytes.ToString() + "," + hex;//将图片生成模板指令
                    //传输文件名变化
                    imageCount++;
                    //将图片的ASCii码存储在List表中
                    itemInfoList.Add(mubanstring);
                    urlList.Add($"{urlPath}/{fileName}");
                }
                if (itemInfo.rule == 1)
                {
                    //获取二维码内容
                    var itemOutQRInfo = XY.Business.Item.Instance.ItemOutQrAdd(item.ItemID, item.Copies, Enums.OrderType.PO, purchaseID, "", PassportInfo?.SupplierID ?? 0);
                    if (itemOutQRInfo == null)
                        return null;

                    string info = itemOutQRInfo?.Split(',')[7];

                    var list = new List<string>();
                    //list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, "部门:" + item.Dept.ToString(), "", fontFamily, fontEmSize, imageSize)[0]);
                    list.Add("外包装");
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemInfo.code, "", fontFamily, fontEmSize, imageSize));
                    list.Add($"{item.Copies}/{unitInfo?.name}");
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, purchaseInfo.purchase_order.ToString(), "", fontFamily, fontEmSize, imageSize));
                    list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, itemOutQRInfo?.Split(',')[1] ?? "", "", fontFamily, fontEmSize, imageSize)[0]);
                    var image = QRCoderHelper.CreateItemLabel(imageSize, itemOutQRInfo.Replace(info, "") + info.Split('-')[0], qrcodeSize, qrcodeLeft, qrcodeTop, fontFamily, fontEmSize, list, textLeft, textTop, textSpace);
                    var fileName = $"{Guid.NewGuid()}.png";
                    image.Save(Path.Combine(tempSaveDir, fileName));
                    int totalbytes = 9600;
                    int rowbytes = 40;
                    string hex = ZebraUnity.BitmapToHex(image, out totalbytes, out rowbytes);//将图片转成ASCii码
                    string mubanstring = "~DGR:ZLOGO" + imageCount + ".GRF," + totalbytes.ToString() + "," + rowbytes.ToString() + "," + hex;//将图片生成模板指令
                    imageCount++;
                    itemInfoList.Add(mubanstring);
                    urlList.Add($"{urlPath}/{fileName}");

                }
            }
            //imageInfo.Count = imageCount;
            //将存储的图片ASCii码返回前端供打印方法调用
            return itemInfoList;
        }



        /// <summary>
        /// 采购订单生成二维码
        /// </summary>
        /// <remarks>林思源</remarks>
        /// <param name="purchaseID">采购订单ID</param>
        /// <param name="itemStr">需打印的物料集合字符串</param>
        /// <returns></returns>
        [HttpPost]
        public string Order_QRCode(int purchaseID, string itemStr)
        {
            itemStr = itemStr.TrimNull();
            //参数验证
            if (purchaseID <= 0 || itemStr.Length == 0)
                return null;
            //物料参数集合处理
            List<PurchaseOrder_Item_QRCode_Para_Model> itemList;
            try
            {
                itemList = XY.Serializable.JsonStringToObject<List<PurchaseOrder_Item_QRCode_Para_Model>>(itemStr.TrimNull());
                if (itemList == null || itemList.Count <= 0)
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
            //获取采购订单信息
            var purchaseInfo = XY.Business.Purchase.Instance.PurchaseInfo(purchaseID);
            if (purchaseInfo == null)
                return null;
            var qrcodeInfo = new PurchaseOrder_QRCode_Model();
            qrcodeInfo.Order = purchaseInfo.purchase_order;
            var itemInfoList = new List<PurchaseOrder_QRCode_Item_Model>();

            foreach (var item in itemList)
            {
                var itemModel = new PurchaseOrder_QRCode_Item_Model();
                //获取物料数据
                var itemInfo = XY.Business.Item.Instance.ItemInfo(item.ItemID);
                if (itemInfo == null)
                    return null;
                //获取单位数据
                var unitInfo = XY.Business.Item.Instance.UnitInfo(itemInfo.unit_id);
                itemModel.Rule = item.Rule;
                itemModel.Dept = item.Dept;

                //备注信息
                string remarks = "";
                if (!string.IsNullOrEmpty(item.remark))
                    remarks = item.remark;
                //itemModel.remark = remarks;

                //获取二维码内容
                if (itemModel.Rule == (int)XY.Enums.boss.item_rule.Every)
                {
                    var QRCode = XY.Business.Item.Instance.ItemQrAddWithBox(item.ItemID, item.LabelQty, Enums.OrderType.PO, purchaseID, remarks, PassportInfo?.SupplierID ?? 0, item.Copies);
                    //QRCode.Add($"{remarks}");
                    itemModel.QRCode = QRCode.Take(QRCode.Length - 1).ToArray();   //物料条码数组
                    itemModel.BoxQRCode = QRCode[QRCode.Length-1];     //外包装条码序号范围
                    itemModel.Qty = $"{1}/{unitInfo?.name}";
                    itemModel.Total = $"{QRCode.Length - 1}/{unitInfo?.name}";
                }
                else
                {
                    var QRCode = XY.Business.Item.Instance.ItemQrAdd(item.ItemID, item.LabelQty, Enums.OrderType.PO, purchaseID, remarks, PassportInfo?.SupplierID ?? 0, item.Copies);
                    //QRCode.Add($"{remarks}");
                    itemModel.QRCode = QRCode;
                    itemModel.BoxQRCode = string.Empty;     //无需外包装条码
                    itemModel.Qty = $"{item.LabelQty}/{unitInfo?.name}";
                }
                itemModel.ItemCode = itemInfo.code;
                itemModel.Desc = itemInfo.name;
                itemInfoList.Add(itemModel);
            }
            qrcodeInfo.ItemList = itemInfoList;
            return JsonConvert.SerializeObject(qrcodeInfo);
        }

        /// <summary>
        /// 报价单主页
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="userId">用户ID</param>
        /// <param name="orderKey">关键字</param>
        /// <param name="begin">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Quote(int status = 0, int userId = 0, string orderKey = "", string begin = "", string end = "", int pageSize = 10, int page = 1)
        {
            var model = new Quote_Model
            {
                OrderStatus = status,
                UserId = userId,
                OrderKey = orderKey.TrimNull()
            };
            begin = begin.TrimNull();
            end = end.TrimNull();

            if (begin.Length == 0)
                begin = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd 00:00:00");
            if (end.Length == 0)
                end = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");

            model.Begin = begin;
            model.End = end;
            //参数验证
            if (!DateTime.TryParse(begin, out DateTime beginTime) || !DateTime.TryParse(end, out DateTime endTime))
            {
                ViewBag.Javascript = JS.AlertFocus("时间格式错误！", "Code");
                return View(model);
            }
            //获取员工名称列表数据
            model.UserList = Business.Boss.Instance.UserList()
                .Select(x => new SelectListItem { Text = x.name, Value = x.user_id.ToString() })
                .Prepend(new SelectListItem() { Text = "全部", Value = "0" });

            //获取询价单状态列表
            model.OrderStatusList = XY.Enums.boss.Tools.purchase_quote_status_list(true, 0, "全部").Select(x => new SelectListItem { Text = x.Value, Value = x.Key.ToString() });
            int x = Business.Purchase.Instance.InquiryStatusAutoUpdate();

            int supplierID = PassportInfo.SupplierID;
            //采购订单列表
            if (PassportInfo.Name == "管理员")
                supplierID = 0;

            model.OrderList = Business.Purchase.Instance.QuoteList(model.OrderKey,beginTime, endTime, model.OrderStatus, model.UserId, supplierID, pageSize, page);
            
            return View(model);
        }

        /// <summary>
        /// 报价单详情页
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="backurl"></param>
        /// <returns></returns>
        public IActionResult Quote_Detail(int inquiryID,int quoteID, int pageSize = 100, int page = 1, string backurl = "/purchase/quote")
        {
            //参数验证
            if (inquiryID <= 0)
                return RedirectPage("请求参数有误！", backurl);
            Quote_Detail_Model model = new Quote_Detail_Model();
            model.BackUrl = backurl;
            //var quote_info = XY.Business.Purchase.Instance.Purchase_Quote_Info(PassportInfo.SupplierID, inquiryID);
            var quote_info = XY.Business.Purchase.Instance.PurchaseQuoteInfo(quoteID);
            var order_info = XY.Business.Purchase.Instance.InquiryInfo(inquiryID);
            if (order_info == null)
                return RedirectPage("无对应ID的询价单", backurl);
            model.Inquiry_Id = order_info.pi_id;
            model.Supplier_Id = PassportInfo.SupplierID;
            model.Order_No = order_info.pi_order;
            model.Creator = order_info.creator;
            model.Creator_Name = XY.Business.Boss.Instance.UserName(model.Creator);
            model.Create_time = order_info.create_time;
            model.Deliver_time = order_info.delivery_time;
            model.End_time = order_info.end_time;
            model.Status = XY.Enums.boss.Tools.purchase_inquiry_status(order_info.status);
            model.Remark = order_info.remark;
            if (quote_info == null)
            {
                //物料列表分页
                var list = XY.Business.Purchase.Instance.Inquiry_ItemList(inquiryID, pageSize, page);
                model.Item_List = list;
                model.IsQuote = false;
            }
            else
            {
                var list = XY.Business.Purchase.Instance.Quote_ItemList(quote_info.pq_id, pageSize, page);;
                model.Item_List = list;
                model.IsQuote = true;
                model.Promise_Deliver_time = quote_info.plan_delivery_time;
            }
            return View(model);
        }

        /// <summary>
        /// 创建报价单
        /// </summary>
        /// <param name="Deliver_time">计划截止时间</param>
        /// <param name="SupplierId">供应商ID</param>
        /// <param name="InquiryId">报价单ID</param>
        /// <param name="InquiryItemIds">报价单物料ID数组</param>
        /// <param name="PriceNums">报价数组</param>
        /// <returns></returns>
        [HttpPost]
        public int Quote_Insert(string Deliver_time, string SupplierId, string InquiryId, string InquiryItemIds, string PriceNums)
        {
            DateTime? time = XY.Serializable.JsonStringToObject<DateTime?>(Deliver_time);
            if (time == null)
                return -2;
            int Supplier_Id = XY.Serializable.JsonStringToObject<int>(SupplierId);
            int Inquiry_Id = XY.Serializable.JsonStringToObject<int>(InquiryId);
            int[] pii_ids = XY.Serializable.JsonStringToObject<int[]>(InquiryItemIds);
            decimal[] Prices = XY.Serializable.JsonStringToObject<decimal[]>(PriceNums);
            if(Supplier_Id < 0 || Inquiry_Id < 0 || pii_ids == null || Prices == null || pii_ids.Length != Prices.Length)
                return -1;
            int pq_id = XY.Business.Purchase.Instance.QuoteAdd(time.Value.ToString("yyyy-MM-dd HH:mm:ss"), Supplier_Id, Inquiry_Id);
            for (int i = 0; i < pii_ids.Length; i++)
            {
                int pii_id = pii_ids[i];
                decimal price = Prices[i];
                int pqi_id = XY.Business.Purchase.Instance.Quote_Item_Add(pq_id, pii_id, price);
            }
            return 1;
        }


        [HttpPost]
        public IEnumerable<string> Order_Generate_QRCode2(int purchaseID, string itemStr, int width1, int height1)
        {
            itemStr = itemStr.TrimNull();

            //参数验证
            if (purchaseID <= 0 || itemStr.Length <= 0)
                return null;

            //物料参数集合处理
            List<PurchaseOrder_Item_QRCode_Para_Model> itemList = null;
            try { itemList = XY.Serializable.JsonStringToObject<List<PurchaseOrder_Item_QRCode_Para_Model>>(itemStr.TrimNull()); } catch (Exception) { }
            if (itemList == null || itemList.Count <= 0)
                return null;

            //获取采购订单信息
            var purchaseInfo = XY.Business.Purchase.Instance.PurchaseInfo(purchaseID);
            if (purchaseInfo == null)
                return null;

            #region 打印参数

            var urlPath = $"{XY.Boss.Configs.BossSiteURL}/temp";
            //生成文件保存目录
            var tempSaveDir = Path.Combine(WebHostEnvironment.WebRootPath, "temp");
            //生成的二维码PDF文件URL路径集合
            var urlList = new List<string>();
            decimal dpi = XY.Boss.Configs.PrintDPI;
            decimal baseWidth = 50; 
            decimal baseHeight = 80;
            int height = System.Convert.ToInt32(baseHeight * dpi / 25.4m);
            int width = System.Convert.ToInt32(baseWidth * dpi / 25.4m);
            int xOffset = System.Convert.ToInt32(0 * dpi / 25.4m);
            int yOffset = System.Convert.ToInt32(0 * dpi / 25.4m);

            //标签纸宽50，高80，二维码大小199
            //二维码左边距
            var qrcodeLeft = 20;
            //二维码上边距
            var qrcodeTop = 20;
            //二维码大小
            //var qrcodeWidth = (height - qrcodeTop * 2)/3;

            var qrcodeWidth = 200;

            var fontEmSize = qrcodeWidth / 8;
            //文本左边距
            var textLeft =20;
            //文本上边距
            var textTop =fontEmSize / 3;
            //文本行间距
            float textSpace = (float)(fontEmSize * 1.5);
            //if (qrcodeWidth > width / 3)
            //{
            //    qrcodeWidth = width / 3;
            //    fontEmSize = qrcodeWidth / 8;
            //    //文本左边距
            //    textLeft = 0;
            //    //文本上边距
            //    textTop = qrcodeTop + fontEmSize / 3;
            //    //文本行间距
            //    textSpace = (float)(fontEmSize * 1.4);
            //}
            //else
            //{
            //    fontEmSize = qrcodeWidth / 8;
            //    //文本左边距
            //    textLeft = 0;
            //    //文本上边距
            //    textTop = qrcodeTop + fontEmSize / 3;
            //    //文本行间距
            //    textSpace = (float)(fontEmSize * 1.3);
            //}

            //单行文本宽度限制：图像宽度-二维码左右边距-最右边距-二维码宽度-文本左边距-最大超长20-初始位置
            //float textLimitLength = width - qrcodeLeft * 4 - qrcodeWidth - textLeft - 20 - xOffset * 2;
            float textLimitLength= width- textLeft*2;
            //整张图片大小
            var imageSize = new System.Drawing.Size(width, height);
            var fontFamily = new System.Drawing.FontFamily("黑体");
            //二维码大小
            var qrcodeSize = new System.Drawing.Size(qrcodeWidth, qrcodeWidth);

            #endregion

            foreach (var item in itemList)
            {
                //获取物料数据
                var itemInfo = XY.Business.Item.Instance.ItemInfo(item.ItemID);
                if (itemInfo == null)
                    return null;

                //获取单位数据
                var unitInfo = XY.Business.Item.Instance.UnitInfo(itemInfo.unit_id);

                //获取生产批号
                string batchNumber = Business.Purchase.Instance.BatchNumberOrder();
                if (string.IsNullOrEmpty(batchNumber))
                    break;

                //备注信息
                string remarks = "";
                if (!string.IsNullOrEmpty(item.remark))
                    remarks = item.remark;

                //打印的数量
                for (int i = 0; i < item.Copies; i++)
                {
                    //获取二维码内容
                    var itemQRInfo = XY.Business.Item.Instance.ItemQrAdd(item.ItemID, item.LabelQty, Enums.OrderType.PO, purchaseID, remarks, PassportInfo?.SupplierID ?? 0, batchNumber);
                    if (itemQRInfo == null)
                        return null;

                    itemQRInfo += $",{remarks}";

                    var list = new List<string>();
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemInfo.code, "", fontFamily, fontEmSize, imageSize));
                    list.Add($"{item.LabelQty}/{unitInfo?.name}");
                    list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, purchaseInfo.purchase_order.ToString(), "", fontFamily, fontEmSize, imageSize)[0]);
                    //list.Add($"{itemQRInfo?.Split(',')[7] ?? ""}");
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemQRInfo?.Split(',')[7] ?? "", "", fontFamily, fontEmSize, imageSize));
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, batchNumber ?? "", "", fontFamily, fontEmSize, imageSize));
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, remarks, "", fontFamily, fontEmSize, imageSize));
                    var image = QRCoderHelper.CreateItemLabel2(imageSize, itemQRInfo, qrcodeSize, qrcodeLeft, qrcodeTop, fontFamily, fontEmSize, list, textLeft, textTop, textSpace);
                    var fileName = $"{Guid.NewGuid()}.png";
                    image.Save(Path.Combine(tempSaveDir, fileName));
                    urlList.Add($"{urlPath}/{fileName}");
                }
                if (itemInfo.rule == 1)
                {
                    //获取二维码内容
                    var itemOutQRInfo = XY.Business.Item.Instance.ItemOutQrAdd(item.ItemID, item.Copies, Enums.OrderType.PO, purchaseID, "", PassportInfo?.SupplierID ?? 0, batchNumber);
                    if (itemOutQRInfo == null)
                        return null;

                    string info = itemOutQRInfo?.Split(',')[7];

                    var list = new List<string>();
                    //list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, "部门:"+item.Dept.ToString(), "", fontFamily, fontEmSize, imageSize)[0]);
                    list.Add("外包装");
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemInfo.code, "", fontFamily, fontEmSize, imageSize));
                    list.Add($"{item.Copies}/{unitInfo?.name}");
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, purchaseInfo.purchase_order.ToString(), "", fontFamily, fontEmSize, imageSize));
                    list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, itemOutQRInfo?.Split(',')[1] ?? "", "", fontFamily, fontEmSize, imageSize)[0]);
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, batchNumber ?? "", "", fontFamily, fontEmSize, imageSize));
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, remarks, "", fontFamily, fontEmSize, imageSize));
                    var image = QRCoderHelper.CreateItemLabel(imageSize, itemOutQRInfo.Replace(info, "") + info.Split('-')[0], qrcodeSize, qrcodeLeft, qrcodeTop, fontFamily, fontEmSize, list, textLeft, textTop, textSpace);
                    var fileName = $"{Guid.NewGuid()}.png";
                    image.Save(Path.Combine(tempSaveDir, fileName));
                    urlList.Add($"{urlPath}/{fileName}");

                }
            }
            return urlList;
        }

        /// <summary>
        /// 获取生产订单信息
        /// </summary>
        /// <param name="purchaseID"></param>
        /// <returns></returns>
        public IActionResult Order_GetProduceOrderInfo(int purchaseID, int offset = 0, int limit = 10)
        {
            var index = (offset / limit) + 1;
            Model.Result<PurchaseOrder_RelationOrderInfo_List> result = new Model.Result<PurchaseOrder_RelationOrderInfo_List>();
            result.Data= new PurchaseOrder_RelationOrderInfo_List();
            result.Data.data = new List<PurchaseOrder_RelationOrderInfo_Model>();
            //通过采购订单查询对应的生产订单信息
            var purchaseInfo= Business.Purchase.Instance.PurchaseInfo(purchaseID);
            int orderID = int.Parse(purchaseInfo.purchase_order.ToUpper().Replace("PO-", ""));
            DataTable dt=Business.Produce.Instance.GetProduceOrderPurchaseInfo(orderID);
            if (dt != null || dt.Rows.Count >= 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    if (string.IsNullOrEmpty(dr[0].ToString()))
                        continue;

                    string produceOrder ="PRO-" + dr[0].ToString();
                    var produceInfo = Business.Produce.Instance.ProduceInfo(produceOrder);
                    if (produceInfo == null)
                        continue;

                    var itemInfo = Business.Item.Instance.ItemInfo(produceInfo.item_id);
                    if (produceInfo==null||itemInfo == null)
                        continue;

                    PurchaseOrder_RelationOrderInfo_Model da = new PurchaseOrder_RelationOrderInfo_Model();
                    da.PurchaseID = purchaseID;
                    da.ProduceID = produceInfo.produce_id;
                    da.ItemCode = itemInfo.code ?? "";
                    da.ItemID = produceInfo.item_id;
                    da.ProduceOrder = produceInfo.produce_order;
                    da.Copies = 1;
                    da.ItemQty = 1;
                    da.Remarks = "";
                    da.ItemRule = XY.Enums.boss.Tools.item_rule(itemInfo.rule);

                    result.Data.data.Add(da);
                }
            }
            int total = 0;
            if (result.Data.data != null)
                total = result.Data.data.Count();

            JObject data = new JObject();
            data.Add("total", total);
            data.Add("data", JsonConvert.SerializeObject(result.Data.data));

            return Json(JsonConvert.SerializeObject(data));
        }

        /// <summary>
        /// 打印生产订单成品物料
        /// </summary>
        /// <param name="produceID"></param>
        public IEnumerable<string> Order_Product_ItemPrint(string printData)
        {
            //生成的二维码PDF文件URL路径集合
            var urlList = new List<string>();
            List<PurchaseOrder_RelationOrderInfo_Model> lstdata= JsonConvert.DeserializeObject<List<PurchaseOrder_RelationOrderInfo_Model>>(printData);
            if (lstdata.Count()>0)
            {
                var strRemarks= lstdata.Where(a => a.Remarks != "").Count();
                #region 打印参数

                var urlPath = $"{XY.Boss.Configs.BossSiteURL}/temp";
                //生成文件保存目录
                var tempSaveDir = Path.Combine(WebHostEnvironment.WebRootPath, "temp");
                decimal dpi = XY.Boss.Configs.PrintDPI;
                decimal baseWidth = XY.Boss.Configs.PrintWidth;
                decimal baseHeight = XY.Boss.Configs.PrintHeight;
                int height = System.Convert.ToInt32(baseHeight * dpi / 25.4m);
                int width = System.Convert.ToInt32(baseWidth * dpi / 25.4m);
                int xOffset = System.Convert.ToInt32(0 * dpi / 25.4m);
                //int yOffset = System.Convert.ToInt32(0 * dpi / 25.4m);

                //二维码左边距
                var qrcodeLeft = 20;
                //二维码上边距
                var qrcodeTop = 20;
                //二维码大小
                var qrcodeWidth = height - qrcodeTop * 2;

                var fontEmSize = qrcodeWidth / 8;
                //文本左边距
                var textLeft = 0;
                //文本上边距
                var textTop = qrcodeTop+ fontEmSize / 3;
                //文本行间距
                float textSpace = (float)fontEmSize * 2;

                if (qrcodeWidth > width / 3)
                {
                    qrcodeWidth = width / 3;
                    fontEmSize = qrcodeWidth / 8;
                    //文本左边距
                    textLeft = 0;
                    //文本上边距
                    textTop = qrcodeTop + fontEmSize / 3;
                    if (strRemarks > 0)
                        //文本行间距
                        textSpace = (float)(fontEmSize * 1.5);
                    else
                        textSpace = (float)(fontEmSize * 2);
                }
                else
                {
                    if (strRemarks>0)
                        fontEmSize = qrcodeWidth / 8;
                    else
                        fontEmSize = qrcodeWidth / 6;
                    //文本左边距
                    textLeft = 0;
                    //文本上边距
                    textTop = qrcodeTop + fontEmSize / 3;
                    //文本行间距
                    textSpace = (float)(fontEmSize * 1.3);
                }
                //单行文本宽度限制：图像宽度-二维码左右边距-最右边距-二维码宽度-文本左边距-最大超长20-初始位置
                float textLimitLength = width - qrcodeLeft * 4 - qrcodeWidth - textLeft - 20 - xOffset * 2;
                //整张图片大小
                var imageSize = new System.Drawing.Size(width, height);
                var fontFamily = new System.Drawing.FontFamily("黑体");
                //二维码大小
                var qrcodeSize = new System.Drawing.Size(qrcodeWidth, qrcodeWidth);

                #endregion


                foreach (var t in lstdata)
                {
                    var produceInfo = Business.Produce.Instance.ProduceInfo(t.ProduceID);
                    if (produceInfo == null)
                        return null;

                    var itemInfo = Business.Item.Instance.ItemInfo(produceInfo.item_id);

                    //获取单位数据
                    var unitInfo = XY.Business.Item.Instance.UnitInfo(itemInfo.unit_id);

                    //获取生产批号
                    string batchNumber = Business.Purchase.Instance.BatchNumberOrder();
                    if (string.IsNullOrEmpty(batchNumber))
                        return null;

                    //获取二维码内容
                    var itemQRInfo = XY.Business.Item.Instance.ItemQrAdd(itemInfo.item_id, t.ItemQty, Enums.OrderType.PRO, t.ProduceID, t.Remarks, this.PassportInfo.SupplierID, batchNumber);
                    if (itemQRInfo == null)
                        return null;

                    if (!string.IsNullOrEmpty(t.Remarks))
                        itemQRInfo += $",{t.Remarks}";

                    for (int i = 0; i < t.Copies; i++)
                    {
                        var list = new List<string>();
                        list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemInfo.code, "", fontFamily, fontEmSize, imageSize));
                        list.Add($"{t.ItemQty}/{unitInfo?.name}");
                        //list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, produceInfo.produce_order, "", fontFamily, fontEmSize, imageSize));
                        list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemQRInfo?.Split(',')[7] ?? "", "", fontFamily, fontEmSize, imageSize));
                        list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, batchNumber ?? "", "", fontFamily, fontEmSize, imageSize));
                        list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, t.Remarks, "", fontFamily, fontEmSize, imageSize));
                        var image = QRCoderHelper.CreateItemLabel(imageSize, itemQRInfo, qrcodeSize, qrcodeLeft, qrcodeTop, fontFamily, fontEmSize, list, textLeft, textTop, textSpace);
                        var fileName = $"{Guid.NewGuid()}.png";
                        image.Save(Path.Combine(tempSaveDir, fileName));
                        urlList.Add($"{urlPath}/{fileName}");
                    }
                }
            }

            return urlList;
        }

        #endregion

        #region 序列号绑定

        /// <summary>
        /// 获取待绑定的序列号
        /// </summary>
        /// <param name="purchaseID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public IActionResult Order_OrderSerialNumberWaitBind(int purchaseID, int itemID)
        {
            Model.Result<List<purchase_item_serial_number_Model>> result = new Model.Result<List<purchase_item_serial_number_Model>>();
            List<purchase_item_serial_number_Model> pisnmList = new List<purchase_item_serial_number_Model>();
            try
            {
                //判断采购订单状态
                var purchaseinfo=Business.Purchase.Instance.PurchaseInfo(purchaseID);
                if (purchaseinfo == null)
                {
                    result.Code = Model.ResultCode.Failure;
                    result.Message = "未获取到采购订单信息";
                    result.Data = pisnmList;
                    return Json(JsonConvert.SerializeObject(result));
                }

                //if (purchaseinfo.status == Enums.boss.purchase_status.complete)
                //{
                //    result.Code = Model.ResultCode.Failure;
                //    result.Message = "该采购单已收货完成，不允许绑定序列号！";
                //    result.Data = pisnmList;
                //    return Json(JsonConvert.SerializeObject(result));
                //}

                var purchaseItemInfo = Business.Purchase.Instance.PurchaseItemInfo(purchaseID, itemID);
                if (purchaseItemInfo != null)
                {
                    var lst = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberList(purchaseID, itemID);
                    if (lst.Count() > 0)
                    {
                        foreach (var serialNumber in lst)
                            pisnmList.Add(new purchase_item_serial_number_Model() { serial_number = serialNumber.serial_number });

                    }
                    //还可以绑定序列号数量=物料总数量-已绑定的序列号数量
                    var count = purchaseItemInfo.item_qty - lst.Count();
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                            pisnmList.Add(new purchase_item_serial_number_Model() { serial_number = "" });
                    }
                }
                else
                {
                    result.Code = Model.ResultCode.Failure;
                    result.Message = "获取采购物料信息失败";
                    result.Data = pisnmList;
                    return Json(JsonConvert.SerializeObject(result));
                }

                result.Code = Model.ResultCode.Success;
                result.Message = "获取采购物料信息成功";
                result.Data = pisnmList;
                return Json(JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                result.Code = Model.ResultCode.Failure;
                result.Message = "获取采购物料信息失败," + ex.Message;
                result.Data = pisnmList;
                return Json(JsonConvert.SerializeObject(result));
            }
        }

        /// <summary>
        /// 采购单物料序列号绑定
        /// </summary>
        /// <param name="purchaseID"></param>
        /// <param name="itemID"></param>
        /// <param name="serialNumbarArr"></param>
        /// <param name="isAdd">0:添加，1：修改</param>
        /// <returns></returns>
        public IActionResult Order_SerialNumberBind2(int purchaseID, int itemID, string serialNumbarArr)
        {
            Model.Result<int> result = new Model.Result<int>();
            DapperHelper dapper = DapperHelper.CreateDapperHelper(XY.Entity.Configs.XyBoss);
            IDbConnection conn = null;
            IDbTransaction tran = null;

            try
            {
                if (string.IsNullOrWhiteSpace(serialNumbarArr))
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "序列号不能为空");
                    return Json(JsonConvert.SerializeObject(result));
                }
                if (purchaseID == 0 || itemID == 0)
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "采购订单信息错误");
                    return Json(JsonConvert.SerializeObject(result));
                }

                string[] serialNumbers = serialNumbarArr.Split(",");
                if (serialNumbers.Length == 0)
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "序列号信息为空");
                    return Json(JsonConvert.SerializeObject(result));
                }

                //判断输入的序列号是否重复
                if (XY.Collection.ArrayIsDuplication(serialNumbers))
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"输入的序列号有重复请确认！");
                    return Json(JsonConvert.SerializeObject(result));
                }

                conn = dapper.GetConn();
                //打开连接
                dapper.OpenConn(conn);
                //开启事务
                tran = dapper.BeginTransaction(conn);
                int count = 0;

                for (int i = 0; i < serialNumbers.Length; i++)
                {
                    if (serialNumbers[i].TrimNull() == "")
                        continue;

                    //判断序列号是否绑定
                    var lst = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberList(serialNumbers[i].TrimNull());
                    if (lst.Count() > 0)
                    {
                        result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"序列号:{serialNumbers[i].TrimNull()}已经绑定，请勿重复绑定");
                        return Json(JsonConvert.SerializeObject(result));
                    }

                    purchase_item_serial_number pisn = new purchase_item_serial_number();
                    pisn.item_id = itemID;
                    pisn.purchase_id = purchaseID;
                    pisn.serial_number = serialNumbers[i].TrimNull();
                    pisn.creator = this.PassportInfo.SupplierID;

                    int t = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberAdd(pisn, conn);
                    if (t <= 0)
                    {
                        result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "序列号绑定失败");
                        return Json(JsonConvert.SerializeObject(result));
                    }
                    else
                    {
                        count++;
                    }
                }

                if (count>0)
                {
                    result = Model.Tools.BuildResult(1, Model.ResultCode.Success, "绑定序列号成功");
                    return Json(JsonConvert.SerializeObject(result));
                }
                else
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "绑定序列号失败");
                    return Json(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception ex)
            {
                result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "绑定序列号失败");
                return Json(JsonConvert.SerializeObject(result));
            }
            finally
            {
                if (tran != null && conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        if (result.Code == Model.ResultCode.Success)
                            //提交事务
                            dapper.Commit(tran, conn);
                        else
                            //回滚事务
                            dapper.Rollback(tran, conn);
                    }
                    //释放连接
                    if (conn != null)
                        dapper.DisposeConn(conn);
                }
            }
        }

        /// <summary>
        /// 获取已绑定的序列号信息
        /// </summary>
        /// <param name="purchaseID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public IActionResult Order_OrderSerialNumberHasBind(int purchaseID, int itemID)
        {
            Model.Result<List<purchase_item_serial_number_Model>> result = new Model.Result<List<purchase_item_serial_number_Model>>();
            List<purchase_item_serial_number_Model> pisnmList = new List<purchase_item_serial_number_Model>();
            try
            {
                var purchaseItemInfo = Business.Purchase.Instance.PurchaseItemInfo(purchaseID, itemID);
                if (purchaseItemInfo==null)
                {
                    result.Code = Model.ResultCode.Failure;
                    result.Message = "未获取到采购单物料信息";
                    result.Data = pisnmList;
                    return Json(JsonConvert.SerializeObject(result));
                }

                //判断序列号是否绑定
                var lst = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberList(purchaseID, itemID);
                if (lst.Count() > 0)
                    pisnmList.AddRange(lst.Select(a => new purchase_item_serial_number_Model() { id=a.id,serial_number = a.serial_number,
                        packageNo=Business.package.Instance.PackageInfo(a.package_id)?.package_no }).ToList());

                result.Code = Model.ResultCode.Success;
                result.Message = "获取采购单物料绑定序列号成功";
                result.Data = pisnmList;
                return Json(JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                result.Code = Model.ResultCode.Failure;
                result.Message = "获取采购单物料绑定序列号失败";
                result.Data = pisnmList;
                return Json(JsonConvert.SerializeObject(result));
            }
        }

        /// <summary>
        /// 采购单物料序列号绑定
        /// </summary>
        /// <param name="purchaseID"></param>
        /// <param name="itemID"></param>
        /// <param name="serialNumbarArr"></param>
        /// <param name="isAdd">0:添加，1：修改</param>
        /// <returns></returns>
        public IActionResult Order_SerialNumberBindUpdate(int purchaseID, int itemID, string serialNumbarArr)
        {
            Model.Result<int> result = new Model.Result<int>();
            DapperHelper dapper = DapperHelper.CreateDapperHelper(XY.Entity.Configs.XyBoss);
            IDbConnection conn = null;
            IDbTransaction tran = null;

            try
            {
                if (string.IsNullOrWhiteSpace(serialNumbarArr))
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "序列号不能为空");
                    return Json(JsonConvert.SerializeObject(result));
                }

                if (purchaseID == 0 || itemID == 0)
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "采购订单信息错误");
                    return Json(JsonConvert.SerializeObject(result));
                }

                string[] serialNumbers = serialNumbarArr.Split(",");
                if (serialNumbers.Length == 0)
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "序列号信息为空");
                    return Json(JsonConvert.SerializeObject(result));
                }

                //判断输入的序列号是否重复
                if (XY.Collection.ArrayIsDuplication(serialNumbers))
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"输入的序列号有重复请确认！");
                    return Json(JsonConvert.SerializeObject(result));
                }

                conn = dapper.GetConn();
                //打开连接
                dapper.OpenConn(conn);
                //开启事务
                tran = dapper.BeginTransaction(conn);
                int count = 0;

                if (serialNumbers.Length > 0)
                {
                    //先删除再添加
                    var serialNumberList=Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberList(purchaseID, itemID);
                    foreach (var item in serialNumberList)
                    {
                        //判断是否已收料
                        //Business.StockReceipt.StockReceipt.Instance.StockReceiptInfo(purchaseID);
                        var stockReceiptItemQrInfo=Business.StockReceipt.StockReceipt.Instance.StockReceiptItemQrInfo(item.serial_number);
                        if (stockReceiptItemQrInfo!=null)
                        {
                            result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"存在已经收料的序列号，不能进行修改！");
                            return Json(JsonConvert.SerializeObject(result));
                        }
                    }

                    var snID = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberDelete(purchaseID, itemID, conn);
                    if (snID <= 0)
                    {
                        result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"序列号绑定修改失败！");
                        return Json(JsonConvert.SerializeObject(result));
                    }

                    for (int i = 0; i < serialNumbers.Length; i++)
                    {
                        if (serialNumbers[i].TrimNull() == "")
                            continue;

                        //判断序列号是否绑定
                        var lst = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberList(serialNumbers[i].TrimNull(),conn);
                        if (lst.Count() > 0)
                        {
                            result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"序列号:{serialNumbers[i].TrimNull()}已经绑定，请勿重复绑定");
                            return Json(JsonConvert.SerializeObject(result));
                        }

                        purchase_item_serial_number pisn = new purchase_item_serial_number();
                        pisn.item_id = itemID;
                        pisn.purchase_id = purchaseID;
                        pisn.serial_number = serialNumbers[i].TrimNull();
                        pisn.creator = this.PassportInfo.SupplierID;

                        int t = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberAdd(pisn, conn);
                        if (t <= 0)
                        {
                            result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "序列号绑定失败");
                            return Json(JsonConvert.SerializeObject(result));
                        }
                        else
                        {
                            count++;
                        }
                    }
                }

                if (count > 0)
                {
                    result = Model.Tools.BuildResult(1, Model.ResultCode.Success, "序列号绑定修改成功");
                    return Json(JsonConvert.SerializeObject(result));
                }
                else
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "序列号绑定修改失败");
                    return Json(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception ex)
            {
                result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "序列号绑定修改失败");
                return Json(JsonConvert.SerializeObject(result));
            }
            finally
            {
                if (tran != null && conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        if (result.Code == Model.ResultCode.Success)
                            //提交事务
                            dapper.Commit(tran, conn);
                        else
                            //回滚事务
                            dapper.Rollback(tran, conn);
                    }
                    //释放连接
                    if (conn != null)
                        dapper.DisposeConn(conn);
                }
            }
        }

        /// <summary>
        /// 序列号绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Order_SerialNumberImport(purchase_Item_Import_Model model)
        {
            Model.Result<int> result = new Model.Result<int>();
            DapperHelper dapper = DapperHelper.CreateDapperHelper(XY.Entity.Configs.XyBoss);
            IDbConnection conn = null;
            IDbTransaction tran = null;
            try
            {
                //参数验证
                if (model.File == null || model.File.Length == 0)
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "导入文件错误");
                    return Json(JsonConvert.SerializeObject(result));
                }

                if (!model.File.ContentType.EqualsIgnoreCase(Enums.Tools.HttpContentType(Enums.HttpContentType.xlsx)) &&
                    !model.File.ContentType.EqualsIgnoreCase(Enums.Tools.HttpContentType(Enums.HttpContentType.xls)))
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "导入文件类型错误");
                    return Json(JsonConvert.SerializeObject(result));
                }

                Enums.HttpContentType type = Enums.HttpContentType.xlsx;
                if (model.File.ContentType.EqualsIgnoreCase(Enums.Tools.HttpContentType(Enums.HttpContentType.xls)))
                    type = Enums.HttpContentType.xls;

                //验证物料
                if (model.itemID==0|| model.purchaseID == 0)
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.ParametersIncorrect, "传入参数错误");
                    return Json(JsonConvert.SerializeObject(result));
                }

                var purchaseInfo = Business.Purchase.Instance.PurchaseInfo(model.purchaseID);
                if (purchaseInfo.status == Enums.boss.purchase_status.complete)
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.ParametersIncorrect, "该采购单已收货完成，不允许绑定序列号");
                    return Json(JsonConvert.SerializeObject(result));
                }

                //判断这个订单物料序列号是否已经有收料
                var serialNumberList = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberList(model.purchaseID, model.itemID);
                foreach (var item in serialNumberList)
                {
                    var stockReceiptItemQrInfo = Business.StockReceipt.StockReceipt.Instance.StockReceiptItemQrInfo(item.serial_number);
                    if (stockReceiptItemQrInfo != null)
                    {
                        result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"存在已经收料的序列号，不能进行绑定！");
                        return Json(JsonConvert.SerializeObject(result));
                    }
                }

                conn = dapper.GetConn();
                //打开连接
                dapper.OpenConn(conn);
                //开启事务
                tran = dapper.BeginTransaction(conn);

                //读取execl数据
                var successCount = 0;
                var failCount = 0;
                DataTable dt = XY.ExeclHelper.ReadExeclData(type, model.File.OpenReadStream());
                if (dt.Rows.Count > 0)
                {
                    if (!dt.Columns.Contains("序列号"))
                    {
                        result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "文件格式不正确，请确认！");
                        return Json(JsonConvert.SerializeObject(result));
                    }
                    List<string> lstSerialNumber = dt.AsEnumerable().Select(x => x.Field<string>("序列号")).ToList();

                    if (lstSerialNumber.Count>0)
                    {
                        if (serialNumberList.Count() > 0)
                        {
                            var snID = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberDelete(model.purchaseID, model.itemID, conn);
                            if (snID <= 0)
                            {
                                result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"序列号绑定修改失败！");
                                return Json(JsonConvert.SerializeObject(result));
                            }
                        }

                        foreach (var serialNumber in lstSerialNumber)
                        {
                            if (string.IsNullOrEmpty(serialNumber))
                                continue;

                            //var snID = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberDelete(purchaseID, itemID, conn);
                            //if (snID <= 0)
                            //{
                            //    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"序列号绑定修改失败！");
                            //    return Json(JsonConvert.SerializeObject(result));
                            //}

                            //判断序列号是否已经绑定
                            var lst = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberList(serialNumber.TrimNull(), conn);
                            if (lst.Count() > 0)
                            {
                                result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"序列号:{serialNumber.TrimNull()}已经绑定，请勿重复绑定");
                                return Json(JsonConvert.SerializeObject(result));
                            }

                            purchase_item_serial_number pisn = new purchase_item_serial_number();
                            pisn.item_id = model.itemID;
                            pisn.purchase_id = model.purchaseID;
                            pisn.serial_number = serialNumber.TrimNull();
                            pisn.creator = this.PassportInfo.SupplierID;

                            int t = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberAdd(pisn, conn);
                            if (t <= 0)
                            {
                                result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "序列号绑定失败");
                                return Json(JsonConvert.SerializeObject(result));
                            }
                            else
                            {
                                successCount++;
                            }
                        }
                    }
                }
                else
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"获取序列号为空！");
                    return Json(JsonConvert.SerializeObject(result));
                }

                result = Model.Tools.BuildResult(0, Model.ResultCode.Success, $"序列号绑定成功 {successCount} 条记录");
                return Json(JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, $"绑定序列号失败");
                return Json(JsonConvert.SerializeObject(result));
            }
            finally
            {
                if (tran != null && conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        if (result.Code == Model.ResultCode.Success)
                            //提交事务
                            dapper.Commit(tran, conn);
                        else
                            //回滚事务
                            dapper.Rollback(tran, conn);
                    }
                    //释放连接
                    if (conn != null)
                        dapper.DisposeConn(conn);
                }
            }
        }

        /// <summary>
        /// 采购单物料序列号绑定
        /// </summary>
        /// <param name="purchaseID"></param>
        /// <param name="itemID"></param>
        /// <param name="serialNumbarArr"></param>
        /// <param name="isAdd">0:添加，1：修改</param>
        /// <returns></returns>
        public IActionResult Order_SerialNumberBind(int purchaseID, int itemID, string serialNumbarArr)
        {
            Model.Result<int> result = new Model.Result<int>();
            DapperHelper dapper = DapperHelper.CreateDapperHelper(XY.Entity.Configs.XyBoss);
            IDbConnection conn = null;
            IDbTransaction tran = null;

            try
            {
                if (string.IsNullOrWhiteSpace(serialNumbarArr))
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "序列号不能为空");
                    return Json(JsonConvert.SerializeObject(result));
                }
                if (purchaseID == 0 || itemID == 0)
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "采购订单信息错误");
                    return Json(JsonConvert.SerializeObject(result));
                }

                string[] serialNumbers = serialNumbarArr.Split(",");
                if (serialNumbers.Length == 0)
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "序列号信息为空");
                    return Json(JsonConvert.SerializeObject(result));
                }

                //判断输入的序列号是否重复
                if (XY.Collection.ArrayIsDuplication(serialNumbers))
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"输入的序列号有重复请确认！");
                    return Json(JsonConvert.SerializeObject(result));
                }

                conn = dapper.GetConn();
                //打开连接
                dapper.OpenConn(conn);
                //开启事务
                tran = dapper.BeginTransaction(conn);
                int count = 0;

                //判断这个订单物料序列号是否已经有收料
                var serialNumberList = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberList(purchaseID, itemID);
                foreach (var item in serialNumberList)
                {
                    var stockReceiptItemQrInfo = Business.StockReceipt.StockReceipt.Instance.StockReceiptItemQrInfo(item.serial_number);
                    if (stockReceiptItemQrInfo != null)
                    {
                        result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"存在已经收料的序列号，不能进行修改！");
                        return Json(JsonConvert.SerializeObject(result));
                    }
                }

                //存在先删除再添加
                if (serialNumbers.Length>0)
                {
                    if (serialNumberList.Count()>0)
                    {
                        var snID = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberDelete(purchaseID, itemID, conn);
                        if (snID <= 0)
                        {
                            result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"序列号绑定修改失败！");
                            return Json(JsonConvert.SerializeObject(result));
                        }
                    }

                    for (int i = 0; i < serialNumbers.Length; i++)
                    {
                        if (serialNumbers[i].TrimNull() == "")
                            continue;

                        //判断序列号是否绑定
                        var lst = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberList(serialNumbers[i].TrimNull(),conn);
                        if (lst.Count() > 0)
                        {
                            result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"序列号:{serialNumbers[i].TrimNull()}已经绑定，请勿重复绑定");
                            return Json(JsonConvert.SerializeObject(result));
                        }

                        purchase_item_serial_number pisn = new purchase_item_serial_number();
                        pisn.item_id = itemID;
                        pisn.purchase_id = purchaseID;
                        pisn.serial_number = serialNumbers[i].TrimNull();
                        pisn.creator = this.PassportInfo.SupplierID;

                        int t = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberAdd(pisn, conn);
                        if (t <= 0)
                        {
                            result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "序列号绑定失败");
                            return Json(JsonConvert.SerializeObject(result));
                        }
                        else
                        {
                            count++;
                        }
                    }
                }

                if (count > 0)
                {
                    result = Model.Tools.BuildResult(1, Model.ResultCode.Success, "绑定序列号成功");
                    return Json(JsonConvert.SerializeObject(result));
                }
                else
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "绑定序列号失败");
                    return Json(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception ex)
            {
                result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "绑定序列号失败");
                return Json(JsonConvert.SerializeObject(result));
            }
            finally
            {
                if (tran != null && conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        if (result.Code == Model.ResultCode.Success)
                            //提交事务
                            dapper.Commit(tran, conn);
                        else
                            //回滚事务
                            dapper.Rollback(tran, conn);
                    }
                    //释放连接
                    if (conn != null)
                        dapper.DisposeConn(conn);
                }
            }
        }

        /// <summary>
        /// 序列号外包装箱号打印
        /// </summary>
        /// <param name="purchaseID"></param>
        /// <param name="itemID"></param>
        /// <param name="serialNumbarInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Order_SerialNumberPrint(int purchaseID, int itemID, string serialNumbarInfo)
        {
            Model.Result<int> result = new Model.Result<int>();
            if (serialNumbarInfo==null)
            {
                result = Model.Tools.BuildResult(0, Model.ResultCode.ParametersIncorrect, "序列号信息不能为空");
                return Json(JsonConvert.SerializeObject(result));
            }
            List<purchase_item_serial_number_Model> pisnmList = new List<purchase_item_serial_number_Model>();
            pisnmList = JsonConvert.DeserializeObject<List<purchase_item_serial_number_Model>>(serialNumbarInfo);

            DapperHelper dapper = DapperHelper.CreateDapperHelper(XY.Entity.Configs.XyBoss);
            IDbConnection conn = null;
            IDbTransaction tran = null;
            int count = 0;
            try
            {
                conn = dapper.GetConn();
                //打开连接
                dapper.OpenConn(conn);
                //开启事务
                tran = dapper.BeginTransaction(conn);

                //获取物料信息
                var itemInfo=Business.Item.Instance.ItemInfo(itemID);
                if (itemInfo==null)
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.ParametersIncorrect, "未获取到物料信息");
                    return Json(JsonConvert.SerializeObject(result));
                }

                var purchaseInfo= Business.Purchase.Instance.PurchaseInfo(purchaseID);
                if (purchaseInfo==null)
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.ParametersIncorrect, "未获取到采购订单信息");
                    return Json(JsonConvert.SerializeObject(result));
                }
                if (pisnmList.Count<=0)
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.ParametersIncorrect, "未获取到序列号信息");
                    return Json(JsonConvert.SerializeObject(result));
                }

                //序列号已收料入库，不允许重新打印

                //生成箱号
                var packageNo = Business.package.Instance.packageNo(purchaseInfo.purchase_order);
                if (string.IsNullOrEmpty(packageNo))
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"生产包装箱单号信息失败");
                    return Json(JsonConvert.SerializeObject(result));
                }

                package packageModel = new package();
                packageModel.package_no = packageNo;
                packageModel.creator = this.PassportInfo.SupplierID;
                int packageID = Business.package.Instance.PackageInfoInsert(packageModel, conn);
                if (packageID <= 0)
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"添加外包装箱号信息失败");
                    return Json(JsonConvert.SerializeObject(result));
                }
                //添加包装箱号关联信息
                package_relation packageRelation = new package_relation()
                {
                    package_id = packageID,
                    relation_order_id = purchaseID,
                    relation_order = purchaseInfo.purchase_order,
                    relation_order_type = XY.Enums.OrderType.PO
                };

                int packageRelaionID=Business.package.Instance.PackageRelationInfoInsert(packageRelation,conn);
                if (packageRelaionID <= 0)
                {
                    result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"添加外包装箱关联信息失败");
                    return Json(JsonConvert.SerializeObject(result));
                }

                //判断是否已经绑定箱号，如果已经绑定，则解绑，重新生成箱号（修改绑定的箱号）
                var snList = Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberList(purchaseID, itemID);
                foreach (var snInfo in pisnmList)
                {
                    purchase_item_serial_number pisn = new purchase_item_serial_number();
                    pisn.id = snInfo.id;
                    pisn.item_id = itemID;
                    pisn.purchase_id = purchaseID;
                    pisn.serial_number = snInfo.serial_number;
                    pisn.package_id = packageID;
                    pisn.bind_time = DateTime.Now;

                    //修改绑定的箱号
                    int pisnID=Business.PurchaseItemSerialNumber.Instance.PurchaseItemSerialNumberUpdate(pisn,conn);
                    if (pisnID<=0)
                    {
                        result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"序列号绑定外包装箱号失败");
                        return Json(JsonConvert.SerializeObject(result));
                    }
                    count++;
                }
                //生成的二维码PDF文件URL路径集合
                var urlList = new List<string>();

                if (count>0)
                {
                    //打印绑定的外包装
                    #region 打印参数
                    var urlPath = $"{XY.Boss.Configs.BossSiteURL}/temp";
                    //生成文件保存目录
                    var tempSaveDir = Path.Combine(WebHostEnvironment.WebRootPath, "temp");
                    decimal dpi = XY.Boss.Configs.PrintDPI;
                    decimal baseWidth = XY.Boss.Configs.PrintWidth;
                    decimal baseHeight = XY.Boss.Configs.PrintHeight;
                    int height = System.Convert.ToInt32(baseHeight * dpi / 25.4m);
                    int width = System.Convert.ToInt32(baseWidth * dpi / 25.4m);
                    int xOffset = System.Convert.ToInt32(0 * dpi / 25.4m);
                    //int yOffset = System.Convert.ToInt32(0 * dpi / 25.4m);

                    //二维码左边距
                    var qrcodeLeft = 20;
                    //二维码上边距
                    var qrcodeTop = 20;
                    //二维码大小
                    var qrcodeWidth = height - qrcodeTop * 2;

                    var fontEmSize = qrcodeWidth / 8;
                    //文本左边距
                    var textLeft = 0;
                    //文本上边距
                    var textTop = qrcodeTop + fontEmSize / 3;
                    //文本行间距
                    float textSpace = (float)fontEmSize * 2;

                    if (qrcodeWidth > width / 3)
                    {
                        qrcodeWidth = width / 3;
                        fontEmSize = qrcodeWidth / 8;
                        //文本左边距
                        textLeft = 0;
                        //文本上边距
                        textTop = qrcodeTop + fontEmSize / 3;
                        //文本行间距
                        textSpace = (float)(fontEmSize * 2.0);
                    }
                    else
                    {
                        fontEmSize = qrcodeWidth / 6;
                        //文本左边距
                        textLeft = 0;
                        //文本上边距
                        textTop = qrcodeTop + fontEmSize / 3;
                        //文本行间距
                        textSpace = (float)(fontEmSize * 1.3);
                    }
                    //fontEmSize = 30;
                    //单行文本宽度限制：图像宽度-二维码左右边距-最右边距-二维码宽度-文本左边距-最大超长20-初始位置
                    float textLimitLength = width - qrcodeLeft * 4 - qrcodeWidth - textLeft - 20 - xOffset * 2;
                    //整张图片大小
                    var imageSize = new System.Drawing.Size(width, height);
                    var fontFamily = new System.Drawing.FontFamily("黑体");
                    //二维码大小
                    var qrcodeSize = new System.Drawing.Size(qrcodeWidth, qrcodeWidth);

                    #endregion

                    //获取生产批号
                    string batchNumber = Business.Purchase.Instance.BatchNumberOrder();
                    if (string.IsNullOrEmpty(batchNumber))
                    {
                        result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"生成生产批号信息失败");
                        return Json(JsonConvert.SerializeObject(result));
                    }

                    //获取二维码内容
                    string qrCodeInfo = Business.package.Instance.packageNoAddWithBox(Enums.BarcodeType.OneDimensionalCode,packageID, pisnmList.Count, itemID,
                        PassportInfo?.SupplierID ?? 0, conn,"采购订单序列号外包装箱号打印",batchNumber, purchaseID);
                    if (string.IsNullOrEmpty(qrCodeInfo))
                    {
                        result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, @$"生成外包装箱号条码失败");
                        return Json(JsonConvert.SerializeObject(result));
                    }

                    var list = new List<string>();
                    list.Add("外包装");
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, itemInfo.code ?? "", "", fontFamily, fontEmSize, imageSize));
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, packageNo, "", fontFamily, fontEmSize, imageSize));
                    list.AddRange(QRCoderHelper.SplitStringByLabel(textLimitLength, 2, batchNumber ?? "", "", fontFamily, fontEmSize, imageSize));
                    list.Add(QRCoderHelper.SplitStringByLabel(textLimitLength, 1, pisnmList.Count + "/PCS", "", fontFamily, fontEmSize, imageSize)[0]);
                    var image = QRCoderHelper.CreateItemLabel(imageSize, qrCodeInfo, qrcodeSize, qrcodeLeft, qrcodeTop, fontFamily, fontEmSize, list, textLeft, textTop, textSpace);
                    var fileName = $"{Guid.NewGuid()}.png";
                    image.Save(Path.Combine(tempSaveDir, fileName));
                    urlList.Add($"{urlPath}/{fileName}");
                }

                if (urlList.Count > 0)
                {
                    string strjson = JsonConvert.SerializeObject(urlList);
                    result = Model.Tools.BuildResult(1, Model.ResultCode.Success, strjson);
                }
                else
                {
                    result = Model.Tools.BuildResult(1, Model.ResultCode.Failure, @$"打印外包装条码失败");
                }
                return Json(JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                result = Model.Tools.BuildResult(0, Model.ResultCode.Failure, "打印外包装箱号失败");
                return Json(JsonConvert.SerializeObject(result));
            }
            finally
            {
                if (tran != null && conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        if (result.Code == Model.ResultCode.Success)
                            //提交事务
                            dapper.Commit(tran, conn);
                        else
                            //回滚事务
                            dapper.Rollback(tran, conn);
                    }
                    //释放连接
                    if (conn != null)
                        dapper.DisposeConn(conn);
                }
            }
        }

        #endregion
    }
}
