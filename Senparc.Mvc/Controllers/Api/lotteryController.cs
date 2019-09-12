using Microsoft.AspNetCore.Mvc;
using Senparc.Mvc.Filter;
using Senparc.Mvc.Models.APIResponse;
using Senparc.Mvc.Models.RequestModel;
using Senparc.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Senparc.Mvc.Controllers
{

   // [Session]
    public  class LotteryController : BaseFrontController
    {


        private readonly ActivityService _activityService;
        #region 构造函数
        public LotteryController(ActivityService activityService)
        {
            _activityService = activityService;
        }


        #endregion



        #region  内部方法


        #endregion


        #region API路由方法

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <returns></returns>

        public IActionResult GetList(LotteryRequestModel request)
        {
            //return Json(new {
            //    code = 0,
            //    msg = "获取数据成功",
            //    data = new List<LotteryResponseModel>() {
            //         new LotteryResponseModel(),
            //         new LotteryResponseModel(),
            //         new LotteryResponseModel(),
            //         new LotteryResponseModel(),
            //         new LotteryResponseModel(),
            //         new LotteryResponseModel(),
            //    }

            //});

          var list =  _activityService.GetFullList(x => true, x => x.Id, Core.Enums.OrderingType.Descending);
            return Json(new
            {
                code = 0,
                msg = "获取数据成功",
                data = list.ToList()

            });
        }

        /// <summary>
        /// 获取抽签详情
        /// </summary>
        /// <param name="program_id"></param>
        /// <returns></returns>

        public IActionResult Detail(string program_id)
        {
            return Json(
                
                new {
                    code = 0,
                    msg = "获取抽签详情",
                    data = new LotteryDetailResponseModel
                    {
                        name = "",
                        company_name = "",
                        summary = "",
                        hall = "",
                        time = "",
                        leader = "",
                        number = "",
                        phone = "",
                    }
                }
                
         );
        }





        #endregion



    }
}
