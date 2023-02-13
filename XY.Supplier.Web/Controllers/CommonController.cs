using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using XY.Entity.xy_boss;
using XY.Supplier.Web.Models.Common;

namespace XY.Supplier.Web.Controllers
{
    public class CommonController : ControllerBase
    {
        public CommonController(IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment) { }

        #region 主框架

        [HttpGet]
        public IActionResult Main()
        {
            //登录凭证
            var passportInfo = base.PassportManager.GetPassport();
            if (passportInfo == null)
                return new RedirectResult("/signin");

            Main_Model model = new Main_Model();
            return View(model);
        }

        #endregion

        #region 登录

        [Route("signin")]
        [HttpGet]
        public IActionResult Signin()
        {
            if (base.PassportManager.IsPassport())
                return new RedirectResult("/");

            ImageCodeCookie imageCodeCookie = new ImageCodeCookie(base.HttpContext, Configs.SiteDomain, Configs.EncryptKey, Configs.EncryptIV, Configs.SignKey);
            var model = new Signin_Model
            {
                Account = string.Empty,
                Password = string.Empty,
                ImageCode = imageCodeCookie.GetCookie(),
                IsRemember = false
            };

            var guid = Guid.NewGuid().ToString().Replace("-", "");
            var result = Tools.HttpGet<string>($"{Configs.QrLoginURL}/qrg?rd={guid}&state=srm");
            if (!string.IsNullOrWhiteSpace(result))
            {
                model.Qr = result.Replace("\"", "");
                model.Guid = guid;

                //申请redis保存
                //var resultRedis = HttpGet($"https://passport.neware.work/l?rd={guid}&state=srm", $"Bearer {resultToken.Data}");
            }

            return View(model);
        }

        [Route("signin")]
        [HttpPost]
        public IActionResult Signin(Signin_Model model)
        {
            //要求图片验证码
            ImageCodeCookie imageCodeCookie = new ImageCodeCookie(base.HttpContext, Configs.SiteDomain, Configs.EncryptKey, Configs.EncryptIV, Configs.SignKey);
            string imageCode = imageCodeCookie.GetCookie();
            if (imageCode.Length > 0 && !imageCode.EqualsIgnoreCase(model.ImageCode))
            {
                ViewBag.Javascript = XY.JS.AlertFocus("验证码输入有误或已失效！", "ImageCode");
                return View(model);
            }

            //登录验证
            var supplierInfo = Business.Boss.Instance.SupplierLogin(model.Account, model.Password, base.RequestIp());
            //成功
            if (supplierInfo != null && supplierInfo.supplier_id > 0)
            {
                if (imageCode.Length > 0)
                    imageCodeCookie.ClearCookie();

                base.PassportManager.SetPassport(
                    new PassportInfo { SupplierID = supplierInfo.supplier_id, Name = supplierInfo.name });
                return new RedirectResult("/");
            }
            //失败
            else
            {
                if (imageCode.Length == 0)
                {
                    model.ImageCode = "12345";
                    imageCodeCookie.SetCookie("12345");
                }

                ViewBag.Javascript = XY.JS.AlertFocus("帐号或密码有误！", "Password");
                return View(model);
            }
        }

        [HttpPost]
        public XWZN_Signin_Qr_Model<string> Signin_Qr(string guid)
        {
            try
            {
                //检查登录状态
                var result = Tools.HttpGet<XWZN_Signin_Qr_Model<string>>($"{Configs.QrLoginURL}/v?rm={guid}");
                if (result.ResultCode == 200)
                {
                    //请求token
                    var resultToken = Tools.HttpPost<XWZN_Signin_Qr_Model<string>>($"{Configs.QrLoginURL}/api/Account/GetAccessToken", new XWZN_Signin_Token() { ClientId = Configs.XWZNClientID, ClientSecret = Configs.XWZNSecret, Scope = Configs.XWZNScope });
                    if (result.ResultCode != 200 || string.IsNullOrWhiteSpace(resultToken.Data))
                    {
                        result.ErrorMessage = $"登录失败，请求令牌失败：{resultToken.ErrorMessage}";
                        result.Success = false;
                        result.ResultCode = 404;
                        return result;
                    }

                    //获取扫码登录用户信息
                    var resultUserInfo = Tools.HttpPost<XWZN_Signin_Qr_Model<XWZN_UserInfo>>($"{Configs.QrLoginURL}/api/Account/AccountBaseInfo", new XWZN_BaseInfo_Para() { PassportId = result.Data }, $"Bearer {resultToken.Data}");
                    if (resultUserInfo.ResultCode != 200 || resultUserInfo.Data == null)
                    {
                        result.ErrorMessage = $"登录失败，请求用户信息失败：{resultUserInfo.ErrorMessage}";
                        result.Success = false;
                        result.ResultCode = 404;
                        return result;
                    }

                    if (string.IsNullOrWhiteSpace(resultUserInfo.Data.Mobile))
                    {
                        result.ErrorMessage = $"登录失败，请求用户信息未关联手机号";
                        result.Success = false;
                        result.ResultCode = 404;
                        return result;
                    }

                    var contactList = Business.Boss.Instance.ContactList(resultUserInfo.Data.Mobile);
                    var contactInfo = contactList.Where(x => x.status == Enums.Status.Enabled).OrderByDescending(x => x.contact_id).FirstOrDefault();
                    if (contactInfo == null)
                    {
                        result.ErrorMessage = "登录失败，请确认用户手机号是否绑定至SRM系统";
                        result.Success = false;
                        result.ResultCode = 404;
                        return result;
                    }

                    var supplierList = Business.Boss.Instance.SupplierByContactID(contactInfo.contact_id);
                    var supplierInfo = supplierList.Where(x => x.status == Enums.Status.Enabled).OrderByDescending(x => x.supplier_id).FirstOrDefault();
                    if (supplierInfo == null)
                    {
                        result.ErrorMessage = "登录失败，请确认用户手机号是否绑定关联供应商";
                        result.Success = false;
                        result.ResultCode = 404;
                        return result;
                    }

                    base.PassportManager.SetPassport(
                    new PassportInfo { SupplierID = supplierInfo.supplier_id, Name = supplierInfo.name });

                    //var response2 = apiClient.GetAsync($"https://passport.neware.work/l?rd={guid}&passportid={result.Data}").Result;
                    //if (response2.IsSuccessStatusCode)
                    //{
                    //    var result2 = XY.Serializable.JsonStringToObject<Signin_Qr_Model>(response2.Content.ReadAsStringAsync().Result);
                    //}
                }

                return result;
            }
            catch (Exception ex)
            {
                Business.Boss.Instance.SysEventAdd("XY.Supplier.Print.Web.Signin_Qr()发生异常",
                    $"{guid}:{ex.Message}",
                    string.Empty,
                    XY.Enums.boss.sys_event_level.error,
                    Enums.boss.sys_event_notice.none);

                return new XWZN_Signin_Qr_Model<string>()
                {
                    ErrorMessage = "服务器发生异常",
                    ResultCode = 404,
                    Success = false,
                };
            }
        }

        #endregion

        #region 注销

        [Route("signout")]
        public IActionResult Signout()
        {
            base.PassportManager.ClearPassport();
            return new RedirectResult("/signin");
        }

        #endregion

        #region 图片验证码

        /// <summary>
        /// 随机生成验证码并生成Cookie，返回验证码字符
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string imageCodeGenerate(int length)
        {
            string imageCode = string.Empty;
            if (length > 0)
            {
                imageCode = XY.Text.Random(length, XY.Enums.RandomType.Number, false);
                if (imageCode.Length > 0)
                {
                    //生成Cookie
                    ImageCodeCookie cookie = new ImageCodeCookie(base.HttpContext, Configs.SiteDomain, Configs.EncryptKey, Configs.EncryptIV, Configs.SignKey);
                    cookie.SetCookie(imageCode);
                }
            }

            return imageCode;
        }

        /// <summary>
        /// 为图片设置干扰点
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="clarity"></param>
        /// <returns></returns>
        private Bitmap imageCodeDisturb(Bitmap bitmap, int clarity)
        {
            // 通过随机数生成
            Random random = new Random();

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    if (random.Next(100) <= clarity)
                        bitmap.SetPixel(i, j, Color.White);
                }
            }

            return bitmap;
        }

        /// <summary>
        /// 绘制验证码图片
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="validateCode"></param>
        /// <param name="fontstyle"></param>
        /// <param name="fontfamily"></param>
        /// <param name="fontsize"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Bitmap imageCodeDraw(Bitmap bitmap, string validateCode, int fontstyle, string fontfamily, int fontsize, int x, int y)
        {
            // 获取绘制器对象
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // 设置绘制字体
                Font font = new Font(fontfamily, fontsize, imageCodeFontStyle(fontstyle));

                // 绘制验证码图像
                g.DrawString(validateCode, font, Brushes.Black, x, y);
            }

            return bitmap;
        }

        /// <summary>
        /// 换算验证码字体样式
        /// </summary>
        /// <param name="fontstyle">1 粗体 2 斜体 3 粗斜体，默认为普通字体</param>
        /// <returns></returns>
        private FontStyle imageCodeFontStyle(int fontstyle)
        {
            if (fontstyle == 1)
                return FontStyle.Bold;
            else if (fontstyle == 2)
                return FontStyle.Italic;
            else if (fontstyle == 3)
                return FontStyle.Bold | FontStyle.Italic;
            else
                return FontStyle.Regular;
        }

        [HttpGet]
        public ActionResult ImageCode()
        {
            // 验证码长度
            int length = 5;
            // 图片清晰度
            int clarity = 80;
            // 图片宽度
            int width = 120;
            // 图片高度
            int height = 31;
            // 字体家族名称
            string fontfamily = "Arial";
            // 字体大小
            int fontsize = 18;
            // 字体样式
            int fontstyle = 2;
            // 绘制起始坐标 X
            int x = 20;
            // 绘制起始坐标 Y
            int y = 0;

            string validateCode = imageCodeGenerate(length);

            // 生成BITMAP图像
            Bitmap bitmap = new Bitmap(width, height);

            // 给图像设置干扰
            bitmap = imageCodeDisturb(bitmap, clarity);

            // 绘制验证码图像
            bitmap = imageCodeDraw(bitmap, validateCode, fontstyle, fontfamily, fontsize, x, y);

            // 保存验证码图像，等待输出
            //bitmap.Save(Response.OutputStream, ImageFormat.Gif);
            //return null;

            // 输出图像
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Gif);
                return File(ms.ToArray(), XY.Enums.Tools.HttpContentType(XY.Enums.HttpContentType.Gif));
            }
        }

        #endregion
    }
}