using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore
{
    public class OAouthMiddleware
    {
        private RequestDelegate _next;

        public OAouthMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (1 == 2)
            {
                await ReturnNoAuthorized(context);
            }
            else
            {
                await _next(context);
            }
        }
        /// <summary>
        /// not authorized request
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task ReturnNoAuthorized(HttpContext context)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync(new Result<bool>(-987, "没有权限middleware", false).ObjectToJson());
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseOAouth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OAouthMiddleware>();
        }
    }
}
