using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using XY.Supplier.Web.Models.My;

namespace XY.Supplier.Web.Controllers
{
    public class MyController : ControllerAuthorized
    {
        public MyController(IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment) { }

        #region 主页

        [HttpGet]
        public IActionResult Default()
        {
            Default_Model model = new Default_Model();
            model.SupplierInfo = Business.Boss.Instance.SupplierInfo(base.PassportInfo.SupplierID);
            return View(model);
        }

        #endregion

        #region 个人资料

        //[HttpGet]
        //public IActionResult Info()
        //{
        // Info_Model model = new Info_Model();
        //    model.UserInfo = Business.Boss.Instance.UserInfo(base.PassportInfo.UserID);
        //    return View(model);
        //}

        //[HttpPost]
        //public IActionResult Info_Update(Info_Update_Model model)
        //{
        //    if (Business.Boss.Instance.UserEdit(base.PassportInfo.UserID, string.Empty, string.Empty, model.Name, model.Gender, model.Mobile, model.Email, model.IM))
        //    {
        //        return RedirectPage("个人资料修改成功！", "/my/info");
        //    }
        //    else
        //    {
        //        return RedirectPage("个人资料修改失败！", "/my/info");
        //    }
        //}

        #endregion

        #region 账号中心

        #region 账号资料

        /// <summary>
        /// 展示供应商信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Account()
        {
            Account_Model model = new Account_Model();
            model.SupplierInfo = Business.Boss.Instance.SupplierInfo(this.PassportInfo.SupplierID);
            return View(model);
        }

        /// <summary>
        /// 编辑供应商信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Account_Update(Account_Update_Model model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                ViewBag.Javascript = XY.JS.AlertFocus("请输入公司名称！", "Name");
                return View("Account");
            }
            var info = Business.Boss.Instance.SupplierInfo(this.PassportInfo.SupplierID);
            bool success = Business.Boss.Instance.SupplierEdit(this.PassportInfo.SupplierID, model.Name, "", "", info.purchase_tax_rate,
                info.invoice_title, info.tax_code, info.tag_industry, info.cate_id);
            if (success)
            {
                return RedirectPage("名称修改成功！", "/my/account");
            }
            else
            {
                ViewBag.Javascript = XY.JS.AlertFocus("编辑失败，请确认名称输入是否合法！", "Code");
                return View("Account");
            }
        }

        #endregion

        #region 修改密码

        [HttpGet]
        public IActionResult Password()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Password_Update(Password_Update_Model model)
        {
            if (Business.Boss.Instance.SupplierChangePassword(base.PassportInfo.SupplierID, model.PasswordOld, model.PasswordNew))
                return RedirectPage("密码修改成功！", "/my/password");
            else
                return RedirectPage("原始密码有误！", "/my/password");
        }

        #endregion

        #endregion

    }
}
