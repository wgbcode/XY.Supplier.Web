using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace XY.Supplier.Web
{
    /// <summary>
    /// 登录凭证Cookie
    /// </summary>
    public class PassportCookie : XY.Web.Cookie
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="domain">站点域名</param>
        /// <param name="dataEncryptKey">加密密钥</param>
        /// <param name="dataEncryptIV">加密向量</param>
        /// <param name="signKey">签名密钥</param>
        public PassportCookie(HttpContext httpContext, string domain, string dataEncryptKey, string dataEncryptIV, string signKey) : base(httpContext)
        {
            this.Domain = domain;
            this.DataEncryptKey = dataEncryptKey;
            this.DataEncryptIV = dataEncryptIV;
            this.SignKey = signKey;
        }

        public override string Name { get { return "SUPPLIER"; } }
        public override DateTime? Expires { get; set; }
        public override string Domain { get; set; }
        public override string Path { get { return "/"; } }
        public override bool HttpOnly { get { return false; } }
        public override bool Secure { get { return false; } }
        public override string Version { get { return "1"; } }
        public override Dictionary<string, string> Data { get; set; }
        public override bool DataEncrypt { get { return true; } }
        public override string DataEncryptKey { get; set; }
        public override string DataEncryptIV { get; set; }
        public override bool Sign { get { return true; } }
        public override string SignKey { get; set; }

        /// <summary>
        /// 设置登录凭证
        /// </summary>
        /// <param name="data"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public void SetCookie(Dictionary<string, string> data, DateTime? expires)
        {
            this.Data = data;
            this.Expires = expires;
            base.Set();
        }

        /// <summary>
        /// 获取登录凭证
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetCookie()
        {
            return base.Get();
        }

        /// <summary>
        /// 清空登录凭证
        /// </summary>
        public void ClearCookie()
        {
            base.Clear();
        }

    }
}