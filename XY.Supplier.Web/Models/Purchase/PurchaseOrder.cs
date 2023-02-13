using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XY.Entity.xy_boss;
using XY.Pager;

namespace XY.Supplier.Web.Models.Purchase
{
    public class PurchaseOrder_Model
    {
        /// <summary>
        /// 采购订单状态列表
        /// </summary>
        public IEnumerable<SelectListItem> OrderStatusList { get; set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// 是否延期
        /// </summary>
        public int IsDelayStatus { get; set; }
        /// <summary>
        /// 采购订单搜索关键字
        /// </summary>
        public string OrderKey { get; set; }
        /// <summary>
        /// 采购订单数据
        /// </summary>
        public IPager<DataRow> OrderList { get; set; }
        /// <summary>
		/// 开始时间
		/// </summary>
		public string Begin { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string End { get; set; }
    }

    public class PurchaseOrder_View_Model
    {
        /// <summary>
        /// 返回URL
        /// </summary>
        public string Backurl { get; set; }

        /// <summary>
        /// 采购订单物料列表
        /// </summary>
        public DataTable OrderItemList { get; set; }

        /// <summary>
        /// 采购订单其他费用列表
        /// </summary>
        public IEnumerable<purchase_fee> OrderFeeList { get; set; }

        /// <summary>
        /// 采购订单ID
        /// </summary>
        public int PurchaseID { get; set; }

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string PurchaseOrder { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 付款条件
        /// </summary>
        public int PayPlanID { get; set; }

        /// <summary>
        /// 负责人名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 订单创建人名称
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 交期类型
        /// </summary>
        public Enums.boss.delivery_type DeliveryType { get; set; }

        /// <summary>
        /// 计划交货时间
        /// </summary>
        public DateTime? DeliveryTime { get; set; }

        /// <summary>
        /// 单价是否含税
        /// </summary>
        public bool IsTax { get; set; }

        /// <summary>
        /// 总价(物料+费用)
        /// </summary>
        public decimal PriceTotal { get; set; }

        /// <summary>
        /// 特价(0表示未申请)
        /// </summary>
        public decimal PriceSpecial { get; set; }

        /// <summary>
        /// 审核流程ID
        /// </summary>
        public int WfID { get; set; }

        /// <summary>
        /// 审核流程名称
        /// </summary>
        public string WfName { get; set; }

        /// <summary>
        /// 收货地区
        /// </summary>
        public string ShipZoneCode { get; set; }

        /// <summary>
        /// 收货详细地址
        /// </summary>
        public string ShipAddr { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ShipName { get; set; }

        /// <summary>
        /// 收货人联系电话
        /// </summary>
        public string ShipPhone { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Enums.boss.purchase_status Status { get; set; }
    }

    public class PurchaseOrder_Shot_Model
    {

        public IPager<DataRow> OrderList { get; set; }

        public IPager<DataRow> DeliveryList { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// 是否延期
        /// </summary>
        public int IsDelayStatus { get; set; }
        /// <summary>
        /// 采购订单号关键字
        /// </summary>
        public string PurchaseOrderKey { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string Begin { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string End { get; set; }

        public string BeginDay { get; set; }

        public string EndDay { get; set; }

        public int PageSize { get; set; }
    }

    public class PurchaseOrder_Print_Detail
    {
        /// <summary>
        /// 返回URL
        /// </summary>
        public string Backurl { get; set; }
        /// <summary>
        /// 采购订单ID
        /// </summary>
        public int PurchaseID { get; set; }
        /// <summary>
        /// 物料信息列表
        /// </summary>
        public DataTable OrderItemList { get; set; }
    }

    public class PurchaseOrder_Item_QRCode_Para_Model
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public int ItemID { get; set; }
        /// <summary>
        /// 标签内数量
        /// </summary>
        public int LabelQty { get; set; }
        /// <summary>
        /// 份数
        /// </summary>
        public int Copies { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string Dept { get; set; }
        /// <summary>
        /// 物料打印规则 见枚举值：XY.Enums.boss.item_rule
        /// </summary>
        public int Rule { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        ///// <summary>
        ///// 供应商SN码
        ///// </summary>
        //public string SupplierSN { get; set; }
        
    }

    public class PurchaseOrder_QRCode_Model
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string Order { get; set; }
        /// <summary>
        /// 物料信息
        /// </summary>
        public IEnumerable<PurchaseOrder_QRCode_Item_Model> ItemList { get; set; }
    }

    public class PurchaseOrder_QRCode_Item_Model
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 物料标签数量
        /// </summary>
        public string Qty { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 二维码内容
        /// </summary>
        public IEnumerable<string> QRCode { get; set; }
        /// <summary>
        /// 物料打印规则 见枚举值：XY.Enums.boss.item_rule
        /// </summary>
        public int Rule { get; set; }
        /// <summary>
        /// 外包装二维码内容 
        /// </summary>
        public string BoxQRCode { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string Dept { get; set; }
        /// <summary>
        /// 外包装总数 如12/PCS
        /// </summary>
        public string Total { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }

    public class PurchaseOrder_RelationOrderInfo_List
    {
        public List<PurchaseOrder_RelationOrderInfo_Model> data { get; set; }
    }
    public class PurchaseOrder_RelationOrderInfo_Model
    {
        /// <summary>
        /// 采购订单ID
        /// </summary>
        public int PurchaseID { get; set; }

        /// <summary>
        /// 生产订单ID
        /// </summary>
        public int ProduceID { get; set; }

        public string ProduceOrder { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料规则
        /// </summary>
        public string ItemRule { get; set; }

        /// <summary>
        /// 标签内数量
        /// </summary>
        public int ItemQty { get; set; }

        /// <summary>
        /// 打印数量
        /// </summary>
        public int Copies { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remarks { get; set; }
    }

    public class PurchaseOrder_QRCode_Print_Model
    {
        /// <summary>
        /// 采购单ID
        /// </summary>
        public int purchaseID { get; set; }
        /// <summary>
        /// 物料ID
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// 标签纸宽
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// 标签纸高
        /// </summary>
        public int height { get; set; }

        /// <summary>
        /// 是否打印外包装
        /// </summary>
        public bool isPrintOutPackage { get; set; }

        /// <summary>
        /// 打印明细参数
        /// </summary>
        public List<Item_Print_Detail_Model> ItemPrintDetail { get; set; }
    }

    public class Item_Print_Detail_Model
    {
        /// <summary>
        /// 标签内数量
        /// </summary>
        public int LabelQty { get; set; }

        /// <summary>
        /// 份数
        /// </summary>
        public int Copies { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
}
