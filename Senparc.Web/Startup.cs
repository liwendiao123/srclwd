using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Core;
using Senparc.Core.Cache;
using Senparc.Core.Config;
using Senparc.Core.Extensions;
using Senparc.Core.Models;
using Senparc.Core.Utility;
using Senparc.Log;
using Senparc.Mvc.Filter;
using Senparc.Repository;
using Senparc.Service;
using Senparc.SMS;
using Senparc.Utility;
using Senparc.Web.Hubs;
using Senparc.Weixin;
using Senparc.Weixin.Cache.Redis;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.Open;
using Senparc.Weixin.Open.ComponentAPIs;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.TenPay;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Senparc.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //读取Log配置文件
            var repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

        public IConfiguration Configuration
        {
            get;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //如果运行在IIS中，需要添加IIS配置
            //https://docs.microsoft.com/zh-cn/aspnet/core/host-and-deploy/iis/index?view=aspnetcore-2.1&tabs=aspnetcore2x#supported-operating-systems
            //services.Configure<IISOptions>(options =>
            //{
            //    options.ForwardClientCertificate = false;
            //});
            services.AddMvc(options =>
                {
                    options.Filters.Add<HttpGlobalExceptionFilter>();
                })
                .AddXmlSerializerFormatters()
                .AddJsonOptions(options =>
                {
                    //忽略循环引用
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //不使用驼峰样式的key
                    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //设置时间格式
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                })
                //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-2.1&tabs=aspnetcore2x
                .AddSessionStateTempDataProvider()
                //忽略JSON序列化过程中的循环引用：https://stackoverflow.com/questions/7397207/json-net-error-self-referencing-loop-detected-for-type
                .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddSession();

            //解决中文进行编码问题
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.AddMemoryCache(); //使用内存缓存

            //services.Configure<RazorViewEngineOptions>();//https://stackoverflow.com/questions/36372926/asp-net-mvc-core-custom-controller-factory
            services.AddSignalR();
            services.AddTransient(typeof(Lazy<>)); //注册Lazy

            //var cache = services.BuildServiceProvider().GetService<IMemoryCache>();//测试成功
            services.Configure<SenparcCoreSetting>(Configuration.GetSection("SenparcCoreSetting"))
                .Configure<SenparcWeixinSetting>(Configuration.GetSection("SenparcWeixinSetting"))
                .Configure<SenparcSmsSetting>(Configuration.GetSection("SenparcSmsSetting"))
                //.AddSenparcDI() //全局注册 SenparcDI
                //.AddSenparcMvcDI() //TODO：需要和AddSenparcDI()进行合并
                .AddSenparcEntitiesDI(); //SQL Server设置
            services.AddSenparcGlobalServices(Configuration) //Senparc.CO2NET 全局注册
                    .AddSenparcWeixinServices(Configuration); //Senparc.Weixin 注册
            //services.AddHttpsRedirection(options =>
            //{
            //    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            //    options.HttpsPort = 443;
            //});

            //注册 Repository 和 Service
            services
                .AddScoped<FullSystemConfigCache>()
                .AddSingleton<PhoneCheckCodeCache>()
                .AddSingleton<QrCodeRegCache>()
                .AddSingleton<OAuthCodeCache>()
                .AddScoped<FullAccountCache>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddScoped<SystemConfigRepository>().AddScoped<SystemConfigService>()
                .AddScoped<AccountRepository>().AddScoped<AccountService>()
                .AddScoped<AdminUserInfoRepository>().AddScoped<AdminUserInfoService>()
                .AddScoped<FeedBackRepository>().AddScoped<FeedBackService>()
                .AddScoped<PointsLogRepository>().AddScoped<PointsLogService>()
                .AddScoped<AccountPayLogRepository>().AddScoped<AccountPayLogService>()

                .AddTransient<EncryptionService>()
                .AddTransient<WeixinService>()
                .AddTransient<QQWry>()
                .AddScoped<SmsRecordService>(); 

            //添加SenparcCoreSetting配置文件（内容可以根据需要对应修改）
            //注册数据库客户端连接
            services.AddScoped(typeof(ISqlClientFinanceData), typeof(SqlClientFinanceData));
            //添加基于Cookie的权限验证：https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.1&tabs=aspnetcore2x
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(AdminAuthorizeAttribute.AuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = "/Admin/Account/Forbidden/";
                    options.LoginPath = "/Admin/Login/Index";
                    options.Cookie.HttpOnly = false;
                })
                .AddCookie(UserAuthorizeAttribute.AuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = "/User/Account/Forbidden/";
                    options.LoginPath = "/User/Home/Login";
                    options.Cookie.HttpOnly = false;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                {
                    policy.RequireClaim("AdminMember");
                });
                options.AddPolicy("UserOnly", policy =>
                {
                    policy.RequireClaim("UserMember");
                });
                options.AddPolicy("UserAnonymous", policy =>
                {
                    policy.RequireClaim("UserMember");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<SenparcSetting> senparcSetting,
            IOptions<SenparcWeixinSetting> senparcWeixinSetting, IHubContext<ReloadPageHub> hubContext)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            #region CO2NET

            // 启动 CO2NET 全局注册，必须！
            IRegisterService register = RegisterService.Start(env, senparcSetting.Value)
                //关于 UseSenparcGlobal() 的更多用法见 CO2NET Demo：https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore/Startup.cs
                .UseSenparcGlobal();

            #region 全局缓存配置（按需）

            //当同一个分布式缓存同时服务于多个网站（应用程序池）时，可以使用命名空间将其隔离（非必须）
            register.ChangeDefaultCacheNamespace("SCFCache");

            #region 配置和使用 Redis

            //配置全局使用Redis缓存（按需，独立）
            var redisConfigurationStr = senparcSetting.Value.Cache_Redis_Configuration;
            var useRedis = !string.IsNullOrEmpty(redisConfigurationStr) && redisConfigurationStr != "Redis配置";
            if (useRedis) //这里为了方便不同环境的开发者进行配置，做成了判断的方式，实际开发环境一般是确定的，这里的if条件可以忽略
            {
                /* 说明：
                 * 1、Redis 的连接字符串信息会从 Config.SenparcSetting.Cache_Redis_Configuration 自动获取并注册，如不需要修改，下方方法可以忽略
                /* 2、如需手动修改，可以通过下方 SetConfigurationOption 方法手动设置 Redis 链接信息（仅修改配置，不立即启用）
                 */
                Senparc.CO2NET.Cache.Redis.Register.SetConfigurationOption(redisConfigurationStr);

                //以下会立即将全局缓存设置为 Redis
                Senparc.CO2NET.Cache.Redis.Register.UseKeyValueRedisNow(); //键值对缓存策略（推荐）
                //Senparc.CO2NET.Cache.Redis.Register.UseHashRedisNow();//HashSet储存格式的缓存策略

                //也可以通过以下方式自定义当前需要启用的缓存策略
                //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisObjectCacheStrategy.Instance);//键值对
                //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisHashSetObjectCacheStrategy.Instance);//HashSet
            }
            //如果这里不进行Redis缓存启用，则目前还是默认使用内存缓存 

            #endregion

            #region 注册日志（按需，建议）

            register.RegisterTraceLog(ConfigTraceLog); //配置TraceLog

            #endregion

            #endregion

            #endregion

            #region Weixin 设置

            /* 微信配置开始
             * 
             * 建议按照以下顺序进行注册，尤其须将缓存放在第一位！
             */

            //注册开始

            #region 微信缓存（按需，必须在 register.UseSenparcWeixin () 之前）

            //微信的 Redis 缓存，如果不使用则注释掉（开启前必须保证配置有效，否则会抛错）
            if (useRedis)
            {
                app.UseSenparcWeixinCacheRedis();
            }

            #endregion

            //开始注册微信信息，必须！
            register.UseSenparcWeixin(senparcWeixinSetting.Value, senparcSetting.Value)
                //注意：上一行没有 ; 下面可接着写 .RegisterXX()
            #region 注册公众号或小程序（按需）

                //注册公众号（可注册多个）
                .RegisterMpAccount(senparcWeixinSetting.Value, "SCF")
                .RegisterMpAccount("", "", "Senparc_Template")

                //注册多个公众号或小程序（可注册多个）
                //.RegisterWxOpenAccount(senparcWeixinSetting.Value, "【盛派网络小助手】小程序")
                //注册第三方平台（可注册多个）
            #region 注册第三方平台

                .RegisterOpenComponent(senparcWeixinSetting.Value,
                    //getComponentVerifyTicketFunc
                    componentAppId =>
                    {

                        var dir = Path.Combine(Server.GetMapPath("~/App_Data/OpenTicket"));
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        var file = Path.Combine(dir, string.Format("{0}.txt", componentAppId));
                        using (var fs = new FileStream(file, FileMode.Open))
                        {
                            using (var sr = new StreamReader(fs))
                            {
                                var ticket = sr.ReadToEnd();
                                return ticket;
                            }
                        }
                    },

                     //getAuthorizerRefreshTokenFunc
                     (componentAppId, auhtorizerId) =>
                     {
                         var dir = Path.Combine(Server.GetMapPath("~/App_Data/AuthorizerInfo/" + componentAppId));
                         if (!Directory.Exists(dir))
                         {
                             Directory.CreateDirectory(dir);
                         }

                         var file = Path.Combine(dir, string.Format("{0}.bin", auhtorizerId));
                         if (!System.IO.File.Exists(file))
                         {
                             return null;
                         }

                         using (Stream fs = new FileStream(file, FileMode.Open))
                         {
                             var binFormat = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                             var result = (RefreshAuthorizerTokenResult)binFormat.Deserialize(fs);
                             return result.authorizer_refresh_token;
                         }
                     },

                     //authorizerTokenRefreshedFunc
                     (componentAppId, auhtorizerId, refreshResult) =>
                     {
                         var dir = Path.Combine(Server.GetMapPath("~/App_Data/AuthorizerInfo/" + componentAppId));
                         if (!Directory.Exists(dir))
                         {
                             Directory.CreateDirectory(dir);
                         }

                         var file = Path.Combine(dir, string.Format("{0}.bin", auhtorizerId));
                         using (Stream fs = new FileStream(file, FileMode.Create))
                         {
                             var binFormat = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                             binFormat.Serialize(fs, refreshResult);
                             fs.Flush();
                         }
                     }, "【盛派网络】开放平台")

            #endregion
                //除此以外，仍然可以在程序任意地方注册公众号或小程序：
                //AccessTokenContainer.Register(appId, appSecret, name);//命名空间：Senparc.Weixin.MP.Containers

            #endregion

            #region 注册微信支付（按需）

                //注册最新微信支付版本（V3）（可注册多个）
                .RegisterTenpayV3(senparcWeixinSetting.Value, "SCF") //记录到同一个 SenparcWeixinSettingItem 对象中

            #endregion

            ;

            #endregion
            
            #region .NET Core默认不支持GB2312

            //http://www.mamicode.com/info-detail-2225481.html
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            #endregion

            #region Senparc.Core 设置

            //用于解决HttpContext.Connection.RemoteIpAddress为null的问题
            //https://stackoverflow.com/questions/35441521/remoteipaddress-is-always-null
            app.UseHttpMethodOverride(new HttpMethodOverrideOptions
            {
                //FormFieldName = "X-Http-Method-Override"//此为默认值
            });

            app.UseSenparcMvcDI();

            //Senparc.Core.Config.SiteConfig.SenparcCoreSetting = senparcCoreSetting.Value;//网站设置

            //提供网站根目录
            if (env.ContentRootPath != null)
            {
                Senparc.Core.Config.SiteConfig.ApplicationPath = env.ContentRootPath;
                Senparc.Core.Config.SiteConfig.WebRootPath = env.WebRootPath;
            }

            #endregion

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseAutoMapper();
            LogUtility.SystemLogger.Info("测试Log4net");
            //app.UseMvcWithDefaultRoute();
            if (env.IsDevelopment())
            {
                app.UsePhysicalFile(hubContext);
                app.UseSignalR(routes =>
                {
                    routes.MapHub<ReloadPageHub>(ReloadPageHub.Route);
                });
            }

            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaW",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });

            #region 异步线程

            {
                ////APM Ending 数据统计
                //var utility = new APMNeuralDataThreadUtility();
                //Thread thread = new Thread(utility.Run) { Name = "APMNeuralDataThread" };
                //SiteConfig.AsynThread.Add(thread.Name, thread);
            }

            SiteConfig.AsynThread.Values.ToList().ForEach(z =>
            {
                z.IsBackground = true;
                z.Start();
            }); //全部运行 

            #endregion
        }

        /// <summary>
        /// 配置微信跟踪日志
        /// </summary>
        private void ConfigTraceLog()
        {
            //这里设为Debug状态时，/App_Data/WeixinTraceLog/目录下会生成日志文件记录所有的API请求日志，正式发布版本建议关闭

            //如果全局的IsDebug（Senparc.CO2NET.Config.IsDebug）为false，此处可以单独设置true，否则自动为true
            CO2NET.Trace.SenparcTrace.SendCustomLog("系统日志",
                "SenparcCoreFramework 系统启动"); //只在Senparc.Weixin.Config.IsDebug = true的情况下生效

            //全局自定义日志记录回调
            CO2NET.Trace.SenparcTrace.OnLogFunc = () =>
            {
                //加入每次触发Log后需要执行的代码
            };

            //当发生基于WeixinException的异常时触发
            WeixinTrace.OnWeixinExceptionFunc = ex =>
            {
                //加入每次触发WeixinExceptionLog后需要执行的代码

                //发送模板消息给管理员
                //var eventService = new Senparc.Weixin.MP.Sample.CommonService.EventService();
                //eventService.ConfigOnWeixinExceptionFunc(ex);
            };
        }

    }
}

public static class PhysicalFileAppBuilderExtensions
{
    private static readonly PhysicalFileProvider _fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

    /// <summary>
    /// 检测我呢见变化
    /// </summary>
    /// <param name="app"></param>
    /// <param name="hubContext"></param>
    /// <returns></returns>
    public static IApplicationBuilder UsePhysicalFile(this IApplicationBuilder app, IHubContext<ReloadPageHub> hubContext)
    {
        RegisterPhysical(hubContext);
        return app;
    }

    /// <summary>
    /// 注册检查
    /// </summary>
    /// <param name="hubContext"></param>
    public static void RegisterPhysical(IHubContext<ReloadPageHub> hubContext)
    {
        Task.Run(() =>
        {
            var tcs = new TaskCompletionSource<object>();
            while (true)
            {
                PhysicalFileAsync(hubContext).GetAwaiter().GetResult();
            }
        });
    }

    /// <summary>
    /// 检查文件变化
    /// </summary>
    /// <param name="hubContext"></param>
    /// <returns></returns>
    private static async Task PhysicalFileAsync(IHubContext<ReloadPageHub> hubContext)
    {
        var jsToken = _fileProvider.Watch("wwwroot/**/*.js");
        var cssToken = _fileProvider.Watch("wwwroot/**/*.css");
        var cshtmlToken = _fileProvider.Watch("**/*.cshtml");
        var tcs = new TaskCompletionSource<object>();
        //TODO:如果考虑效率问题可以不使用while(true)，可以选中在RegisterChangeCallback 内部再次调用fileProvider.Watch
        jsToken.RegisterChangeCallback(state =>
        {
            ((TaskCompletionSource<object>)state).TrySetResult(null);
            hubContext.Clients.All.SendAsync("ReloadPage", "js文件发生变化");
        }, tcs);
        cssToken.RegisterChangeCallback(state =>
        {
            ((TaskCompletionSource<object>)state).TrySetResult(null);
            hubContext.Clients.All.SendAsync("ReloadPage", "css文件发生变化");
        }, tcs);
        cshtmlToken.RegisterChangeCallback(state =>
        {
            ((TaskCompletionSource<object>)state).TrySetResult(null);
            hubContext.Clients.All.SendAsync("ReloadPage", "cshtml文件发生变化");
        }, tcs);
        await tcs.Task.ConfigureAwait(false);
    }
}
