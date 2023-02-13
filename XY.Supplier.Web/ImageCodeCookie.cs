using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace XY.Supplier.Web
{
    /// <summary>
    /// 图片验证码Cookie
    /// </summary>
    public class ImageCodeCookie : XY.Web.Cookie
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="domain">站点域名</param>
        /// <param name="dataEncryptKey">加密密钥</param>
        /// <param name="dataEncryptIV">加密向量</param>
        /// <param name="signKey">签名密钥</param>
        public ImageCodeCookie(HttpContext httpContext, string domain, string dataEncryptKey, string dataEncryptIV, string signKey) : base(httpContext)
        {
            this.Domain = domain;
            this.DataEncryptKey = dataEncryptKey;
            this.DataEncryptIV = dataEncryptIV;
            this.SignKey = signKey;
        }

        public override string Name { get { return "IC"; } }
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
        /// 设置图片验证码
        /// </summary>
        /// <param name="imageCode">图片验证码</param>
        /// <returns></returns>
        public bool SetCookie(string imageCode)
        {
            imageCode = imageCode.Trim();
            if (imageCode.Length > 0)
            {
                this.Data = new Dictionary<string, string>();
                this.Data.Add("IMAGECODE", imageCode);

                this.Expires = DateTime.Now.AddMinutes(10);

                //设置
                base.Set();

                //返回成功
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取图片验证码
        /// </summary>
        /// <returns></returns>
        public string GetCookie()
        {
            string imageCode = "";

            Dictionary<string, string> d = base.Get();
            if (d != null && d.Count > 0 && d.ContainsKey("IMAGECODE"))
            {
                imageCode = d["IMAGECODE"];
            }

            return imageCode;
        }

        /// <summary>
        /// 清空图片验证码
        /// </summary>
        public void ClearCookie()
        {
            base.Clear();
        }

    }
}