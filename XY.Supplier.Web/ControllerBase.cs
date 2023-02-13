using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;

namespace XY.Supplier.Web
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class ControllerBase : XY.Web.Controller
    {
        public ControllerBase(IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment) { }

        /// <summary>
        /// 授权管理
        /// </summary>
        protected PassportManager PassportManager;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            PassportManager = new PassportManager(
                context.HttpContext,
                Configs.SiteDomain,
                Configs.EncryptKey,
                Configs.EncryptIV,
                Configs.SignKey
            );
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //
        }

    }
}