using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace XY.Supplier.Web
{
    /// <summary>
    /// 登录凭证管理
    /// </summary>
    public class PassportManager
    {
        /// <summary>
        /// Cookie管理对象
        /// </summary>
        private PassportCookie cookie = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="domain">站点域名</param>
        /// <param name="dataEncryptKey">加密密钥</param>
        /// <param name="dataEncryptIV">加密向量</param>
        /// <param name="signKey">签名密钥</param>
        public PassportManager(HttpContext httpContext, string domain, string dataEncryptKey, string dataEncryptIV, string signKey)
        {
            cookie = new PassportCookie(httpContext, domain, dataEncryptKey, dataEncryptIV, signKey);
        }

        /// <summary>
        /// 判断登录凭证是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsPassport()
        {
            if (this.GetPassport() != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 设置登录凭证
        /// </summary>
        /// <param name="passportInfo">凭证对象</param>
        /// <param name="isRemember">凭证是否保持</param>
        /// <returns></returns>
        public bool SetPassport(PassportInfo passportInfo, bool isRemember = true)
        {
            //memberInfo验证
            if (passportInfo != null &&
                passportInfo.SupplierID > 0 &&
                passportInfo.Name.Length > 0)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("SUPPLIERID", passportInfo.SupplierID.ToString());
                data.Add("NAME", passportInfo.Name);

                //如用户记住密码，凭证保持10天
                DateTime? expires = null;
                if (isRemember)
                    expires = DateTime.Now.AddDays(1);

                //设置
                cookie.SetCookie(data, expires);

                //返回成功
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取登录凭证
        /// </summary>
        /// <returns></returns>
        public PassportInfo GetPassport()
        {
            PassportInfo passportInfo = null;

            Dictionary<string, string> data = cookie.GetCookie();
            if (data != null && data.Count > 0 &&
                data.ContainsKey("SUPPLIERID") && data.ContainsKey("NAME"))
            {
                passportInfo = new PassportInfo();
                passportInfo.SupplierID = int.Parse(data["SUPPLIERID"]);
                passportInfo.Name = data["NAME"];
            }

            return passportInfo;
        }

        /// <summary>
        /// 清空登录凭证
        /// </summary>
        public void ClearPassport()
        {
            cookie.ClearCookie();
        }

    }
}