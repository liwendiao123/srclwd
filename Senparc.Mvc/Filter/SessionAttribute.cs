using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Senparc.Service;

namespace Senparc.Mvc.Filter
{
    public class SessionAttribute : ActionFilterAttribute, IAuthorizationFilter
    {

        private bool forceLogout = false;//被强制退出登录
        private readonly EncryptionService _encryptionService;
        private readonly AdminUserInfoService _adminUserInfoService;

        public SessionAttribute()
        {
            _encryptionService = CO2NET.SenparcDI.GetService<EncryptionService>();
            _adminUserInfoService = CO2NET.SenparcDI.GetService<AdminUserInfoService>();
        }


        protected bool IsLogined(HttpContext httpContext)
        {
            var strjson = httpContext.Request.Query["session"];

            if (string.IsNullOrEmpty(strjson))
            {
                return false;
            }
            else
            {
                
                var result = _encryptionService.CommonDecrypt(strjson);

                if (!string.IsNullOrEmpty(result))
                {
                    var arr = result.Split("-");
                    if (arr.Length == 3)
                    {

                        httpContext.Session.SetString("userName", arr[0]);
                        httpContext.Session.SetString("ctoken", strjson);
                     // var sresult =  _adminUserInfoService.GetUserInfo(arr[0]);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }


        protected virtual bool AuthorizeCore(HttpContext httpContext)
        {
            //return true;
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            var strjson = httpContext.Request.Query["session"];

            if (string.IsNullOrEmpty(strjson))
            {
                strjson = httpContext.Request.Form["session"];
            }
           
            if (string.IsNullOrEmpty(strjson))
            {
                return false;
            }
            else
            {
              var result =  _encryptionService.CommonDecrypt(strjson);

                

                if (!string.IsNullOrEmpty(result))
                {
                 var arr =   result.Split("-");
                    if (arr.Length == 3)
                    {
                        var sresult = _adminUserInfoService.GetUserInfo(arr[0]);


                        //httpContext.Session.Set("UserInfo", sresult);
                      ///  httpContext.Session.i
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
                       

            return true;

        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
     
        

            if (filterContext == null)
            {
                var data = new
                {
                    code = -1,
                    msg = "系统未知错误",
                    data = false
                };
                var jsonResult = new JsonResult(data);
                filterContext.Result = jsonResult;
            }
            if (AuthorizeCore(filterContext.HttpContext))
            {
                // ** IMPORTANT **
                // Since we're performing authorization at the action level, the authorization code runs
                // after the output caching module. In the worst case this could allow an authorized user
                // to cause the page to be cached, then an unauthorized user would later be served the
                // cached page. We work around this by telling proxies not to cache the sensitive page,
                // then we hook our custom authorization code into the caching mechanism so that we have
                // the final say on whether a page should be served from the cache.

                //HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
                //cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                //cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
            }
            else
            {
                // auth failed, redirect to login page


                var data = new
                {
                    code = -1,
                    msg = "您尚未登录，请登录后再试！",
                    data = false
                };
                var jsonResult = new JsonResult(data);
                filterContext.Result = jsonResult;
            }
            //  return true;
        }

    }
}
