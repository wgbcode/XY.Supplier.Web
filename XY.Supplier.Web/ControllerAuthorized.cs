using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XY.Entity.xy_boss;

namespace XY.Supplier.Web
{
    /// <summary>
    /// 授权控制器基类
    /// </summary>
    public class ControllerAuthorized : ControllerBase
    {
        public ControllerAuthorized(IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment) { }

        /// <summary>
        /// 登录凭证
        /// </summary>
        protected PassportInfo PassportInfo { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.Count == 0 && !context.ActionArguments.ContainsKey("pageSize"))
            {
                context.ActionArguments.Add("pageSize", 30);
            }
            base.OnActionExecuting(context);

            //获取用户凭证
            this.PassportInfo = base.PassportManager.GetPassport();
            if(this.PassportInfo == null)
                context.Result = new RedirectResult("/deny");
        }

    }
}