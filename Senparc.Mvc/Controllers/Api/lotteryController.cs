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
    public  class LotteryController : BaseFrontController
    {

         
        #region 构造函数
        public LotteryController()
        {

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
            return Json(new {
                code = 0,
                msg = "获取数据成功",
                data = new List<LotteryResponseModel>() {
                     new LotteryResponseModel(),
                     new LotteryResponseModel(),
                     new LotteryResponseModel(),
                     new LotteryResponseModel(),
                     new LotteryResponseModel(),
                     new LotteryResponseModel(),
                }

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
