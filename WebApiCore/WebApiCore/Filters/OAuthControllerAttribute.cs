using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// 拦截器 继承 ActionFilterAttribute 或 controller 实现
/// 特性标记类或方法  不建议全局注册
/// </summary>
namespace WebApiCore
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class OAuthControllerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //判断session是否超时
            if (1 == 1)
            {
                if (true)
                {
                    filterContext.Result = new RedirectResult("/login");
                }
                else
                {
                    filterContext.Result = new JsonResult(new Result<int>() { code = -9999, message = "登录已超时，请重新登录。" });
                }
                return;
            }


        }
    }
}
