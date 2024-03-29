﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Senparc.CO2NET;
using Senparc.CO2NET.Extensions;
using Senparc.Core.Cache;
using Senparc.Core.Enums;
using Senparc.Core.Extensions;
using Senparc.Core.Models;
using Senparc.Core.Models.VD;
using Senparc.Log;
using Senparc.Mvc.Models.RequestModel;
using Senparc.Service;
using Senparc.Utility;
using System;
using System.Collections.Generic;
using BaseVD = Senparc.Core.Models.VD;

namespace Senparc.Mvc.Controllers
{
    //[RequireHttps]
    //[SenparcHandleError]
    [UserAuthorize("UserAnonymous")]
    public class BaseController : Controller, IResultFilter
    {
        //private ISystemConfigService _systemConfigService;
      //  protected readonly AdminUserInfoService _adminUserInfoService;
        protected readonly EncryptionService _encryptionService;
        protected FullSystemConfig _fullSystemConfig;
        protected DateTime PageStartTime { get; set; }
        protected DateTime PageEndTime { get; set; }

       // protected SessionInfo Session { get; set; }

       // protected AdminUserInfo AdminUser { get; set; }
        //protected st
        public BaseController()
        {
          //  _adminUserInfoService = CO2NET.SenparcDI.GetService<AdminUserInfoService>(); 
            _encryptionService    = CO2NET.SenparcDI.GetService<EncryptionService>();
            PageStartTime = DateTime.Now;
        }

        public FullAccount FullAccount { get; set; }

        public string UserName => User.Identity.IsAuthenticated ? User.Identity.Name : null;

        public bool IsAdmin => base.HttpContext.Session.GetString("AdminLogin") as string == "1" && FullAccount != null;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                TempData["Messager"] = TempData["Messager"];


               var result = context.HttpContext.Session.GetString("userName");

                if (!string.IsNullOrEmpty(result))
                {
                  //var  _adminUserInfoService = CO2NET.SenparcDI.GetService<AdminUserInfoService>();
                  //  this.AdminUser = _adminUserInfoService.GetUserInfo(result);
                }

                //var ctoken = context.HttpContext.Session.GetString("session");

                //if (!string.IsNullOrEmpty(ctoken))
                //{
                //   var er = _encryptionService.CommonDecrypt(ctoken);
                //    var arr = result.Split("-");
                //    if (arr.Length == 3)
                //    {

                //        long ticks = 0;
                //        Int64.TryParse(arr[1], out ticks);
                //        this.Session = new SessionInfo
                //        {
                //            ClientKey = arr[2],
                //             ExpireTime =new DateTime(ticks),
                //              UserName = arr[0]
                //        };

                //    }
                //}

                var fullSystemConfigCache = SenparcDI.GetService<FullSystemConfigCache>();

                _fullSystemConfig = fullSystemConfigCache.Data;
                var fullAccountCache = SenparcDI.GetService<FullAccountCache>();
                if (this.UserName != null)
                {
                    FullAccount = fullAccountCache.GetObject(this.UserName);
                    if (FullAccount != null)
                    {
                        //...
                    }
                    else
                    {
                        //用户不存在，发生严重异常
                        LogUtility.Account.Error($"发生严重错误，用户已登录，但缓存及数据库中不存在：{this.UserName}");
                    }
                }

                FullAccount = FullAccount ?? new FullAccount();
                FullAccount.LastActiveTime = DateTime.Now;

                base.OnActionExecuting(context);
            }
            catch (Exception ex)
            {
                context.Result = RenderError(ex.Message, ex);
            }

        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (ViewData.Model is BaseVD.IBaseVD)
            {
                var vd = ViewData.Model as BaseVD.IBaseVD;
                vd.UserName = this.UserName;
                vd.IsAdmin = this.IsAdmin;
                vd.FullAccount = this.FullAccount;
                vd.RouteData = this.RouteData;

                vd.FullSystemConfig = _fullSystemConfig;

                vd.MessagerList = vd.MessagerList ?? new List<Messager>();
                if (TempData["Messager"] as List<Messager> != null)
                {
                    vd.MessagerList.AddRange(TempData["Messager"] as List<Messager>);
                }
                //else
                //{
                //    if (!ModelState.IsValid)
                //    {
                //        vd.MessagerList.Add(new Messager(MessageType.danger, "提交信息有错误，请检查。"));
                //    }
                //}

                if (vd.FullSystemConfig == null)
                {
                    LogUtility.WebLogger.Error("FullSystemConfig为null");
                    vd.FullSystemConfig = new FullSystemConfig();
                }
            }

            if (ViewData.Model is BaseVD.IBaseUiVD)
            {
                var vd = ViewData.Model as BaseVD.IBaseUiVD;
                PageEndTime = DateTime.Now;
                vd.PageStartTime = PageStartTime;
                vd.PageEndTime = PageEndTime;
            }
        }

        /// <summary>
        /// 默认转到Edit页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return RedirectToAction("Edit");
        }

        /// <summary>
        /// 显示Json结果
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="result">数据，如果失败，可以为字符串的说明</param>
        /// <returns></returns>
        [NonAction]
        public ActionResult RenderJsonSuccessResult(bool success, object result,
            Newtonsoft.Json.JsonSerializerSettings jsonSerializerSettings = null,
            /*JsonRequestBehavior jsonRequestBehavior,*/
            string jsonpCallbackFunction = null)
        {
            var json = new
            {
                Success = success,
                Result = result
            };
            if (!jsonpCallbackFunction.IsNullOrEmpty())
            {
                return Content($"{jsonpCallbackFunction}({json.ToJson()})");
            }
            else
            {
                //return Json(json, jsonRequestBehavior);
                //https://stackoverflow.com/questions/4155014/json-asp-net-mvc-maxjsonlength-exception
                //解决JSON JavaScriptSerializer 进行序列化或反序列化时出错。字符串的长度超过了为 maxJsonLength 属性设置的值

                jsonSerializerSettings = jsonSerializerSettings ?? new Newtonsoft.Json.JsonSerializerSettings()
                {
                    //自定义设置
                };

                return new JsonResult(json, jsonSerializerSettings);
                //{
                //    JsonRequestBehavior = jsonRequestBehavior,
                //    MaxJsonLength = Int32.MaxValue
                //};
            }
        }


        #region 成功或错误提示页面

        [NonAction]
        public virtual ActionResult RenderSuccess(string message, string backAction = null,
            string backController = null, RouteValueDictionary backRouteValues = null)
        {
            backAction = backAction ?? RouteData.Values["controller"] as string;
            backController = backController ?? RouteData.Values["controller"] as string;
            backRouteValues = backRouteValues ?? new RouteValueDictionary();
            SuccessVD vd = new SuccessVD()
            {
                Message = message,
                BackAction = backAction,
                BackController = backController,
                BackRouteValues = backRouteValues
            };
            return View("Success");
        }

        [NonAction]
        public virtual ActionResult RenderError(string message, Exception ex = null)
        {
            //保留原有的controller和action信息
            ViewData["FakeControllerName"] = RouteData.Values["controller"] as string;
            ViewData["FakeActionName"] = RouteData.Values["action"] as string;

            return View("Error", new Error_ExceptionVD
            {
                Message = message,
                Exception = ex
            });
        }

        #endregion

        public void SetMessager(MessageType messageType, string messageText, bool showClose = true)
        {
            if (TempData["Messager"] == null || !(TempData["Messager"] is List<Messager>))
            {
                TempData["Messager"] = new List<Messager>();
            }

            (TempData["Messager"] as List<Messager>).Add(new Messager(messageType, messageText, showClose));
        }


        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}