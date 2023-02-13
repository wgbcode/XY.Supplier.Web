using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XY.Enums.boss;

namespace XY.Supplier.Web.Models.Transport
{
    public class TransportDoc
    {
    }

	public class TransportDoc_View_Model
	{
		/// <summary>
		/// 返回URL
		/// </summary>
		public string Backurl { get; set; }

		/// <summary>
		/// 运费单ID
		/// </summary>
		public int TransportDocID { get; set; }

		/// <summary>
		/// 运费单
		/// </summary>
		public string TransportDocOrder { get; set; }

		/// <summary>
		/// 状态
		/// </summary>
        public string Status { get; set; }

		/// <summary>
		/// 供应商/客户名称
		/// </summary>
        public string CardNname { get; set; }

		/// <summary>
		/// 供应商业务代码
		/// </summary>
        public string CardCode { get; set; }

		/// <summary>
		/// 交货日期
		/// </summary>
        public DateTime DocDueDate { get; set; }

		/// <summary>
		/// 过账日期
		/// </summary>
        public DateTime DocDate { get; set; }

		/// <summary>
		/// 联系人
		/// </summary>
        public string CntctCode { get; set; }

		/// <summary>
		/// 采购员
		/// </summary>
        public string SlpName { get; set; }

		/// <summary>
		/// 收款方地址
		/// </summary>
        public string Address { get; set; }

		/// <summary>
		/// 收货地址
		/// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comments { get; set; }

		/// <summary>
		/// 运费单包含物料
		/// </summary>
		public DataTable TransportDocItemList { get; set; }

		/// <summary>
		/// 支付信息
		/// </summary>
		public DataTable TransportPayInfo { get; set; }

		/// <summary>
		/// 付款条件
		/// </summary>
        public string PymntGroup { get; set; }

		/// <summary>
		/// 付款方式
		/// </summary>
		public string PaymentMethod { get; set; }

		/// <summary>
		/// 预付款天数
		/// </summary>
		public string PrepaDay { get; set; }

		/// <summary>
		/// 预付百分比
		/// </summary>
		public string PrepaPro { get; set; }

		/// <summary>
		/// 发货前付
		/// </summary>
		public string PayBefShip { get; set; }

		/// <summary>
		/// 货到付百分比
		/// </summary>
		public string GoodsToPro { get; set; }

		/// <summary>
		/// 货到付款天数
		/// </summary>
		public string GoodsToDay { get; set; }

		/// <summary>
		/// 收款单位
		/// </summary>
        public string URecComp { get; set; }

		/// <summary>
		/// 收款方账号
		/// </summary>
        public string UACCT { get; set; }

		/// <summary>
		/// 开户行
		/// </summary>
        public string UBANK { get; set; }

		/// <summary>
		/// 付款摘要
		/// </summary>
        public string UFKZY { get; set; }

		/// <summary>
		/// 发票类型
		/// </summary>
        public string UFPLB { get; set; }

		/// <summary>
		/// 设备编码/箱号
		/// </summary>
		public string U_CPH { get; set; }

		/// <summary>
		/// 验收期限
		/// </summary>
        public string U_YSQX { get; set; }

		/// <summary>
		/// 税率
		/// </summary>
        public string U_SL { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public string U_YF { get; set; }

		/// <summary>
		/// 快递费
		/// </summary>
		public string U_KDF { get; set; }

		/// <summary>
		/// 包装费
		/// </summary>
        public string U_BZF { get; set; }

		/// <summary>
		/// 收件公司
		/// </summary>
        public string U_OC_CN { get; set; }

		/// <summary>
		/// 收件地址
		/// </summary>
        public string U_OC_AD { get; set; }

		/// <summary>
		/// 收件人
		/// </summary>
        public string U_OC_NA { get; set; }

		/// <summary>
		/// 收件人联系电话
		/// </summary>
        public string U_OC_TE { get; set; }

		/// <summary>
		/// SID
		/// </summary>
        public string U_PRX_SID { get; set; }

		/// <summary>
		/// 领料/退料人
		/// </summary>
        public string U_PRX_TkNo { get; set; }

		/// <summary>
		/// 发件人/联系方式
		/// </summary>
        public string U_PRX_SRVR { get; set; }

		/// <summary>
		/// 交货方式
		/// </summary>
        public string U_ShipName { get; set; }

		/// <summary>
		/// 上门安装
		/// </summary>
        public string U_SMAZ { get; set; }

		/// <summary>
		/// 保险费
		/// </summary>
		public string U_YCF { get; set; }

		/// <summary>
		/// 提车费
		/// </summary>
		public string U_S_YWF { get; set; }

		/// <summary>
		/// 叉车费
		/// </summary>
		public string U_CCF { get; set; }

		/// <summary>
		/// 提货费
		/// </summary>
		public string U_THF { get; set; }
		/// <summary>
		/// 物流订单信息
		/// </summary>
		public TransportDoc TransportDocInfo { get; set; }

		/// <summary>
		/// 物流订单状态
		/// </summary>
		public Enums.boss.transport_status TransportStatus { get; set; }
		/// <summary>
		/// 总金额
		/// </summary>
		public decimal Price { get; set; }
		/// <summary>
		/// 运费
		/// </summary>
		public decimal FreightFee { get; set; }
		/// <summary>
		/// 送货费
		/// </summary>
		public decimal DeliveryFee { get; set; }
		/// <summary>
		/// 包装费
		/// </summary>
		public decimal PackagingFee { get; set; }
		/// <summary>
		/// 保险费
		/// </summary>
		public decimal Insurance { get; set; }
		/// <summary>
		/// 提货费
		/// </summary>
		public decimal PickupFee { get; set; }
		/// <summary>
		/// 叉车费
		/// </summary>
		public decimal ForkliftFee { get; set; }
		/// <summary>
		/// 装箱单号
		/// </summary>
		public string PackingNumber { get; set; }
		/// <summary>
		/// 车牌号
		/// </summary>
		public string LicensePlateNumber { get; set; }
		/// <summary>
		/// 托运数量(箱数)  
		/// </summary>
		public int ConsignmentQuantity { get; set; }
		/// <summary>
		/// 托运物品
		/// </summary>
		public string ConsignmentDescription { get; set; }

		/// <summary>
		/// 原因分析
		/// </summary>
		public string AnalysisReason { get; set; }
		/// <summary>
		/// 物料来源
		/// </summary>
		public string MaterialSource { get; set; }

		/// <summary>
		/// 起运地
		/// </summary>
		public string PlaceShipment { get; set; }
		/// <summary>
		/// 发货主体(寄件公司)
		/// </summary>
		public string OidcCode { get; set; }

		/// <summary>
		/// 到达地
		/// </summary>
		public string Destination { get; set; }

		/// <summary>
		/// 国税编号
		/// </summary>
        public string LicTradNum { get; set; }

		public List<TransportDocDetail> transportDocDetailList { get; set; }

		/// <summary>
		/// 合计
		/// </summary>
        public string DocTotal { get; set; }

		/// <summary>
		/// 收货人
		/// </summary>
		public string Consignee { get; set; }
		/// <summary>
		/// 收货公司
		/// </summary>
		public string ConsigneeCompany { get; set; }
		/// <summary>
		/// 收货人地址
		/// </summary>
		public string ConsigneeAddress { get; set; }
		/// <summary>
		/// 收货人电话
		/// </summary>
		public string ConsigneeTel { get; set; }

		/// <summary>
		/// 发货人
		/// </summary>
		public string Consignor { get; set; }

		/// <summary>
		/// 发货公司
		/// </summary>
		public string ConsignorCompany { get; set; }

		/// <summary>
		/// 发货地址
		/// </summary>
		public string ConsignorAddress { get; set; }
		/// <summary>
		/// 发货人电话
		/// </summary>
		public string ConsignorTel { get; set; }

		/// <summary>
		/// 快递单号
		/// </summary>
        public string CourierNumber { get; set; }

		/// <summary>
		/// 托运日期
		/// </summary>
		public string DateOfConsignment { get; set; }

		/// <summary>
		/// 运费单创建时间
		/// </summary>
		public DateTime CreateDate { get; set; }

		/// <summary>
		/// 金额（万）
		/// </summary>
		public string DocTotal_w { get; set; }

		/// <summary>
		/// 金额（仟）
		/// </summary>
		public string DocTotal_q { get; set; }

		/// <summary>
		/// 金额（佰）
		/// </summary>
		public string DocTotal_b { get; set; }

		/// <summary>
		/// 金额（拾）
		/// </summary>
		public string DocTotal_s { get; set; }

		/// <summary>
		/// 金额（元）
		/// </summary>
		public string DocTotal_y { get; set; }

		/// <summary>
		/// 金额（角）
		/// </summary>
		public string DocTotal_j { get; set; }

		/// <summary>
		/// 金额（分）
		/// </summary>
		public string DocTotal_f { get; set; }

		/// <summary>
		/// 卸货费
		/// </summary>
		public decimal DischarGecargoFee { get; set; }

		/// <summary>
		/// 等候费
		/// </summary>
        public decimal WaitFee { get; set; }

		/// <summary>
		/// 入仓费
		/// </summary>
        public decimal WarehousingFee { get; set; }

		/// <summary>
		/// 装柜费
		/// </summary>
		public decimal CabinetInstallationFee { get; set; }

		/// <summary>
		/// 返程带货费
		/// </summary>
		public decimal ReturnWithGoodsFee { get; set; }

		/// <summary>
		/// 上楼费
		/// </summary>
		public decimal UpstairsFee { get; set; }

		/// <summary>
		/// 压夜费
		/// </summary>
		public decimal OvernightChargeFee { get; set; }

		/// <summary>
		/// 回写ERP3.0状态(0:未回写，1:回写成功，2:回写失败)
		/// </summary>
		public write_back_status write_back_status { get; set; }

		/// <summary>
		/// 运输方式
		/// </summary>
        public string ModeOfTransport { get; set; }
    }

	public class TransportDocDetail
	{
		/// <summary>
		/// 货物名称
		/// </summary>
		public string goods { get; set; }
		/// <summary>
		/// 物料编号
		/// </summary>
		public string item_number { get; set; }
		/// <summary>
		/// 包装
		/// </summary>
		public string packingType { get; set; }
		/// <summary>
		/// 件数
		/// </summary>
		public string pieces { get; set; }
		/// <summary>
		///  体积（m³）
		/// </summary>
		public string volume { get; set; }
		/// <summary>
		///   重量（公斤）
		/// </summary>
		public string weight { get; set; }
		/// <summary>
		///  装箱单号
		/// </summary>
		public string packingNumber { get; set; }

		public string num { get; set; }

		/// <summary>
		/// 物料ID
		/// </summary>
        public int itemID { get; set; }
    }
}
