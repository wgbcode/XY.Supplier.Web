using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XY.Pager;

namespace XY.Supplier.Web.Models.Purchase
{

	public class Quote_Model
	{
		/// <summary>
		/// 询价单状态列表
		/// </summary>
		public IEnumerable<SelectListItem> OrderStatusList { get; set; }

		/// <summary>
		/// 员工列表
		/// </summary>
		public IEnumerable<SelectListItem> UserList { get; set; }

		/// <summary>
		/// 询价单列表
		/// </summary>
		public IPager<DataRow> OrderList { get; set; }

		/// <summary>
		/// 当前状态
		/// </summary>
		public int OrderStatus { get; set; }

		/// <summary>
		/// 询价单号关键字
		/// </summary>
		public string OrderKey { get; set; }

		/// <summary>
		/// 发起人Id
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// 开始时间
		/// </summary>
		public string Begin { get; set; }

		/// <summary>
		/// 结束时间
		/// </summary>
		public string End { get; set; }
	}

	public class Quote_Detail_Model
	{
		/// <summary>
		/// 是否报过价
		/// </summary>
		public bool IsQuote { get; set; }
		public string BackUrl { get; set; }
		public string Order_No { get; set; }
		public int Inquiry_Id { get; set; }
		public int Supplier_Id { get; set; }
		public int Creator { get; set; }
		public string Creator_Name { get; set; }

		public DateTime Create_time { get; set; }
		public DateTime Deliver_time { get; set; }
		public DateTime End_time { get; set; }
		public DateTime Public_time { get; set; }
		public string Status { get; set; }
		public string Remark { get; set; }
		public DateTime? Promise_Deliver_time { get; set; }

		public IPager<DataRow> Item_List { get; set; }
	}
}
