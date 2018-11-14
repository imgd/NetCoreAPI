using AspNetCore.CacheOutput;
using AspNetCore.CacheOutput.Extensions;
using AspNetCore.CacheOutput.Redis;
using Dnc.Api.Throttle;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Linq;

namespace WebApiCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 限流配置
            services.AddApiThrottle(options =>
            {
                //配置redis
                //如果Cache和Storage使用同一个redis，则可以按如下配置
                //options.UseRedisCacheAndStorage(opts =>
                //{
                //    opts.ConnectionString = "192.168.1.199:26379,password=123456,connectTimeout=5000,allowAdmin=false,defaultDatabase=1";
                //    //opts.KeyPrefix = "apithrottle"; //指定给所有key加上前缀，默认为apithrottle
                //});
                //如果Cache和Storage使用不同redis库，可以按如下配置
                //cache
                options.UseRedisCache(opts =>
                {
                    opts.ConnectionString = "192.168.1.199:26379,password=123456,connectTimeout=5000,allowAdmin=false,defaultDatabase=0";
                });
                //Storage
                options.UseRedisStorage(opts =>
                {
                    opts.ConnectionString = "192.168.1.199:26379,password=123456,connectTimeout=5000,allowAdmin=false,defaultDatabase=1";
                });

                //重写 限流输出
                options.onIntercepted = (context, valve, where) =>
                {
                    return new JsonResult(new Result<bool>(-99, "访问过于频繁，请稍后重试！", false));
                };
                //重写ip获取方式
                options.OnIpAddress = (context) =>
                {
                    var ip = context.Request.Headers["X-Forwarded-IP"].FirstOrDefault();
                    if (string.IsNullOrEmpty(ip))
                    {
                        ip = context.Connection.RemoteIpAddress.ToString();
                    }
                    return ip;
                };
                ////重写
                //options.OnUserIdentity = (context) =>
                //{
                //    var token = context.Request.Headers["X-Forwarded-TOKEN"].FirstOrDefault();
                //    return token;
                //};

                //options.Global.AddValves(new BlackListValve
                //{
                //    Policy = Policy.Ip,
                //    PolicyKey = "X-Forwarded-IP",
                //    WhenNull = WhenNull.Intercept,
                //    Priority = 99
                //}, new BlackListValve
                //{
                //    Policy = Policy.UserIdentity,
                //    PolicyKey = "X-Forwarded-TOKEN",
                //    WhenNull = WhenNull.Intercept,
                //    Priority = 88
                //},
                //new BlackListValve
                //{
                //    Policy = Policy.Header,
                //    PolicyKey = "throttle"
                //});
            });
            #endregion

            services.AddResponseCompression();            

            ConnectionMultiplexer redis =
                ConnectionMultiplexer.Connect("192.168.1.199:26379,password=123456,connectTimeout=5000,allowAdmin=false,defaultDatabase=2");

            services.AddSingleton<ICacheKeyGenerator, DefaultCacheKeyGenerator>();
            services.AddSingleton<IApiOutputCache, StackExchangeRedisOutputCacheProvider>((sprovder) =>
            {
                return new StackExchangeRedisOutputCacheProvider(redis.GetDatabase());
            });
            //services.AddMvc();

            services.AddMvc(opts =>
            {
                //这里添加ApiThrottleActionFilter拦截器
                opts.Filters.Add(typeof(ApiThrottleActionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            

            //启用http输出缓存
            //app.UseResponseCaching();
            //启用消息压缩
            app.UseResponseCompression();
            //输出缓存
            app.UseCacheOutput();
            //
            app.UseApiThrottle();

            app.UseMvc();
        }
    }
}
