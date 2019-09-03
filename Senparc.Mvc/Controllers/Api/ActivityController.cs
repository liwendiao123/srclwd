using Microsoft.AspNetCore.Mvc;
using Senparc.Mvc.Filter;
using Senparc.Mvc.Models.APIResponse;
using Senparc.Mvc.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Controllers
{

    [Session]
    public  class ActivityController : BaseFrontController
    {

        public ActivityController()
        {

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


       

    }
}
