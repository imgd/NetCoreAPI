using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 下面总结一下中间件类的一些约定

//关于中间件的方法：

//1.中间件的方法必须叫Invoke，且为public，非static。

//2.Invoke方法第一个参数必须是HttpContext类型。

//3.Invoke方法必须返回Task。

//4.Invoke方法可以有多个参数，除HttpContext外其它参数会尝试从依赖注入容器中获取。

//5.Invoke方法不能有重载。



//关于构造函数：

//1.构造函数必须包含RequestDelegate参数，该参数传入的是下一个中间件。

//2.构造函数参数中的RequestDelegate参数不是必须放在第一个，可以是任意位置。

//3.构造函数可以有多个参数，参数会优先从给定的参数列表中找，其次会从依赖注入容器中获取，获取失败会尝试获取默认值，都失败会抛出异常。

//4.构造函数可以有多个，届时会根据构造函数参数列表和给定的参数列表选择匹配度最高的一个。

//        中间间所依赖的初始化数据可以在  ConfigureServices(IServiceCollection services) 依赖注入  services.AddTransient<Ioc>();
//        也可以在添加中间件的时候初始化  app.UseMiddleware<IocMiddleware>(new Ioc());       
//        
/// </summary>
namespace WebApiCore
{
    public class IocMiddleware
    {
        private RequestDelegate _next;
        //private Ioc _ioc;

        public IocMiddleware(RequestDelegate next)
        {
            this._next = next;
            //this._ioc = ioc;
        }

        public async Task Invoke(HttpContext context, Ioc _ioc)
        {
            await _next(context);
            await context.Response.WriteAsync(_ioc.GetUserStr());
        }
    }

    public class Ioc
    {
        public List<string> users { get; set; }
        public Ioc()
        {
            users = new List<string>() {
                "ygd",
                "zengyu",
                "yunpucai"
            };
        }

        public string GetUserStr()
        {
            string result = "";
            foreach (var item in users)
            {
                result += $"|{item}";
            }
            return result.Substring(1);
        }
    }
}
