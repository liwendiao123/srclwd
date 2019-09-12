using Microsoft.AspNetCore.Mvc;
using Senparc.Core.Models;
using Senparc.Mvc.Filter;
using Senparc.Mvc.Models.APIResponse;
using Senparc.Mvc.Models.RequestModel;
using Senparc.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Controllers
{

    //[Session]
    public  class ActivityController : BaseFrontController
    {
        private readonly ActivityService _activityService;
        private readonly AdminUserInfoService _adminUserInfoService;
        public ActivityController(ActivityService activityService, AdminUserInfoService adminUserInfoService)
        {
            _activityService = activityService;
            _adminUserInfoService = adminUserInfoService;
        }
        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <returns></returns>
        public IActionResult GetList(ActivityRequestModel request)
        {
            return Json(new {
                code = 0,
                msg = "获取数据成功",
                data = new List<ActivityResponse>() {
                    new ActivityResponse(),
                    new ActivityResponse(),

                }
            });
        }

        public IActionResult Detail(string article_id)
        {
            return Json(new ActivityDetailResponse
            {
                cover_url = "",
                title = "",
                summary = "",
                description = "",
                issue_time = "",
                content = ""
            });
        }

        public IActionResult UpdateStatus(string activityId, int status,string session)
        {
            try
            {
                //var strjson = httpContext.Request.Query["session"];

                //if (string.IsNullOrEmpty(strjson))
                //{
                //    strjson = httpContext.Request.Form["session"];
                //}
                AdminUserInfo adminUser = null;
                adminUser = VerifySession(session, adminUser,_adminUserInfoService);
                if (adminUser == null)
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "请先登录",
                        data = new
                        {
                        }


                    });
                }

                var activity = _activityService.GetObject(x => x.Id == activityId);

                if (activity != null)
                {
                    activity.ScheduleStatus = status;
                    _activityService.SaveObject(activity);

                    return Json(new
                    {
                        code = 0,
                        msg = "修改状态成功",
                        data = activity

                    });
                }
                else
                {

                    return Json(new
                    {
                        code = -1,
                        msg = "该活动不存在",
                        data = new { }

                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = -1,
                    msg = "服务器异常：" + ex.Message,
                    data = new {

                    }

                });
            }
        }

       
    }
}
