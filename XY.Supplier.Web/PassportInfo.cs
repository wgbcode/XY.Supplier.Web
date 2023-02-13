using System;

namespace XY.Supplier.Web
{
    /// <summary>
    /// 登录凭证
    /// </summary>
    public class PassportInfo
    {
        /// <summary>
        /// 供应商ID
        /// </summary>
        public int SupplierID { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }
    }
}
