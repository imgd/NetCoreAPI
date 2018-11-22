using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// 全局异常拦截
/// </summary>
namespace WebApiCore
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {

            //标识已处理异常
            context.ExceptionHandled = true;
            //记录错误日志

            //判断客户端是否是ajax请求
            //ajax请求
            if (1 == 1)
            {
                context.Result = new JsonResult(new Result<int>(-1, $"sorry,出现了一个服务器错误(code:500).<br /> {context.Exception.Message}<br /> 如有疑问：grayd@foxmail.com"));
            }
            else
            {
                context.Result = new PartialViewResult()
                {
                    ViewName = "~/Views/Error/Index.cshtml",
                    //ViewData = new ViewDataDictionary<string>(message),

                };
            }

        }
    }
}
